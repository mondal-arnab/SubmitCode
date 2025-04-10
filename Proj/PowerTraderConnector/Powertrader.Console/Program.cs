using Axpo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PowerTrader.Core;
using PowerTrader.Core.Interface;
using System;

namespace Powertrader.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello, World!");

            var host = CreateHost(args);

            Process(args, host);
        }

        private static void Process(string[] args, IHost host)
        {
            // Ask the service provider for the configuration abstraction.
            IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

            // Get values from the config given their key and their target type.
            var downloadPath = config.GetValue<string>("DownloadPath");

            var processor = host.Services.GetRequiredService<IPowerTraderManager>();
            var generatedData = processor.GenerateTradeData();
            var processedData = processor.GetTraderData(generatedData.ToList(), null);
            var csvContent = processor.GetCSVTraderData(processedData);

            var filename = $"PowerPosition_{DateTime.Now.ToString("yyyyMMdd_hhmm")}.csv" ;
            File.WriteAllText(Path.Combine(downloadPath, filename), csvContent);
        }

        static IHost CreateHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices)
                .ConfigureLogging((hostContext, logging) =>
                {
                    // Clear default providers
                    logging.ClearProviders();

                    // Add log4net
                    logging.AddLog4Net("log4net.config");
                })
                .Build();

        static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IPowerTraderManager, PowerTraderManager>()
                .AddSingleton<IPowerService, PowerService>();
        }
    }
}
