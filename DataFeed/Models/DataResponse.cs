using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataFeed.Models
{
    public class DataResponse
    {
        public List<string> ColumnsList { get; set; } = new List<string>();

        public List<LeagueSummary> SummaryData { get; set; }
    }
}
