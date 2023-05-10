using OpenAI_WPF_Client.ChatGPT_API;

namespace OpenAI_WinForms_Client
{
    public partial class Form1 : Form
    {
        private ChatGPT_Context chatGPT_Client;
        public Form1()
        {
            InitializeComponent();
            chatGPT_Client = new ChatGPT_Context(new ChatGPT_WinFormsRichtTextBox(richTextBox1));
            this.richTextBox1.Text += "User: ";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Request sent at {DateTime.Now}");
            await chatGPT_Client.Invoke();
        }

    }
}