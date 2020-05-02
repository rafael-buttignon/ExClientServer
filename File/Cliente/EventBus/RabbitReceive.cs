﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

public static class RabbitReceive
{
    public static void RabbitReceivedStart()
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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Recebido com sucesso! {0}", message);
            };
            channel.BasicConsume(queue: "eventbussEventPI",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}