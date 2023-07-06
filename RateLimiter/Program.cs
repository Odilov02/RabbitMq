
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using RateLimiter.DataAcces;
using RateLimiter.Services;

namespace RateLimiter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.



            builder.Services.AddHostedService<ListenQueue>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
         //   builder.Services.AddCors();
            builder.Services.AddRateLimiter(rateLimiterOptions =>
            rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
            {
                options.PermitLimit = 5;
                //options.QueueLimit = 5;
                options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                options.Window = TimeSpan.FromSeconds(5);
            }));

            builder.Services.AddRateLimiter(rateLimiterOptions =>
            rateLimiterOptions.AddSlidingWindowLimiter("sliding", options =>
            {
                options.QueueLimit = 5;
                options.PermitLimit = 10;
                options.SegmentsPerWindow = 2;
                options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                options.Window = TimeSpan.FromSeconds(10);
            }));
            builder.Services.AddRateLimiter(rateLimiterOptions =>
            rateLimiterOptions.AddTokenBucketLimiter("token", options =>
            {
                options.QueueLimit = 5;
                options.TokenLimit = 5;
                options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                options.AutoReplenishment = true;
                options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
                options.TokensPerPeriod = 2;
            }));
            builder.Services.AddRateLimiter(rateLimiterOptions =>
            rateLimiterOptions.AddConcurrencyLimiter("concurrency", options =>
            {
                options.QueueLimit = 5;
                options.PermitLimit = 10;
                options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            }));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRateLimiter();
            app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseCors(x =>
            //{
            //    x.WithMethods("GET");
            //    x.WithOrigins("https://microsoft.com");
            //  //  x.WithHeaders("microsoft");
            //});
            app.UseAuthorization();
            //app.UseCors(x =>
            //{
            //    x.WithMethods("GET","POST");
            //    x.WithOrigins("https://online.pdp.uz");
            //});
            app.MapControllers();

            app.Run();
        }
    }
}