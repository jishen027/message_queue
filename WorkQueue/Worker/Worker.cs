using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// Create a connection to the RabbitMQ server
var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declare a queue, parameters are: queue name, durable, exclusive, autoDelete, arguments
channel.QueueDeclare(queue: "task_queue",
                     // We need to make sure that the queue won't be lost if RabbitMQ server crashes or quits, so we need to mark it as durable
                     // * Note that we need to declare the queue as durable both in the producer and the consumer and restart the producer and the consumer
                     // * if we change the queue to be durable or non-durable.
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);


Console.WriteLine(" [*] Waiting for messages.");

// Create a consumer
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
  var body = ea.Body.ToArray();
  var message = Encoding.UTF8.GetString(body);
  Console.WriteLine($" [x] Received {message}");

  int dots = message.Split('.').Length - 1;
  Thread.Sleep(dots * 1000);

  Console.WriteLine(" [x] Done");

  //manually send the acknowledgment from the worker, once we're done with a task
  channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
};

channel.BasicConsume(queue: "task_queue",
                     autoAck: false,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();