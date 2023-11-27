namespace BasketService.API
{
    public class MessageProcessor : BackgroundService
    {
        private readonly IServiceProvider _provider;

        public MessageProcessor(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _provider.CreateScope())
            {
                var consumer = scope.ServiceProvider.GetRequiredService<RabbitMQConsumer>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    consumer.ConsumeMessages();
                    await Task.Delay(1000, stoppingToken); // Adjust the delay as needed.
                }
            }
        }
    }

}
