using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            CreateConfiguration();

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