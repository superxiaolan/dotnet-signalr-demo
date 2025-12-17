using Scalar.AspNetCore;
using SignalRDemo.Api.Hubs;

namespace SignalRDemo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services
            builder.Services.AddControllers();
            builder.Services.AddSignalR();

            // 只用于生成 OpenAPI 描述
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            // 只暴露 OpenAPI JSON
            app.UseSwagger();

            app.MapControllers();
            app.MapHub<ChatHub>("/hub/chat");

            

            // OpenAPI + Scalar
            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference(options =>
                {
                    options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
                });
            }

            app.Run();
        }
    }
}
