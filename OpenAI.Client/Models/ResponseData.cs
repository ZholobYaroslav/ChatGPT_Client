using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.Client.Models
{
    public class ResponseData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("object")]
        public string Object { get; set; } = string.Empty;
        [JsonPropertyName("created")]
        public ulong Created { get; set; }
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; } = new();
        [JsonPropertyName("usage")]
        public Usage Usage { get; set; } = new();   
    }
}
