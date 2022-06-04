using System;
using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.BasicQos(0,1,false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received+=(sender, args) => {

              var body=  args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
                channel.BasicAck(args.DeliveryTag,true);

            };
            channel.BasicConsume("myqueue01", true, consumer);
            Console.ReadLine();
           
        }
    }
}
