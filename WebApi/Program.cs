using Domain.Models.Others;
using Infrastrructure.Helpers;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Ū�������ܼ�
            var environment = builder.Environment.EnvironmentName;  // �o�O .NET ���ت������ܼ�

            // ���U ConfigurationHelper
            builder.Services.AddSingleton<ConfigurationHelper>();

            // �ϥ� ConfigurationHelper �[�� YAML �t�m�ê�����^ AppSettings
            builder.Services.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ConfigurationHelper>>();
                var configurationHelper = new ConfigurationHelper(logger);

                // �ϥΪx�����J���w�t�m�]�Ҧp AppSettings�^
                return configurationHelper.LoadYamlConfiguration<AppSettings>(environment);
            });

            // ���U AppSettings �t�m�A���[���� YAML �t�m
            builder.Services.Configure<AppSettings>(builder.Configuration);

            // ���U��x�M��L�A��
            builder.Services.AddLogging();

            // ���U Web API ����ά����A��
            builder.Services.AddControllers();

            // ���U Swagger �]�w�]�ȶ}�o���ҡ^
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // �t�m HTTP �ШD�޹D
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
