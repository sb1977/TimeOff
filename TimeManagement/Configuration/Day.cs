using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagement.Configuration
{
    public class Day
    {
        public DayOfWeek Name { get; set; }
        public DateTime FromHour { get; set; }
        public DateTime ToHour { get; set; }

        public override string ToString()
        {
            return string.Format("{0} @ {1}->{2}", Name.ToString(), FromHour.ToString("HH:mm"), ToHour.ToString("HH:mm"));
        }
    }
}
