using Microsoft.EntityFrameworkCore;
using Trabalho_Fruteira.Data;

namespace Trabalho_Fruteira
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(
                    builder.Configuration
                        .GetConnectionString("DefaultConnection"),

                    ServerVersion.AutoDetect(
                        builder.Configuration
                            .GetConnectionString("DefaultConnection")
                    )
                );
            });

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();


            // Permite que o Frontend JavaScript
            // faça requisições para a API.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }


            // CORS deve ser executado antes dos Controllers.
            app.UseCors("Frontend");


           

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}