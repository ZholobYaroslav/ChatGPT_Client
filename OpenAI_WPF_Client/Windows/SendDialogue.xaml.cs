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
            _emailOperations.SendEmail(fromNameTextBox.Text, fromEmailTextBox.Text, toNameTextBox.Text, toEmailTextBox.Text, subjectTextBox.Text,
                bodyTextBox.Text, appPasswordBox.Password.Trim());
        }

        private void addAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            _emailOperations.AddAttachment();
            sendEmailButton.IsEnabled = true;
        }

        private void saveAttachSendButton_Click(object sender, RoutedEventArgs e)
        {
            _emailOperations.SaveAddAttachmentSend(fromNameTextBox.Text, fromEmailTextBox.Text, toNameTextBox.Text, toEmailTextBox.Text, subjectTextBox.Text,
                bodyTextBox.Text, appPasswordBox.Password.Trim());
        }
    }
}
