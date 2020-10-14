using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR;

namespace DataFeed.Models
{
    public class FinalSummary
    {
        public string TeamName { get; set; }
        public string Points { get; set; }
        public string TransfersDone { get; set; }
        public string Captain { get; set; }
        public string ViceCaptain { get; set; }
        public string Rank { get; set; }
    }

    public class TeamDataList
    {
        public long TeamId { get; set; }
        public string TeamName { get; set; }
        public List<Team> Teams { get; set; } = new List<Team>();
        public string Ovpts { get; set; }
        public List<Gdpt> Gdpts { get; set; } = new List<Gdpt>();
        public long Rank { get; set; }
    }

    public class LeagueSummary
    {
        public long TeamId { get; set; }
        public long Rank { get; set; }
        public string TeamName { get; set; }
        public decimal Points { get; set; }
        public long Transfers { get; set; }
        public decimal PointsPerTransfer { get; set; }
        public decimal DayPoints { get; set; }
        public long DayTransfers { get; set; }
        public string Captain { get; set; }
        public string ViceCaptain { get; set; }
        public long DayPointsMatchNumber { get; set; }
        public long DayTransfersMatchNumber { get; set; }
        public decimal YesterdayPoints { get; set; }
        public long YesterdayTransfers { get; set; }
    }

}
