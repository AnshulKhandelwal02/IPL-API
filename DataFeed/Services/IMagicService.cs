using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataFeed.Models;

namespace DataFeed.Services
{
    public interface IMagicService
    {
        Task<List<TeamDataList>> GetTransfers(RequestData request, Leaderboard leaderboard);
        Task<Leaderboard> GetLeaderboard(RequestData request);
        List<LeagueSummary> AnalyzeData(List<TeamDataList> rawData);
        List<LeagueSummary> AnalyzeDataAdmin(List<TeamDataList> rawData, List<PlayerElement> playersData);
        List<LeagueSummary> AnalyzeDataAdminWithPlayers(List<TeamDataList> rawData, List<PlayerElement> playersData);
        List<LeagueSummary> SummarizeData(List<LeagueSummary> result);

        List<LeagueSummary> PivotData(List<TeamDataList> rawData);
        Task<List<PlayerElement>> GetPlayerList(RequestData request);
        long GetTodayMatchday();
    }
}
