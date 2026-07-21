using System;
using System.Collections.Generic;
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
        public InternalPriority SelectedPriority { get => _selectedPriority; set => Update(ref _selectedPriority, value); }
        public InternalDifficulty? SelectedDifficulty { get => _selectedDifficulty; set => Update(ref _selectedDifficulty, value); }

        public Task(int pId, InternalData pData, InternalPriority pSelectedPriority) : base(pId, pData)
        {
            _selectedPriority = pSelectedPriority;
            System.Diagnostics.Debug.WriteLine($"Class -Appointment-; Found: {Data.Label}");
        }

        private void Update<T>(ref T field, T value, [CallerMemberName] string fieldName = "")
        {
            field = value;
            new DataBase.Task().Update(new DataBase.Task.TaskData
            {
                Id = Id,
                PriorityId = _selectedPriority.Id,
                DifficultyId = _selectedDifficulty.Value.Id
            });
        }
    }
}
