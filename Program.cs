using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FadTask.Repositories;
using FadTask.Services;
using DotNetEnv;

namespace FadTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env");
            Env.Load(envPath);
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var key = Env.GetString("Jwt__Key", null)
                ?? throw new InvalidOperationException(
                    "JWT Key is not configured. Set Jwt__Key in the .env file.");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

            builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Create demo user
            var userRepo = app.Services.GetRequiredService<IUserRepository>();
            var demoEmail = Environment.GetEnvironmentVariable("DEMO_EMAIL");
            var demoPass = Environment.GetEnvironmentVariable("DEMO_PASSWORD");
            if (!string.IsNullOrEmpty(demoEmail) && !string.IsNullOrEmpty(demoPass))
            {
                if (userRepo.GetByEmail(demoEmail) == null)
                {
                    userRepo.Add(new Models.User { Email = demoEmail, Password = demoPass });
                }
            }



            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
