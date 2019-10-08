using System;
using System.Threading.Tasks;

using Discord.Commands;

namespace Hermes.Core.Modules.EightBallModule
{
    public class EightBallModule : ModuleBase<SocketCommandContext>
    {
        private static readonly string[] _responses =
        {
            "It is certain.",
            "It is decidely so.",
            "Without a doubt.",
            "Yes - definitely.",
            "You may rely on it.",
            "As I see it, yes.",
            "Most likely",
            "Outlook good.",
            "Yes",
            "Signs point to yes.",
            "Reply hazy, try again.",
            "Ask again later.",
            "Better not tell you now.",
            "Cannot predict now.",
            "Concentrate and ask again.",
            "Don't count on it.",
            "My reply is no.",
            "My sources say no.",
            "Outlook not so good.",
            "Very doubtful.",
            "Only Ken knows.",
            "Blow it out your ass, idgaf.",
            "https://media.giphy.com/media/AD2uIPYVqszdK/giphy.gif",
            "https://media.giphy.com/media/XozypzpGakVuX2ciZJ/giphy.gif",
            "https://media.giphy.com/media/ZLzIgHvizfF6M/giphy.gif",
            "https://media.giphy.com/media/3o7btT1T9qpQZWhNlK/giphy.gif",
            "https://media.giphy.com/media/4WHkXdDx8wjS0/giphy.gif",
            "https://media.giphy.com/media/3og0INyCmHlNylks9O/giphy.gif",
            "https://media.giphy.com/media/bPTXcJiIzzWz6/giphy.gif",
            "https://media.giphy.com/media/ptDRdwFkFVAkg/giphy.gif",
            "https://media.giphy.com/media/FEikw3bXVHdMk/giphy.gif",
            "https://media.giphy.com/media/oGO1MPNUVbbk4/giphy.gif",
            "https://media.giphy.com/media/l1BgRIamescnkx5Dy/giphy.gif"
        };

        [Command("8ball")]
        [Summary("Complex decision engine")]
        public async Task EightBall([Remainder] [Summary("The important question")] string question)
        {
            var rng = new Random(DateTime.UtcNow.Millisecond);

            await ReplyAsync($"_{question}?_");
            await ReplyAsync(_responses[rng.Next(_responses.Length)]);
        }
    }
}