using OpenAI.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenAI.Client.Exceptions;
using System.Net.Http.Json;

namespace OpenAI.Client
{
    public class OpenAIClient : IOpenAIClient
    {
        private readonly string _role = "user";
        private List<Message> _messageLog = new();

        private readonly string _configSectionName = "OpenAIClient";
        private readonly string _completionEndpointConfigName = "CompletionEndpoint";
        private readonly string _openAIModelIdConfigName = "ModelId";
        private readonly string _apiKeyConfigName = "ApiKey";

        private readonly string _apiKey;
        private readonly string _modelId;
        private readonly string _modelApiEndpoint;

        private HttpClient _httpClient;

        public OpenAIClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, string modelId)
        {
            var configSection = configuration.GetSection(_configSectionName);

            _apiKey = configSection[_apiKeyConfigName] ?? string.Empty;
            _modelId = configSection[_openAIModelIdConfigName] ?? string.Empty;
            _modelApiEndpoint = configSection[_completionEndpointConfigName] ?? string.Empty;

            if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_modelApiEndpoint) || string.IsNullOrEmpty(_modelId))
            {
                throw new OpenAIClientConfigException("ApiKey, ModelAPIEndpoint or ModelId is not set in config file");
            }

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> SendMessageAsync(string requestMessage)
        {
            if (string.IsNullOrEmpty(requestMessage))
            {
                throw new ArgumentException("Request message cannot be null or empty");
            }

            var msg = new Message { Role = _role, Content = requestMessage };
            _messageLog.Add(msg);

            var requestData = new RequestData
            {
                Messages = _messageLog,
                ModelId = _modelId
            };

            using var response = await _httpClient.PostAsJsonAsync(_modelApiEndpoint, requestData);

            if (!response.IsSuccessStatusCode)
            {
                throw new OpenAIClientException(response.ReasonPhrase ?? "Error has occured", (int)response.StatusCode);
            }

            var responseData = await response.Content.ReadFromJsonAsync<ResponseData>();
            var choices = responseData?.Choices ?? new List<Choice>();

            if (!choices.Any())
            {
                return "No response from chatGpt...";
            }
            var responseMessage = choices.First().Message;
            _messageLog.Add(responseMessage);

            var responseContent = responseMessage.Content.Trim();

            return responseContent;
        }

        public void StartNewSession()
        { 
            _messageLog.Clear();
        }

        public List<Message> GetMessageLog => _messageLog;
    }
}
