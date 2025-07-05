﻿namespace RosterLib.Tests
{
    public class FakeNflGame : NFLGame
    {
        public FakeNflGame()
        {
            //  fictional game AF @ NE

            Season = "2016";
            Week = "01";
            AwayNflTeam = new FakeNflTeam();
            var ru = new FakeRushUnit();
            ru.Load("AF");
            AwayNflTeam.RunUnit = ru;

            var puAway = new FakePassUnit();
            puAway.Load("AF");
            AwayNflTeam.PassUnit = puAway;

            HomeNflTeam = new FakeNflTeam();
            var homeru = new FakeRushUnit();
            homeru.Load("NE");
            HomeNflTeam.RunUnit = homeru;

            var puHome = new FakePassUnit();
            puHome.Load("NE");
            HomeNflTeam.PassUnit = puHome;

            PlayerGameMetrics = new List<PlayerGameMetrics>();
        }

        public override NFLResult GetPrediction(string method)
        {
            var dummyResult = new NFLResult()
            {
                HomeTeam = "NE",
                AwayTeam = "AF",
                HomeScore = 38,
                AwayScore = 34,

                HomeTDp = 3,
                HomeYDp = 430,
                HomeTDr = 2,
                HomeFg = 1,
                HomeTDd = 0,
                HomeTDs = 0,
                HomeYDr = 112,

                AwayTDp = 3,
                AwayYDp = 330,
                AwayTDr = 2,
                AwayFg = 3,
                AwayTDd = 0,
                AwayTDs = 0,
                AwayYDr = 82
            };
            return dummyResult;
        }
    }
}
