using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Classes.Data.Retrieval;
using Tasker.Classes.Templates;

namespace Tasker.Classes.Data.Conversion.Tables
{
    public class Appointment : ObservableObjectTemplate
    {
        public Appointment(int pId, InternalData pData) : base(pId, pData)
        {
            System.Diagnostics.Debug.WriteLine($"Class -Appointment-; Found: {Data.Label}");
        }
    }
}
