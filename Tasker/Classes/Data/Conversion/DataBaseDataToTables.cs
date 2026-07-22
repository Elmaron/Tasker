using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Tasker.Classes.Data.Conversion.Tables;
using Tasker.Classes.Data.Retrieval;

namespace Tasker.Classes.Data.Conversion
{
    public class DataBaseDataToTables
    {
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
