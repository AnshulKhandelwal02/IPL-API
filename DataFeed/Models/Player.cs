using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataFeed.Models
{
    public class Player
    {
        [JsonProperty("Data")]
        public PlayerData Data { get; set; }
    }

    public class PlayerData
    {
        [JsonProperty("Value")]
        public PlayerValue Value { get; set; }
    }

    public class PlayerValue
    {
        [JsonProperty("Players")]
        public List<PlayerElement> Players { get; set; }
    }

    public class PlayerElement
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("ShortName")]
        public string ShortName { get; set; }

        [JsonProperty("TeamId")]
        public long TeamId { get; set; }

        [JsonProperty("TeamName")]
        public string TeamName { get; set; }

        [JsonProperty("TeamShortName")]
        public string TeamShortName { get; set; }

        [JsonProperty("SkillName")]
        public string SkillName { get; set; }

        [JsonProperty("SkillId")]
        public long SkillId { get; set; }

        [JsonProperty("Value")]
        public double Value { get; set; }

        [JsonProperty("IsActive")]
        public long IsActive { get; set; }

        [JsonProperty("OverallPoints")]
        public long OverallPoints { get; set; }

        [JsonProperty("GamedayPoints")]
        public long GamedayPoints { get; set; }

        [JsonProperty("IsAnnounced")]
        public string IsAnnounced { get; set; }

        [JsonProperty("PlayerDesc")]
        public string PlayerDesc { get; set; }
    }
}
