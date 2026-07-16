using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Tasker.Classes;

namespace Tasker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    //Simple Data Bindings

    private string _newProjectPlaceholder = "New Projectname";
    public string NewProjectPlaceholder
    {
        get => _newProjectPlaceholder;
        set => SetProperty(ref _newProjectPlaceholder, value);
    }
    private string _newTaskPlaceholder = "New Taskname";
    public string NewTaskPlaceholder
    {
        get => _newTaskPlaceholder;
        set => SetProperty(ref _newTaskPlaceholder, value);
    }

    private string _newProjectName = "";
    public string NewProjectName
    {
        get => _newProjectName;
        set => SetProperty(ref _newProjectName, value);
    }

    private string _newTaskName = "";
    public string NewTaskName
    {
        get => _newTaskName;
        set => SetProperty(ref _newTaskName, value);
    }

    //Collection bindings
    private Classes.DataStructure _data = new();


    private ObservableCollection<Classes.DataView.Category> _categories;

    public ObservableCollection<Classes.DataView.Category> Categories
    {
        get => _categories;
        set => SetProperty(ref _categories, value);
    }

    private ObservableCollection<Classes.DataView.Project> _projects;

    public ObservableCollection<Classes.DataView.Project> Projects
    {
        get => _projects;
        set => SetProperty(ref _projects, value);
    }

    private ObservableCollection<Classes.DataView.Task> _tasks;

    public ObservableCollection<Classes.DataView.Task> Tasks
    {
        get => _tasks;
        set => SetProperty(ref _tasks, value);
    }

    //Handle selection logic
    private Classes.DataView.Category _selectedCategory;

    public Classes.DataView.Category SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if(_selectedCategory == value) return;
            _selectedCategory = value;

            OnPropertyChanged();

            Projects = new Classes.ViewControl().LoadProjects(_data, value);

            if (Projects.Count > 0) SelectedProject = Projects[0];
        }
    }

    private Classes.DataView.Project _selectedProject;

    public Classes.DataView.Project SelectedProject
    {
        get => _selectedProject;
        set
        {
            if (_selectedProject == value) return;
            _selectedProject = value;

            OnPropertyChanged();

            Tasks = new Classes.ViewControl().LoadTasks(_data, _selectedCategory, value);

        }
    }

    //Handle Button logic
    public void CreateNewTask()
    {
        int? checkPriorityId = new Classes.ViewControl().LoadPriorites(_data).Where(x => x.Label == "normal").ToArray()[0].Id;
        int newPriorityId = checkPriorityId != null ? checkPriorityId.Value : new Classes.ViewControl().LoadPriorites(_data)[0].Id;
        DataView.Task dvnewTask = new (
                _data,
                newPriorityId
            )
            {
                Id = -1,
                Label = _newTaskName
            };
        Tasks.Add(dvnewTask);
        NewTaskName = "";
    }

    public MainWindowViewModel()
    {
        Categories = new Classes.ViewControl().LoadCategories(_data);
        if(Categories.Count > 0) SelectedCategory = Categories[0];
    }
}
