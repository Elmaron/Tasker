using Avalonia.Controls;
using Avalonia.Interactivity;

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
    }
}