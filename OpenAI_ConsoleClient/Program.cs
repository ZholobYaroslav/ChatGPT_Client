using System.Net.Http;
using System.Net;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Windows.Controls;
using OpenAI_WPF_Client.ChatGPT_API;

namespace ConsoleClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ChatGPT_Context chatGPT_Client = new ChatGPT_Context(new ChatGPT_Console());
            bool result;
            do
            {
                result = await chatGPT_Client.Invoke();
            } while (result) ;
            Console.WriteLine("Program End");
        }
    }
}