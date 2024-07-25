using System.Text;
using RabbitMQ.Client;

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

const string message = "Hello World!";

// Convert the message to a byte array
var body = Encoding.UTF8.GetBytes(message);

// Publish the message to the queue, parameters are: exchange, routingKey, basicProperties, body
