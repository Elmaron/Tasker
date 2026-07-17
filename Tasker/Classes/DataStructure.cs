using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using static Tasker.Classes.DataBase;
using static Tasker.Classes.DataStructure;
using static Tasker.Classes.DataStructure.Category;

namespace Tasker.Classes
{
    //Handles Communication between DataBase and program
    public class DataStructure
    {
        public List<Category> categories;
        public List<InternalPriority> priorities;

        public DataStructure() 
        {
            System.Diagnostics.Debug.WriteLine($"Initializing new class Datastructure...");
            Reload();
            categories ??= [];
            priorities ??= [];
        }

        public void Reload()
        {
            categories = [];
            priorities = [];
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

            public void Update(string? label = null, string? description = null)
            {

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
                }
            }
        }

        public int AddCategory(string label)
        {
            int[] newDataId = generateNewData(label);
            int newPriorityId = getStandardPriorityId();

            DataBase.Category dbCategory = new();
            IEnumerable<int> possibleCategoryIds = dbCategory.Get().Select(x => x.Id);
            int newCategoryId = possibleCategoryIds.Count() > 0 ? possibleCategoryIds.Max() + 1 : 1;
            dbCategory.Add(new DataBase.Category.CategoryData
            {
                Id = newCategoryId,
                DataId = newDataId[0],
                PriorityId = newPriorityId
            });

            DataBase.Project dbProject = new();
            IEnumerable<int> possibleProjectIds = dbProject.Get().Select(x => x.Id);
            int newProjectId = possibleProjectIds.Count() > 0 ? possibleProjectIds.Max() + 1 : 1;
            dbProject.Add(new DataBase.Project.ProjectData
            {
                Id = newProjectId,
                DataId = newDataId[1],
                CategoryId = newCategoryId,
                PriorityId = newPriorityId
            }
            );

            this.Reload();
            return newCategoryId;
        }

        public int AddProject(string label, int newCategoryId)
        {
            int newDataId = generateNewData(label)[0];
            int newPriorityId = getStandardPriorityId();

            DataBase.Project dbProject = new();
            IEnumerable<int> possibleProjectIds = dbProject.Get().Select(x => x.Id);
            int newProjectId = possibleProjectIds.Count() > 0 ? possibleProjectIds.Max() + 1 : 1;
            dbProject.Add(new DataBase.Project.ProjectData
            {
                Id = newProjectId,
                DataId = newDataId,
                CategoryId = newCategoryId,
                PriorityId = newPriorityId
            }
            );

            this.Reload();
            return newProjectId;
        }

        public int AddTask(string label, int newProjectId)
        {
            int newDataId = generateNewData(label)[0];
            int newPriorityId = getStandardPriorityId();

            DataBase.Task dbTask = new();
            IEnumerable<int> possibleTaskIds = dbTask.Get().Select(x => x.Id);
            int newTaskId = possibleTaskIds.Count() > 0 ? possibleTaskIds.Max() + 1 : 1;
            dbTask.Add(new DataBase.Task.TaskData
            {
                Id = newTaskId,
                DataId = newDataId,
                ProjectId = newProjectId,
                PriorityId = newPriorityId
            }
            );

            this.Reload();
            return newTaskId;
        }

        public int AddAppointmment(string label, int newProjectId)
        {
            int newDataId = generateNewData(label)[0];

            DataBase.Appointment dbAppointment = new();
            IEnumerable<int> possibleAppointmentIds = dbAppointment.Get().Select(x => x.Id);
            int newAppointmentId = possibleAppointmentIds.Count() > 0 ? possibleAppointmentIds.Max() + 1 : 1;
            dbAppointment.Add(new DataBase.Appointment.AppointmentData
            {
                Id = newAppointmentId,
                DataId = newDataId,
                ProjectId = newProjectId
            }
            );

            this.Reload();
            return newAppointmentId;
        }

        private int[] generateNewData(string label)
        {
            DataBase.Data dbData = new();
            int newDataId = dbData.Get().Select(x => x.Id).Max() + 1;
            dbData.Add(new DataBase.Data.DataOfData
            {
                Id = newDataId,
                Label = label,
                Created = DateTime.Now,
                Updated = DateTime.Now,
            });
            return [newDataId, dbData.Get().Where(x => x.Label == DataBaseStandards.R_NOPROJECT).Select(x => x.Id).ToArray()[0]];
        }

        private int getStandardPriorityId()
        {
            List<DataBase.Priority.PriorityData> dbPriority = new DataBase.Priority().Get();
            return dbPriority.Where(x => x.Label == "normal").Select(x => x.Id).ToArray().Count() != 1
                ? dbPriority.Select(x => x.Id).ToArray()[0]
                : dbPriority.Where(x => x.Label == "normal").Select(x => x.Id).ToArray()[0];
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

            public void Update(string? pLabel = null, string? pDescription = null)
            {
                if (pLabel == "") return;
                if (pLabel != null) Label = pLabel;
                if (pDescription != null) Description = pDescription;
                Updated = DateTime.Now;

                DataBase.Data dbData = new();
                dbData.Update(new DataBase.Data.DataOfData
                {
                    Id = Id,
                    Label = Label,
                    Description = Description,
                    Updated = Updated
                });
            }

            public void Finish(bool isTrue = true)
            {
                Finished = isTrue ? DateTime.Now : null;
                Updated = DateTime.Now;

                DataBase.Data dbData = new();
                dbData.Update(new DataBase.Data.DataOfData
                {
                    Id = Id,
                    Finished = Finished,
                    Updated = Updated
                });
            }

            public void setDelete(DateTime? pDeleteOn = null)
            {
                if (pDeleteOn != null && pDeleteOn < DateTime.Now) return;
                DeleteOn = pDeleteOn;
                Updated = DateTime.Now;

                DataBase.Data dbData = new();
                dbData.Update(new DataBase.Data.DataOfData
                {
                    Id = Id,
                    DeleteOn = DeleteOn,
                    Updated = Updated
                });
            }
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
