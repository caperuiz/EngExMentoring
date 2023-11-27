namespace CartingService.Messaging
{
    public interface IRabbitMqService
    {
        void PublishMessage(string message);
    }
}
