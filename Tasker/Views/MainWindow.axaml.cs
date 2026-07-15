using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;

namespace Tasker.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        //Create or/and Load Database
        Tasker.Classes.DataBase.Initialize();

        //*
        Tasker.Classes.DataBase.Data data = new ();
        Tasker.Classes.DataBase.Priority priority = new ();
        Tasker.Classes.DataBase.Category category = new();
        Tasker.Classes.DataBase.Project project = new ();

        foreach (Tasker.Classes.DataBase.Project.ProjectData proj in project.Get())
        {
            System.Diagnostics.Debug.WriteLine("Project found");
            Tasker.Classes.DataBase.Data.DataOfData dataOfProject = data.Get().Where(x => x.Id == proj.DataId).ToArray()[0];
            Button button = new ();
            button.Content = dataOfProject.Label;
            System.Diagnostics.Debug.WriteLine(dataOfProject.Label);

            stackpanel_projects.Children.Add(button);
        }//*/

    }
}