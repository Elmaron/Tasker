using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Classes.Templates;

namespace Tasker.Classes.Data.Conversion.Tables
{
    public class Task : ObservableObjectTemplate
    {
        private InternalPriority _selectedPriority;
        public InternalPriority SelectedPriority { get => _selectedPriority; set => _selectedPriority = value; }

        public Task(int pId, InternalData pData, InternalPriority pSelectedPriority) : base(pId, pData)
        {
            _selectedPriority = pSelectedPriority;
            System.Diagnostics.Debug.WriteLine($"Class -Appointment-; Found: {Data.Label}");
        }
    }
}
