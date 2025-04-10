using Axpo;
using Microsoft.Extensions.Logging;
using PowerTrader.Core.Interface;
using PowerTrader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Axpo.PowerService;

namespace PowerTrader.Core
{
    public class PowerTraderManager : IPowerTraderManager
    {
        public readonly IPowerService _powerService;
        public readonly ILogger _logger;
        public PowerTraderManager(IPowerService powerService, ILogger<PowerTraderManager> logger)
        {
            _powerService = powerService;
            _logger = logger;
        }

        public IEnumerable<PowerTrade> GenerateTradeData()
        {
            _logger.LogInformation("Logging");
            var trades = _powerService.GetTrades(DateTime.Today);
            return trades;
        }

        public PowerPositionDTO GetTraderData(IList<PowerTrade> trades, DateTime? processingData)
        {
            var flatList = ((trades ?? new List<PowerTrade>()) as List<PowerTrade>)
                .SelectMany(x => x.Periods.Select(
                    y => new HourVolume()
                    {
                        StartTimeInt = y.Period,
                        Volume = y.Volume
                    }
                    ));
            var x = from row in flatList
                    group row by row.StartTimeInt into g
                    select new HourVolume()
                    {
                        StartTimeInt = g.FirstOrDefault().StartTimeInt,
                        Volume = g.Sum(x => x.Volume)
                    };
            var y = new PowerPositionDTO()
            {
                HourlyVolume = x.ToList(),
                Date = processingData ?? DateTime.Now
            };
            return y;
        }

        public string GetCSVTraderData(PowerPositionDTO processedData)
        {
            var csv = new StringBuilder();
            //if(processedData?.HourlyVolume.Count)
            csv.AppendLine($"Local Time,Volume");
            foreach (var item in processedData?.HourlyVolume)
            {
                csv.AppendLine($"{item.StartTime},{item.Volume.ToString("#.##")}");
            }
            return csv.ToString();
        }
    }
}
