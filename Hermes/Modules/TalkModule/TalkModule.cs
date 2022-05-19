using System.Linq;
using System.Threading.Tasks;

using Discord.Commands;

using Markov;

using Hermes.Database.Repositories;

namespace Hermes.Modules.TalkModule
{
    public class TalkModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMessageRepository _messageRepository;

        public TalkModule(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [Command("talk")]
        [Summary("Synthesis speech using Markov model")]
        public async Task TalkAsync([Summary("The user to make talk")] string user)
        {
            var messages = _messageRepository
                .Where(x => x.User.ToLower() == user.ToLower())
                .ToArray();

            if (messages.Length < 25)
            {
                await ReplyAsync($"{user} hasn't said enough to make them talk yet");
                return;
            }

            var chain = new MarkovChain<string>(1);

            foreach (var message in messages)
            {
                var split = message.Content.Split(' ');

                if (split.Length >= 5)
                {
                    chain.Add(split);
                }
            }

            await ReplyAsync(string.Join(" ", chain.Chain()));
        }
    }
}