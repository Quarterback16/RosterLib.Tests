namespace RosterLib.Tests
{
    [TestClass]
    public class UnitPredictorTests
    {
        public TimeKeeper? _tk;

        [TestInitialize]
        public void Setup()
        {
            _tk = new TimeKeeper(clock: null);
        }

        [TestMethod]
        public void TestUnitPredictorPredictGame()
        {
            var predictor = new UnitPredictor
            {
                TakeActuals = true,
                AuditTrail = true,
                WriteProjection = true,
                StorePrediction = false,
                RatingsService = new UnitRatingsService(_tk)
            };
            var game = new NFLGame("2025:01-A");
            var result = predictor.PredictGame(
                game: game,
                persistor: new FakePredictionStorer(),
                predictionDate: new DateTime(2025, 06, 25, 0, 0, 0, DateTimeKind.Unspecified));

            Console.WriteLine(
                $@"Prediction: {result.PredictedScore()}");

            Assert.IsTrue(
                result.HomeWin());
        }

        [TestMethod]
        public void TestSundayForReturnsFirstSunday()
        {
            var theSunday = _tk?.GetRatingSundayFor(
                new DateTime(2025,6,26,0,0,0,DateTimeKind.Unspecified));
            Assert.IsTrue(theSunday.Equals(
                new DateTime(2025,9,7,0,0,0,DateTimeKind.Unspecified)));
        }

        [TestMethod]
        public void TestUnitPredictorPredict_IC_Game()
        {
            var predictor = new UnitPredictor
            {
                TakeActuals = true,
                AuditTrail = true,
                WriteProjection = false,
                StorePrediction = false,
                RatingsService = new UnitRatingsService(
                    new TimeKeeper(
                        clock: null))
            };
            var game = new NFLGame(
                "2020:01-B");  //  IC @ JJ  6.5 to the colts
            var result = predictor.PredictGame(
                game: game,
                persistor: new FakePredictionStorer(),
                predictionDate: new DateTime(2020, 08, 21, 0, 0, 0, DateTimeKind.Unspecified));
            Assert.IsTrue(result.AwayWin());
            Assert.IsTrue(
                result.HomeScore.Equals(20),
                $"Home score should be 20 not {result.HomeScore}");
            Assert.IsTrue(
                result.AwayScore.Equals(24),
                $"Away score should be 24 not {result.AwayScore}");
        }

        [TestMethod]
        public void TestUnitPredictorPredict_OpenningGame()
        {
            var predictor = new UnitPredictor
            {
                TakeActuals = true,
                AuditTrail = true,
                WriteProjection = false,
                StorePrediction = false,
                RatingsService = new UnitRatingsService(
                    new TimeKeeper(
                        clock: null))
            };
            var game = new NFLGame(
                "2024:01-A");  //  BR @ KC  
            var result = predictor.PredictGame(
                game: game,
                persistor: new FakePredictionStorer(),
                predictionDate: new DateTime(2024, 07, 12, 0, 0, 0, DateTimeKind.Unspecified));
            Assert.IsTrue(result.HomeWin());
            Assert.IsTrue(
                result.HomeScore.Equals(41),
                $"Home score should be 41 not {result.HomeScore}");
            Assert.IsTrue(
                result.AwayScore.Equals(31),
                $"Away score should be 31 not {result.AwayScore}");
        }

        [TestMethod]
        public void TestUnitPredictorPredict_BigGame()
        {
            var predictor = new UnitPredictor
            {
                TakeActuals = true,
                AuditTrail = true,
                WriteProjection = false,
                StorePrediction = false,
                RatingsService = new UnitRatingsService(
                    new TimeKeeper(
                        clock: null))
            };
            var game = new NFLGame(
                "2023:16-P");  //  BR @ SF  41-17 to the Niners
            var result = predictor.PredictGame(
                game: game,
                persistor: new FakePredictionStorer(),
                predictionDate: new DateTime(2023, 12, 23));

            Assert.IsTrue(result.HomeWin());

            Console.WriteLine(result.Explanation.ToString());
        }
    }
}
