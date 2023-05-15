using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OpenAI_WPF_Client.BusinessLogic.Interfaces
{
    public interface IEmailOperations
    {
        public MimePart Attachment { get; set; }
        public RichTextBox Rtb { get; set; }
        void AddAttachment();
        void SendEmail(string fromName, string fromEmailAddress, string toName, string toEmailAddress, string subject, string body, string appPassword);
        void SaveAddAttachmentSend(string fromName, string fromEmailAddress, string toName, string toEmailAddress, string subject, string body, string appPassword);
    }
}
