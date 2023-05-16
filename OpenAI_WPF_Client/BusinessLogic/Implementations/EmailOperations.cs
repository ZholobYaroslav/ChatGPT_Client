using Microsoft.Win32;
using MimeKit;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using GemBox.Document;
using System.Reflection;
using System.Windows.Documents;
using OpenAI_WPF_Client.BusinessLogic.Interfaces;

namespace OpenAI_WPF_Client.BusinessLogic
{
    public class EmailOperations : IEmailOperations
    {
        public MimePart Attachment { get; set; }
        public RichTextBox Rtb { get; set; }

        public EmailOperations() { }

        public void AddAttachment()
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
                Attachment = new MimePart("application", "octet-stream")
                {
                    Content = new MimeContent(File.OpenRead(openFileDialog.FileName)),
                    ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = openFileDialog.FileName
                };
            }
        }

        public void SendEmail(string fromName, string fromEmailAddress, string toName, string toEmailAddress, string subject, string body, string appPassword)
        {
            var email = new MimeMessage();
            var builder = new BodyBuilder();

            email.From.Add(new MailboxAddress(fromName, fromEmailAddress));
            email.To.Add(new MailboxAddress(toName, toEmailAddress));
            email.Subject = subject;

            builder.TextBody = body;
            builder.Attachments.Add(Attachment);

            email.Body = builder.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 465, true);

                smtp.Authenticate(fromEmailAddress, appPassword);

                smtp.Send(email);
                MessageBox.Show("Email was sent!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                smtp.Disconnect(true);
            }
        }

        public void SaveAddAttachmentSend(string fromName, string fromEmailAddress, string toName, string toEmailAddress, string subject, string body, string appPassword)
        {
            using (MemoryStream _stream = new MemoryStream())
            {
                var textRange = new TextRange(this.Rtb.Document.ContentStart, this.Rtb.Document.ContentEnd);
                textRange.Save(_stream, DataFormats.Rtf);
                _stream.Position = 0;

                DocumentModel.Load(_stream, LoadOptions.RtfDefault).Save("..//..//..//WPF_ChatGPT_Dialogue.pdf");

                Attachment = new MimePart("application", "octet-stream")
                {
                    Content = new MimeContent(File.OpenRead("..//..//..//WPF_ChatGPT_Dialogue.pdf")),
                    ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "WPF_ChatGPT_Dialogue_Sended.pdf"
                };
                SendEmail(fromName, fromEmailAddress, toName, toEmailAddress, subject, body, appPassword);
            }
        }
    }
}