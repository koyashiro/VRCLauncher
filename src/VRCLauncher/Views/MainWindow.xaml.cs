using System;
using System.Windows;
using VRCLauncher.ViewModels;

namespace VRCLauncher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length < 2)
            {
                MessageBox.Show("Invalid parameter", "VRChatLauncher");
                Environment.Exit(1);
            }

            var uri = args[1];
            if (!LaunchParameter.TryParse(uri, out var launchParameter))
            {
                MessageBox.Show("Invalid parameter", "VRChatLauncher");
                Environment.Exit(1);
            }

            InitializeComponent();

            DataContext = new MainWindowViewModel(uri, launchParameter);
        }
    }
}
