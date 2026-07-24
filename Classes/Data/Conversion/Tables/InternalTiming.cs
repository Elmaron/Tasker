using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Tasker.Classes.Data.Retrieval;
using Tasker.Classes.Templates;

namespace Tasker.Classes.Data.Conversion.Tables
{
    public class InternalTiming(
        int pId,
        int pTypeId,
        DateTime? pStart,
        DateTime? pEnd) : ObservableTableTemplate(pId)
    {
        private readonly int _typeId = pTypeId;
        private DateTime _start = pStart ?? DateTime.Now;
        private DateTime _end = pEnd.HasValue && pEnd > pStart ? pEnd.Value : DateTime.Now;

        public string? Type
        {
            get => DataStructure.Types.Where(x => x.Id == _typeId).Select(x => x.Label).FirstOrDefault();
        }

        public DateTime Start
        {
            get => _start;
        }

        public DateTime? End
        {
            get => _end;
            set
            {
                if (!value.HasValue) return;
                if (value < _start) return;
                _end = value.Value;
                Update();
                OnPropertyChanged();
            }
        }

        public void Create()
        {
            new DataBase.Timing().Add(new DataBase.Timing.TimingData
            {
                Id = Id,
                TypeId = _typeId,
                Start = _start,
                End = _end,
            });
        }

        private void Update()
        {
            System.Diagnostics.Debug.WriteLine("Help ME");
            new DataBase.Timing().Update(new DataBase.Timing.TimingData
            {
                Id = Id,
                TypeId = _typeId,
                Start = _start,
                End = _end,
            });
        }
    }
}
