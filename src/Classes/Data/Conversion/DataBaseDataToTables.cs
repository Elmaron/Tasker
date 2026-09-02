using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tasker.Classes.Data.Conversion.Tables;
using Tasker.Classes.Data.Retrieval;
using static Tasker.Classes.Data.Retrieval.DataBase;

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
            ObservableCollection<InternalTiming> pTimings = [];
            foreach(DataBase.Timing.TimingData timing in new DataBase.Timing()
                .Get()
                .Where(timing =>
                    new DataBase.TimingInTask()
                        .Get()
                        .Where(timingInTask => timingInTask.TaskId == pTaskData.Id)
                        .Select(timingInTask => timingInTask.TimingId)
                        .Contains(timing.Id)
                        )
                .ToList())
            {
                pTimings.Add(GetTiming(timing));
            }
            return new Tables.Task(pTaskData.Id, taskData, taskPriority, taskDifficulty, pTaskData.Expiry, pTimings);
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

        public static InternalTiming GetTiming(DataBase.Timing.TimingData pTimingData)
        {
            return new InternalTiming(pTimingData.Id, pTimingData.TypeId, pTimingData.Start, pTimingData.End);
        }

        public static InternalPriority GetPriority(DataBase.Priority.PriorityData pPriorityData)
        {
            return new InternalPriority(pPriorityData.Id, pPriorityData.Label, pPriorityData.Ordering, pPriorityData.Color);
        }

        public static InternalDifficulty GetDifficulty(DataBase.Difficulty.DifficultyData pDifficultyData)
        {
            return new InternalDifficulty(pDifficultyData.Id, pDifficultyData.Label, pDifficultyData.Description, pDifficultyData.Recommendation, pDifficultyData.Color);
        }

        public static InternalType GetType(DataBase.Type.TypeData pTypeData)
        {
            return new InternalType(pTypeData.Id, pTypeData.Label);
        }
    }
}
