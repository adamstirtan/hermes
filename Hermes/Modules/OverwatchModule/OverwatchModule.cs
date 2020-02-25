using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using Newtonsoft.Json;

namespace Hermes.Modules.OverwatchModule
{
    public class OverwatchModule : ModuleBase<SocketCommandContext>
    {
        private readonly Dictionary<string, string> users = new Dictionary<string, string>
        {
            { "rhaydeo", "Rhaydeo-11799" },
            { "lewzer", "lewzer-1695"},
            { "dizavef", "lewzer-1695"},
            { "mastadonn", "Mastadonn-11946" }
        };

        [Command("ow")]
        [Summary("Overwatch statistics")]
        public async Task StatsAsync([Remainder] [Summary("user")] string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                await ReplyAsync($"Usage: !ow <user>");
            }
            else
            {
                string battleTag;

                users.TryGetValue(user.ToLower(), out battleTag);

                if (battleTag == null)
                {
                    await ReplyAsync($"Couldn't find the BattleTag for that user");
                    return;
                }

                string result = null;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User-Agent", "HermesDiscordBot");

                    result = Task.Run(async () => await client.GetStringAsync(
                        $"https://ow-api.com/v1/stats/pc/us/{battleTag}/complete")).Result;
                }

                if (string.IsNullOrEmpty(result))
                {
                    await ReplyAsync($"Nothing found for {user}");
                    return;
                }
                
                OverwatchProfile profile = JsonConvert.DeserializeObject<OverwatchProfile>(result);

                var embed = new EmbedBuilder
                {
                    ImageUrl = profile.Icon,
                    Title = profile.Title(battleTag),
                    Description = profile.Description(),

                }.Build();

                await ReplyAsync(embed: embed);
            }
        }
    }
}