using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Tasker.Classes.Data.Conversion;
using Tasker.Classes.Data.Retrieval;

namespace Tasker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    //Colors
    private const string color_primary_hue = "275";
    
    private const string color_secondary_hue = "242";
    
    private const string color_background_hue = "0";

    private const string color_difficulty_hue0 = "145";
    private const string color_difficulty_hue1 = "165";
    private const string color_difficulty_hue2 = "185";
    private const string color_difficulty_hue3 = "205";
    private const string color_difficulty_hue4 = "225";

    private const string color_priority_hue0 = "45";
    private const string color_priority_hue1 = "65";
    private const string color_priority_hue2 = "85";
    private const string color_priority_hue3 = "105";
    private const string color_priority_hue4 = "125";


    private const string color_background_saturation = "0";

    private const string color_generic_saturation0 = "60";
    private const string color_generic_saturation1 = "40";
    private const string color_generic_saturation2 = "30";
    private const string color_generic_saturation3 = "40";
    private const string color_generic_saturation4 = "50";


    private const string color_generic_lightness0 = "70";
    private const string color_generic_lightness1 = "60";
    private const string color_generic_lightness2 = "50";
    private const string color_generic_lightness3 = "40";
    private const string color_generic_lightness4 = "30";

    private const string color_background_lightness0 = "35";
    private const string color_background_lightness1 = "30";
    private const string color_background_lightness2 = "25";
    private const string color_background_lightness3 = "20";
    private const string color_background_lightness4 = "12";

    public static string ColorPrimary0 => $"hsl({color_primary_hue},{color_generic_saturation0}%,{color_generic_lightness0}%)";
    public static string ColorPrimary1 => $"hsl({color_primary_hue},{color_generic_saturation1}%,{color_generic_lightness1}%)";
    public static string ColorPrimary2 => $"hsl({color_primary_hue},{color_generic_saturation2}%,{color_generic_lightness2}%)";
    public static string ColorPrimary3 => $"hsl({color_primary_hue},{color_generic_saturation3}%,{color_generic_lightness3}%)";
    public static string ColorPrimary4 => $"hsl({color_primary_hue},{color_generic_saturation4}%,{color_generic_lightness4}%)";

    public static string ColorSecondary0 => $"hsl({color_secondary_hue},{color_generic_saturation0}%,{color_generic_lightness0}%)";
    public static string ColorSecondary1 => $"hsl({color_secondary_hue},{color_generic_saturation1}%,{color_generic_lightness1}%)";
    public static string ColorSecondary2 => $"hsl({color_secondary_hue},{color_generic_saturation2}%,{color_generic_lightness2}%)";
    public static string ColorSecondary3 => $"hsl({color_secondary_hue},{color_generic_saturation3}%,{color_generic_lightness3}%)";
    public static string ColorSecondary4 => $"hsl({color_secondary_hue},{color_generic_saturation4}%,{color_generic_lightness4}%)";

    public static string ColorBackground0 => $"hsl({color_background_hue},{color_background_saturation}%,{color_background_lightness0}%)";
    public static string ColorBackground1 => $"hsl({color_background_hue},{color_background_saturation}%,{color_background_lightness1}%)";
    public static string ColorBackground2 => $"hsl({color_background_hue},{color_background_saturation}%,{color_background_lightness2}%)";
    public static string ColorBackground3 => $"hsl({color_background_hue},{color_background_saturation}%,{color_background_lightness3}%)";
    public static string ColorBackground4 => $"hsl({color_background_hue},{color_background_saturation}%,{color_background_lightness4}%)";

    public static string ColorDifficulty0 => $"hsl({color_difficulty_hue0},{color_generic_saturation0}%,{color_generic_lightness0}%)";
    public static string ColorDifficulty1 => $"hsl({color_difficulty_hue1},{color_generic_saturation1}%,{color_generic_lightness1}%)";
    public static string ColorDifficulty2 => $"hsl({color_difficulty_hue2},{color_generic_saturation2}%,{color_generic_lightness2}%)";
    public static string ColorDifficulty3 => $"hsl({color_difficulty_hue3},{color_generic_saturation3}%,{color_generic_lightness3}%)";
    public static string ColorDifficulty4 => $"hsl({color_difficulty_hue4},{color_generic_saturation4}%,{color_generic_lightness4}%)";

    public static string ColorPriority0 => $"hsl({color_priority_hue0},{color_generic_saturation0}%,{color_generic_lightness0}%)";
    public static string ColorPriority1 => $"hsl({color_priority_hue1},{color_generic_saturation1}%,{color_generic_lightness1}%)";
    public static string ColorPriority2 => $"hsl({color_priority_hue2},{color_generic_saturation2}%,{color_generic_lightness2}%)";
    public static string ColorPriority3 => $"hsl({color_priority_hue3},{color_generic_saturation3}%,{color_generic_lightness3}%)";
    public static string ColorPriority4 => $"hsl({color_priority_hue4},{color_generic_saturation4}%,{color_generic_lightness4}%)";

    //TextFields and Buttons

    private const string _deleteButtonText = "Delete";
    public string DeleteButtonText { get => _deleteButtonText; }

    private const string _newCategoryPlaceholder = "New Category...";
    public string NewCategoryPlaceholder { get => _newCategoryPlaceholder; }

    private const string _newProjectPlaceholder = "New Project...";
    public string NewProjectPlaceholder { get => _newProjectPlaceholder; }

    private const string _newTaskPlaceholder = "New Task...";
    public string NewTaskPlaceholder { get => _newTaskPlaceholder; }

    //Editable Textfields

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

    //Data

    private DataStructure _data;

    public DataStructure Data
    {
        get => _data;
        set => SetProperty(ref _data, value);
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
        Data.categories.Remove(Data.categories.Where(x => x.Id == (int)pId).ToArray()[0]);

        Categories = new Classes.ViewControl().LoadCategories(Data);
    }
    //Move Tasks and Appointments somewhere else OR delete them (user decision)
    public void DeleteProject(object? pId)
    {
        if (pId == null || pId is not int k || SelectedCategory == null) return;
        DataStructure.Category category =
        Data.categories.Where(x => x.Id == SelectedCategory.Id).ToArray()[0];
        category.projects.Where(x => x.Id == (int)pId).ToArray()[0].Delete();
        category.projects.Remove(category.projects.Where(x => x.Id == (int)pId).ToArray()[0]);

        Projects = new Classes.ViewControl().LoadProjects(Data, SelectedCategory);
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
        DataBase.Initialize();

        //Load Data from database
        _data = new();

        //Create Collection of Categories
        Categories = new Classes.ViewControl().LoadCategories(Data);
        if(Categories.Count > 0) SelectedCategory = Categories[0];
    }
}
