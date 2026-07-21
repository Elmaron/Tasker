using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using Tasker.Classes.Data.Retrieval;
using Tasker.Classes.Data.Conversion.Tables;
using static Tasker.Classes.Data.Retrieval.DataBase;
using static Tasker.Classes.Data.Conversion.DataStructure;
using static Tasker.Classes.Data.Conversion.DataStructure.Category;

namespace Tasker.Classes.Data.Conversion
{
    //Handles Communication between DataBase and program
    public class DataStructure
    {
        public List<Category> categories;
        public List<InternalPriority> priorities;
        public List<InternalDifficulty> difficulties;

        public DataStructure() 
        {
            System.Diagnostics.Debug.WriteLine($"Initializing new class Datastructure...");
            Reload();
            categories ??= [];
            priorities ??= [];
            difficulties ??= [];
        }

        public void Reload()
        {
            categories = [];
            priorities = [];
            difficulties = [];
            DataBase.Priority dbPriority = new();
            foreach (DataBase.Priority.PriorityData priority in dbPriority.Get())
            {
                priorities.Add(GetPriority(priority));
            }
            DataBase.Difficulty dbDifficulty = new();
            foreach (DataBase.Difficulty.DifficultyData difficulty in dbDifficulty.Get())
            {
                difficulties.Add(GetDifficulty(difficulty));
            }
            DataBase.Category dbCategory = new();
            foreach (DataBase.Category.CategoryData category in dbCategory.Get())
            {
                categories.Add(new Category(category));
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
                DataBase.Data.DataOfData dbData = new DataBase.Data().Get().Where(x => x.Id == pDbCategory.DataId).ToArray()[0];
                DataBase.Priority.PriorityData dbPriority = new DataBase.Priority().Get().Where(x => x.Id == pDbCategory.PriorityId).ToArray()[0];

                Id = pDbCategory.Id;
                data = GetData(dbData);
                priority = GetPriority(dbPriority);

                System.Diagnostics.Debug.WriteLine($"Found Category: {data.Label}");

                DataBase.Project dbProject = new();
                foreach (DataBase.Project.ProjectData project in dbProject.Get())
                {
                    if (project.CategoryId != Id) continue;
                    projects.Add(new Project(project));
                }
            }

            public void Delete()
            {
                DataBase.Data dbData = new();
                DataBase.Category dbCategory = new();
                dbCategory.Delete(Id);
                dbData.Delete(data.Id);
            }

            public void Update(string? label = null, string? description = null, int? newPriorityId = null)
            {
                if (label != null) data.Label = label;
                if (description != null) data.Description = description;
                if (newPriorityId == null) return;
                DataBase.Priority.PriorityData[] newPriority = new DataBase.Priority().Get().Where(x => x.Id == newPriorityId).ToArray();
                if (newPriority.Count() != 1) return;
                priority = GetPriority(newPriority[0]);
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
                    DataBase.Data.DataOfData dbData = new DataBase.Data().Get().Where(x => x.Id == pDbProject.DataId).ToArray()[0];
                    DataBase.Priority.PriorityData dbPriority = new DataBase.Priority().Get().Where(x => x.Id == pDbProject.PriorityId).ToArray()[0];

                    Id = pDbProject.Id;
                    data = new(dbData.Id, dbData.Label, dbData.Description, dbData.Created, dbData.Updated, dbData.Finished, dbData.DeleteOn);
                    priority = GetPriority(dbPriority);
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
                public void Update(string? label = null, string? description = null, int? newPriorityId = null, DateTime? expiry = null)
                {
                    if (label != null) data.Label = label;
                    if (description != null) data.Description = description;
                    Expiry = expiry;
                    new DataBase.Project().Update(new DataBase.Project.ProjectData
                    {
                        Id = Id,
                        Expiry = Expiry
                    });
                    if (newPriorityId == null) return;
                    DataBase.Priority.PriorityData[] newPriority = new DataBase.Priority().Get().Where(x => x.Id == newPriorityId).ToArray();
                    if (newPriority.Count() != 1) return;
                    priority = GetPriority(newPriority[0]);
                }

                public void Delete()
                {
                    DataBase.Data dbData = new();
                    DataBase.Project dbProject = new();
                    dbProject.Delete(Id);
                    dbData.Delete(data.Id);
                }

                public class Appointment
                {
                    public int Id { get; }

                    public InternalData data;

                    public Appointment(DataBase.Appointment.AppointmentData pDbAppointment)
                    {
                        DataBase.Data.DataOfData dbData = new DataBase.Data().Get().Where(x => x.Id == pDbAppointment.DataId).ToArray()[0];

                        Id = pDbAppointment.Id;
                        data = new(dbData.Id, dbData.Label, dbData.Description, dbData.Created, dbData.Updated, dbData.Finished, dbData.DeleteOn);

                        System.Diagnostics.Debug.WriteLine($"Found Appointment: {data.Label}");
                    }

                    public void Update(string? label = null, string? description = null)
                    {
                        if (label != null) data.Label = label;
                        if (description != null) data.Description = description;
                    }
                    public void Delete()
                    {
                        DataBase.Data dbData = new();
                        DataBase.Appointment dbAppointment = new();
                        dbAppointment.Delete(Id);
                        dbData.Delete(data.Id);
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
                        DataBase.Data.DataOfData dbData = new DataBase.Data().Get().Where(x => x.Id == pDbTask.DataId).ToArray()[0];
                        DataBase.Priority.PriorityData dbPriority = new DataBase.Priority().Get().Where(x => x.Id == pDbTask.PriorityId).ToArray()[0];
                        DataBase.Difficulty dbDifficulty = new();

                        Id = pDbTask.Id;
                        data = new(dbData.Id, dbData.Label, dbData.Description, dbData.Created, dbData.Updated, dbData.Finished, dbData.DeleteOn);
                        priority = GetPriority(dbPriority);

                        //nullable Definition
                        DataBase.Difficulty.DifficultyData[] dbDifficulty_data = dbDifficulty.Get().Where(x => x.Id == pDbTask.DifficultyId).ToArray();
                        difficulty = dbDifficulty_data.Length <= 0 ? null : GetDifficulty(dbDifficulty_data[0]);
                        Expiry = pDbTask.Expiry;
                    }

                    public void Update(string? label = null, string? description = null, int? newPriorityId = null, int? difficultyId = null, DateTime? expiry = null)
                    {
                        if (label != null) data.Label = label;
                        if (description != null) data.Description = description;
                        if (difficultyId == null) difficulty = null;
                        else LoadDifficulty(difficultyId ?? 0);
                        Expiry = expiry;
                        new DataBase.Task().Update(new DataBase.Task.TaskData
                        {
                            Id = Id,
                            DifficultyId = difficultyId,
                            Expiry = Expiry
                        });
                        if (newPriorityId == null) return;
                        DataBase.Priority.PriorityData[] newPriority = new DataBase.Priority().Get().Where(x => x.Id == newPriorityId).ToArray();
                        if (newPriority.Count() != 1) return;
                        priority = GetPriority(newPriority[0]);
                    }
                    public void Delete()
                    {
                        DataBase.Data dbData = new();
                        DataBase.Task dbTask = new();
                        dbTask.Delete(Id);
                        dbData.Delete(data.Id);
                    }

                    private void LoadDifficulty(int pDifficultyId)
                    {
                        if (difficulty != null && difficulty.Value.Id == pDifficultyId) return;
                        DataBase.Difficulty.DifficultyData[] newDifficulty = new DataBase.Difficulty().Get().Where(x => x.Id == pDifficultyId).ToArray();
                        if (newDifficulty.Count() != 1) return;
                        difficulty = GetDifficulty(newDifficulty[0]);                   
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

        public static int CreateOrLoadData(string pLabel)
        {
            DataBase.Data dbData = new();
            if (dbData.Get().Where(x => x.Label == pLabel).Count() > 0) return dbData.Get().Where(x => x.Label == pLabel).ToArray()[0].Id;
            int newDataId = dbData.GenerateId();
            dbData.Add(new DataBase.Data.DataOfData
            {
                Id = newDataId,
                Label = pLabel
            });
            return newDataId;
        }

        public static void DeleteData(int pId)
        {
            try { new DataBase.Data().Delete(pId); } catch (Exception e) { System.Diagnostics.Debug.WriteLine($"DATABASE ERROR WHILE TRYING TO DELETE DATA:\n{e}"); }
        }

        public static Tables.Task GetTask(DataBase.Task.TaskData pTaskData)
        {
            InternalData taskData = GetData(new DataBase.Data().Get().Where(x => x.Id == pTaskData.DataId).ToArray()[0]);
            InternalPriority taskPriority = GetPriority(new DataBase.Priority().Get().Where(x => x.Id == pTaskData.PriorityId).ToArray()[0]);
            InternalDifficulty? taskDifficulty = pTaskData.DifficultyId != null ? GetDifficulty(new DataBase.Difficulty().Get().Where(x => x.Id == pTaskData.DifficultyId).ToArray()[0]) : null;
            return new Tables.Task(pTaskData.Id, taskData, taskPriority, taskDifficulty, pTaskData.Expiry);
        }

        public static Tables.Appointment GetAppointment(DataBase.Appointment.AppointmentData pAppointmentData)
        {
            InternalData appointmentData = GetData(new DataBase.Data().Get().Where(x => x.Id == pAppointmentData.DataId).ToArray()[0]);
            return new Tables.Appointment(pAppointmentData.Id, appointmentData);
        }

        public static InternalData GetData(DataBase.Data.DataOfData pData)
        {
            return new InternalData(pData.Id, pData.Label, pData.Description, pData.Created, pData.Updated, pData.Finished, pData.DeleteOn);
        }

        public static InternalPriority GetPriority(DataBase.Priority.PriorityData pPriorityData)
        {
            return new InternalPriority(pPriorityData.Id, pPriorityData.Label, pPriorityData.Ordering, pPriorityData.Color);
        }

        public static InternalDifficulty GetDifficulty(DataBase.Difficulty.DifficultyData pDifficultyData)
        {
            return new InternalDifficulty(pDifficultyData.Id, pDifficultyData.Label, pDifficultyData.Description, pDifficultyData.Recommendation, pDifficultyData.Color);
        }
    }
}
