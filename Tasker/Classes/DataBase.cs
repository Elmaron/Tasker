using Avalonia;
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
using static Tasker.Classes.DataBase.Difficulty;
using static Tasker.Classes.DataBase.Table;

namespace Tasker.Classes {
    public static class DataBase
    {
        public static string DbPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Tasker", "appData.db");
        public static string ConnectionString => $"Data Source={DbPath}";

        //Create a new Database, if none has been created and fill it with the base content
        public static void Initialize()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DbPath)!);

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = getSQL("Database.Create");
            command.ExecuteNonQuery();
            command.CommandText = getSQL("Database.Initialize");
            command.ExecuteNonQuery();
        }

        //Use this function to get the content of a Database file (sql-File)
        public static String getSQL(string file)
        {
            var assembly = Assembly.GetExecutingAssembly();

            String streamBuilder = "ADHDWORKER.SQlite." + file + ".sql";
            using Stream stream = assembly.GetManifestResourceStream(streamBuilder);
            if (stream == null) return "";
            using StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        // Abstract Class Table, which has all implementations ready for the different tables
        public abstract class Table
        {
            protected string Name { get; set; } = "";
            public interface IData
            {
                public int Id { get; set; }
            };

            protected abstract IData CreateData(SqliteDataReader reader);

            public List<IData> Get()
            {
                List<IData> data = new();

                using var reader = getData(this.Name);
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

            //Use this function in others to read the data of a specific table
            private static SqliteDataReader getData(string table)
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "SELECT * FROM " + table + ";";

                return command.ExecuteReader();
            }

            //Use this function to add inputed data to a specific table
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
                    "(" + string.Join(", ", parameters) + ") " +
                    "VALUES(" + string.Join(", ", data) + ")";

                command.ExecuteNonQuery();
            }

            //Use this function to update inputed data of a specific table
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

            //Use this function to delete data from a table
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

        
        //Logic for m to n connections
        //MISSING



        //Handle the Table Data
        public class Data : Table
        {
            protected new string Name = "Data";
            public class DataOfData : IData
            {
                public int Id { get; set; }
                public string Label { get; set; } = "";
                public string Description { get; set; } = "";
                public DateTime Created { get; set; }
                public DateTime Updated { get; set; }
                public DateTime Finished { get; set; }
                public DateTime DeleteOn { get; set; }
            }

            protected override IData CreateData(SqliteDataReader reader)
            {
                return new DataOfData
                {
                    Id = reader.GetInt32(0),
                    Label = reader.GetString(1),
                    Description = reader.GetString(2),
                    Created = reader.GetDateTime(3),
                    Updated = reader.GetDateTime(4),
                    Finished = reader.GetDateTime(5),
                    DeleteOn = reader.GetDateTime(6)
                };
            }
        }

        //Handle the Table Difficulties
        public class Difficulty : Table
        {
            protected new string Name = "Difficulty";
            public class DifficultyData : IData
            {
                public int Id { get; set; }
                public string Label { get; set; } = "";
                public string Description { get; set; } = "";
                public string Recommendation { get; set; } = "";
            }

            protected override IData CreateData(SqliteDataReader reader)
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

        //Handle the Table Priorities
        public class Priority : Table
        {
            protected new string Name = "Priority";
            public class PriorityData : IData
            {
                public int Id { get; set; }
                public string Label { get; set; } = "";
                public int Ordering { get; set; }
            }

            protected override IData CreateData(SqliteDataReader reader)
            {
                return new PriorityData
                {
                    Id = reader.GetInt32(0),
                    Label = reader.GetString(1),
                    Ordering = reader.GetInt32(2)
                };
            }
        }

        //Handle the Table Types
        public class Type : Table
        {
            protected new string Name = "Type";
            public class TypeData : IData
            {
                public int Id { get; set; }
                public string Label { get; set; } = "";
            }

            protected override IData CreateData(SqliteDataReader reader)
            {
                return new TypeData
                {
                    Id = reader.GetInt32(0),
                    Label = reader.GetString(1)
                };
            }
        }

        //Handle the Table Timings
        public class Timing : Table
        {
            protected new string Name = "Timing";
            public class TimingData : IData
            {
                public int Id { get; set; }
                public int TypeID { get; set; }
                public DateTime Start { get; set; }
                public DateTime End { get; set; }
            }

            protected override IData CreateData(SqliteDataReader reader)
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

        //Handle the Table Repeater
        public class Repeater : Table
        {
            protected new string Name = "Repeater";
            public class RepeaterData : IData
            {
                public int Id { get; set; }
                public int TypeID { get; set; }
                public int MonthlyInterval { get; set; }
                public int DailyInterval { get; set; }
            }

            protected override IData CreateData(SqliteDataReader reader)
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

        //Handle the Table Category
        public class Category : Table
        {
            protected new string Name = "Category";
            public class CategoryData : IData
            {
                public int Id { get; set; }
                public int DataId { get; set; }
                public int PriorityId { get; set; }
            }

            protected override IData CreateData(SqliteDataReader reader)
            {
                return new CategoryData
                {
                    Id = reader.GetInt32(0),
                    DataId = reader.GetInt32(1),
                    PriorityId = reader.GetInt32(2)
                };
            }
        }

        //Handle the Table Project
        public class Project : Table
        {
            protected new string Name = "Project";
            public class ProjectData : IData
            {
                public int Id { get; set; }
                public int DataId { get; set; }
                public int CategoryId { get; set; }
                public int PriorityId { get; set; }
                public DateTime Expiry {  get; set; }
            }

            protected override IData CreateData(SqliteDataReader reader)
            {
                return new ProjectData
                {
                    Id = reader.GetInt32(0),
                    DataId = reader.GetInt32(1),
                    CategoryId = reader.GetInt32(2),
                    PriorityId = reader.GetInt32(3),
                    Expiry = reader.GetDateTime(4)
                };
            }
        }

        //Handle the Table Appointments
        public class Appointment : Table
        {
            protected new string Name = "Appointment";
            public class AppointmentData : IData
            {
                public int Id { get; set; }
                public int DataId { get; set; }
                public int ProjectId { get; set; }
            }

            protected override IData CreateData(SqliteDataReader reader)
            {
                return new AppointmentData
                {
                    Id = reader.GetInt32(0),
                    DataId = reader.GetInt32(1),
                    ProjectId = reader.GetInt32(2)
                };
            }
        }

        //Handle the Table Tasks
        public class Task : Table
        {
            protected new string Name = "Task";
            public class TaskData : IData
            {
                public int Id { get; set; }
                public int DataId { get; set; }
                public int ProjectId { get; set; }
                public int PriorityId { get; set; }
                public int DifficultyId { get; set; }
                public DateTime Expiry { get; set; }
            }

            protected override IData CreateData(SqliteDataReader reader)
            {
                return new TaskData
                {
                    Id = reader.GetInt32(0),
                    DataId = reader.GetInt32(1),
                    ProjectId = reader.GetInt32(2),
                    PriorityId = reader.GetInt32(3),
                    DifficultyId = reader.GetInt32(4),
                    Expiry = reader.GetDateTime(5)
                };
            }
        }

        //Handle the Table WorktimeLimit
        public class WorktimeLimit : Table
        {
            protected new string Name = "WorktimeLimit";
            public class WorktimeLimitData : IData
            {
                public int Id { get; set; }
                public int CategoryId { get; set; }
                public int TypeId { get; set; }
                public int LimitInMinutes { get; set; }
            }

            protected override IData CreateData(SqliteDataReader reader)
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

    }
}