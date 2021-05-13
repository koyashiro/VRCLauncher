using Prism.Ioc;
using System.Windows;
using VRCLauncher.Utils;
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
            if (!Config.ExistConfigFile())
            {
                Config.Initialize();
            }

            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
