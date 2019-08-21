using RabbitMQ.Client;
using System;
using System.Text;

namespace publish_subscribe
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            var factory = new ConnectionFactory() { HostName = "Localhost" };
            using (var connection = factory.CreateConnection())
            using (var channle = connection.CreateModel())
            {

                channle.ExchangeDeclare(exchange: "log",type:"fanout" );
                var message = GetMessage(args);
                var contain = Encoding.UTF8.GetBytes(message.ToString());
                channle.BasicPublish(exchange:"log",
                                     routingKey:"",
                                     basicProperties:null,
                                     body:contain);
                Console.WriteLine("[x]sent[0]", message);
            }
            Console.WriteLine("Press Enter For Exit");
            Console.ReadLine();



        }

        private static object GetMessage(string[] args)
        {
            if (args.Length > 0)
            {
                return string.Join(" ", args);
            }
            return "Hiiii ";
            
        }
    }
}
