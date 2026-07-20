using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tasker.ViewModels;

namespace Tasker.Classes
{
    //This class holds ObservableObjects, so they can be used in the ViewModels
    //They are only used to store specific data visible to the user and the Ids from the database to load different items
    public class DataView
    {
        //private DataStructure _data = new();

        public abstract class ObservableTemplate : ObservableObject
        {
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
            private bool _isFinished;
            private bool _isVisible_StartStopButton;

            public bool IsFinished
            {
                get => _isFinished;
                set { 
                    SetProperty(ref _isFinished, value); 
                    IsVisible_StartStopButton = !value; 
                }
            }

            public bool IsVisible_StartStopButton
            {
                get => _isVisible_StartStopButton;
                set => SetProperty(ref _isVisible_StartStopButton, value);
            }

            private static ObservableCollection<Difficulty> _difficulties;
            public static ObservableCollection<Difficulty> Difficulties
            {
                get => _difficulties;
                set => _difficulties =  value;
            }

            private static ObservableCollection<Priority> _priorities;
            public static ObservableCollection<Priority> Priorities
            {
                get => _priorities;
                set => _priorities = value;
            }

            private Difficulty _selectedDifficulty;
            public Difficulty SelectedDifficulty
            {
                get => _selectedDifficulty;
                set 
                {
                    if (_selectedDifficulty == value) return;
                    _selectedDifficulty = value;

                    OnPropertyChanged();
                }
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

            public Task(DataStructure data, int pSelectedPriorityId, int? pSelectedDifficultyId) {
                Priorities ??= new ViewControl().LoadPriorites(data);
                Difficulties ??= new ViewControl().LoadDifficulties(data);
                foreach (Priority priority in Priorities)
                {
                    if (priority.Id != pSelectedPriorityId) continue;
                    SelectedPriority = priority;
                    break;
                } 
                SelectedPriority ??= Priorities[0];
                SelectedDifficulty = Difficulties.Last();
                if (pSelectedDifficultyId == null) return;
                foreach (Difficulty difficulty in Difficulties)
                {
                    if (difficulty.Id != pSelectedDifficultyId) continue;
                    SelectedDifficulty = difficulty;
                    break;
                }

            }
        }

        public class Difficulty : ObservableTemplate
        {
            private string _description;
            private string _recommendation;

            public string Description
            {
                get => _description;
                set => SetProperty(ref _description, value);
            }

            public string Recommendation
            {
                get => _recommendation;
                set => SetProperty(ref _recommendation, value);
            }

            public string Tip
            {
                get
                {
                    return $"{Description}\nRecommendation: {Recommendation}";
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
