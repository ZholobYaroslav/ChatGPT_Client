using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using OpenAI_WPF_Client.BusinessLogic;
using OpenAI_WPF_Client.ChatGPT_API;
using GemBox.Document;
using Microsoft.Win32;
using System.Globalization;
using System.Reflection.Metadata;
using OpenAI.Client;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using OpenAI_WPF_Client.BusinessLogic.Interfaces;

namespace OpenAI_WPF_Client.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatGPT_Context chatGPT_Client;

        //private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOpenAIClient _openAIClient;
        private IScenarioRepository _scenarioRepository;
        private IEmailOperations _emailOperations;
        private IFileOperations _fileOperations;
        public MainWindow(IOpenAIClient openAIClient, IScenarioRepository scenarioRepository, IEmailOperations emailOperations, IFileOperations fileOperations)
        {
            InitializeComponent();
            Console.OutputEncoding = Encoding.UTF8;
            this._fileOperations = fileOperations;
            this._fileOperations.Rtb = richTextBox;
            this._emailOperations = emailOperations;
            this._scenarioRepository = scenarioRepository;
            this._openAIClient = openAIClient;
            //this._openAIClient = new OpenAIClient(App.Configuration, _httpClientFactory, "2");

            App.LanguageChanged += LanguageChanged;
            CultureInfo currentLang = App.Language;

            menuInterfaceLanguage.Items.Clear();
            var langs = App.Languages;
            foreach (var lang in App.Languages)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = lang.DisplayName;
                menuItem.Tag = lang;
                menuItem.IsChecked = lang.Equals(currentLang);
                menuItem.Click += ChangeLanguageClick;
                menuInterfaceLanguage.Items.Add(menuItem);
            }

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            chatGPT_Client = new ChatGPT_Context(new ChatGPT_WPF_RichTextBox_localized(richTextBox));
            //scenariosRepository = new();
            scenariosComboBox.ItemsSource = _scenarioRepository.Scenarios;// uses to display .ToString() of the item class => .Content
            richTextBox.Document.Blocks.Clear();
            sendButton.IsEnabled = false;
            richTextBox.IsEnabled = false;
            scenariosComboBox.IsEnabled = false;
            this._scenarioRepository = scenarioRepository;
            _emailOperations = emailOperations;
        }

        private void LanguageChanged(object? sender, EventArgs e)
        {
            CultureInfo currentLang = App.Language;

            var items = menuInterfaceLanguage.Items;
            int count = items.Count;
            foreach (MenuItem item in menuInterfaceLanguage.Items)
            {
                CultureInfo ci = item.Tag as CultureInfo;
                item.IsChecked = ci != null && ci.Equals(currentLang);
            }

            //_scenarioRepository = new();
            _scenarioRepository.ScenarioDataSeeding();

            scenariosComboBox.ItemsSource = _scenarioRepository.Scenarios;

            (string, string) curLangTuple;
            string previousLang = string.Empty;
            if (App.Language.Name.Equals("uk-UA"))
            {
                curLangTuple = ("Користувач:", "Чат GPT:");
                previousLang = "User:";
            }
            else
            {
                curLangTuple = ("User:", "ChatGPT:");
                previousLang = "Користувач:";
            }

            string userText = new TextRange(richTextBox.Document.Blocks.LastBlock.ContentStart, richTextBox.Document.Blocks.LastBlock.ContentEnd).Text;

            int startIndexOfWordToDelete = userText.LastIndexOf(previousLang);
            //Console.WriteLine("start ind:"+startIndexOfWordToDelete);
            userText = userText.Remove(startIndexOfWordToDelete, previousLang.Length+1);
            //Console.WriteLine($"text after remove:|{userText
            userText += $"{curLangTuple.Item1} ";
            //Console.WriteLine($"text after insert correct word:|{userText}|");

            richTextBox.Document.Blocks.Remove(richTextBox.Document.Blocks.LastBlock);
            richTextBox.Document.Blocks.Add(new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run(userText)));

            richTextBox.CaretPosition = richTextBox.CaretPosition.DocumentEnd;
            Keyboard.Focus(richTextBox);
        }

        private void ChangeLanguageClick(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                CultureInfo lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
            }
        }

        private void scenariosComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (scenariosComboBox.SelectedItem is Scenario scenario)
            {
                richTextBox.AppendText(scenario.Description);
            }
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            //deprecated method invocation from "Template method" pattern 
            //await chatGPT_Client.Invoke();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Request sent at {DateTime.Now}");

            (string, string) user_or_chat_in_some_language = App.Language.Name.Equals("uk-UA") ? ("Користувач:", "Чат GPT:") : ("User:", "ChatGPT:");
            string userInput = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            string requestMsg = userInput.Split(new string[] { user_or_chat_in_some_language.Item1 }, StringSplitOptions.None).Last().Trim();

            Console.WriteLine("Request message:{\"" + requestMsg + "\"}");
            Console.ResetColor();

            string chatGPTResponse = await _openAIClient.SendMessageAsync(requestMsg);

            user_or_chat_in_some_language = App.Language.Name.Equals("uk-UA") ? ("Користувач:", "Чат GPT:") : ("User:", "ChatGPT:");
            System.Windows.Documents.Paragraph chatGPT_paragraph = new System.Windows.Documents.Paragraph(
                new System.Windows.Documents.Run($"{user_or_chat_in_some_language.Item2} {chatGPTResponse}"))
            { Foreground = Brushes.MediumBlue };
            chatGPT_paragraph.Margin = new System.Windows.Thickness(0, 0, 0, 7);
            richTextBox.Document.Blocks.Add(chatGPT_paragraph);
            richTextBox.Document.Blocks.Add(new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run($"{user_or_chat_in_some_language.Item1} ")));

            richTextBox.CaretPosition = richTextBox.CaretPosition.DocumentEnd;
            Keyboard.Focus(richTextBox);
        }

        private void richTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                sendButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void authorizeButton_Click(object sender, RoutedEventArgs e)
        {
            //Environment.SetEnvironmentVariable("OPENAI_API_KEY", keyTextBox.Text, EnvironmentVariableTarget.User);
            if (App.Language.Name.Equals("uk-UA"))
            {
                richTextBox.AppendText("Користувач: ");
            }
            else
            {
                richTextBox.AppendText("User: ");
            }
            sendButton.IsEnabled = true;
            richTextBox.IsEnabled = true;
            scenariosComboBox.IsEnabled = true;
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            keyTextBox.Clear();
        }

        private void scenariosButton_Click(object sender, RoutedEventArgs e)
        {
            ScenariosWindow scenarios = new ScenariosWindow(_scenarioRepository, scenariosComboBox);
            scenarios.Owner = this;
            this.Hide();
            scenarios.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            scenarios.Show();
        }

        private void goToEmailPageButton_Click(object sender, RoutedEventArgs e)
        {
            SendDialogue saveDialogue = new SendDialogue(this.richTextBox, _emailOperations);
            saveDialogue.Owner = this;
            this.Hide();
            saveDialogue.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            saveDialogue.Show();
        }
        private void openFilebutton_Click(object sender, RoutedEventArgs e)
        {
            _fileOperations.OpenFile();
        }

        private void saveAsbutton_Click(object sender, RoutedEventArgs e)
        {
            _fileOperations.SaveFileAs();
        }

    }
}
