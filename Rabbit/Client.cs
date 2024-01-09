using Microsoft.Extensions.DependencyInjection;
using RandomNameGeneratorLibrary;
using Services;
using SimpleRpc.Serialization.Hyperion;
using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Client;
using NLog;


class Client
{
    private readonly RabbitDesc rabbit = new RabbitDesc();
    private readonly Random rng = new Random();
    private readonly IWolfService _wolfService;

    Logger mLog = LogManager.GetCurrentClassLogger();

    public Client()
    {
        var httpClient = new HttpClient();
        _wolfService = new WolfServiceHttpAdapter(httpClient);
    }
	private void ConfigureLogging()
	{
		var config = new NLog.Config.LoggingConfiguration();

		var console =
			new NLog.Targets.ConsoleTarget("console")
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

    try
    {
        InitializeRabbit();

        while(true)
        {
            while(_wolfService.IsRabbitAlive(rabbit))
            {
                rabbit.DistanceToWolf = rng.Next(1, 100);
                _wolfService.UpdateRabbitDistanceToWolf(rabbit);
                mLog.Info($"The Rabbit is {rabbit.DistanceToWolf}m away");
                Thread.Sleep(3000);
            }

            mLog.Info("Rabbit has died RIP");
            Thread.Sleep(5000);
            InitializeRabbit();
        }
    }
    catch(Exception err)
    {
        mLog.Error($"Error has occurred: {err.Message}", err);
        Thread.Sleep(3000);
    }

}
    static void Main(string[] args)
    {
        var self = new Client();

        self.Run();
    }

    private void InitializeRabbit()
    {
        var personGenerator = new PersonNameGenerator();
        rabbit.RabbitName = personGenerator.GenerateRandomFirstAndLastName();
        rabbit.Weight = rng.Next(0, 10);
        rabbit.isRabbitAlive = true;
        rabbit.DistanceToWolf = 1000;
        rabbit.RabbitID = _wolfService.EnterWolfArea(rabbit);

        mLog.Info($"{rabbit.RabbitName} ({rabbit.Weight}) the Rabbit is born! #{rabbit.RabbitID}");
    }
}