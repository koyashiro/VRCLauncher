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
            InitializeComponent();

            var args = Environment.GetCommandLineArgs();
            if (args.Length < 2)
            {
                DataContext = new MainWindowViewModel();
            }
            else
            {
                DataContext = new MainWindowViewModel(args[1]);
            }
        }
    }
}
