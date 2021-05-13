using Prism.Ioc;
using System;
using System.IO;
using System.Windows;
using VRCLauncher.Models;
using VRCLauncher.ViewModels;
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
            containerRegistry.RegisterScoped<IConfigService>(() => new ConfigService($"{Path.Join(AppDomain.CurrentDomain.BaseDirectory, "VRCLauncher.json")}"));
            containerRegistry.RegisterScoped<ILauncher, Launcher>();
            containerRegistry.RegisterForNavigation<IMainWindowViewModel, MainWindowViewModel>();
        }
    }
}
