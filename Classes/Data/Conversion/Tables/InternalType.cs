using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Classes.Templates;

namespace Tasker.Classes.Data.Conversion.Tables
{
    public class InternalType(
        int pId,
        string pLabel) : ObservableTableTemplate(pId)
    {
        private readonly string _label = pLabel;

        public string Label { get => _label; }
    }
}
