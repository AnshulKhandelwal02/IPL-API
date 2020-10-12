using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataFeed.Models
{

    public class Leaderboard
    {
        [JsonProperty("Data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("Value")]
        public List<Value> Value { get; set; }

        [JsonProperty("FeedTime")]
        public FeedTime FeedTime { get; set; }
    }

    public class FeedTime
    {
        [JsonProperty("UTCTime")]
        public string UtcTime { get; set; }

        [JsonProperty("ISTTime")]
        public string IstTime { get; set; }

        [JsonProperty("CESTTime")]
        public string CestTime { get; set; }
    }

    public class Value
    {
        [JsonProperty("rno")]
        public long Rno { get; set; }

        [JsonProperty("temid")]
        public long Temid { get; set; }

        [JsonProperty("temname")]
        public string Temname { get; set; }

        [JsonProperty("usrname")]
        public string Usrname { get; set; }

        [JsonProperty("usrscoid")]
        public string Usrscoid { get; set; }

        [JsonProperty("usrclnid")]
        public long Usrclnid { get; set; }

        [JsonProperty("rank")]
        public long Rank { get; set; }

        [JsonProperty("points")]
        public double Points { get; set; }

        [JsonProperty("ftgdid")]
        public long Ftgdid { get; set; }

        [JsonProperty("prfimg")]
        public Uri Prfimg { get; set; }
    }
}
