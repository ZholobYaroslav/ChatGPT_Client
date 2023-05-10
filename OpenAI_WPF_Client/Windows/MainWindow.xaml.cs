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

namespace OpenAI_WPF_Client.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatGPT_Context chatGPT_Client;
        private ScenariosRepository scenariosRepository;
        public MainWindow()
        {
            InitializeComponent();

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
            scenariosRepository = new();
            scenariosComboBox.ItemsSource = scenariosRepository.Scenarios;// uses to display .ToString() of the item class => .Content
            richTextBox.Document.Blocks.Clear();
            sendButton.IsEnabled = false;
            richTextBox.IsEnabled = false;
            scenariosComboBox.IsEnabled = false;
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

            scenariosRepository = new();
            scenariosComboBox.ItemsSource = scenariosRepository.Scenarios;
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
            if(scenariosComboBox.SelectedItem is Scenario scenario)
            {
                richTextBox.AppendText(scenario.Description);
            }
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"Request sent at {DateTime.Now}");
            await chatGPT_Client.Invoke();

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
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", keyTextBox.Text, EnvironmentVariableTarget.User);
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
            ScenariosWindow scenarios = new ScenariosWindow(scenariosRepository, scenariosComboBox);
            scenarios.Owner = this;
            this.Hide();
            scenarios.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            scenarios.Show();
        }

        private void goToMalePageButton_Click(object sender, RoutedEventArgs e)
        {
            SendDialogue saveDialogue = new SendDialogue(this.richTextBox);
            saveDialogue.Owner = this;
            this.Hide();
            saveDialogue.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            saveDialogue.Show();
        }
        private void openFilebutton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                AddExtension = true,
                Filter =
                        "All Documents (*.docx;*.docm;*.doc;*.dotx;*.dotm;*.dot;*.htm;*.html;*.rtf;*.xml;*.txt)|*.docx;*.docm;*.dotx;*.dotm;*.doc;*.dot;*.htm;*.html;*.rtf;*.xml;*.txt|" +
                        "Word Documents (*.docx)|*.docx|" +
                        "Word Macro-Enabled Documents (*.docm)|*.docm|" +
                        "Word 97-2003 Documents (*.doc)|*.doc|" +
                        "Word Templates (*.dotx)|*.dotx|" +
                        "Word Macro-Enabled Templates (*.dotm)|*.dotm|" +
                        "Word 97-2003 Templates (*.dot)|*.dot|" +
                        "Web Pages (*.htm;*.html)|*.htm;*.html|" +
                        "PDF (*.pdf)|*.pdf|" +
                        "Rich Text Format (*.rtf)|*.rtf|" +
                        "Flat OPC (*.xml)|*.xml|" +
                        "Plain Text (*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                using (var stream = new MemoryStream())
                {
                    DocumentModel.Load(dialog.FileName).Save(stream, SaveOptions.RtfDefault);
                    stream.Position = 0;

                    richTextBox.Document.Blocks.Clear();
                    var textRange = new TextRange(this.richTextBox.Document.ContentStart, this.richTextBox.Document.ContentEnd);
                    textRange.Load(stream, DataFormats.Rtf);
                }
            }
        }

        private void saveAsbutton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                Filter =
                        "Word Document (*.docx)|*.docx|" +
                        "Word Macro-Enabled Document (*.docm)|*.docm|" +
                        "Word Template (*.dotx)|*.dotx|" +
                        "Word Macro-Enabled Template (*.dotm)|*.dotm|" +
                        "PDF (*.pdf)|*.pdf|" +
                        "XPS Document (*.xps)|*.xps|" +
                        "Web Page (*.htm;*.html)|*.htm;*.html|" +
                        "Single File Web Page (*.mht;*.mhtml)|*.mht;*.mhtml|" +
                        "Rich Text Format (*.rtf)|*.rtf|" +
                        "Flat OPC (*.xml)|*.xml|" +
                        "Plain Text (*.txt)|*.txt|" +
                        "Image (*.png;*.jpg;*.jpeg;*.gif;*.bmp;*.tif;*.tiff;*.wdp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp;*.tif;*.tiff;*.wdp"
            };

            if (dialog.ShowDialog(this) == true)
            {
                using (MemoryStream _stream = new MemoryStream())
                {
                    var textRange = new TextRange(this.richTextBox.Document.ContentStart, this.richTextBox.Document.ContentEnd);
                    textRange.Save(_stream, DataFormats.Rtf);
                    _stream.Position = 0;

                    DocumentModel.Load(_stream, LoadOptions.RtfDefault).Save(dialog.FileName);
                }
            }
        }

    }
}
