using CartingService.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CatalogService.Messaging
{
    public class Worker : BackgroundService
    {
        private readonly IRabbitMqService _rabbitMqService;

        public Worker(IServiceProvider services)
        {
            _rabbitMqService = services.GetRequiredService<IRabbitMqService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_rabbitMqService.PublishMessage("New scheduled message");
                await Task.Delay(15000, stoppingToken);
            }
        }
    }
}
