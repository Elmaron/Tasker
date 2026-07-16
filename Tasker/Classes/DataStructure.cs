using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Tasker.Classes.DataBase;
using static Tasker.Classes.DataStructure;
using static Tasker.Classes.DataStructure.Category;

namespace Tasker.Classes
{
    //Handles Communication between DataBase and program
    public class DataStructure
    {
        public List<Category> categories = new();
        public List<InternalPriority> priorities = new();

        public DataStructure() 
        {
            System.Diagnostics.Debug.WriteLine($"Initializing new class Datastructure...");
            DataBase.Category dbCategory = new();
            foreach (DataBase.Category.CategoryData category in dbCategory.Get())
            {
                categories.Add(new Category(category));
            }
            DataBase.Priority dbPriority = new();
            foreach (DataBase.Priority.PriorityData priority in dbPriority.Get())
            {
                priorities.Add(new InternalPriority(priority));
            }
        }

        public class Category
        {

            public int Id { get; }
            
            public InternalData data;
            
            public InternalPriority priority;

            public List<Project> projects = new();

            public Category(DataBase.Category.CategoryData pDbCategory)
            {
                DataBase.Data dbData = new();
                DataBase.Priority dbPriority = new();

                Id = pDbCategory.Id;
                data = new(dbData.Get().Where(x => x.Id == pDbCategory.DataId).ToArray()[0]);
                priority = new(dbPriority.Get().Where(x => x.Id == pDbCategory.PriorityId).ToArray()[0]);

                System.Diagnostics.Debug.WriteLine($"Found Category: {data.Label}");

                DataBase.Project dbProject = new();
                foreach (DataBase.Project.ProjectData project in dbProject.Get())
                {
                    if (project.CategoryId != Id) continue;
                    projects.Add(new Project(project));
                }
            }
            public class Project
            {
                public int Id { get; }

                public InternalData data;

                public InternalPriority priority;

                public DateTime? Expiry;

                public List<Appointment> appointments = new();
                public List<Task> tasks = new();

                public Project(DataBase.Project.ProjectData pDbProject)
                {
                    DataBase.Data dbData = new();
                    DataBase.Priority dbPriority = new();

                    Id = pDbProject.Id;
                    data = new(dbData.Get().Where(x => x.Id == pDbProject.DataId).ToArray()[0]);
                    priority = new(dbPriority.Get().Where(x => x.Id == pDbProject.PriorityId).ToArray()[0]);
                    Expiry = pDbProject.Expiry;

                    System.Diagnostics.Debug.WriteLine($"Found Project: {data.Label}");

                    DataBase.Appointment dbAppointment = new();
                    foreach (DataBase.Appointment.AppointmentData appointment in dbAppointment.Get())
                    {
                        if (appointment.ProjectId != Id) continue;
                        appointments.Add(new Appointment(appointment));
                    }

                    DataBase.Task dbTask = new();
                    foreach (DataBase.Task.TaskData task in dbTask.Get())
                    {
                        if (task.ProjectId != Id) continue;
                        tasks.Add(new Task(task));
                    }
            }
                public class Appointment
                {
                    public int Id { get; }

                    public InternalData data;

                    public Appointment(DataBase.Appointment.AppointmentData pDbAppointment)
                    {
                        DataBase.Data dbData = new();

                        Id = pDbAppointment.Id;
                        data = new(dbData.Get().Where(x => x.Id == pDbAppointment.DataId).ToArray()[0]);

                        System.Diagnostics.Debug.WriteLine($"Found Appointment: {data.Label}");
                    }
                }
                public class Task
                {
                    public int Id { get; }

                    public InternalData data;

                    public InternalPriority priority;

                    public InternalDifficulty? difficulty;

                    public DateTime? Expiry;

                    public Task (DataBase.Task.TaskData pDbTask)
                    {
                        DataBase.Data dbData = new();
                        DataBase.Priority dbPriority = new();
                        DataBase.Difficulty dbDifficulty = new();

                        Id = pDbTask.Id;
                        data = new(dbData.Get().Where(x => x.Id == pDbTask.DataId).ToArray()[0]);
                        priority = new(dbPriority.Get().Where(x => x.Id == pDbTask.PriorityId).ToArray()[0]);

                        //nullable Definition
                        DataBase.Difficulty.DifficultyData[] dbDifficulty_data = dbDifficulty.Get().Where(x => x.Id == pDbTask.DifficultyId).ToArray();
                        difficulty = dbDifficulty_data.Length <= 0 ? null : new(dbDifficulty_data[0]);
                        Expiry = pDbTask.Expiry;
                    }

                    /*
                    public int Add(int pDataId, int pProjectId, int pPriorityId, int? pDifficultyId, DateTime? pExpiry)
                    {
                        DataBase.Task dbTask = new();
                        dbTask.Add(
                                new DataBase.Task.TaskData
                                {
                                    DataId = pDataId,
                                    ProjectId = pProjectId,
                                    PriorityId = pPriorityId,
                                    DifficultyId = pDifficultyId,
                                    Expiry = pExpiry
                                }
                            );
                    }
                    */
                }
            }
        }

        public struct InternalData
        {
            public InternalData(Tasker.Classes.DataBase.Data.DataOfData dbData)
            {
                Id = dbData.Id;
                Label = dbData.Label;
                Description = dbData.Description;
                Created = dbData.Created;
                Updated = dbData.Updated;
                Finished = dbData.Finished;
                DeleteOn = dbData.DeleteOn;
            }

            public int Id { get; }
            public string Label { get; set; }
            public string? Description { get; set; }
            public DateTime Created { get; set; }
            public DateTime Updated { get; set; }
            public DateTime? Finished { get; set; }
            public DateTime? DeleteOn { get; set; }
        }

        public struct InternalPriority
        {
            public InternalPriority(Tasker.Classes.DataBase.Priority.PriorityData dbPriority)
            {
                Id = dbPriority.Id;
                Label = dbPriority.Label;
                Ordering = dbPriority.Ordering;
            }
            public int Id { get; }
            public string Label { get; set; }
            public int Ordering { get; set; }
        }

        public struct InternalDifficulty
        {
            public InternalDifficulty(Tasker.Classes.DataBase.Difficulty.DifficultyData dbDifficulty)
            {
                Id = dbDifficulty.Id;
                Label = dbDifficulty.Label;
                Description = dbDifficulty.Description;
                Recommendation = dbDifficulty.Recommendation;
            }
            public int Id { get; }
            public string Label { get; set; }
            public string Description { get; set; }
            public string Recommendation { get; set; }
        }
    }
}
