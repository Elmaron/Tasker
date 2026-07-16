using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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

            public int Id
            {
                get => _id;
                set => SetProperty(ref _id, value);
            }

            public string Label
            {
                get => _label;
                set => SetProperty(ref _label, value);
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
