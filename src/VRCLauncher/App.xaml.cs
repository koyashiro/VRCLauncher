using System.Windows;
using VRCLauncher.Utils;

namespace VRCLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            if (!Config.ExistConfigFile())
            {
                Config.Initialize();
            }
        }
    }
}
