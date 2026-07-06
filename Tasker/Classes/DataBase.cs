using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.Data.Sqlite;

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

        //Handle the Table Difficulties
        public static class Difficulties
        {

        }

        //Handle the Table Priorities
        public static class Priorities
        {

        }

        //Handle the Table Category and add necessary items automatically
        public static class Category
        {

        }

        //Handle the Table Project and add necessary items automatically
        public static class Project
        {

        }

        //Handle the Table Appointments and add necessary items automatically
        public static class Appointments
        {

        }

        //Handle the Table Tasks and add necessary items automatically
        public static class Tasks
        {

        }
    }
}