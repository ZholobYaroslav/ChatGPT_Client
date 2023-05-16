using OpenAI.Client;
using OpenAI_WPF_Client.ChatGPT_API;

namespace OpenAI_WinForms_Client
{
    public partial class Form1 : Form
    {
        private ChatGPT_Context chatGPT_Client;

        private IHttpClientFactory _httpClientFactory;
        private IOpenAIClient _openAIClient;
        public Form1(IHttpClientFactory httpClientFactory)
        {
            InitializeComponent();
            _httpClientFactory = httpClientFactory;

            _openAIClient = new OpenAIClient(Program.Configuration, _httpClientFactory, "4");

            this.richTextBox.Text += "User: ";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Request sent at {DateTime.Now}");

            string request = richTextBox.Text.Split(new string[] { "User:" }, StringSplitOptions.None).Last().Trim();
            string response = await _openAIClient.SendMessageAsync(request);
            richTextBox.Text += $"\nChatGPT: {response}\nUser: ";
        }

    }
}