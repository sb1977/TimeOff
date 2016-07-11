using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TimeManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static System.IO.FileSystemWatcher _watcher = null;

        public static bool IsSilent { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (e.Args.Any(i => i.ToLower().Equals("/silent")))
            {
                IsSilent = true;
            }
            else
            {
                IsSilent = false;
            }

            if (_watcher == null)
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
                _watcher = new System.IO.FileSystemWatcher(
                    System.IO.Path.GetDirectoryName(assembly),
                    System.IO.Path.GetFileName(assembly));
                _watcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
                _watcher.EnableRaisingEvents = true;
                _watcher.Changed += OnAssemblyChanged;
            }
        }

        private void OnAssemblyChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            Log.Append("Assembly changed, restarting the process");
            Utils.Restart();
        }
    }
}
