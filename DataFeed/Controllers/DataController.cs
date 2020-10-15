using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using DataFeed.Models;
using DataFeed.Services;
using DataFeed.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataFeed.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {   
        private readonly ILogger<DataController> _logger;
        private readonly IMagicService _magicService;

        public DataController(ILogger<DataController> logger, IMagicService magicService)
        {
            _logger = logger;
            _magicService = magicService;
        }

        [Route("summary")]
        public async Task<ResponseEnvelope> GetSummary()
        {
            var response = new DataResponse();
            var request = GetMahasangramRequestData();

            var leaderboard = _magicService.GetLeaderboard(request).Result;

            var teamDataList = _magicService.GetTransfers(request, leaderboard).Result;

            var analyzedResult = _magicService.AnalyzeData(teamDataList);

            var summarizedData = _magicService.SummarizeData(analyzedResult);

            summarizedData.ForEach(x => x.PointsPerTransfer = Math.Round(x.Points / x.Transfers,2));

            response.SummaryData = summarizedData;
            response.MatchDay = _magicService.GetTodayMatchday();
            response.ColumnsList = summarizedData.ToDataTable().Columns.Cast<DataColumn>()
                .Select(x => char.ToLowerInvariant(x.ColumnName[0]) + x.ColumnName.Substring(1))
                .ToList();

            response.ColumnsList.Remove("teamId");
            response.ColumnsList.Remove("dayPointsMatchNumber");
            response.ColumnsList.Remove("dayTransfersMatchNumber");
            response.ColumnsList.Remove("captain");
            response.ColumnsList.Remove("viceCaptain");
            response.ColumnsList.Remove("p1");
            response.ColumnsList.Remove("p2");
            response.ColumnsList.Remove("p3");
            response.ColumnsList.Remove("p4");
            response.ColumnsList.Remove("p5");
            response.ColumnsList.Remove("p6");
            response.ColumnsList.Remove("p7");
            response.ColumnsList.Remove("p8");
            response.ColumnsList.Remove("p9");
            //response.ColumnsList.Remove("yesterdayPoints");
            //response.ColumnsList.Remove("yesterdayTransfers");

            return ResponseEnvelope.Success(response);
        }

        [Route("gokuldham-summary")]
        public async Task<ResponseEnvelope> GetSummaryForGokuldham()
        {
            var response = new DataResponse();
            RequestData request = GetGokuldhamRequestData();

            var leaderboard = _magicService.GetLeaderboard(request).Result;

            var teamDataList = _magicService.GetTransfers(request, leaderboard).Result;

            var analyzedResult = _magicService.AnalyzeData(teamDataList);

            analyzedResult.ForEach(x => x.PointsPerTransfer = Math.Round(x.Points / x.Transfers, 2));

            response.SummaryData = analyzedResult;
            response.MatchDay = _magicService.GetTodayMatchday();
            response.ColumnsList = analyzedResult.ToDataTable().Columns.Cast<DataColumn>()
                .Select(x => char.ToLowerInvariant(x.ColumnName[0]) + x.ColumnName.Substring(1))
                .ToList();

            response.ColumnsList.Remove("teamId");
            response.ColumnsList.Remove("dayPointsMatchNumber");
            response.ColumnsList.Remove("dayTransfersMatchNumber");
            response.ColumnsList.Remove("p1");
            response.ColumnsList.Remove("p2");
            response.ColumnsList.Remove("p3");
            response.ColumnsList.Remove("p4");
            response.ColumnsList.Remove("p5");
            response.ColumnsList.Remove("p6");
            response.ColumnsList.Remove("p7");
            response.ColumnsList.Remove("p8");
            response.ColumnsList.Remove("p9");
            response.ColumnsList.Remove("captain");
            response.ColumnsList.Remove("viceCaptain");
            //response.ColumnsList.Remove("yesterdayPoints");
            //response.ColumnsList.Remove("yesterdayTransfers");

            return ResponseEnvelope.Success(response);

        }

        [Route("mahasangram-summary")]
        public async Task<ResponseEnvelope> GetSummaryForMahasangram()
        {

            var response = new DataResponse();
            RequestData request = GetMahasangramRequestData();

            var leaderboard = _magicService.GetLeaderboard(request).Result;

            var teamDataList = _magicService.GetTransfers(request, leaderboard).Result;

            var analyzedResult = _magicService.AnalyzeData(teamDataList);

            analyzedResult.ForEach(x => x.PointsPerTransfer = Math.Round(x.Points / x.Transfers, 2));

            response.SummaryData = analyzedResult;
            response.MatchDay = _magicService.GetTodayMatchday();
            response.ColumnsList = analyzedResult.ToDataTable().Columns.Cast<DataColumn>()
                .Select(x => char.ToLowerInvariant(x.ColumnName[0]) + x.ColumnName.Substring(1))
                .ToList();

            response.ColumnsList.Remove("teamId");
            response.ColumnsList.Remove("dayPointsMatchNumber");
            response.ColumnsList.Remove("dayTransfersMatchNumber");
            response.ColumnsList.Remove("captain");
            response.ColumnsList.Remove("viceCaptain");
            response.ColumnsList.Remove("p1");
            response.ColumnsList.Remove("p2");
            response.ColumnsList.Remove("p3");
            response.ColumnsList.Remove("p4");
            response.ColumnsList.Remove("p5");
            response.ColumnsList.Remove("p6");
            response.ColumnsList.Remove("p7");
            response.ColumnsList.Remove("p8");
            response.ColumnsList.Remove("p9");
            //response.ColumnsList.Remove("yesterdayPoints");
            //response.ColumnsList.Remove("yesterdayTransfers");

            return ResponseEnvelope.Success(response);
        }


        [Route("mahasangram-summary-admin")]
        public async Task<ResponseEnvelope> GetSummaryForMahasangramAdmin()
        {

            var response = new DataResponse();
            RequestData request = GetMahasangramRequestData();

            var leaderboard = _magicService.GetLeaderboard(request).Result;

            var teamDataList = _magicService.GetTransfers(request, leaderboard).Result;

            var playerList = _magicService.GetPlayerList(request).Result;

            var analyzedResult = _magicService.AnalyzeDataAdmin(teamDataList, playerList);

            analyzedResult.ForEach(x => x.PointsPerTransfer = Math.Round(x.Points / x.Transfers, 2));

            response.SummaryData = analyzedResult;
            response.MatchDay = _magicService.GetTodayMatchday();
            response.ColumnsList = analyzedResult.ToDataTable().Columns.Cast<DataColumn>()
                .Select(x => char.ToLowerInvariant(x.ColumnName[0]) + x.ColumnName.Substring(1))
                .ToList();

            response.ColumnsList.Remove("teamId");
            response.ColumnsList.Remove("dayPointsMatchNumber");
            response.ColumnsList.Remove("dayTransfersMatchNumber");
            //response.ColumnsList.Remove("captain");
            //response.ColumnsList.Remove("viceCaptain");
            //response.ColumnsList.Remove("yesterdayPoints");
            //response.ColumnsList.Remove("yesterdayTransfers");

            return ResponseEnvelope.Success(response);
        }


        [Route("gokuldham-summary-admin")]
        public async Task<ResponseEnvelope> GetSummaryForGokuldhamAdmin()
        {
            var response = new DataResponse();
            RequestData request = GetGokuldhamRequestData();

            var leaderboard = _magicService.GetLeaderboard(request).Result;

            var teamDataList = _magicService.GetTransfers(request, leaderboard).Result;

            var playerList = _magicService.GetPlayerList(request).Result;

            var analyzedResult = _magicService.AnalyzeDataAdmin(teamDataList, playerList);

            analyzedResult.ForEach(x => x.PointsPerTransfer = Math.Round(x.Points / x.Transfers, 2));

            response.SummaryData = analyzedResult;
            response.MatchDay = _magicService.GetTodayMatchday();
            response.ColumnsList = analyzedResult.ToDataTable().Columns.Cast<DataColumn>()
                .Select(x => char.ToLowerInvariant(x.ColumnName[0]) + x.ColumnName.Substring(1))
                .ToList();

            response.ColumnsList.Remove("teamId");
            response.ColumnsList.Remove("dayPointsMatchNumber");
            response.ColumnsList.Remove("dayTransfersMatchNumber");
            //response.ColumnsList.Remove("p1");
            //response.ColumnsList.Remove("p2");
            //response.ColumnsList.Remove("p3");
            //response.ColumnsList.Remove("p4");
            //response.ColumnsList.Remove("p5");
            //response.ColumnsList.Remove("p6");
            //response.ColumnsList.Remove("p7");
            //response.ColumnsList.Remove("p8");
            //response.ColumnsList.Remove("p9");
            //response.ColumnsList.Remove("captain");
            //response.ColumnsList.Remove("viceCaptain");
            //response.ColumnsList.Remove("yesterdayPoints");
            //response.ColumnsList.Remove("yesterdayTransfers");

            return ResponseEnvelope.Success(response);

        }
        
        public RequestData GetMahasangramRequestData()
        {
            RequestData request = new RequestData
            {
                Headers = new Dictionary<string, string>
                {
                    {
                        "cookie",
                        "dh_user_id=22e8c050-04b6-11eb-b538-2f689998b9eb; _ga=GA1.2.951161637.1600348938; __csrf=702ce218-de56-0080-d659-6b3295387394; G_ENABLED_IDPS=google; ajs_anonymous_id=%223f8637fa-14a3-42cb-960b-f8be90ab9a98%22; WZRK_G=49c0b0ba404a45f5a8dd7cd933b30fec; connect.sid=s%3AQaM4AUSjr-P5atfjvkhVI5tLYNZbO1Yz.5oXm0IH457HEqLSbGkPtxn3LW6T3enw82KzvshYNmso; PUBLIC_LEAGUE_BUSTER=20200917132719; d11partner=%7B%0A%20%20%22UserName%22%3A%20%22DREAMIPLAK11%22%2C%0A%20%20%22HasTeam%22%3A%201%2C%0A%20%20%22TeamName%22%3A%20%22DREAMIPLAK11%22%2C%0A%20%20%22FavTeamId%22%3A%20%221106%22%2C%0A%20%20%22SocialId%22%3A%20%2299731947%22%2C%0A%20%20%22GUID%22%3A%20%2255ad0698-f847-11ea-b8ff-023696af5d81%22%2C%0A%20%20%22ActiveTour%22%3A%20null%2C%0A%20%20%22IsTourActive%22%3A%200%2C%0A%20%20%22UserId%22%3A%20%22EF2DA77F1666588EBD%22%2C%0A%20%20%22TeamId%22%3A%20%22EF2DA77F1666588EBD%22%2C%0A%20%20%22ProfileURL%22%3A%20%22https%3A%2F%2Fwww.dream11.com%2Fpublic%2Fimgs%2Fleaderboard_default_image.png%22%2C%0A%20%20%22TeamName_Allow%22%3A%20%220%22%0A%7D; PRIVATE_LEAGUE_BUSTER=20200918092057; TEAM_BUSTER=20201009135509; autorefresh=20201010093326"
                    },
                    {"x-csrf", "702ce218-de56-0080-d659-6b3295387394"}
                },
                LeaderboardUrl =
                    "https://fantasy.iplt20.com/season/services/user/contest/2150102/leaderboard?optType=1&gamedayId=21&phaseId=1&pageNo=1&topNo=100&pageChunk=100&pageOneChunk=100&minCount=33&leagueId=2150102",
                TransferListUrl =
                    "https://fantasy.iplt20.com/season/services/user/55ad0698-f847-11ea-b8ff-023696af5d81/lb-team/overall?optType=2&teamgamedayId=1&arrtourGamedayId={0}&phaseId=1&teamId={1}&SocialId={2}"

            };

            return request;
        }

        public RequestData GetGokuldhamRequestData()
        {
            RequestData request = new RequestData
            {
                Headers = new Dictionary<string, string>
                {
                    {
                        "cookie",
                        "__csrf=48eee4f5-12c6-ccb1-bdb3-6f288b65cc3e; dh_user_id=17b78820-084d-11eb-a305-353939f488b4; G_ENABLED_IDPS=google; ajs_anonymous_id=%22a8aa2199-25d8-41d8-ba05-aa5c4f5747c8%22; WZRK_G=469fb23716da4311b46a79cfad01dd9e; connect.sid=s%3AFISryn9KeZJzhOerSj-BKH_eSWG8bjd4.aglvbUlCCYkf0sRJEE9Hll7OSsz44YE6%2BP%2FDDlnF8bY; WZRK_S_W4R-49K-494Z=%7B%22p%22%3A1%2C%22s%22%3A1602041317%2C%22t%22%3A1602041326%7D; d11partner=%7B%0A%20%20%22UserName%22%3A%20%22ANSHUL5086WX%22%2C%0A%20%20%22HasTeam%22%3A%201%2C%0A%20%20%22TeamName%22%3A%20%22ANSHUL5086WX%22%2C%0A%20%20%22FavTeamId%22%3A%20%221106%22%2C%0A%20%20%22SocialId%22%3A%20%2283119300%22%2C%0A%20%20%22GUID%22%3A%20%22e6e30ce8-fb44-11ea-9879-0ae60acb5e41%22%2C%0A%20%20%22ActiveTour%22%3A%20null%2C%0A%20%20%22IsTourActive%22%3A%200%2C%0A%20%20%22UserId%22%3A%20%22E921A771116F5C8FB8E1%22%2C%0A%20%20%22TeamId%22%3A%20%22E921A771116F5C8FB8E1%22%2C%0A%20%20%22ProfileURL%22%3A%20%22https%3A%2F%2Fwww.dream11.com%2Fpublic%2Fimgs%2Fleaderboard_default_image.png%22%2C%0A%20%20%22TeamName_Allow%22%3A%20%220%22%0A%7D"
                    },
                    {"x-csrf", "48eee4f5-12c6-ccb1-bdb3-6f288b65cc3e"}
                },
                LeaderboardUrl =
                    "https://fantasy.iplt20.com/season/services/user/contest/391740206/leaderboard?optType=1&gamedayId=18&phaseId=1&pageNo=1&topNo=100&pageChunk=100&pageOneChunk=100&minCount=9&leagueId=391740206",
                TransferListUrl =
                    "https://fantasy.iplt20.com/season/services/user/e6e30ce8-fb44-11ea-9879-0ae60acb5e41/lb-team/overall?optType=2&teamgamedayId=1&arrtourGamedayId={0}&phaseId=1&teamId={1}&SocialId={2}"
            };

            return request;
        }
    }
}
