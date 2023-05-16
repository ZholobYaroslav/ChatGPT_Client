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
        void AddAttachment(string fileName);
        void SendEmail(EmailMessage message);
        void SaveAddAttachmentSend(EmailMessage message);
    }
}
