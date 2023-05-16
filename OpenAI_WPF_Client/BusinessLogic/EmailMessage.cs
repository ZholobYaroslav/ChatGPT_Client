using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_WPF_Client.BusinessLogic
{
    public class EmailMessage
    {
        public string FromName { get; }
        public string FromEmailAddress { get; }
        public string ToName { get; }
        public string ToEmailAddress { get; }
        public string Subject { get; }
        public string Body { get; }
        public string AppPassword { get; }

        public EmailMessage(string fromName, string fromEmailAddress, string toName, string toEmailAddress, string subject, string body, string appPassword)
        {
            FromName = fromName;
            FromEmailAddress = fromEmailAddress;
            ToName = toName;
            ToEmailAddress = toEmailAddress;
            Subject = subject;
            Body = body;
            AppPassword = appPassword;
        }
    }
}
