using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VRCLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LaunchParameter _parameter;

        public MainWindow()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length < 2)
            {
                MessageBox.Show("Invalid parameter", "VRChatLauncher");
                Environment.Exit(1);
            }

            var parameterArg = args[1];
            if (!LaunchParameter.TryParse(parameterArg, out _parameter))
            {
                MessageBox.Show("Invalid parameter", "VRChatLauncher");
                Environment.Exit(1);
            }

            InitializeComponent();
        }
    }
}
