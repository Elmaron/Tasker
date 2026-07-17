using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Tasker.ViewModels;

namespace Tasker.Classes
{
    //This class holds ObservableObjects, so they can be used in the ViewModels
    //They are only used to store specific data visible to the user and the Ids from the database to load different items
    public class DataView
    {
        public abstract class ObservableTemplate : ObservableObject
        {
            private int _id;
            private string _label = "";
            private object[] _data = new object[2];

            public int Id
            {
                get => (int)_data[0];
                set => SetProperty(ref _data[0], value);
            }

            public string Label
            {
                get => (string)_data[1];
                set => SetProperty(ref _data[1], value);
            }

            public object[] Data
            {
                get => _data;
                set => SetProperty(ref _data, value);
            }
        }

        public class Category : ObservableTemplate
        {
            
        }

        public class Project : ObservableTemplate
        {

        }

        public class Task : ObservableTemplate
        {

            private ObservableCollection<Priority> _priorites;
            public ObservableCollection<Priority> Priorities
            {
                get => _priorites;
                set => SetProperty(ref _priorites, value);
            }

            private Priority _selectedPriority;
            public Priority SelectedPriority {
                get => _selectedPriority;
                set
                {
                    if (_selectedPriority == value) return;
                    _selectedPriority = value;

                    OnPropertyChanged();
                }
            }

            public Task(DataStructure data, int pSelectedPriorityId) {
                _priorites = new ViewControl().LoadPriorites(data);
                foreach (Priority priority in _priorites)
                {
                    if (priority.Id != pSelectedPriorityId) continue;
                    _selectedPriority = priority;
                    break;
                }
            }
        }

        public class Priority : ObservableTemplate
        {
            private int _ordering;
            public int Ordering
            {
                get => _ordering;
                set => SetProperty(ref _ordering, value);
            }
        }
    }
}
