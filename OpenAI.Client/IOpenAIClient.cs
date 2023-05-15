using OpenAI.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Client
{

    public interface IOpenAIClient
    {
        Task<string> SendMessageAsync(string requestMessage);
        void StartNewSession();
        List<Message> GetMessageLog { get; }
    }
}

