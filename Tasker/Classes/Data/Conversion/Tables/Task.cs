using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public InternalPriority SelectedPriority { get => _selectedPriority; set => Update(ref _selectedPriority, value); }
        public InternalDifficulty? SelectedDifficulty { get => _selectedDifficulty; set => Update(ref _selectedDifficulty, value); }
        public static ObservableCollection<InternalPriority> AvailablePriorites { get => DataStructure.Priorities; set => DataStructure.Priorities = value; }
        public static ObservableCollection<InternalDifficulty> AvailableDifficulties { get => DataStructure.Difficulties; set => DataStructure.Difficulties = value; }
        public DateTime? Expiry { get => _expiry; set => Update(ref _expiry, value);  }

        public Task(int pId, InternalData pData, InternalPriority pSelectedPriority, InternalDifficulty? pSelectedDifficulty, DateTime? pExpiry) : base(pId, pData)
        {
            _selectedPriority = pSelectedPriority;
            _selectedDifficulty = pSelectedDifficulty;
            _expiry = pExpiry;
            System.Diagnostics.Debug.WriteLine($"Class -Task-; Found: {Data.Label}");
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
