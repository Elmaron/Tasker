using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Tasker.Classes.Data.Retrieval;
using Tasker.Classes.Templates;

namespace Tasker.Classes.Data.Conversion.Tables
{
    public class InternalData(int pId,
    string pLabel,
    string? pDescription,
    DateTime? pCreated,
    DateTime? pUpdated,
    DateTime? pFinished,
    DateTime? pDeleteOn) : ObservableTableTemplate(pId)
    {
        private string _label = pLabel;
        private string? _description = pDescription;
        private readonly DateTime _created = pCreated ?? DateTime.Now;
        private DateTime _updated = pUpdated ?? DateTime.Now;
        private DateTime? _finished = pFinished;
        private DateTime? _deleteOn = pDeleteOn;
        public string Label {
            get
            {
                if (_label == DataBaseStandards.R_NOCATEGORY) return "Uncategorized";
                if (_label == DataBaseStandards.R_NOPROJECT) return "Allgemein";
                return _label;
            }
            set => Update(ref _label, value);
        }
        public string? Description { 
            get => _description; 
            set => Update(ref _description, value); 
        }
        public DateTime Created { get => _created; }
        public DateTime Updated { get => _updated; }
        public DateTime? Finished { 
            get => _finished;
            set {
                System.Diagnostics.Debug.WriteLine($"Trying to set Finished Value to {value}");
                if (value != null && value > DateTime.Now) return;
                Update(ref _finished, value);
                System.Diagnostics.Debug.WriteLine($"Updated to {value}");
            }
        }
        public DateTime? DeleteOn { 
            get => _deleteOn;
            set {
                if (value != null && value < DateTime.Now) return;
                Update(ref _deleteOn, value);
            }
        }
        public bool IsFinishedInversed
        {
            get => !IsFinished;
        }
        public bool IsFinished
        {
            get => Finished != null;
            set => Finish(!IsFinished);
        }
        
        public void Finish(bool isTrue = true)
        {
            Finished = isTrue ? DateTime.Now : null;
        }

        private void Update<T>(ref T? field, T? value, [CallerMemberName] string fieldName = "")
        {
            if (fieldName == null) { System.Diagnostics.Debug.WriteLine("Struct -InternalData-, Critical Error: Missing value name"); return; }
            if (fieldName == "_label" && value == null) { System.Diagnostics.Debug.WriteLine("Struct -InternalData-, Critical Error: Label is null"); return; }
            if (fieldName.Contains('_') && fieldName.Length > 2) fieldName = string.Concat(char.ToUpper(fieldName[1]), new string(fieldName.AsSpan(2)));

            field = value;
            _updated = DateTime.Now;
            System.Diagnostics.Debug.WriteLine($"Changed {fieldName} to value {value}");

            new DataBase.Data().Update(Id, _updated, fieldName, value);
        }
    }
}
