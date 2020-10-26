using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("TestQueue", durable: true, exclusive: false, autoDelete: false, null);
                    /*
                    *  Mesajların eşit dağılımı için derekli yapılandırmayı gerçekleştiriyoruz.
                    */
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume(queue: "TestQueue", autoAck: false, consumer: consumer);
                    consumer.Received += (model, argument) =>
                    {
                        var message = Encoding.UTF8.GetString(argument.Body.ToArray());
                        Console.WriteLine($"Mesaj Alındı:{message}");
                        Thread.Sleep(2000);
                        Console.WriteLine($"Mesaj işlendi!");
                        /*
                         * Mesaj başarılı bir şekilde işlendi.Yeni mesajı gönderebilirsin demiş olduk
                         */
                        channel.BasicAck(deliveryTag: argument.DeliveryTag, multiple: false);
                    };
                }
            }
            Console.ReadLine();
        }
    }
}
