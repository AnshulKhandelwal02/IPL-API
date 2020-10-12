using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataFeed.Models;
using DataFeed.Utilities;
using Newtonsoft.Json;

namespace DataFeed.Services
{
    public class MagicService: IMagicService
    {
        //private string TransferListDayOne =
        //    "https://fantasy.iplt20.com/season/services/user/e6e30ce8-fb44-11ea-9879-0ae60acb5e41/lb-team/overall?optType=1&teamgamedayId=1&arrtourGamedayId=1&phaseId=1&teamId={0}&SocialId={1}";

        //private string TransferListComplete =
        //    "https://fantasy.iplt20.com/season/services/user/e6e30ce8-fb44-11ea-9879-0ae60acb5e41/lb-team/overall?optType=2&teamgamedayId=1&arrtourGamedayId={0}&phaseId=1&teamId={1}&SocialId={2}";


        //private Dictionary<string, string> Headers = new Dictionary<string, string>
        //{
        //    { "cookie", "__csrf=48eee4f5-12c6-ccb1-bdb3-6f288b65cc3e; dh_user_id=17b78820-084d-11eb-a305-353939f488b4; G_ENABLED_IDPS=google; ajs_anonymous_id=%22a8aa2199-25d8-41d8-ba05-aa5c4f5747c8%22; WZRK_G=469fb23716da4311b46a79cfad01dd9e; connect.sid=s%3AFISryn9KeZJzhOerSj-BKH_eSWG8bjd4.aglvbUlCCYkf0sRJEE9Hll7OSsz44YE6%2BP%2FDDlnF8bY; WZRK_S_W4R-49K-494Z=%7B%22p%22%3A1%2C%22s%22%3A1602041317%2C%22t%22%3A1602041326%7D; d11partner=%7B%0A%20%20%22UserName%22%3A%20%22ANSHUL5086WX%22%2C%0A%20%20%22HasTeam%22%3A%201%2C%0A%20%20%22TeamName%22%3A%20%22ANSHUL5086WX%22%2C%0A%20%20%22FavTeamId%22%3A%20%221106%22%2C%0A%20%20%22SocialId%22%3A%20%2283119300%22%2C%0A%20%20%22GUID%22%3A%20%22e6e30ce8-fb44-11ea-9879-0ae60acb5e41%22%2C%0A%20%20%22ActiveTour%22%3A%20null%2C%0A%20%20%22IsTourActive%22%3A%200%2C%0A%20%20%22UserId%22%3A%20%22E921A771116F5C8FB8E1%22%2C%0A%20%20%22TeamId%22%3A%20%22E921A771116F5C8FB8E1%22%2C%0A%20%20%22ProfileURL%22%3A%20%22https%3A%2F%2Fwww.dream11.com%2Fpublic%2Fimgs%2Fleaderboard_default_image.png%22%2C%0A%20%20%22TeamName_Allow%22%3A%20%220%22%0A%7D"
        //    },
        //    { "x-csrf", "48eee4f5-12c6-ccb1-bdb3-6f288b65cc3e"}
        //};

        //private List<string> DayList = new List<string>
        //{
        //    "1,2,3,4,5",
        //    "6,7,8,9,10",
        //    "11,12,13,14,15",
        //    "16,17,18,19,20",
        //    "21,22,23,24,25",
        //    //"26,27,28,29,30",
        //    //"31,32,33,34,35",
        //    //"36,37,38,39,40",
        //    //"41,42,43,44,45",
        //    //"46,47,48,49,50",
        //    //"51,52,53,54,55",
        //    //"56,57,58,59,60"
        //};

