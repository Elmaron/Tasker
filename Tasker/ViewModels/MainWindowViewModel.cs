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

    private string _newCategoryPlaceholder = "New Categoryname";
    public string NewCategoryPlaceholder
    {
        get => _newCategoryPlaceholder;
        set => SetProperty(ref _newCategoryPlaceholder, value);
    }

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

    private string _newCategoryName = "";
    public string NewCategoryName
    {
        get => _newCategoryName;
        set => SetProperty(ref _newCategoryName, value);
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
    private Classes.DataStructure _data;


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

            if (_selectedCategory == null) return;
            Projects = new Classes.ViewControl().LoadProjects(_data, value);

            if (Projects.Count > 0) SelectedProject = Projects[0];

            OnPropertyChanged();
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

            if (_selectedProject == null) return;
            Tasks = new Classes.ViewControl().LoadTasks(_data, SelectedCategory, value);

            OnPropertyChanged();

        }
    }

    //Handle Button logic
    public void CreateNewCategory()
    {
        if(NewCategoryName == null) return;
        int newCategoryId = _data.AddCategory(NewCategoryName);

        Categories = new Classes.ViewControl().LoadCategories(_data);
        if (Categories.Count <= 0) return;
        SelectedCategory = Categories.Where(x => x.Id == newCategoryId).ToList()[0] ?? Categories[0];
    }

    public void CreateNewProject()
    {
        if(NewProjectName == null) return;
        int newProjectId = _data.AddProject(NewProjectName, SelectedCategory.Id);

        Projects = new Classes.ViewControl().LoadProjects(_data, SelectedCategory);
        if (Projects.Count <= 0) return;
        SelectedProject = Projects.Where(x => x.Id == newProjectId).ToList()[0] ?? Projects[0];
    }

    public void CreateNewTask()
    {
        if (NewTaskName == null) return;
        int newTaskId = _data.AddTask(NewTaskName, SelectedProject.Id);

        Tasks = new Classes.ViewControl().LoadTasks(_data, SelectedCategory, SelectedProject);
    }

    public MainWindowViewModel()
    {
        //Create Database, if not available
        Tasker.Classes.DataBase.Initialize();

        //Load Data from database
        _data = new();

        //Create Collection of Categories
        Categories = new Classes.ViewControl().LoadCategories(_data);
        if(Categories.Count > 0) SelectedCategory = Categories[0];
    }
}
