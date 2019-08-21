using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Receive
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            var factory = new ConnectionFactory() { HostName = "LocalHost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //Here the exchange type is fanout , 
                channel.ExchangeDeclare(exchange: "log", type: "fanout");

                // when to declare the queue no parameter is pass
                var queuename = channel.QueueDeclare().QueueName;

                // using exchane bind the queue 
                channel.QueueBind(queue: queuename,
                                    exchange: "log",
                                    routingKey: "");
                Console.WriteLine("waiting for a logs");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                     {
                         var contain = ea.Body;
                         var message = Encoding.UTF8.GetString(contain);
                         Console.WriteLine("[x] {0}", message);
                     };
                channel.BasicConsume(queue:queuename,
                                     autoAck:true,
                                     consumer:consumer
                    );
                Console.WriteLine("Press the Enter for exit.");
                Console.ReadLine();
                
            }
        }
    }
}
