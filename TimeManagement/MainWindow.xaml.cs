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
using TimeManagement.Configuration;

namespace TimeManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Scheduler _scheduler;

        private void Initialize()
        {
            try
            {
                Log.Append("Configured to obtain settings from {0}", txtPath.Text);
                var basePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var config = new ConfigurationManager(System.IO.Path.Combine(basePath, txtPath.Text));
                _scheduler = new Scheduler(config);
                Hide();
            }
            catch (Exception ex)
            {
                Log.Append("Initialization failed: {0}", ex.Message);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.IsSilent)
            {
                Initialize();
            }
        }
    }
}
