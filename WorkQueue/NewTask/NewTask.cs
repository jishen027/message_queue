using System.Text;
using RabbitMQ.Client;

// Create a connection to the RabbitMQ server
var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}

// Declare a queue, parameters are: queue name, durable, exclusive, autoDelete, arguments
channel.QueueDeclare(queue: "task_queue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var message = GetMessage(args);

// Convert the message to a byte array
var body = Encoding.UTF8.GetBytes(message);

var properties = channel.CreateBasicProperties();
properties.Persistent = true; // Make message persistent

// Publish the message to the queue, parameters are: exchange, routingKey, basicProperties, body
channel.BasicPublish(exchange: "",
                     routingKey: "task_queue",
                     basicProperties: properties,
                     body: body);

Console.WriteLine(" [x] Sent {0}", message);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();