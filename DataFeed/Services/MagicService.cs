﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using DataFeed.Models;
using DataFeed.Utilities;
using Newtonsoft.Json;

namespace DataFeed.Services
{
    public class MagicService: IMagicService
    {
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
                        if (item?.Data?.Value?.Gdpts != null)
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

        public List<LeagueSummary> PivotData(List<TeamDataList> rawData)
        {
            List<LeagueSummary> result = new List<LeagueSummary>();

            foreach (var data in rawData)
            {
                var temp4 = data.Gdpts.ToArray().ToPivotTableModified(
                    item => item.Gdid,
                    item => item.Gdpts,
                    data.TeamName);

                var leagueSummary = new LeagueSummary
                {
                    TeamId = data.TeamId,
                    TeamName = data.TeamName,
                    Rank = data.Rank,
                    Transfers = data.Teams.Sum(x => x.SubsUsed),
                    Points = data.Gdpts.Sum(x => Convert.ToDecimal(x.Gdpts)),
                    Captain = data.Teams.FirstOrDefault(x => x.Gdid == (data.Teams.Max(y => y.Gdid)))?.CaptainName,
                    ViceCaptain = data.Teams.FirstOrDefault(x => x.Gdid == (data.Teams.Max(y => y.Gdid)))
                        ?.ViceCaptainName

                };

                result.Add(leagueSummary);
            }


            return result;
        }

        public List<LeagueSummary> AnalyzeData(List<TeamDataList> rawData)
        {
            DateTime halfway = new DateTime(2020, 10, 12);
            DateTime todayDate = DateTime.Today.Date;

            // halfway matchday = 24
            long todayMatchday = 24 + Convert.ToInt64((todayDate - halfway).TotalDays);

            List<LeagueSummary> result = new List<LeagueSummary>();
            foreach (var data in rawData)
            {
                var leagueSummary = new LeagueSummary
                {
                    TeamId = data.TeamId,
                    TeamName = data.TeamName,
                    Rank = data.Rank,
                    Transfers = data.Teams.Sum(x => x.SubsUsed),
                    Points = data.Gdpts.Sum(x => Convert.ToDecimal(x.Gdpts)),

                    DayPoints = data.Gdpts.Exists(x => x.Gdid == todayMatchday)
                        ? Convert.ToDecimal(data.Gdpts.FirstOrDefault(x => x.Gdid == todayMatchday)?.Gdpts)
                        : 0,

                    DayTransfers = data.Teams.Exists(x => x.Gdid == todayMatchday)
                        ? data.Teams.FirstOrDefault(x => x.Gdid == todayMatchday).SubsUsed
                        : 0,

                    YesterdayPoints = data.Gdpts.Exists(x => x.Gdid == todayMatchday - 1)
                    ? Convert.ToDecimal(data.Gdpts.FirstOrDefault(x => x.Gdid == todayMatchday - 1)?.Gdpts)
                    : 0,

                    YesterdayTransfers = data.Teams.Exists(x => x.Gdid == todayMatchday - 1)
                        ? data.Teams.FirstOrDefault(x => x.Gdid == todayMatchday - 1).SubsUsed
                        : 0
                    //Captain = data.Teams.FirstOrDefault(x => x.Gdid == (data.Teams.Max(y => y.Gdid)))?.CaptainName,
                    //ViceCaptain = data.Teams.FirstOrDefault(x => x.Gdid == (data.Teams.Max(y => y.Gdid)))
                    //    ?.ViceCaptainName,

                    //DayPoints = Convert.ToDecimal(data.Gdpts.FirstOrDefault(x => x.Gdid == (data.Gdpts.Max(y => y.Gdid)))?.Gdpts),
                    //DayTransfers = data.Teams.FirstOrDefault(x => x.Gdid == data.Teams.Max(y => y.Gdid)).SubsUsed,

                    //DayPoints = Convert.ToDecimal(dayPoints?.Gdpts),
                    //DayTransfers = dayTeam?.SubsUsed ?? 0,

                    //YesterdayPoints = Convert.ToDecimal(data.Gdpts.FirstOrDefault(x => x.Gdid == todayMatchday - 1)?.Gdpts),
                    //YesterdayTransfers = data.Teams.FirstOrDefault(x => x.Gdid == todayMatchday - 1).SubsUsed

                    //DayPointsMatchNumber = data.Gdpts.Max(y => y.Gdid),
                    //DayTransfersMatchNumber = data.Teams.Max(y => y.Gdid)
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
                Transfers = x.Transfers,
                DayTransfers = x.DayTransfers,
                DayPoints = x.DayPoints
            }).ToList();

            //return newResult;

        }

        public LeagueSummary MergeData(LeagueSummary Team1, LeagueSummary Team2, string TeamName)
        {
            LeagueSummary item = new LeagueSummary
            {
                TeamName = TeamName,
                Points = Team1.Points + Team2.Points,
                Transfers = Team1.Transfers + Team2.Transfers,

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

        public void CalculationLogic()
        {
            DateTime halfway = new DateTime(2020,10,12);
            DateTime todayDate = DateTime.Today.Date;

            var halfwayMatchday = 24;

            var todayMatchday = halfwayMatchday + (todayDate - halfway).TotalDays;
        }

        
    }
}
