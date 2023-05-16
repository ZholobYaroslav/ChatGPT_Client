using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MailKit.Net.Smtp;
using MailKit.Security;
using MailKit;
using MimeKit;
using GemBox.Document;
using System.Net.Mime;
using OpenAI_WPF_Client.BusinessLogic.Interfaces;
using OpenAI_WPF_Client.BusinessLogic;

namespace OpenAI_WPF_Client.Windows
{
    /// <summary>
    /// Interaction logic for SaveDialogue.xaml
    /// </summary>
    public partial class SendDialogue : Window
    {
        private IEmailOperations _emailOperations;
        public SendDialogue(RichTextBox rtb, IEmailOperations emailOperations)
        {
            InitializeComponent();
            appPasswordBox.Password = Environment.GetEnvironmentVariable("C#_appPassword", EnvironmentVariableTarget.User);
            _emailOperations = emailOperations;
            _emailOperations.Rtb = rtb;
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Owner.Show();
            sendEmailButton.IsEnabled = false;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.Show();
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            _emailOperations.SendEmail(InitializeEmailMessage());
        }

        private void addAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                AddExtension = true,
                Filter =
                "All Documents (*.docx;*.docm;*.doc;*.dotx;*.dotm;*.dot;*.htm;*.html;*.rtf;*.xml;*.txt)|" +
                "*.docx;*.docm;*.dotx;*.dotm;*.doc;*.dot;*.htm;*.html;*.rtf;*.xml;*.txt|" +
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
                "Plain Text (*.txt)|*.txt",
                FilterIndex = 9
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _emailOperations.AddAttachment(openFileDialog.FileName);
            }
            sendEmailButton.IsEnabled = true;
        }

        private void saveAttachSendButton_Click(object sender, RoutedEventArgs e)
        {
            _emailOperations.SaveAddAttachmentSend(InitializeEmailMessage());
        }
        private EmailMessage InitializeEmailMessage()
        {
            return new EmailMessage(fromNameTextBox.Text, fromEmailTextBox.Text, toNameTextBox.Text, toEmailTextBox.Text, subjectTextBox.Text,
                bodyTextBox.Text, appPasswordBox.Password.Trim());
        }
    }
}
