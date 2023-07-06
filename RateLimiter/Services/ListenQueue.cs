using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Reflection;

namespace RateLimiter.Services
{
    public class ListenQueue : BackgroundService
    {
        private IConnection connaction;
        private IModel model;
        private readonly MqService _mqService;

        public ListenQueue(MqService mqService)
        {
            _mqService = mqService;
            var factory = new ConnectionFactory()
            {
                HostName = "10.10.1.181",
                Password = "Javlon",
                UserName = "Javlon"
            };
            connaction = factory.CreateConnection();
            model = connaction.CreateModel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           
          
           // model.QueueDeclare("DiyorbekSendQueue", true, false, false);
            EventingBasicConsumer consumer = new(model);
            model.BasicConsume("DiyorbekGetQueue",true, consumer);
            consumer.Received += (chanel, ea) =>
            {
                string body = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine(body);
                Console.WriteLine("Javob kiriting:");
                string result = Console.ReadLine()!;
                _mqService.requestSend(result);
            };
        }
    }
}