        public async Task<List<TeamDataList>> GetTransfers(RequestData request, Leaderboard leaderboard)
        {
            HttpClient client = new HttpClient();
            foreach (var header in request.Headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            try
            {
                DataTable dtResult = new DataTable();
                List<TeamDataList> finalResults = new List<TeamDataList>();

                var stopWatch = new Stopwatch();

                foreach (var team in leaderboard.Data.Value)
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                    Console.WriteLine("Getting data for Team Id = {0}", team.Temid.ToString());

                    var teamResultsList = new List<Transfers>();
                    var teamResults = new Transfers();
                    var teamDataList = new TeamDataList();

                    teamDataList.TeamId = team.Temid;
                    teamDataList.TeamName = team.Temname;
                    teamDataList.Rank = team.Rank;

                    Parallel.ForEach(request.DayList, new ParallelOptions() { MaxDegreeOfParallelism = 6 }, index =>
                    {
                        var response = client.GetAsync(string.Format(request.TransferListUrl, index, team.Temid, team.Usrscoid)).Result;

                        var contents = response.Content.ReadAsStringAsync().Result;

                        var result = JsonConvert.DeserializeObject<Transfers>(contents);

                        teamResultsList.Add(result);
                    });

                    foreach (var item in teamResultsList)
                    {
                        if (item.Data.Value.Gdpts != null)
                        {
                            teamDataList.Gdpts.AddRange(item.Data.Value.Gdpts);
                            teamDataList.Teams.AddRange(item.Data.Value.Teams);
                            //teamDataList.Ovpts = item.Data.Value.Ovpts;
                        }
                    }

                    finalResults.Add(teamDataList);

                    stopWatch.Stop();
                    Console.WriteLine("Fetched data for Team Id = {0} || Time Taken = {1}", team.Temid, stopWatch.ElapsedMilliseconds.ToString());

                }

                return finalResults;

                //GroupData(dtResult);

                //ExportToCsv(dtResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public List<LeagueSummary> AnalyzeData(List<TeamDataList> rawData)
        {
            List<LeagueSummary> result = new List<LeagueSummary>();
            foreach (var data in rawData)
            {
                var leagueSummary = new LeagueSummary
                {
                    TeamId = data.TeamId,
                    TeamName = data.TeamName,
                    Rank = data.Rank,
                    TransfersDone = data.Teams.Sum(x => x.SubsUsed),
                    Points = data.Gdpts.Sum(x => Convert.ToDecimal(x.Gdpts)),
                    Captain = data.Teams.FirstOrDefault(x => x.Gdid == (data.Teams.Max(y => y.Gdid)))?.CaptainName,
                    ViceCaptain = data.Teams.FirstOrDefault(x => x.Gdid == (data.Teams.Max(y => y.Gdid)))
                        ?.ViceCaptainName,

                    DayPoints = Convert.ToDecimal(data.Gdpts.FirstOrDefault(x => x.Gdid == (data.Gdpts.Max(y => y.Gdid)))?.Gdpts),
                    DayTransfers = data.Teams.FirstOrDefault(x => x.Gdid == data.Teams.Max(y => y.Gdid)).SubsUsed,

                    DayPointsMatchNumber = data.Gdpts.Max(y => y.Gdid),
                    DayTransfersMatchNumber = data.Teams.Max(y => y.Gdid)
                };

                result.Add(leagueSummary);
            }

            return result;
        }

        public List<LeagueSummary> SummarizeData(List<LeagueSummary> result)
        {

            List<LeagueSummary> newResult = new List<LeagueSummary>();

            newResult.Add(MergeData(result.First(x => x.TeamId == 21250110), result.First(x => x.TeamId == 193400309), "AM"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 1097230408), result.First(x => x.TeamId == 1033330107), "AP"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 20090210), result.First(x => x.TeamId == 165330106), "AR"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 19570310), result.First(x => x.TeamId == 82340403), "AS"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 70870307), result.First(x => x.TeamId == 1184850407), "DPR"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 21140104), result.First(x => x.TeamId == 20500107), "DY"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 1123400102), result.First(x => x.TeamId == 784390209), "GK"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 987530201), result.First(x => x.TeamId == 988860307), "HP"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 171670204), result.First(x => x.TeamId == 173220206), "MS"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 19470302), result.First(x => x.TeamId == 72440106), "NA"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 1066590309), result.First(x => x.TeamId == 64680407), "NH"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 3350210), result.First(x => x.TeamId == 3550204), "NV"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 87390302), result.First(x => x.TeamId == 943170101), "PA"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 928310403), result.First(x => x.TeamId == 789770404), "RA"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 19170203), result.First(x => x.TeamId == 48170204), "RK"));
            newResult.Add(MergeData(result.First(x => x.TeamId == 1178370206), result.First(x => x.TeamId == 1106590310), "SP"));

            return newResult.OrderByDescending(x => x.Points).Select((x, i) => new LeagueSummary
            {
                Rank = i + 1,
                TeamName = x.TeamName,
                Points = x.Points,
                TransfersDone = x.TransfersDone,
                DayTransfers = x.DayTransfers,
                DayPoints = x.DayPoints,
                DayTransfersMatchNumber = x.DayTransfersMatchNumber,
                DayPointsMatchNumber = x.DayPointsMatchNumber
            }).ToList();

            //return newResult;

        }

        public LeagueSummary MergeData(LeagueSummary Team1, LeagueSummary Team2, string TeamName)
        {
            LeagueSummary item = new LeagueSummary
            {
                TeamName = TeamName,
                Points = Team1.Points + Team2.Points,
                TransfersDone = Team1.TransfersDone + Team2.TransfersDone,

                DayPoints = Team1.DayPoints + Team2.DayPoints,
                DayTransfers = Team1.DayTransfers + Team2.DayTransfers,

                DayPointsMatchNumber = Team1.DayPointsMatchNumber,
                DayTransfersMatchNumber = Team1.DayTransfersMatchNumber
            };

            return item;

        }

        public async Task<Leaderboard> GetLeaderboard(RequestData request)
        {
            try
            {
                HttpClient client = new HttpClient();
                foreach (var header in request.Headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                //var requestUrl = "https://fantasy.iplt20.com/season/services/user/contest/2150102/leaderboard?optType=1&gamedayId=16&phaseId=1&pageNo=1&topNo=100&pageChunk=100&pageOneChunk=100&minCount=33&leagueId=2150102";
                //var requestUrl =
                //    "https://fantasy.iplt20.com/season/services/user/contest/391740206/leaderboard?optType=1&gamedayId=18&phaseId=1&pageNo=1&topNo=100&pageChunk=100&pageOneChunk=100&minCount=9&leagueId=391740206";

                var resultJSON = (client.GetAsync(request.LeaderboardUrl).Result).Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<Leaderboard>(resultJSON);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void ExportToCsv(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText("test.csv", sb.ToString());
        }

    }
}
