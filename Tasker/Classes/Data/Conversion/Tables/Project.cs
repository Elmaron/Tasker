using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Tasker.Classes.Data.Retrieval;
using Tasker.Classes.Templates;
using static Tasker.Classes.Data.Retrieval.DataBase.Task;

namespace Tasker.Classes.Data.Conversion.Tables
{
    public class Project : ObservableObjectTemplate
    {
        private InternalPriority _selectedPriority;
        private DateTime? _expiry;
        private ObservableCollection<Task> _tasks;
        private ObservableCollection<Appointment> _appointments;
        public InternalPriority SelectedPriority { get => _selectedPriority; set => Update(ref _selectedPriority, value); }
        public DateTime? Expiry { get => _expiry; set => Update(ref _expiry, value); }

        public ObservableCollection<Task> Tasks { get => _tasks; }
        public ObservableCollection<Appointment> appointments { get => _appointments; }

        public Project(int pId, InternalData pData, InternalPriority pSelectedPriority, DateTime? pExpiry) : base(pId, pData)
        {
            _selectedPriority = pSelectedPriority;
            _expiry = pExpiry;

            _tasks = [];
            _appointments = [];
        }
        public void Load(DataBase.Task.TaskData[]? pTasks = null, DataBase.Appointment.AppointmentData[]? pAppointments = null, bool pClear = true)
        {
            if (pClear) {
                _tasks = [];
                _appointments = [];
            }
            if (pTasks != null) LoadTasks(pTasks);
            if (pAppointments != null) LoadAppointments(pAppointments);
        }

        private void LoadTasks(DataBase.Task.TaskData[] pTasks)
        {
            foreach (DataBase.Task.TaskData task in pTasks) _tasks.Add(GetTask(task));
        }

        private void LoadAppointments(DataBase.Appointment.AppointmentData[] pAppointments)
        {
            foreach (DataBase.Appointment.AppointmentData appointment in pAppointments) _appointments.Add(GetAppointment(appointment));
        }

        private Task GetTask(DataBase.Task.TaskData pTaskData)
        {
            InternalData taskData = GetData(new DataBase.Data().Get().Where(x => x.Id == pTaskData.DataId).ToArray()[0]);
            InternalPriority taskPriority = GetPriority(new DataBase.Priority().Get().Where(x => x.Id == pTaskData.PriorityId).ToArray()[0]);
            InternalDifficulty? taskDifficulty = pTaskData.DifficultyId != null ? GetDifficulty(new DataBase.Difficulty().Get().Where(x => x.Id == pTaskData.DifficultyId).ToArray()[0]) : null;
            return new Task(pTaskData.Id, taskData, taskPriority, taskDifficulty, pTaskData.Expiry);
        }

        private Appointment GetAppointment(DataBase.Appointment.AppointmentData pAppointmentData)
        {
            InternalData appointmentData = GetData(new DataBase.Data().Get().Where(x => x.Id == pAppointmentData.DataId).ToArray()[0]);
            return new Appointment(pAppointmentData.Id, appointmentData);
        }

        private InternalData GetData(DataBase.Data.DataOfData pData)
        {
            return new InternalData(pData.Id, pData.Label, pData.Description, pData.Created, pData.Updated, pData.Finished, pData.DeleteOn);
        }

        private InternalPriority GetPriority(DataBase.Priority.PriorityData pPriorityData)
        {
            return new InternalPriority(pPriorityData.Id, pPriorityData.Label, pPriorityData.Ordering, pPriorityData.Color);
        }

        private InternalDifficulty GetDifficulty(DataBase.Difficulty.DifficultyData pDifficultyData)
        {
            return new InternalDifficulty(pDifficultyData.Id, pDifficultyData.Label, pDifficultyData.Description, pDifficultyData.Recommendation, pDifficultyData.Color);
        }

        private void Update<T>(ref T field, T value)
        {
            field = value;
            new DataBase.Project().Update(new DataBase.Project.ProjectData
            {
                Id = Id,
                PriorityId = _selectedPriority.Id,
                Expiry = _expiry.Value,
            });
        }
    }
}
