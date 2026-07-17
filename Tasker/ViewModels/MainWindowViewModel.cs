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

    private Classes.DataStructure _data;

    public Classes.DataStructure Data
    {
        get => _data;
        set => SetProperty(ref _data, value);
    }

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
            Projects = new Classes.ViewControl().LoadProjects(Data, value);

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
            Tasks = new Classes.ViewControl().LoadTasks(Data, SelectedCategory, value);

            OnPropertyChanged();

        }
    }

    //Handle Button logic
    public void CreateNewCategory()
    {
        if(NewCategoryName == null) return;
        int newCategoryId = Data.AddCategory(NewCategoryName);
        NewCategoryName = "";

        Categories = new Classes.ViewControl().LoadCategories(Data);
        if (Categories.Count <= 0) return;
        SelectedCategory = Categories.Where(x => x.Id == newCategoryId).ToList()[0] ?? Categories[0];
    }

    public void CreateNewProject()
    {
        if(NewProjectName == null) return;
        int newProjectId = Data.AddProject(NewProjectName, SelectedCategory.Id);
        NewProjectName = "";

        Projects = new Classes.ViewControl().LoadProjects(Data, SelectedCategory);
        if (Projects.Count <= 0) return;
        SelectedProject = Projects.Where(x => x.Id == newProjectId).ToList()[0] ?? Projects[0];
    }

    public void CreateNewTask()
    {
        if (NewTaskName == null) return;
        int newTaskId = Data.AddTask(NewTaskName, SelectedProject.Id);
        NewTaskName = "";

        Tasks = new Classes.ViewControl().LoadTasks(Data, SelectedCategory, SelectedProject);
    }

    public void UpdateCategory(object? pId)
    {
        if(pId == null || pId is not int k) return;
        Data.categories.Where(x => x.Id == (int)pId).ToArray()[0].Update();
    }
    public void UpdateProject(object? pId)
    {
        if (pId == null || pId is not int k || SelectedCategory == null) return;
        Data.categories.Where(x => x.Id == SelectedCategory.Id).ToArray()[0]
            .projects.Where(x => x.Id == (int)pId).ToArray()[0]
            .Update();
    }
    public void UpdateTask(object? pData)
    {
        if (pData == null || pData is not object[] || SelectedCategory == null || SelectedProject == null) return;
        if (((object[])pData)[0] is not int || ((object[])pData)[1] is not string) return;
        Data.categories.Where(x => x.Id == SelectedCategory.Id).ToArray()[0]
            .projects.Where(x => x.Id == SelectedProject.Id).ToArray()[0]
            .tasks.Where(x => x.Id == (int)((object[])pData)[0]).ToArray()[0]
            .Update((string)((object[])pData)[1]);
    }
    //Move Projects, Tasks and Appointments somewhere else OR delete them (user decision)
    public void DeleteCategory(object? pId)
    {
        if (pId == null || pId is not int k) return;
        Data.categories.Where(x => x.Id == (int)pId).ToArray()[0].Delete();
    }
    //Move Tasks and Appointments somewhere else OR delete them (user decision)
    public void DeleteProject(object? pId)
    {
        if (pId == null || pId is not int k || SelectedCategory == null) return;
        Data.categories.Where(x => x.Id == SelectedCategory.Id).ToArray()[0]
            .projects.Where(x => x.Id == (int)pId).ToArray()[0]
            .Delete();
    }
    public void DeleteTask(object? pId)
    {
        if (pId == null || pId is not int k || SelectedCategory == null || SelectedProject == null) return;
        DataStructure.Category.Project project =
        Data.categories.Where(x => x.Id == SelectedCategory.Id).ToArray()[0]
            .projects.Where(x => x.Id == SelectedProject.Id).ToArray()[0];
        project.tasks.Where(x => x.Id == (int)pId).ToArray()[0].Delete();
        project.tasks.Remove(project.tasks.Where(x => x.Id == (int)pId).ToArray()[0]);

        Tasks = new Classes.ViewControl().LoadTasks(Data, SelectedCategory, SelectedProject);
    }


    public MainWindowViewModel()
    {
        //Create Database, if not available
        Tasker.Classes.DataBase.Initialize();

        //Load Data from database
        _data = new();

        //Create Collection of Categories
        Categories = new Classes.ViewControl().LoadCategories(Data);
        if(Categories.Count > 0) SelectedCategory = Categories[0];
    }
}
