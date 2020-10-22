using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://rqsnzfbj:5DYTnOwAMXKArn0bR0yAGxVpZm78mP2O@toad.rmq.cloudamqp.com/rqsnzfbj");
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("FirstQueue", false, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += Consumer_Received;
                    channel.BasicConsume(queue: "FirstQueue", autoAck: true, consumer: consumer);
                }
            }
            Console.ReadLine();
        }
        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine($"Mesaj Alındı:{message}");
        }
    }
}
