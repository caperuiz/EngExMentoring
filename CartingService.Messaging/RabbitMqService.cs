using CartingService.Messaging;
using RabbitMQ.Client;
using System.Text;

public class RabbitMqService : IRabbitMqService
{
    private readonly ConnectionFactory _factory;

    public RabbitMqService()
    {
        _factory = new ConnectionFactory
        {
            HostName = "localhost", // Replace with your RabbitMQ server hostname
            Port = 5672,
            UserName = "guest",
            Password = "guest",
        };
    }

    public void PublishMessage(string message)
    {
        using (var connection = _factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            string queueName = "my-queue"; // Replace with your queue name

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                routingKey: queueName,
                                basicProperties: null,
                                body: body);

            Console.WriteLine($"Sent: {message}");
        }
    }
}
