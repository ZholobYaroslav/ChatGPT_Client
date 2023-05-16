﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.Client.Models
{
    public class RequestData
    {
        [JsonPropertyName("model")]
        public string ModelId { get; set; } = string.Empty;
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; } = new();
    }
}
