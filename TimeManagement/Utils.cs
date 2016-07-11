using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagement
{
    public static class Utils
    {
        #region Private methods

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWorkStation();

        #endregion

        public static void LockScreen()
        {
            if (!LockWorkStation())
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public static void Restart()
        {
            var tmp = System.IO.Path.GetTempFileName() + ".bat";
            System.IO.File.WriteAllText(tmp, string.Format(@"
echo starting timer
timeout /t 10 /nobreak
echo .
echo execute command
""{0}"" /silent", System.Reflection.Assembly.GetExecutingAssembly().Location));

            var process = new System.Diagnostics.Process();
            process.StartInfo = new System.Diagnostics.ProcessStartInfo(tmp)
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            };
            process.Start();
            Environment.Exit(1);
        }
    }
}
