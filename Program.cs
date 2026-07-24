using Avalonia;
using System;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;

namespace Tasker;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        //Update-Manager
        VelopackApp.Build().Run();

        //Avalonia-App
        BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
    }

    public static async Task UpdateApp()
    {
        var mgr = new UpdateManager(new GithubSource("https://github.com/Elmaron/Tasker", accessToken: null, prerelease: false));

        var newVersion = await mgr.CheckForUpdatesAsync();
        if (newVersion == null) return;

        await mgr.DownloadUpdatesAsync(newVersion);

        mgr.ApplyUpdatesAndRestart(newVersion);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();
}
