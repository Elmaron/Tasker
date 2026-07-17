using Avalonia;
using Avalonia.Platform;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Markup;
using Tmds.DBus.Protocol;
using static Tasker.Classes.DataBase.TimingInAppointment;

namespace Tasker.Classes {
    //Communicates directly with the DataBase to read and write the data accordingly
    public static class DataBase
    {
        public static string DbPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Tasker", "appData.db");
        public static string ConnectionString => $"Data Source={DbPath}";

        //Create a new Database, if none has been created and fill it with the base content
        public static void Initialize()
        {
            if (File.Exists(DbPath)) return;
            Directory.CreateDirectory(Path.GetDirectoryName(DbPath)!);

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = getSQL("Database/Create");
            command.ExecuteNonQuery();

            addValuesToTable("Difficulty", DataBaseStandards.Difficulty);
            addValuesToTable("Priority", DataBaseStandards.Priority);
            addValuesToTable("Type", DataBaseStandards.Type);
            addValuesToTable("Data", DataBaseStandards.Data);
            addValuesToTable("Category", DataBaseStandards.Category);
            addValuesToTable("Project", DataBaseStandards.Project);

        }

        private static void addValuesToTable(string table, List<Dictionary<string, object>> tableValues)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();

            List<string> commandList = [];

            foreach (Dictionary<string, object> value in tableValues)
            {
                commandList.Add($"INSERT OR IGNORE INTO {table}({
                    string.Join(", ", value.Keys)
                    }) VALUES ({string.Join(", ", (
                    value.Values.Select(x => 
                    x is Dictionary<string, object> dict ? getConditions(dict) 
                    : (x is int val ? val.ToString() :
                    "\'" + x?.ToString() + "\'" ?? $"\'{DataBaseStandards.DBE_EMPTY}\'"))
                    ))});");
            }

