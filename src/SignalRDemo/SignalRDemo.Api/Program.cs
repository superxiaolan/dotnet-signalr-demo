using Scalar.AspNetCore;
using SignalRDemo.Api.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            // 配置 CORS（支持 SignalR 的 credentials 模式）
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.SetIsOriginAllowed(_ => true) // 允许任何来源，包括 null (file://)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials(); // SignalR 需要凭据支持
                });
            });

            // 只用于生成 OpenAPI 描述
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var jwtSection = builder.Configuration.GetSection("Jwt");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSection["Issuer"],
                        ValidAudience = jwtSection["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSection["SecretKey"]!))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // SignalR WebSocket 连接无法使用 Header
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/hub"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };

                });

            var app = builder.Build();

            app.UseHttpsRedirection();

            // 启用 CORS（必须在认证授权之前）
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            // 只暴露 OpenAPI JSON
            app.UseSwagger();

            // OpenAPI + Scalar
            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference(options =>
                {
                    options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
                });
            }

            app.MapControllers();
            app.MapHub<ChatHub>("/hub/chat");

            app.Run();
        }
    }
}
