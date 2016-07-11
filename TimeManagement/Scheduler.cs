using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagement.Configuration;

namespace TimeManagement
{
    public class Scheduler
    {
        #region Private data

        private ConfigurationManager _config;
        private System.Timers.Timer _timer;

        #endregion

        #region Constructors

        public Scheduler(ConfigurationManager config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            _config = config;
            _config.ReloadConfiguration();
            _timer = new System.Timers.Timer();
            var delta = 60 - DateTime.Now.Minute;
            Log.Append("Trigger timer in " + delta + " minutes");
            _timer.Interval = TimeSpan.FromMinutes(delta).TotalMilliseconds;
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = false;
            _timer.Start();

            Log.Append("Scheduler started");
        }

        #endregion

        #region Event handlers

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            #region Act upon timer

            _config.ReloadConfiguration();
            if (_config.IsEnabled)
            {
                Log.Append("Scheduler enabled");
                var now = DateTime.Now;
                var today = _config.Days.FirstOrDefault(i => i.Name == now.DayOfWeek);
                if (today != null &&
                    now.Hour >= today.FromHour.Hour &&
                    now.Hour < today.ToHour.Hour)
                {
                    try
                    {
                        Log.Append("Screen locked");
                        Utils.LockScreen();
                    }
                    catch (Exception ex)
                    {
                        Log.Append(string.Format("Unable to lock screen: {0}", ex.Message));
                    }
                }
                else
                {
                    Log.Append("Don't need to lock the screen according to configuration");
                }
            }
            else
            {
                Log.Append("Scheduler disabled");
            }

            #endregion

            // Restart the timer
            var interval = 60;
            Log.Append("Renew timer in " + interval + " minutes");
            _timer.Interval = TimeSpan.FromMinutes(interval).TotalMilliseconds;
            _timer.Start();
        }

        #endregion
    }
}