            foreach (string commandString in commandList)
                try {
                    command.CommandText = commandString;
                    System.Diagnostics.Debug.WriteLine($"Trying \"{commandString}\"");
                    command.ExecuteNonQuery();
                }
                catch (Exception e) {
                    System.Diagnostics.Debug.WriteLine($"Error in \"{commandString}\": {e.Message}");
                }
            
        }

        private static string getConditions(Dictionary<string, object> conditions)
        {
            object? table = "";
            conditions.TryGetValue(DataBaseStandards.R_TABLE, out table);
            List<string> conditionLayout = [];
            foreach (string key in conditions.Where(x => x.Key != DataBaseStandards.R_TABLE).Select(x => x.Key))
            {
                object? value = "";
                conditions.TryGetValue(key, out value);
                value = value is Dictionary<string, object> dict ? getConditions(dict) : (value is int val ? val.ToString() : "\'" + value?.ToString() + "\'" ?? $"<{DataBaseStandards.DBE_EMPTY}: VALUE>");
                conditionLayout.Add($"{key} = {value}");
            }
            return $"(SELECT Id FROM {table?.ToString() ?? $"<{DataBaseStandards.DBE_NOT_FOUND}: TABLE NOT FOUND IN KEYS>"} WHERE {
                string.Join(" AND ", conditionLayout)
                })";
        }

        //Use this function to get the content of a Database file (sql-File)
        public static String getSQL(string file)
        {
            var assembly = Assembly.GetExecutingAssembly();


            //String streamBuilder = "Tasker.SQlite." + file + ".sql";
            //using Stream stream = assembly.GetManifestResourceStream(streamBuilder);

            var uri = new Uri($"avares://Tasker/SQlite/{file}.sql");
            using var stream = AssetLoader.Open(uri);

            //if (stream == null) System.Diagnostics.Debug.WriteLine("File not found");
            if (stream == null) return "";
            using StreamReader reader = new StreamReader(stream);

            //System.Diagnostics.Debug.WriteLine(reader.ReadToEnd());

            return reader.ReadToEnd();
        }

        // For internal usage in the Table Class
        public interface IData
        {
            public int Id { get; set; }
        };

        // Abstract Class Table, which has all implementations ready for the different tables
        public abstract class Table<T> where T : IData
        {
            public abstract string Name { get; }
            
            protected abstract T CreateData(SqliteDataReader reader);

            //The following function returns the Table set under "Name"
            public List<T> Get()
            {
                List<T> data = new();

                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "SELECT * FROM " + Name + ";";

                using var reader = command.ExecuteReader(); ;
                while (reader.Read())
                {
                    data.Add(CreateData(reader));
                }

                return data;
            }
            
            public void Add(IData newData)
            {
                PropertyInfo[] dataProperties = newData.GetType().GetProperties();
                string[] parameters = dataProperties.Select(property => property.Name).ToArray();
                object?[] data = dataProperties.Select(property => property.GetValue(newData)).ToArray();
                addData(this.Name, parameters, data);
            }

            public void Update(IData newData)
            {
                PropertyInfo[] dataProperties = newData.GetType().GetProperties();
                string[] parameters = dataProperties.Select(property => property.Name).ToArray();
                object?[] data = dataProperties.Select(property => property.GetValue(newData)).ToArray();
                updateData(this.Name, parameters, data);
            }

            public void Delete(int id)
            {
                deleteData(this.Name, id);
            }
            

            private static void addData(string table, string[] parameters, object?[] data)
            {
                if (parameters.Length != data.Length) return;

                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                foreach (object? dataObject in data)
                {
                    if (dataObject != null) continue;
                    parameters = parameters.Where((x, i) => i != Array.IndexOf(data, dataObject)).ToArray();
                    data = data.Where((x, i) => i != Array.IndexOf(data, dataObject)).ToArray();
                }

                var command = connection.CreateCommand();
                command.CommandText =
                    "INSERT INTO " + table +
                    "(" + string.Join(", ", 
                        parameters.Where((x, index) => data[index] != null)) 
                    + ") " + "VALUES(" + string.Join(", ", 
                    data.Where(x => x != null).Select(x => 
                    x is string str ? "\'" + str + "\'" : x is DateTime dat ? "\'" + dat.ToString("O") + "\'" : x?.ToString()
                    )) + ")";

                command.ExecuteNonQuery();
            }

            private static void updateData(string table, string[] parameters, object?[] data)
            {
                if (parameters.Length != data.Length) return;
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                if (!parameters.Contains("Id")) return;
                if (data[Array.IndexOf(parameters, "Id")] is not int id) return;

                foreach (object? dataObject in data)
                {
                    if (dataObject != null) continue;
                    parameters = parameters.Where((x, i) => i != Array.IndexOf(data, dataObject)).ToArray();
                    data = data.Where((x, i) => i != Array.IndexOf(data, dataObject)).ToArray();
                }

                var command = connection.CreateCommand();
                command.CommandText =
                    "UPDATE " + table +
                    "SET " + string.Join(", ", parameters
                                    .Zip(data, (a, b) => new { Name = a, Value = b })
                                    .Where(x => !x.Name.Equals("Id"))
                                    .Select(x => $"{x.Name} = {x.Value}")
                                    ) +
                    "WHERE Id = " + id + ";";

                command.ExecuteNonQuery();
            }

            private static void deleteData(string table, int id)
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "DELETE FROM " + table + " WHERE Id = " + id + ";";

                command.ExecuteNonQuery();
            }

        }


        //Tables without relations to other tables
        public class Data : Table<Data.DataOfData>
        {
            public override string Name { get { return "Data"; } }
            public class DataOfData : IData
            {
                public int Id { get; set; }
                public string Label { get; set; } = "";
                public string? Description { get; set; } = "";
                public DateTime Created { get; set; }
                public DateTime Updated { get; set; }
                public DateTime? Finished { get; set; }
                public DateTime? DeleteOn { get; set; }
            }

            protected override DataOfData CreateData(SqliteDataReader reader)
            {
                return new DataOfData
                {
                    Id = reader.GetInt32(0),
                    Label = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Created = reader.GetDateTime(3),
                    Updated = reader.GetDateTime(4),
                    Finished = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                    DeleteOn = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
                };
            }
        }

        public class Difficulty : Table<Difficulty.DifficultyData>
        {
            public override string Name { get { return "Difficulty"; } }
            public class DifficultyData : IData
            {
                public int Id { get; set; }
                public string Label { get; set; } = "";
                public string Description { get; set; } = "";
                public string Recommendation { get; set; } = "";
            }

            protected override DifficultyData CreateData(SqliteDataReader reader)
            {
                return new DifficultyData
                {
                    Id = reader.GetInt32(0),
                    Label = reader.GetString(1),
                    Description = reader.GetString(2),
                    Recommendation = reader.GetString(3)
                };
            }
        }

        public class Priority : Table<Priority.PriorityData>
        {
            public override string Name { get { return "Priority"; } }
            public class PriorityData : IData
            {
                public int Id { get; set; }
                public string Label { get; set; } = "";
                public int Ordering { get; set; }
            }

            protected override PriorityData CreateData(SqliteDataReader reader)
            {
                return new PriorityData
                {
                    Id = reader.GetInt32(0),
                    Label = reader.GetString(1),
                    Ordering = reader.GetInt32(2)
                };
            }
        }

        public class Type : Table<Type.TypeData>
        {
            public override string Name { get { return "Type"; } }
            public class TypeData : IData
            {
                public int Id { get; set; }
                public string Label { get; set; } = "";
            }

            protected override TypeData CreateData(SqliteDataReader reader)
            {
                return new TypeData
                {
                    Id = reader.GetInt32(0),
                    Label = reader.GetString(1)
                };
            }
        }

        //Table with 1 to m connections
        public class Timing : Table<Timing.TimingData>
        {
            public override string Name { get { return "Timing"; } }
            public class TimingData : IData
            {
                public int Id { get; set; }
                public int TypeID { get; set; }
                public DateTime Start { get; set; }
                public DateTime End { get; set; }
            }

            protected override TimingData CreateData(SqliteDataReader reader)
            {
                return new TimingData
                {
                    Id = reader.GetInt32(0),
                    TypeID = reader.GetInt32(1),
                    Start = reader.GetDateTime(2),
                    End = reader.GetDateTime(3)
                };
            }
        }

        public class Repeater : Table<Repeater.RepeaterData>
        {
            public override string Name { get { return "Repeater"; } }
            public class RepeaterData : IData
            {
                public int Id { get; set; }
                public int TypeID { get; set; }
                public int MonthlyInterval { get; set; }
                public int DailyInterval { get; set; }
            }

            protected override RepeaterData CreateData(SqliteDataReader reader)
            {
                return new RepeaterData
                {
                    Id = reader.GetInt32(0),
                    TypeID = reader.GetInt32(1),
                    MonthlyInterval = reader.GetInt32(2),
                    DailyInterval = reader.GetInt32(3)
                };
            }
        }

        public class Category : Table<Category.CategoryData>
        {
            public override string Name { get { return "Category"; } }
            public class CategoryData : IData
            {
                public int Id { get; set; }
                public int DataId { get; set; }
                public int PriorityId { get; set; }
            }

            protected override CategoryData CreateData(SqliteDataReader reader)
            {
                return new CategoryData
                {
                    Id = reader.GetInt32(0),
                    DataId = reader.GetInt32(1),
                    PriorityId = reader.GetInt32(2)
                };
            }
        }

        public class Project : Table<Project.ProjectData>
        {
            public override string Name { get { return "Project"; } }
            public class ProjectData : IData
            {
                public int Id { get; set; }
                public int DataId { get; set; }
                public int CategoryId { get; set; }
                public int PriorityId { get; set; }
                public DateTime? Expiry {  get; set; }
            }

            protected override ProjectData CreateData(SqliteDataReader reader)
            {
                return new ProjectData
                {
                    Id = reader.GetInt32(0),
                    DataId = reader.GetInt32(1),
                    CategoryId = reader.GetInt32(2),
                    PriorityId = reader.GetInt32(3),
                    Expiry = reader.IsDBNull(4) ? null : reader.GetDateTime(4)
                };
            }
        }

        public class Appointment : Table<Appointment.AppointmentData>
        {
            public override string Name { get { return "Appointment"; } }
            public class AppointmentData : IData
            {
                public int Id { get; set; }
                public int DataId { get; set; }
                public int ProjectId { get; set; }
            }

            protected override AppointmentData CreateData(SqliteDataReader reader)
            {
                return new AppointmentData
                {
                    Id = reader.GetInt32(0),
                    DataId = reader.GetInt32(1),
                    ProjectId = reader.GetInt32(2)
                };
            }
        }

        public class Task : Table<Task.TaskData>
        {
            public override string Name { get { return "Task"; } }
            public class TaskData : IData
            {
                public int Id { get; set; }
                public int DataId { get; set; }
                public int ProjectId { get; set; }
                public int PriorityId { get; set; }
                public int? DifficultyId { get; set; }
                public DateTime? Expiry { get; set; }
            }

            protected override TaskData CreateData(SqliteDataReader reader)
            {
                return new TaskData
                {
                    Id = reader.GetInt32(0),
                    DataId = reader.GetInt32(1),
                    ProjectId = reader.GetInt32(2),
                    PriorityId = reader.GetInt32(3),
                    DifficultyId = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    Expiry = reader.IsDBNull(5) ? null : reader.GetDateTime(5)
                };
            }
        }

        public class WorktimeLimit : Table<WorktimeLimit.WorktimeLimitData>
        {
            public override string Name { get { return "WorktimeLimit"; } }
            public class WorktimeLimitData : IData
            {
                public int Id { get; set; }
                public int CategoryId { get; set; }
                public int TypeId { get; set; }
                public int LimitInMinutes { get; set; }
            }

            protected override WorktimeLimitData CreateData(SqliteDataReader reader)
            {
                return new WorktimeLimitData
                {
                    Id = reader.GetInt32(0),
                    CategoryId = reader.GetInt32(1),
                    TypeId = reader.GetInt32(2),
                    LimitInMinutes = reader.GetInt32(3)
                };
            }
        }

        // Tables with m to n relations
        public class TimingInCategory : Table<TimingInCategory.TimingInCategoryData>
        {
            public override string Name { get { return "TimingInCategory"; } }
            public class TimingInCategoryData : IData
            {
                public int Id { get; set; }
                public int TimingId { get; set; }
                public int CategoryId { get; set; }
            }

            protected override TimingInCategoryData CreateData(SqliteDataReader reader)
            {
                return new TimingInCategoryData
                {
                    Id = reader.GetInt32(0),
                    TimingId = reader.GetInt32(1),
                    CategoryId = reader.GetInt32(2)
                };
            }
        }

        public class TimingInProject : Table<TimingInProject.TimingInProjectData>
        {
            public override string Name { get { return "TimingInProject"; } }
            public class TimingInProjectData : IData
            {
                public int Id { get; set; }
                public int TimingId { get; set; }
                public int ProjectId { get; set; }
            }

            protected override TimingInProjectData CreateData(SqliteDataReader reader)
            {
                return new TimingInProjectData
                {
                    Id = reader.GetInt32(0),
                    TimingId = reader.GetInt32(1),
                    ProjectId = reader.GetInt32(2)
                };
            }
        }

        public class TimingInAppointment : Table<TimingInAppointment.TimingInAppointmentData>
        {
            public override string Name { get { return "TimingInAppointment"; } }
            public class TimingInAppointmentData : IData
            {
                public int Id { get; set; }
                public int TimingId { get; set; }
                public int AppointmentId { get; set; }
            }

            protected override TimingInAppointmentData CreateData(SqliteDataReader reader)
            {
                return new TimingInAppointmentData
                {
                    Id = reader.GetInt32(0),
                    TimingId = reader.GetInt32(1),
                    AppointmentId = reader.GetInt32(2)
                };
            }
        }

        public class TimingInTask : Table<TimingInTask.TimingInTaskData>
        {
            public override string Name { get { return "TimingInTask"; } }
            public class TimingInTaskData : IData
            {
                public int Id { get; set; }
                public int TimingId { get; set; }
                public int TaskId { get; set; }
            }

            protected override TimingInTaskData CreateData(SqliteDataReader reader)
            {
                return new TimingInTaskData
                {
                    Id = reader.GetInt32(0),
                    TimingId = reader.GetInt32(1),
                    TaskId = reader.GetInt32(2)
                };
            }
        }
    }
}