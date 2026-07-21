using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Tasker.Classes.Data.Retrieval;

namespace Tasker.Classes.Data.Conversion.Tables
{
    public struct InternalPriority(
        int pId,
        string pLabel,
        int pOrdering,
        string? pColor)
    {
        private const string _colorNotExisting = "hsla(0,0%,0%,0)";

        private readonly int _id = pId;
        private string _label = pLabel;
        private int _ordering = pOrdering;
        private string _color = pColor ?? _colorNotExisting;

        public readonly int Id { get => _id; }
        public string Label
        {
            get => _label;
            set => Update(ref _label, value);
        }
        public string? Color
        {
            get => _color;
            set => Update(ref _color, value ?? _colorNotExisting);
        }

        public int Ordering
        {
            get => _ordering;
            set => Update(ref _ordering, value);
        }

        private void Update<T>(ref T? field, T? value, [CallerMemberName] string fieldName = "")
        {
            if (fieldName == null) { System.Diagnostics.Debug.WriteLine("Struct -InternalPriority-, Critical Error: Missing value name"); return; }
            if (value == null) { System.Diagnostics.Debug.WriteLine("Struct -InternalPriority-, Critical Error: Fieldname is null"); return; }
            if (fieldName.Contains('_') && fieldName.Length > 2) fieldName = string.Concat(char.ToUpper(fieldName[1]), new string(fieldName.AsSpan(2)));

            field = value;

            new DataBase.Priority().Update(_id, fieldName, value);
        }
    }
}
