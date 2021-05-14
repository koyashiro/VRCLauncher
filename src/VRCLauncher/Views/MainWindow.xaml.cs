using System.Windows;
using VRCLauncher.Services;
using VRCLauncher.ViewModels;
using VRCLauncher.Wrappers;

namespace VRCLauncher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ILaunchService launcher)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(launcher, new WindowWrapper(this));
        }
    }
}
