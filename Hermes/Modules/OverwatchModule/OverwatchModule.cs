using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

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
                }
                else
                {
                    try
                    {
                        var json = JObject.Parse(result);

                        var level = json["level"].ToString();
                        var imageUrl = json["icon"].ToString();
                        //var games = json["quickPlayStats"]["games"];
                        //var gamesPlayed = games["played"].ToString();
                        //var gamesWon = games["won"].ToString();
                        //var winPercentage = (int)((decimal.Parse(gamesPlayed) / decimal.Parse(gamesWon)) * 100);

                        var embed = new EmbedBuilder
                        {
                            Title = $"{user} (Level {level})",
                            //Description = $"Played: {gamesPlayed}, Won: {gamesWon}, Win %: {winPercentage}",
                            ImageUrl = imageUrl
                        }.Build();

                        await ReplyAsync(embed: embed);
                    }
                    catch
                    {
                        await ReplyAsync("Overwatch API isn't working properly");
                    }
                }
            }
        }
    }
}