using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord.Commands;

using Markov;

using Hermes.Database;
using Hermes.Models;

namespace Hermes.Modules.TalkModule
{
    public class TalkModule : ModuleBase<SocketCommandContext>
    {
        private readonly DbContextFactory _factory;

        public TalkModule()
        {
            _factory = new DbContextFactory();
        }

        [Command("talk")]
        [Summary("Synthesis speech using Markov model")]
        public async Task TalkAsync([Summary("The user to make talk")] string user)
        {
            List<Message> messages = null;

            using (var db = _factory.CreateDbContext())
            {
                messages = db.Messages
                    .Where(x => x.User == user && x.Content.Length > 10)
                    .OrderByDescending(x => x.Id)
                    .Take(100)
                    .ToList();
            }

            if (messages.Count < 50)
            {
                await ReplyAsync($"{user} hasn't said enough to make them talk yet");
                return;
            }

            var chain = new MarkovChain<string>(1);

            foreach (var message in messages)
            {
                chain.Add(message.Content.Split(' '));
            }

            await ReplyAsync(string.Join(" ", chain.Chain()));
        }
    }
}