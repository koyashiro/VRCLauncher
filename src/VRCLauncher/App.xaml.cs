using Prism.Ioc;
using System.Windows;
using VRCLauncher.Models;
using VRCLauncher.Views;

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
            containerRegistry.RegisterScoped<IFileWrapper, FileWrapper>();
            containerRegistry.RegisterScoped<IProcessWrapper, ProcessWrapper>();
            containerRegistry.RegisterScoped<IConfigService, ConfigService>();
            containerRegistry.RegisterScoped<ILauncher, Launcher>();
        }
    }
}
