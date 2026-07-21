using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Tasker.Classes.Data.Retrieval;

namespace Tasker.Classes.Data.Conversion.Tables
{
    public struct InternalData(
        int pId, 
        string pLabel, 
        string? pDescription, 
        DateTime? pCreated, 
        DateTime? pUpdated, 
        DateTime? pFinished, 
        DateTime? pDeleteOn)
    {
        private readonly int _id = pId;
        private string _label = pLabel;
        private string? _description = pDescription;
        private readonly DateTime _created = pCreated ?? DateTime.Now;
        private DateTime _updated = pUpdated ?? DateTime.Now;
        private DateTime? _finished = pFinished;
        private DateTime? _deleteOn = pDeleteOn;

        public readonly int Id { get => _id; }
        public string Label { 
            get => _label;
            set => Update(ref _label, value);
        }
        public string? Description { 
            get => _description; 
            set => Update(ref _description, value); 
        }
        public readonly DateTime Created { get => _created; }
        public readonly DateTime Updated { get => _updated; }
        public DateTime? Finished { 
            get => _finished;
            set {
                if (value != null && value > DateTime.Now) return;
                Update(ref _finished, value);
            }
        }
        public DateTime? DeleteOn { 
            get => _deleteOn;
            set {
                if (value != null && value < DateTime.Now) return;
                Update(ref _deleteOn, value);
            }
        }
        public void Finish(bool isTrue = true)
        {
            Finished = isTrue ? DateTime.Now : null;
            Update(ref _finished, Finished);
        }

        private void Update<T>(ref T? field, T? value, [CallerMemberName] string fieldName = "")
        {
            if (fieldName == null) { System.Diagnostics.Debug.WriteLine("Struct -InternalData-, Critical Error: Missing value name"); return; }
            if (fieldName == "_label" && value == null) { System.Diagnostics.Debug.WriteLine("Struct -InternalData-, Critical Error: Label is null"); return; }
            if (fieldName.Contains('_') && fieldName.Length > 2) fieldName = string.Concat(char.ToUpper(fieldName[1]), new string(fieldName.AsSpan(2)));

            field = value;
            _updated = DateTime.Now;

            new DataBase.Data().Update(_id, _updated, fieldName, value);
        }
    }
}
