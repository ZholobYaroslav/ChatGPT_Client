using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using OpenAI_WPF_Client.ChatGPT_API;

namespace OpenAI_WPF_Client.BusinessLogic
{
    internal class ChatGPT_WPF_RichTextBox_localized : ChatGPT
    {
        public RichTextBox MyRichTextBox { get; set; }
        private static (string, string) user_or_chat_in_some_language;
        public ChatGPT_WPF_RichTextBox_localized(RichTextBox richTextBox)
        {
            MyRichTextBox = richTextBox;
        }

        protected override string ReadUserInput()
        {
            user_or_chat_in_some_language = App.Language.Name.Equals("uk-UA") ? ("Користувач:", "Чат GPT:") : ("User:", "ChatGPT:");
            string userInput = new TextRange(MyRichTextBox.Document.ContentStart, MyRichTextBox.Document.ContentEnd).Text;
            return userInput.Split(new string[] { user_or_chat_in_some_language.Item1 }, StringSplitOptions.None).Last().Trim();
        }

        protected override void ResponseErrorOutput(HttpStatusCode statusCode)
        {
            MyRichTextBox.Document.Blocks.Add(new Paragraph(new Run($"{(int)statusCode} {statusCode}")));
        }

        protected override void ResponseEmptyChoicesOutput(string errorMessage)
        {
            MyRichTextBox.Document.Blocks.Add(new Paragraph(new Run(errorMessage)));
        }

        protected override void ChatGPT_Response(string response)
        {
            user_or_chat_in_some_language = App.Language.Name.Equals("uk-UA") ? ("Користувач:", "Чат GPT:") : ("User:", "ChatGPT:");
            Paragraph chatGPT_paragraph = new Paragraph(new Run($"{user_or_chat_in_some_language.Item2} {response}")) { Foreground = Brushes.MediumBlue };
            chatGPT_paragraph.Margin = new System.Windows.Thickness(0, 0, 0, 7);
            MyRichTextBox.Document.Blocks.Add(chatGPT_paragraph);
            MyRichTextBox.Document.Blocks.Add(new Paragraph(new Run($"{user_or_chat_in_some_language.Item1} ")));
        }
    }
}