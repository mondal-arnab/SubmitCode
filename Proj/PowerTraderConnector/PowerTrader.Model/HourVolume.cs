using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrader.Model
{
    public class HourVolume
    {
        public string StartTime
        {
            get
            {
                return $"{((StartTimeInt + 22) % 24):00}:00";
            }
        }
        public int StartTimeInt { get; set; }
        //public string EndTime { get; set; }
        public double Volume { get; set; }
    }
}
