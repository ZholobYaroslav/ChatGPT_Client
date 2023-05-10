using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_WPF_Client.ChatGPT_API
{
    public class ChatGPT_Context
    {
        private ChatGPT _concreteChatGPT;

        public ChatGPT_Context(ChatGPT concreteChatGPT)
        {
            _concreteChatGPT = concreteChatGPT;
        }

        public async Task<bool> Invoke()
        {
            return await _concreteChatGPT.StartChatting();
        }
    }
}
