using RabbitMQ.Client;
using System.Text;

// Create a connection to the RabbitMQ server
// Create a connection factory
var factory = new ConnectionFactory { HostName = "localhost" };
// Create a connection
using var connection = await factory.CreateConnectionAsync();
// Create a channel
using var channel = await connection.CreateChannelAsync();

// Declare a fanout exchange named "logs", exchange type is fanout meaning it will broadcast messages to all queues
await channel.ExchangeDeclareAsync(exchange: "logs", type: ExchangeType.Fanout);

var message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);

// Publish the message to the "logs" exchange with an empty routing key
await channel.BasicPublishAsync(exchange: "logs", routingKey: string.Empty, body: body);

Console.WriteLine($" [x] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

static string GetMessage(string[] args)
{
  return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
}