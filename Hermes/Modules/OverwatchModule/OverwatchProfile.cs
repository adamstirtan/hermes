using Newtonsoft.Json;

namespace Hermes.Modules.OverwatchModule
{
    public class OverwatchProfile
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("level")]
        public int? Level { get; set; }

        [JsonProperty("levelIcon")]
        public string LevelIcon { get; set; }

        [JsonProperty("prestige")]
        public int? Prestige { get; set; }

        [JsonProperty("prestigeIcon")]
        public string PrestigeIcon { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }

        [JsonProperty("ratingIcon")]
        public string RatingIcon { get; set; }

        [JsonProperty("gamesWon")]
        public int? GamesWon { get; set; }

        [JsonProperty("quickPlayStats")]
        public GameModeStats QuickPlayStats { get; set; }

        [JsonProperty("competitiveStats")]
        public GameModeStats CompetitiveStats { get; set; }
    }

    public class GameModeStats
    {
        [JsonProperty("eliminationsAvg")]
        public float? AverageEliminations { get; set; }

        [JsonProperty("damageDoneAvg")]
        public float? AverageDamageDone { get; set; }

        [JsonProperty("deathsAvg")]
        public float? AverageDeaths { get; set; }

        [JsonProperty("finalBlowsAvg")]
        public float? AverageFinalBlows { get; set; }

        [JsonProperty("healingDoneAvg")]
        public float? AverageHealingDone { get; set; }

        [JsonProperty("objectiveKillsAvg")]
        public float? AverageObjectiveKills { get; set; }

        [JsonProperty("objectiveTimeAvg")]
        public string AverageObjectiveTime { get; set; }

        [JsonProperty("soloKillsAvg")]
        public float? AverageSoloKills { get; set; }

        [JsonProperty("games")]
        public Games Games { get; set; }

        [JsonProperty("awards")]
        public Awards Awards { get; set; }
    }

    public class Games
    {
        [JsonProperty("played")]
        public int? Played { get; set; }

        [JsonProperty("won")]
        public int? Won { get; set; }
    }

    public class Awards
    {
        [JsonProperty("cards")]
        public int? Cards { get; set; }

        [JsonProperty("metals")]
        public int? Medals { get; set; }

        [JsonProperty("medalsBronze")]
        public int? MedalsBronze { get; set; }

        [JsonProperty("medalsSilver")]
        public int? MedalsSilver { get; set; }

        [JsonProperty("medalsGold")]
        public int? MedalsGold { get; set; }
    }
}