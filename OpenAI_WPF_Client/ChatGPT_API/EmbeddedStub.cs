using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_WPF_Client.ChatGPT_API
{
    internal class EmbeddedStub
    {
        private const string apiKey = "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";// my personal token on chatopenai website

        public static string ApiKey
        {
            get { return apiKey; }
        }
    }
}
