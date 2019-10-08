using System.Threading.Tasks;

using Discord.Commands;

namespace Hermes.Core.Modules.TalkModule
{
    public class TalkModule : ModuleBase<SocketCommandContext>
    {
        [Command("talk")]
        [Summary("Synthesis speech using Markov model")]
        public async Task TalkAsync()
        {
            await ReplyAsync("I'm still learning about you");
        }
    }
}