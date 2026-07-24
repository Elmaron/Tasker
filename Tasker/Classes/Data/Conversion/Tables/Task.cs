using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Tasker.Classes.Data.Retrieval;
using Tasker.Classes.Templates;

namespace Tasker.Classes.Data.Conversion.Tables
{
    public class Task : ObservableObjectTemplate
    {
        private InternalPriority _selectedPriority;
        private InternalDifficulty? _selectedDifficulty;
        private DateTime? _expiry;
        private ObservableCollection<InternalTiming> _worktimes;
        private int? _active_Worktime;

        private TimeSpan _duration;
        private string _durationString;
        private string _buttonText_StartStopButton;

        public InternalPriority SelectedPriority { get => _selectedPriority; set => Update(ref _selectedPriority, value); }
        public InternalDifficulty? SelectedDifficulty { get => _selectedDifficulty; set => Update(ref _selectedDifficulty, value); }
        public static ObservableCollection<InternalPriority> AvailablePriorities { get => DataStructure.Priorities; set => DataStructure.Priorities = value; }
        public static ObservableCollection<InternalDifficulty> AvailableDifficulties { get => DataStructure.Difficulties; set => DataStructure.Difficulties = value; }

        public DateTime? Expiry { get => _expiry; set => Update(ref _expiry, value);  }

        public string DurationString
        {
            get => _durationString;
            set => SetProperty(ref _durationString, value);
        }

        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                SetProperty(ref _duration, value);
                DurationString = $"{value.Hours:D2}:{value.Minutes:D2}:{value.Seconds:D2}";
            }
        }

        public string ButtonText_StartStopButton
        {
            get => _buttonText_StartStopButton;
            set => SetProperty(ref _buttonText_StartStopButton, value);
        }

        public Task(int pId, InternalData pData, InternalPriority pSelectedPriority, InternalDifficulty? pSelectedDifficulty, DateTime? pExpiry, ObservableCollection<InternalTiming> pTimings) : base(pId, pData)
        {
            _selectedPriority = pSelectedPriority;
            _selectedDifficulty = pSelectedDifficulty;
            _expiry = pExpiry;
            _worktimes = [];
            foreach (InternalTiming timing in pTimings)
            {
                if (timing.Type == null) System.Diagnostics.Debug.WriteLine("Type is somehow null");
                if (timing.Type != DataBaseStandards.TYPE_WORKTIME) continue;
                ///////HIER GUCKEN NACH FEHLERN!!!
                _worktimes.Add(timing);
            }

            _buttonText_StartStopButton = "Start";
            UpdateDuration();
            _durationString ??= "";

            System.Diagnostics.Debug.WriteLine($"Class -Task-; Found: {Data.Label}");
        }

        public void ButtonCommand_StartStopFunction()
        {
            System.Diagnostics.Debug.WriteLine($"Class -Task-; Found Worktimes:\n{string.Join("\n", _worktimes.Select(x => $"{x.Start} | {x.End}"))}");
            if(!_active_Worktime.HasValue)
            {
                SetProperty(ref _active_Worktime, CreateWorktime());
                ButtonText_StartStopButton = "Stop";
                UpdateDuration();
                return;
            }
            _worktimes.Where(x => x.Id == _active_Worktime).First().End = DateTime.Now;
            SetProperty(ref _active_Worktime, null);
            ButtonText_StartStopButton = "Start";
            UpdateDuration();
            return;
        }

        private void UpdateDuration()
        {
            Duration = new(0);
            foreach (InternalTiming worktime in _worktimes)
            {
                TimeSpan? worktimeDuration = worktime.End - worktime.Start;
                if (worktimeDuration.HasValue) Duration = Duration.Add(worktimeDuration.Value);
            }
        }

        private int CreateWorktime()
        {
            DataBase.Timing.TimingData? worktime = CreateTiming(DataBaseStandards.TYPE_WORKTIME);
            if (worktime == null) return -1;
            _worktimes.Add(DataBaseDataToTables.GetTiming(worktime));
            return worktime.Id;
        }

        public DataBase.Timing.TimingData? CreateTiming(string pType, DateTime? pStart = null, DateTime? pEnd = null)
        {
            if (!DataStructure.Types.Select(x => x.Label).Contains(pType)) return null;
            System.Diagnostics.Debug.WriteLine($"Creating new Timing in task {Data.Label}");
            int typeId = DataStructure.Types.Where(x => x.Label == pType).Select(x => x.Id).First();

            DataBase.Timing dbTiming = new ();

            int newTimingId = dbTiming.GenerateId();

            DataBase.Timing.TimingData dbTimingData = new()
            {
                Id = newTimingId,
                TypeId = typeId,
                Start = pStart ?? DateTime.Now,
                End = pEnd ?? DateTime.Now
            };

            dbTiming.Add(dbTimingData);

            new DataBase.TimingInTask().Add(new DataBase.TimingInTask.TimingInTaskData
            {
                Id = new DataBase.TimingInTask().GenerateId(),
                TaskId = Id,
                TimingId = newTimingId,
            });
            return dbTimingData;
        }

        private void Update<T>(ref T field, T value)
        {
            field = value;
            new DataBase.Task().Update(new DataBase.Task.TaskData
            {
                Id = Id,
                PriorityId = _selectedPriority.Id,
                DifficultyId = _selectedDifficulty?.Id,
                Expiry = _expiry,
            });
        }
    }
}
