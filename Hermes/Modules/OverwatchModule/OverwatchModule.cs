using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hermes.Modules.OverwatchModule
{
    // https://ow-api.com/docs/#profile
    public class OverwatchModule : ModuleBase<SocketCommandContext>
    {
        private readonly Dictionary<string, string> users = new Dictionary<string, string>
        {
            { "rhaydeo", "Rhaydeo-11799" },
            { "lewzer", "lewzer-1695"},
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
                var battleTag = users[user.ToLower()];

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
                }
                else
                {
                    OverwatchProfile profile = JsonConvert.DeserializeObject<OverwatchProfile>(result);
                    
                    // try
                    // {
                    //     var json = JObject.Parse(result);
                    //     var response = json["list"][0]["definition"].ToString();

                    //     await ReplyAsync($"_{phrase}?_");
                    //     await ReplyAsync(response);
                    // }
                    // catch
                    // {
                    //     await ReplyAsync($"Nothing found for {phrase}");
                    // }
                }
            }
        }
    }
}