using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataFeed.Models
{
    public class Summary
    {
        [JsonProperty("rank")]
        public long Rank { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("team1")]
        public string Team1 { get; set; }

        [JsonProperty("team2")]
        public string Team2 { get; set; }

        [JsonProperty("points")]
        public double Points { get; set; }

        [JsonProperty("subs")]
        public long Subs { get; set; }

        [JsonProperty("subs1")]
        public long Subs1 { get; set; }

        [JsonProperty("subs2")]
        public long Subs2 { get; set; }
    }
}
