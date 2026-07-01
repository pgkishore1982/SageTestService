using Serilog;

namespace SageTestService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    Database.SageDBAccess.OpenJob();
                   Log.Information("Sage Test running at: {time}", DateTimeOffset.Now);
                }
            //    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            //}
        }
    }
}
