using OpenAI_WPF_Client.ChatGPT_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ConsoleClient
{
    internal class ChatGPT_Console : ChatGPT
    {
        protected override string ReadUserInput()
        {
            Console.Write("User: ");
            return Console.ReadLine();
        }

        protected override void ResponseErrorOutput(HttpStatusCode statusCode)
        {
            Console.WriteLine($"{(int)statusCode} {statusCode}");
        }

        protected override void ResponseEmptyChoicesOutput(string errorMessage)
        {
            Console.WriteLine(errorMessage);
        }

        protected override void ChatGPT_Response(string response)
        {
            Console.WriteLine($"ChatGPT: {response}");
        }
    }
}