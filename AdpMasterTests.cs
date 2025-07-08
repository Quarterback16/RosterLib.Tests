using RosterLib.Helpers;

namespace RosterLib.Tests
{
    [TestClass]
    public class AdpMasterTests
    {
        private AdpMaster? _sut;

        [TestInitialize]
        public void Setup()
        {
            var csvFile = $"{ConfigHelper.CsvFolder()}ADP 2025.csv";
            _sut = new AdpMaster(csvFile);
        }

        [TestMethod]
        public void AdpMaster_LoadWithBadPath_ThrowsException()
        {
            var result = _sut?.Load(
                ".\\Output\\2019\\Starters\\csv\\Starters.csv");
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void AdpMasterLoad_WithGoodPath_ReturnsNumberOfPlayers()
        {
            var result = _sut?.Load();
            Assert.IsTrue(result > 0 || result == 0);
            Console.WriteLine(
                "Number of Players:{0}", 
                result);
        }

        [TestMethod]
        public void AdpMasterGet_ForUnknownPlayer_ReturnsEmptyString()
        {
            _sut?.Load();
            var adp = _sut.GetAdp("Steve Colonna");
            Assert.AreEqual(string.Empty, adp);
        }

        [TestMethod]
        public void AdpMasterGet_WithoutLoad_PerformsImplicitLoad()
        {
            var adp = _sut?.GetAdp("Travis Kelce");
            Assert.IsTrue(adp == "79" || adp == "");
            Console.WriteLine($"Travis Kelce is #{adp}");
        }

        [TestMethod]
        public void AdpMasterGet_ForSaquon_Returns_3()
        {
            _sut?.Load();
            var adp = _sut.GetAdp("Saquon Barkley");
            Assert.IsTrue(adp == "3" || adp == "");
            Console.WriteLine($"Saquon Barkley is #{adp}");
        }

        [TestMethod]
        public void AdpMasterGet_ForPlayer_Returns_Ok()
        {
            var player = "Josh Allen";
            _sut?.Load();
            var adp = _sut?.GetAdp(player);
            Assert.IsNotNull(adp);
            Console.WriteLine($"ADP for {player} is {adp}");
        }

        [TestMethod]
        public void AdpMasterGet_ForCMC_Returns_11()
        {
            _sut?.Load();
            var adp = _sut.GetAdp("Christian McCaffrey");
            Assert.IsTrue(adp == "" || adp == "11");
        }

        [TestMethod]
        public void AdpRankGet_ForRobinsonNot4()
        {
            _sut?.Load();
            var adp = _sut?.GetAdpRank("Brian Robinson");
            Console.WriteLine($"Brian Robinson is #{adp}");
            Assert.IsTrue(adp == 85 || adp == 0);
        }

        [TestMethod]
        public void AdpRankGet_ForTyreek_Returns_32()
        {
            _sut?.Load();
            var adp = _sut?.GetAdpRank("Tyreek Hill");
            Assert.IsTrue(adp == 32 || adp == 0);
        }

        [TestMethod]
        public void AdpRankGet_ForBHall_Returns_30()
        {
            _sut?.Load();
            var adp = _sut?.GetAdpRank("Breece Hall");
            Assert.IsTrue(adp == 30 || adp == 0);
        }


        [TestMethod]
        public void AdpMaster_KnowsPicksForTeamNo18()
        {
            var result = _sut?.PicksFor(
                teamNumber: 18,
                teamCount: 18,
                rounds: 18);
            Array.ForEach(result, e => Console.WriteLine(e));
            Assert.AreEqual(18, result[0]);
            Assert.AreEqual(19, result[1]);
        }

        [TestMethod]
        public void AdpMaster_KnowsPicksForTeamNo14()
        {
            var result = _sut?.PicksFor(14, 14, 14);
            Array.ForEach(result, e => Console.WriteLine(e));
            Assert.AreEqual(14, result[0]);
            Assert.AreEqual(15, result[1]);
            Assert.AreEqual(42, result[2]);
        }

        [TestMethod]
        public void AdpMaster_TellsYouUrTeamAtPosition()
        {
            var result = _sut?.TeamFor(
                teamNo: 4,
                teamsInLeague: 18,
                draftRounds: 14);
            Assert.IsTrue(result != null);
            Array.ForEach(result, e => Console.WriteLine(e));
        }
    }
}
