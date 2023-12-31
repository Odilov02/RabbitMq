﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace RateLimiter.Services
{
    public class MqService
    {
        public void GetRequest()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "10.10.1.181",
                Password = "Javlon",
                UserName = "Javlon"
            };
            using var connaction = factory.CreateConnection();
            var model = connaction.CreateModel();
            model.QueueDeclare("DiyorbekSendQueue", true, false, false);
            EventingBasicConsumer consumer = new(model);
            consumer.Received += (chanel, ea) =>
            {
                string body = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine(body);
                Console.WriteLine("Javob kiriting:");
                string result = Console.ReadLine()!;
                requestSend(result);
            };
        }
        public void requestSend(string message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "10.10.1.181",
                Password = "Javlon",
                UserName = "Javlon"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(exchange: "DiyorbekChatService",
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($" [x] Sent {message}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
