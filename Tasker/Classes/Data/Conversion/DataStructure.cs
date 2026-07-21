using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using Tasker.Classes.Data.Conversion.Tables;
using Tasker.Classes.Data.Retrieval;
//using static Tasker.Classes.Data.Retrieval.DataBase;


namespace Tasker.Classes.Data.Conversion
{
    //Handles Visuals of the program
    public class DataStructure
    {
        public ObservableCollection<Tables.Category> categories;
        public ObservableCollection<InternalPriority> priorities;
        public ObservableCollection<InternalDifficulty> difficulties;

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
            foreach (DataBase.Priority.PriorityData priority in new DataBase.Priority().Get())
            {
                priorities.Add(GetPriority(priority));
            }
            foreach (DataBase.Difficulty.DifficultyData difficulty in new DataBase.Difficulty().Get())
            {
                difficulties.Add(GetDifficulty(difficulty));
            }
            List<DataBase.Project.ProjectData> dbProjects = new DataBase.Project().Get();
            List<DataBase.Task.TaskData> dbTasks = new DataBase.Task().Get();
            List<DataBase.Appointment.AppointmentData> dbAppointments = new DataBase.Appointment().Get();
            foreach (DataBase.Category.CategoryData category in new DataBase.Category().Get())
            {
                Tables.Category newCategory = GetCategory(category);

                List<DataBase.Project.ProjectData> projectsInCategory = dbProjects.Where(project => project.CategoryId == newCategory.Id).ToList();
                dbProjects = dbProjects.Except(projectsInCategory).ToList();

                newCategory.Load(projectsInCategory);
                foreach (Tables.Project project in newCategory.Projects)
                {
                    List<DataBase.Task.TaskData> tasksInProject = dbTasks.Where(task => task.ProjectId == project.Id).ToList();
                    dbTasks = dbTasks.Except(tasksInProject).ToList();

                    List<DataBase.Appointment.AppointmentData> appointmentsInTask = dbAppointments.Where(appointment => appointment.ProjectId == project.Id).ToList();
                    dbAppointments = dbAppointments.Except(appointmentsInTask).ToList();

                    project.Load(dbTasks, dbAppointments);
                }
                categories.Add(newCategory);
            }
        }

        private int getStandardPriorityId()
        {
            List<DataBase.Priority.PriorityData> dbPriority = new DataBase.Priority().Get();
            return dbPriority.Where(x => x.Label == "normal").Select(x => x.Id).ToArray().Count() != 1
                ? dbPriority.Select(x => x.Id).ToArray()[0]
                : dbPriority.Where(x => x.Label == "normal").Select(x => x.Id).ToArray()[0];
        }

        //NEU

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

        public static Tables.Category GetCategory(DataBase.Category.CategoryData pCategoryData)
        {
            InternalData categoryData = GetData(new DataBase.Data().Get().Where(x => x.Id == pCategoryData.DataId).ToArray()[0]);
            InternalPriority categoryPriority = GetPriority(new DataBase.Priority().Get().Where(x => x.Id == pCategoryData.PriorityId).ToArray()[0]);
            return new Tables.Category(pCategoryData.Id, categoryData, categoryPriority);
        }

        public static Tables.Project GetProject(DataBase.Project.ProjectData pProjectData)
        {
            InternalData projectData = GetData(new DataBase.Data().Get().Where(x => x.Id == pProjectData.DataId).ToArray()[0]);
            InternalPriority projectPriority = GetPriority(new DataBase.Priority().Get().Where(x => x.Id == pProjectData.PriorityId).ToArray()[0]);
            return new Tables.Project(pProjectData.Id, projectData, projectPriority, pProjectData.Expiry);
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
