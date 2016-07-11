using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TimeManagement.Configuration
{
    public class ConfigurationManager
    {
        #region Private data

        private string _configurationPath;

        #endregion

        #region Private methods

        private IEnumerable<Day> ParseSchedule(XmlDocument xml)
        {
            var days = xml.SelectNodes("/Settings/Schedule/Day");
            foreach (XmlNode day in days)
            {
                yield return new Day()
                {
                    Name = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day.Attributes["Name"].Value),
                    FromHour = DateTime.ParseExact(day.SelectSingleNode("./From").InnerText, "HH:mm", CultureInfo.InvariantCulture),
                    ToHour = DateTime.ParseExact(day.SelectSingleNode("./To").InnerText, "HH:mm", CultureInfo.InvariantCulture)
                };
            }
        }

        private bool ParseEnabled(XmlDocument xml)
        {
            return bool.Parse(xml.SelectSingleNode("/Settings/Enable").InnerText);
        }

        #endregion

        #region Constructors

        public ConfigurationManager(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path should not be empty");
            }
            _configurationPath = path;
            IsEnabled = false;
            Days = new Day[0];
        }

        #endregion

        #region Public properties

        public Day[] Days { get; private set; }
        public bool IsEnabled { get; private set; }

        #endregion

        #region Public methods

        public void ReloadConfiguration()
        {
            Log.Append("Read configuration from " + _configurationPath);
            try
            {
                if (File.Exists(_configurationPath))
                {
                    var xml = new XmlDocument();
                    xml.Load(_configurationPath);
                    Days = ParseSchedule(xml).ToArray();
                    IsEnabled = ParseEnabled(xml);

                    Log.Append(this.ToString());
                }
                else
                {
                    Log.Append("Configuration file doesn't exist");
                }
            }
            catch (Exception ex)
            {
                Log.Append("Unable to read configuration: " + ex.Message);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}",
                IsEnabled ? "Enabled" : "Disabled",
                string.Join(", ", Days.Select(i => i.ToString()).ToArray()));
        }

        #endregion
    }
}
