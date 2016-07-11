using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagement
{
    public static class Log
    {
        private static DateTime? _lastLog = null;

        private static string GenerateLogFileName()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Environment.MachineName + ".log");
        }

        public static void Append(string format, params object[] arg)
        {
            try
            {
                var header = string.Format("[{0}] ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                var message = string.Format(header + format, arg) + Environment.NewLine;

                var fileName = GenerateLogFileName();
                if (File.Exists(fileName) &&
                    (!_lastLog.HasValue || (_lastLog.HasValue && _lastLog.Value < DateTime.UtcNow.AddHours(-24))))
                {
                    File.WriteAllText(fileName, string.Empty);
                }

                _lastLog = DateTime.UtcNow;
                File.AppendAllText(fileName, message);
            }
            catch
            {
            }
        }
    }
}
