using RabbitMQ.Client;
using System.Text;
using System;

public static class RabbitSend
{
    public static void RabbitSendStart()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "eventbussEventPI",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string message = "\n [x] Todos os dados foram salvos!";
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                 routingKey: "eventbussEventPI",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine("\n [x] Event Bus Concluido!{0}", message);
        }
    }
}