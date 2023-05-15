using System.Net.Http;
using System.Net;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using OpenAI.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
                services.AddTransient<IOpenAIClient, OpenAIClient>();
            }).UseConsoleLifetime();

            var host = builder.Build();

            IConfiguration config = new ConfigurationBuilder()
                                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                            .Build();

            var chatGPT = ActivatorUtilities.CreateInstance<OpenAIClient>(host.Services, config, "2");

            bool continueChat = true;
            do
            {
                Console.Write("User: ");
                string response = await chatGPT.SendMessageAsync(Console.ReadLine());
                await Console.Out.WriteLineAsync("ChatGPT:" + response);
                await Console.Out.WriteLineAsync("Continue chat session?: Yes - \'1\'. No - press any button");
                continueChat = await Console.In.ReadLineAsync() == "1" ? true : false;
            } while (continueChat);

        }
    }
}