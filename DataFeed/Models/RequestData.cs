using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataFeed.Models
{
    public class RequestData
    {
        public List<string> DayList { get; set; } = new List<string>
        {
            "1,2,3,4,5",
            "6,7,8,9,10",
            "11,12,13,14,15",
            "16,17,18,19,20",
            "21,22,23,24,25",
            "26,27,28,29,30",
            "31,32,33,34,35",
            //"36,37,38,39,40",
            //"41,42,43,44,45",
            //"46,47,48,49,50",
            //"51,52,53,54,55",
            //"56,57,58,59,60"
        };

        public Dictionary<string, string> Headers { get; set; }
        public string TransferListUrl { get; set; }
        public string LeaderboardUrl { get; set; }

        public string PlayerListUrl { get; set; } =
            "https://fantasy.iplt20.com/season/services/feed/players?lang=en&tourgamedayId={0}&teamgamedayId={1}&announcedVersion=10142020182314";

        public long MatchDay
        {
            get
            {
                var halfway = new DateTime(2020, 10, 12);
                var todayDate = DateTime.Today.Date;

                // halfway matchday = 24
                return 24 + Convert.ToInt64((todayDate - halfway).TotalDays);
            }
        }
    }
}
