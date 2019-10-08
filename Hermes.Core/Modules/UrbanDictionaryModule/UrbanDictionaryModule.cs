using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Discord.Commands;

using Newtonsoft.Json.Linq;

namespace Hermes.Core.Modules.UrbanDictionaryModule
{
    public class UrbanDictionaryModule : ModuleBase<SocketCommandContext>
    {
        [Command("ud")]
        [Summary("Urban dictionary search")]
        public async Task AolSayAsync([Remainder] [Summary("The phrase to search for")] string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
            {
                await ReplyAsync($"Usage: !ud <search>");
            }
            else
            {
                string result = null;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User-Agent", "HermesDiscordBot");

                    result = Task.Run(async () => await client.GetStringAsync(
                        $"http://api.urbandictionary.com/v0/define?term={phrase}")).Result;
                }

                if (string.IsNullOrEmpty(result))
                {
                    await ReplyAsync($"Nothing found for {phrase}");
                }
                else
                {
                    try
                    {
                        var json = JObject.Parse(result);

                        await ReplyAsync($"_{phrase}?_");
                        await ReplyAsync(json["list"][0]["definition"].ToString());
                    }
                    catch
                    {
                        await ReplyAsync($"Nothing found for {phrase}");
                    }
                }
            }
        }
    }
}