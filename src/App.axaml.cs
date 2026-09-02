using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using System.Linq;
using System.Threading.Tasks;
using Tasker.Classes.Data.Conversion;
using Tasker.Classes.Data.Retrieval;
using Tasker.ViewModels;
using Tasker.Views;
using Velopack;

namespace Tasker;

public partial class App : Application
{
    public override void Initialize()
    {
        //Avalonia
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        //Create Database for User data, if not available
        DataBase.Initialize();

        //Load Data from database
        DataStructure.Reload();

        //Window Initialization
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}