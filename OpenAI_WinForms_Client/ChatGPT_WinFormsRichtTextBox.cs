using OpenAI_WPF_Client.ChatGPT_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace OpenAI_WinForms_Client
{
    internal class ChatGPT_WinFormsRichtTextBox : ChatGPT
    {
        public RichTextBox MyRichTextBox { get; set; }
        public ChatGPT_WinFormsRichtTextBox(RichTextBox richTextBox)
        {
            this.MyRichTextBox = richTextBox;
        }

        protected override string ReadUserInput()
        {
            string userInput = MyRichTextBox.Text;
            return userInput.Split(new string[] { "User:" }, StringSplitOptions.None).Last().Trim();
        }

        protected override void ResponseErrorOutput(HttpStatusCode statusCode)
        {
            MyRichTextBox.Text+= $"\n{(int)statusCode} {statusCode}";
        }

        protected override void ResponseEmptyChoicesOutput(string errorMessage)
        {
            MyRichTextBox.Text += "\n"+ errorMessage;
        }

        protected override void ChatGPT_Response(string response)
        {
            MyRichTextBox.Text += $"\nChatGPT: {response}\nUser: ";
        }
    }
}
