using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenAI.Client;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace OpenAI_WinForms_Client
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            //CreateConfiguration(); //both variants seem to work ok

            Application.Run(ServiceProvider.GetRequiredService<Form1>());
        }
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfiguration Configuration { get; private set; }
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<Form1>();
                    services.AddHttpClient();
                    services.AddTransient<IOpenAIClient, OpenAIClient>(
                        serviceProvider => new OpenAIClient(
                        //configuration: serviceProvider.GetRequiredService<IConfiguration>(), //both variants seem to work ok
                        configuration: new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .Build(),
                        httpClientFactory: serviceProvider.GetRequiredService<IHttpClientFactory>(),
                        modelId: "44")
                        );
                });
        }
        static void CreateConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                            .Build();
        }
    }
}