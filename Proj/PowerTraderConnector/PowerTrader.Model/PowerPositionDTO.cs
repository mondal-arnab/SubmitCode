using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrader.Model
{
    public class PowerPositionDTO
    {
        public IList<HourVolume> HourlyVolume { get; set; } = new List<HourVolume>();
        public DateTime Date { get; set; }
    }

}
