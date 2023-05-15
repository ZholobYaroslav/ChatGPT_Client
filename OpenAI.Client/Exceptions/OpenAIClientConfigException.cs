using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Client.Exceptions
{
    internal class OpenAIClientConfigException: Exception
    {
        public OpenAIClientConfigException(string message) : base(message) { }

        public OpenAIClientConfigException(string message, Exception innerException) : base(message, innerException) { }

        public OpenAIClientConfigException(Exception ex) { }

    }
}
