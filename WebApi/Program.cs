
using Domain.Models.Others;
using Infrastrructure.Helpers;
using Microsoft.Extensions.Options;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Ū�������ܼ�
            var environment = builder.Environment.EnvironmentName;  // �o�O .NET ���ت������ܼ�

            builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddYamlFile("appsettings.yaml", optional: true, reloadOnChange: true) // �[���q�{���t�m���
            .AddYamlFile($"appsettings.{environment}.yaml", optional: true, reloadOnChange: true); // �[�����үS�w���t�m���


            // �`�U�t�m�ú�ť�ܧ�
            builder.Services.Configure<AppSettings>(builder.Configuration);

            // �`�U��x�A�ȡA��ť�t�m�ܧ�ðO�����
            builder.Services.AddSingleton<IHostedService, AppSettingsMonitorService>();

            // Register logging and other services
            builder.Services.AddLogging();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
