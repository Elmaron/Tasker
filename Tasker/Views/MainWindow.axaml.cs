using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using System.Linq;

namespace Tasker.Views;

public partial class MainWindow : Window
{
    //private Classes.DataStructure data;

    public MainWindow()
    {
        InitializeComponent();
        //data ??= new();
    }

    

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        //Create Database, if not available
        Tasker.Classes.DataBase.Initialize();

        /*
        //Load Data into a more usable structure
        data = new();

        //Load Category Items into ComboBox
        foreach(Classes.DataStructure.Category category in data.categories)
        {
            ComboBoxItem item = new();
            item.Name = category.Id.ToString();
            item.Content = category.data.Label.Contains("RESERVED_") ? "Allgemein" : category.data.Label;

            combobox_categories.Items.Add(item);
        }

        combobox_categories.SelectedIndex = 0;
        //*/

        /*
        foreach (Classes.DataStructure.Category category in data.categories)
            foreach (Classes.DataStructure.Category.Project project in category.projects)
            {
                System.Diagnostics.Debug.WriteLine($"Project {project.data.Label} found");
                Button button = new();
                button.Content = project.data.Label.Contains("RESERVED_") ? "Allgemein" : project.data.Label;

                stackpanel_projects.Children.Add(button);
            }
        //*/
    }   
}