using NLog;
using System;
using System.Net.Http;
using Services;

namespace Clients
{
    class Client
    {
        private readonly WaterDesc water = new WaterDesc();
        private readonly Random rng = new Random();
        Logger mLog = LogManager.GetCurrentClassLogger();

        private void ConfigureLogging()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var console = new NLog.Targets.ConsoleTarget("console")
            {
                Layout = @"${date:format=HH\:mm\:ss}|${level}| ${message} ${exception}"
            };
            config.AddTarget(console);
            config.AddRuleForAllLevels(console);
            LogManager.Configuration = config;
        }

        private void Run()
        {
            ConfigureLogging();

            HttpClient httpClient = new HttpClient();
            GrpcWolfServiceAdapter adapter = new GrpcWolfServiceAdapter(httpClient);

            while (true)
            {
                try
                {
                    InitializeWater(adapter);

                    while (true)
                    {
                        bool isWaterAlive = adapter.IsWaterAlive(water); // Assuming IsWaterAlive is synchronous
                        if (isWaterAlive)
                        {
                            mLog.Info("~~~~~~~~~~~~~~~~~");
                            Thread.Sleep(500);
                        }
                        else
                        {
                            mLog.Info("The water is empty");
                            Thread.Sleep(5000);
                            InitializeWater(adapter);
                        }
                    }
                }
                catch (Exception err)
                {
                    mLog.Error("Error has occurred...", err);
                    Thread.Sleep(3000);
                }
            }
        }

        private void InitializeWater(GrpcWolfServiceAdapter adapter)
        {
            water.Volume = rng.Next(0, 10);
            water.X = rng.Next(-50, 50);
            water.Y = rng.Next(-50, 50);
            water.WaterID = adapter.SpawnWaterNearWolf(water); // Assuming SpawnWaterNearWolf is synchronous
        }

        static void Main(string[] args)
        {
            var client = new Client();
            client.Run();
        }
    }
}
