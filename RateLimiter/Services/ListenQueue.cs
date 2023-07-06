namespace RateLimiter.Services
{
    public class ListenQueue : BackgroundService
    {
        private readonly MqService _mqService;

        public ListenQueue(MqService mqService)
        {
            _mqService = mqService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _mqService.GetRequest();
        }
    }
}
