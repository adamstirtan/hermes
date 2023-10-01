using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using Newtonsoft.Json.Linq;

namespace Hermes.Modules.UrbanDictionaryModule
{
    public class UrbanDictionaryModule : ModuleBase<SocketCommandContext>
    {
        [Command("ud")]
        [Summary("Urban dictionary search")]
        public async Task UrbanDictionarySearchAsync([Remainder] [Summary("The phrase to search for")] string phrase = null)
        {
            string requestUri = string.Empty;

            if (string.IsNullOrEmpty(phrase))
            {
                requestUri = "https://api.urbandictionary.com/v0/random";
            }
            else
            {
                requestUri = $"http://api.urbandictionary.com/v0/define?term={phrase}";
            }
                
            string result = null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "HermesDiscordBot");

                result = Task.Run(async () => await client.GetStringAsync(requestUri)).Result;
            }

            if (string.IsNullOrEmpty(result))
            {
                await ReplyAsync($"No response from UrbanDictionary");
                return;
            }

            var json = JObject.Parse(result);
            var definition = json["list"][0]["definition"].ToString();
            var word = json["list"][0]["word"].ToString();

            var builder = new EmbedBuilder();
            
            builder.AddField("!ud", $"{word}: {definition}", true);
            builder.WithThumbnailUrl("https://upload.wikimedia.org/wikipedia/commons/thumb/f/f0/Urban_Dictionary_logo.svg/512px-Urban_Dictionary_logo.svg.png");
            builder.WithColor(Color.LightOrange);

            await ReplyAsync(embed: builder.Build());
        }
    }
}