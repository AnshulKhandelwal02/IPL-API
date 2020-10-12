using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataFeed.Models
{
    public partial class Transfers
    {
        [JsonProperty("Data")]
        public TransfersData Data { get; set; }
    }

    public partial class TransfersData
    {
        [JsonProperty("Value")]
        public TransfersValue Value { get; set; }
    }

    public partial class TransfersValue
    {
        [JsonProperty("teams")]
        public List<Team> Teams { get; set; }

        [JsonProperty("ovpts")]
        public string Ovpts { get; set; }

        [JsonProperty("gdpts")]
        public List<Gdpt> Gdpts { get; set; }
    }

    public partial class Gdpt
    {
        [JsonProperty("gdid")]
        public long Gdid { get; set; }

        [JsonProperty("gdpts")]
        public string Gdpts { get; set; }
    }

    public partial class Team
    {
        [JsonProperty("gdid")]
        public long Gdid { get; set; }

        [JsonProperty("frgdid")]
        public long Frgdid { get; set; }

        [JsonProperty("togdid")]
        public long Togdid { get; set; }

        [JsonProperty("ftgdid")]
        public long Ftgdid { get; set; }

        [JsonProperty("istrfgd")]
        public long Istrfgd { get; set; }

        [JsonProperty("isacct")]
        public long Isacct { get; set; }

        [JsonProperty("subusr")]
        public long SubsUsed { get; set; }

        [JsonProperty("subleft")]
        public long Subleft { get; set; }

        [JsonProperty("mcapt")]
        public long Mcapt { get; set; }

        [JsonProperty("mcaptnm")]
        public string CaptainName { get; set; }

        [JsonProperty("mcapisinjured")]
        public string Mcapisinjured { get; set; }

        [JsonProperty("mcapis_fp")]
        public string McapisFp { get; set; }

        [JsonProperty("mcapisannounced")]
        public string Mcapisannounced { get; set; }

        [JsonProperty("mcapIsActive")]
        public long McapIsActive { get; set; }

        [JsonProperty("vcapIsActive")]
        public long VcapIsActive { get; set; }

        [JsonProperty("vcapt")]
        public long Vcapt { get; set; }

        [JsonProperty("vcaptnm")]
        public string ViceCaptainName { get; set; }

        [JsonProperty("vcapisannounced")]
        public string Vcapisannounced { get; set; }

        [JsonProperty("vcapisinjured")]
        public string Vcapisinjured { get; set; }

        [JsonProperty("vcapis_fp")]
        public string VcapisFp { get; set; }

        [JsonProperty("plyid")]
        public List<long> Plyid { get; set; }

        [JsonProperty("skillcnt")]
        public long Skillcnt { get; set; }
    }
}
