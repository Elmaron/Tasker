using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Tasker.Classes.Data.Retrieval;
using Tasker.Classes.Templates;
using static Tasker.Classes.Data.Retrieval.DataBase.Task;
using static Tasker.Classes.Data.Conversion.DataStructure;

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

        public void CreateTask(object? pLabel)
        {
            if (pLabel == null || pLabel is not string newLabel || newLabel == "") return;
            int newDataId = CreateOrLoadData(newLabel);

            DataBase.Task dbTask = new();
            
            int newTaskId = dbTask.GenerateId();

            dbTask.Add(new DataBase.Task.TaskData
            {
                Id = newTaskId,
                DataId = newDataId,
                ProjectId = Id,
                PriorityId = _selectedPriority.Id
            });

            _tasks.Add(GetTask(new DataBase.Task.TaskData
            {
                Id = newTaskId,
                DataId = newDataId,
                ProjectId = Id,
                PriorityId = _selectedPriority.Id
            }));
        }

        public void CreateAppointment(object? pLabel)
        {
            if (pLabel == null || pLabel is not string newLabel || newLabel == "") return;
            int newDataId = CreateOrLoadData(newLabel);

            DataBase.Appointment dbAppointment = new();

            int newAppointmentId = dbAppointment.GenerateId();

            dbAppointment.Add(new DataBase.Appointment.AppointmentData
            {
                Id = newAppointmentId,
                DataId = newDataId,
                ProjectId = Id,
            });

            _appointments.Add(GetAppointment(new DataBase.Appointment.AppointmentData
            {
                Id = newAppointmentId,
                DataId = newDataId,
                ProjectId = Id,
            }));
        }

        public void DeleteTask(object? pId)
        {
            if (pId == null || pId is not int taskId || taskId <= 0) return;
            try
            {
                Task task = _tasks.Where(x => x.Id == taskId).ToArray()[0];
                if (_tasks.Where(x => x.Data.Id == task.Data.Id).Count() == 1) DeleteData(task.Data.Id);
                new DataBase.Task().Delete(taskId);
                _tasks.Remove(task);
            } catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Class -Project-; Critical Error while trying to delete task.\nMessage:{e}");
            }
        }

        public void DeleteAppointment(object? pId)
        {
            if (pId == null || pId is not int appointmentId || appointmentId <= 0) return;
            try
            {
                Appointment appointment = _appointments.Where(x => x.Id == appointmentId).ToArray()[0];
                if (_tasks.Where(x => x.Data.Id == appointment.Data.Id).Count() == 1) DeleteData(appointment.Data.Id);
                new DataBase.Appointment().Delete(appointment.Id);
                _appointments.Remove(appointment);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Class -Project-; Critical Error while trying to delete Appointment.\nMessage:{e}");
            }
        }

        private void LoadTasks(DataBase.Task.TaskData[] pTasks)
        {
            foreach (DataBase.Task.TaskData task in pTasks) _tasks.Add(GetTask(task));
        }

        private void LoadAppointments(DataBase.Appointment.AppointmentData[] pAppointments)
        {
            foreach (DataBase.Appointment.AppointmentData appointment in pAppointments) _appointments.Add(GetAppointment(appointment));
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
