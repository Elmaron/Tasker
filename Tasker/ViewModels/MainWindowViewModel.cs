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

    private DataStructure _data = new();

    public DataStructure Data
    {
        get => _data;
        set => SetProperty(ref _data, value);
    }

    public ObservableCollection<Classes.Data.Conversion.Tables.Category> Categories => _data.categories;

    //Handle selection logic
    private Classes.Data.Conversion.Tables.Category _selectedCategory;
    private Classes.Data.Conversion.Tables.Project _selectedProject;


    public MainWindowViewModel()
    {
        //Load Data from database
        //_data = new();
    }
}
