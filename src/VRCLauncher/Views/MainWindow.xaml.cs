using System.Windows;
using VRCLauncher.Models;
using VRCLauncher.ViewModels;

namespace VRCLauncher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ILauncher launcher)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(launcher, new WindowWrapper(this));
        }
    }
}
