using System;
using System.Text;

using RabbitMQ.Client;

namespace Sender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare("myqueue01", true, false, false, null);
            for (int i = 1; i < 100; i++)
            {
                string message = $"This is test message for my sender at{DateTime.Now.Ticks}";
                var body = Encoding.UTF8.GetBytes(message);
                var peroperties = channel.CreateBasicProperties();
                peroperties.Persistent = true; //save message in disk
                channel.BasicPublish("", "myqueue01", peroperties, body);
            }
            channel.Close();
            connection.Close();
            Console.ReadLine();
        }
    }
}
