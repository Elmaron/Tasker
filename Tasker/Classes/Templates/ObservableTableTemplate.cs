using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Classes.Templates
{
    public abstract class ObservableTableTemplate : ObservableObject
    {
        private int _id;
        public int Id { get => _id; }

        public ObservableTableTemplate(int pId)
        {
            _id = pId;
        }
    }
}
