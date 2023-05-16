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

        public void AddAttachment(string fileName)
        {
            Attachment = new MimePart("application", "octet-stream")
            {
                Content = new MimeContent(File.OpenRead(fileName)),
                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = fileName
            };
        }

        public void SendEmail(EmailMessage message)
        {
            var email = new MimeMessage();
            var builder = new BodyBuilder();

            email.From.Add(new MailboxAddress(message.FromName, message.FromEmailAddress));
            email.To.Add(new MailboxAddress(message.ToName, message.ToEmailAddress));
            email.Subject = message.Subject;

            builder.TextBody = message.Body;
            builder.Attachments.Add(Attachment);

            email.Body = builder.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 465, true);

                smtp.Authenticate(message.FromEmailAddress, message.AppPassword);

                smtp.Send(email);
                MessageBox.Show("Email was sent!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                smtp.Disconnect(true);
            }
        }

        public void SaveAddAttachmentSend(EmailMessage message)
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
                SendEmail(message);
            }
        }
    }
}