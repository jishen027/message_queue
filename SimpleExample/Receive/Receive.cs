using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// Create a connection to the RabbitMQ server
var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declare a queue, parameters are: queue name, durable, exclusive, autoDelete, arguments
channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);


Console.WriteLine(" [*] Waiting for messages.");

// Create a consumer
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
  var body = ea.Body.ToArray();
  var message = Encoding.UTF8.GetString(body);
  Console.WriteLine(" [x] Received {0}", message);
};

channel.BasicConsume(queue: "hello",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();