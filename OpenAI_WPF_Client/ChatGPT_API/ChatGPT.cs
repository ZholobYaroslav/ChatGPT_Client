using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OpenAI_WPF_Client.ChatGPT_API
{
    abstract public class ChatGPT
    {
        private readonly string _apiKey;
        private readonly string _endpoint;
        private List<Message> _messages;
        private HttpClient _httpClient;

        protected ChatGPT()
        {
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User);
            _apiKey = EmbeddedStub.ApiKey;
            _endpoint = "https://api.openai.com/v1/chat/completions";
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _messages = new List<Message>();
        }

        public async Task<bool> StartChatting()// Template Method
        {
            string userInput = ReadUserInput();
            if (userInput == "\\exit")
            {
                return false;
            }

            Message msg = new Message() { Role = "user", Content = userInput };
            _messages.Add(msg);

            RequestData requestData = new RequestData()
            {
                Messages = _messages,
                ModelId = "gpt-3.5-turbo"
            };

            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_endpoint, requestData);

            if (!response.IsSuccessStatusCode)
            {
                ResponseErrorOutput(response.StatusCode);
                return false;
            }

            ResponseData? responseData = await response.Content.ReadFromJsonAsync<ResponseData>();
            List<Choice> choices = responseData?.Choices ?? new List<Choice>();
            if (choices.Count == 0)
            {
                ResponseEmptyChoicesOutput("No choices were returned by the API");
                return false;
            }

            Choice choice = choices[0];
            Message responseMessage = choice.Message;

            _messages.Add(responseMessage);
            string responseContent = responseMessage.Content.Trim();

            ChatGPT_Response(responseContent);
            return true;
        }
        protected abstract string ReadUserInput();
        protected abstract void ResponseErrorOutput(HttpStatusCode statusCode);
        protected abstract void ResponseEmptyChoicesOutput(string errorMessage);
        protected abstract void ChatGPT_Response(string response);
    }
}