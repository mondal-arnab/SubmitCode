using Axpo;
using PowerTrader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrader.Core.Interface
{
    public interface IPowerTraderManager
    {
        IEnumerable<PowerTrade> GenerateTradeData();
        PowerPositionDTO GetTraderData(IList<PowerTrade> trades, DateTime? processingData);
        string GetCSVTraderData(PowerPositionDTO processedData);
    }
}
