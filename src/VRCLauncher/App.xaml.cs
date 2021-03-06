using Prism.Ioc;
using System.Windows;
using VRCLauncher.Services;
using VRCLauncher.Views;
using VRCLauncher.Wrappers;

namespace VRCLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterScoped<IDirectoryWrapper, DirectoryWrapper>();
            containerRegistry.RegisterScoped<IFileWrapper, FileWrapper>();
            containerRegistry.RegisterScoped<IEnvironmentWrapper, EnvironmentWrapper>();
            containerRegistry.RegisterScoped<IProcessWrapper, ProcessWrapper>();
            containerRegistry.RegisterScoped<IConfigService, ConfigService>();
            containerRegistry.RegisterScoped<ILaunchService, LaunchService>();
        }
    }
}
