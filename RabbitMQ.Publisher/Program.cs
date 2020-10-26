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
                    channel.QueueDeclare("TestQueue", durable: true, exclusive: false, autoDelete: false, null);
                    
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    /*
                     * RabbitMQ instance restart yada çökme durumunda mesajların güvenliği için Persistent özelliğini true set ediyoruz.
                     */
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: "", routingKey: "TestQueue", properties, body: body);
                }
                Console.WriteLine("Mesajınız gönderildi!");
            }
            Console.ReadLine();
        }
    }
}
