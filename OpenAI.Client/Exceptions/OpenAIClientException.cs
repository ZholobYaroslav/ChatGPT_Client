using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Client.Exceptions
{
    internal class OpenAIClientException: Exception
    {
        public int StatusCode { get; set; }

        public OpenAIClientException(string message, int statusCode): base(message) { StatusCode = statusCode;}

        public OpenAIClientException(Exception innerException, string message, int statusCode) : base(message, innerException) { StatusCode = statusCode; }

        public OpenAIClientException(Exception exception) { }
    }
}
