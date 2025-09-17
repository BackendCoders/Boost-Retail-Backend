using ChatGPT.Net;

namespace SIMWORKER
{
    public class AI
    {
        public ChatGpt _bot;
        public AI()
        {
            _bot = new ChatGpt("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");
        }

        public async Task<string> GetCategory(string message)
        {
            var response = await _bot.Ask(message);
            return response;
        }
    }
}
