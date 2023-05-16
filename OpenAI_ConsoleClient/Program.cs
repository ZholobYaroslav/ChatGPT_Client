using System.Net.Http;
using System.Net;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using OpenAI.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConsoleClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
                services.AddTransient<IOpenAIClient, OpenAIClient>(
                    serviceProvider => new OpenAIClient(
                        //configuration: serviceProvider.GetRequiredService<IConfiguration>(), //both variants seem to work ok
                        configuration: new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .Build(),
                        httpClientFactory: serviceProvider.GetService<IHttpClientFactory>(),
                        modelId: "2")
                    );
            }).UseConsoleLifetime().Build();


            //var chatGPT = ActivatorUtilities.CreateInstance<OpenAIClient>(host.Services, "3"); //both variants seem to work ok
            var chatGPT = host.Services.GetService<IOpenAIClient>();

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