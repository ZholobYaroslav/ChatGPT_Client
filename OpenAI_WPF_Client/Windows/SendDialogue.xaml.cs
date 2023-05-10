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

namespace OpenAI_WPF_Client.Windows
{
    /// <summary>
    /// Interaction logic for SaveDialogue.xaml
    /// </summary>
    public partial class SendDialogue : Window
    {
        private MimePart _attachment;
        private RichTextBox _rtb;

        public SendDialogue(RichTextBox rtb)
        {
            InitializeComponent();
            _rtb = rtb; 
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
            string fromName = fromNameTextBox.Text;
            string toName = toNameTextBox.Text;
            string fromEmailAddress = fromEmailTextBox.Text;
            string toEmailAddress = toEmailTextBox.Text;

            string subject = subjectTextBox.Text;
            string body = bodyTextBox.Text;

            string appPasswordToken = appPasswordBox.Password.Trim();

            var email = new MimeMessage();
            var builder = new BodyBuilder();

            email.From.Add(new MailboxAddress(fromName, fromEmailAddress));
            email.To.Add(new MailboxAddress(toName, toEmailAddress));
            email.Subject = subject;

            builder.TextBody = body;
            builder.Attachments.Add(_attachment);

            email.Body = builder.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 465, true);

                smtp.Authenticate(fromEmailAddress, appPasswordToken);

                smtp.Send(email);
                MessageBox.Show("Email was sent!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                smtp.Disconnect(true);
            }
        }

        private void addAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
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
                            "Plain Text (*.txt)|*.txt",
                FilterIndex = 9
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _attachment = new MimePart("application", "octet-stream")
                {
                    Content = new MimeContent(File.OpenRead(openFileDialog.FileName)),
                    ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = openFileDialog.FileName
                };
            }
            sendEmailButton.IsEnabled = true;
        }

        private void saveAttachSendButton_Click(object sender, RoutedEventArgs e)
        {
            using (MemoryStream _stream = new MemoryStream())
            {
                var textRange = new TextRange(this._rtb.Document.ContentStart, this._rtb.Document.ContentEnd);
                textRange.Save(_stream, DataFormats.Rtf);
                _stream.Position = 0;

                DocumentModel.Load(_stream, LoadOptions.RtfDefault).Save("..//..//..//WPF_ChatGPT_Dialogue.pdf");

                _attachment = new MimePart("application", "octet-stream")
                {
                    Content = new MimeContent(File.OpenRead("..//..//..//WPF_ChatGPT_Dialogue.pdf")),
                    ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "WPF_ChatGPT_Dialogue_Sended.pdf"
                };
                sendButton_Click(sender, e);
            }
        }
    }
}
