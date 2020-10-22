using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = "Test Mesaj";
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var result = channel.QueueDeclare("FirstQueue", false, false, false, null);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: "FirstQueue", null, body: body);
                }
                Console.WriteLine("Mesajınız gönderildi!");
            }
            Console.ReadLine();
        }
    }
}
