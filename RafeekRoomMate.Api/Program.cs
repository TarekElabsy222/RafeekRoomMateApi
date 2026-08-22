
using Microsoft.OpenApi;
using RafeekRoomMate.Application.DependencyInjections;
using RafeekRoomMate.Infrastructure.DependencyInjection;
using System.Reflection;

namespace RafeekRoomMate.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            #region register services
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Rafeek RoomMate API",
                    Version = "v1",
                    Description = "API for Rafeek RoomMate application made by Tarek Elabsy "
                });
            });
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            #endregion

            var app = builder.Build();
            await app.UseSeedRolesScopeAsync();
            app.UseSwagger();
            app.UseSwaggerUI();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
