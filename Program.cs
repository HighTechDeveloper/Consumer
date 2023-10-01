using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
{
    using (var chanel = connection.CreateModel())
    {
        chanel.QueueDeclare(queue: "first",
            exclusive: false,
            durable: true,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(chanel);

        consumer.Received += (model, es) =>
        {
            var body = es.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
        };

        chanel.BasicConsume(queue: "first",
            autoAck: true,
            consumer: consumer);
        Console.ReadKey();

    }
}