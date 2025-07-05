using RosterLib.Helpers;
using RosterLib.Implementations;
using RosterLib.Services;

namespace RosterLib.Tests
{
    [TestClass]
    public class RushUnitTests
    {
        [TestMethod]
        public void Test_GB_Has_A_Starter()
        {
            var team = new NflTeam("GB")
            {
                RunUnit = new RushUnit()
            };
            team.RunUnit.Load("GB");
            Console.WriteLine($"Run approach is {team.RunUnit.DetermineApproach()}");
            Assert.IsTrue(team.RunUnit.Starters.Count > 0);
        }

        [TestMethod]
        public void TestFakeData_BB_HasAnAceBack()
        {
            var team = new FakeNflTeam()
            {
                RunUnit = new FakeRushUnit()
            };
            team.RunUnit.Load("BB");
            Assert.IsTrue(team.RunUnit.IsAceBack);
        }

        [TestMethod]
        public void TestFakeData_NE_HasTwoRunners()
        {
            var team = new FakeNflTeam()
            {
                RunUnit = new FakeRushUnit()
            };
            team.RunUnit.Load("NE");
            Assert.IsTrue(team.RunUnit.Runners.Count == 2);
        }

        [TestMethod]
        public void TestFakeData_NE_HasNoIntegrityErrors()
        {
            var team = new FakeNflTeam()
            {
                RunUnit = new FakeRushUnit()
            };
            team.RunUnit.Load("NE");
            Assert.IsFalse(team.RunUnit.HasIntegrityError());
        }

        [TestMethod]
        public void TestFakeData_AF_HasNoIntegrityErrors()
        {
            var team = new FakeNflTeam()
            {
                RunUnit = new FakeRushUnit()
            };
            team.RunUnit.Load("AF");
            Assert.IsFalse(team.RunUnit.HasIntegrityError());
        }

        [TestMethod]
        public void TestFakeData_BB_HasNoIntegrityErrors()
        {
            var team = new FakeNflTeam()
            {
                RunUnit = new FakeRushUnit()
            };
            team.RunUnit.Load("BB");
            Assert.IsFalse(team.RunUnit.HasIntegrityError());
        }

        [TestMethod]
        public void TestFakeData_BR_HasNoIntegrityErrors()
        {
            var team = new FakeNflTeam()
            {
                RunUnit = new FakeRushUnit()
            };
            team.RunUnit.Load("BR");
            Assert.IsFalse(team.RunUnit.HasIntegrityError());
        }

        [TestMethod]
        public void TestFakeData_BR_ApproachIs_Committee()
        {
            var team = new FakeNflTeam()
            {
                RunUnit = new FakeRushUnit()
            };
            team.RunUnit.Load("BR");
            Assert.AreEqual(expected: RunApproach.Committee,
                actual: team.RunUnit.DetermineApproach());
        }

        [TestMethod]
        public void TestFakeData_BB_ApproachIs_Ace()
        {
            var team = new FakeNflTeam()
            {
                RunUnit = new FakeRushUnit()
            };
            team.RunUnit.Load("BB");
            Assert.AreEqual(expected: RunApproach.Ace,
                actual: team.RunUnit.DetermineApproach());
        }

        [TestMethod]
        public void TestFakeData_NE_ApproachIs_Standard()
        {
            var team = new FakeNflTeam()
            {
                RunUnit = new FakeRushUnit()
            };
            team.RunUnit.Load("NE");
            Assert.AreEqual(expected: RunApproach.Standard,
                actual: team.RunUnit.DetermineApproach());
        }


        [TestMethod]
        public void TestLoad()
        {
            var team = new NflTeam("PS");
            team.LoadRushUnit();
            Console.WriteLine("   >>> Rush unit loaded {0} rushers; Ace back {1}",
               team.RunUnit.Runners.Count, team.RunUnit.AceBack);
            Assert.IsTrue(team.RunUnit.Runners.Count < 50 && team.RunUnit.Runners.Count > 0);
            Assert.IsFalse(team.RunUnit.HasIntegrityError());
        }

        [TestMethod]
        public void TestDoubleLoad()
        {
            var team = new NflTeam("PS");
            var ru = team.LoadRushUnit();
            Console.WriteLine("   >>> Rush unit loaded {0} rushers; Ace back {1}",
               team.RunUnit.Runners.Count, team.RunUnit.AceBack);
            var count1 = ru.Count;
            var ru2 = team.LoadRushUnit();
            var count2 = ru2.Count;
            Assert.IsTrue(count1 == count2);
            Assert.IsFalse(team.RunUnit.HasIntegrityError());
        }

        [TestMethod]
        public void BR_RunUnit_UsesStdQb()
        {
            var team = new NflTeam("BR");
            team.LoadRushUnit(
                new RunApproachService());
            Assert.AreEqual(
                RunApproach.StandardQb,
                team.RunUnit.DetermineApproach(
                    new RunApproachService()));
        }

        [TestMethod]
        public void CanLoadAllRushUnits()
        {
            var season = new NflSeason("2025");
            var service = new RunApproachService();
            season.TeamList.ForEach(t => t.LoadRushUnit(
                service));
            Assert.AreEqual(
                32,
                season.TeamList.Count);

            var md = SeasonHelper.RushUnitsToMarkdown(
                season,
                new MarkdownInjector());
            Assert.IsFalse(string.IsNullOrEmpty(md));
            Console.WriteLine(new String('+', 80));
            Console.WriteLine(md);
            Console.WriteLine(new String('+', 80));
        }

        [TestMethod]
        public void CanInjectAllRushUnitProjections()
        {
            var mi = new MarkdownInjector();
            var season = new NflSeason("2024");
            season.TeamList.ForEach(t => t.LoadRushUnit(
                new RunApproachService()));
            Assert.AreEqual(32, season.TeamList.Count);

            season.TeamList.ForEach(t =>
            {
                var md = SeasonHelper.RushUnitsTdrDistributionToMarkdown(
                    season,
                    t.TeamCode);
                Assert.IsFalse(string.IsNullOrEmpty(md));
                Console.WriteLine(md);
                mi.InjectMarkdown(
                    targetfile: $"{season}\\Teams\\{t.TeamCode}.md",
                    tagName: "ru-proj",
                    md);
            });
        }

        [TestMethod]
        public void CanShowTdrRushUnits()
        {
            var md = SeasonHelper.RushUnitsTdrDistributionToMarkdown(
                season: new NflSeason("2024"),
                teamCode: "GB");
            Assert.IsFalse(string.IsNullOrEmpty(md));
            Console.WriteLine(md);
        }

        [TestMethod]
        public void CanShowTdrRushUnitsForAceQb()
        {
            var md = SeasonHelper.RushUnitsTdrDistributionToMarkdown(
                season: new NflSeason("2024"),
                teamCode: "BB");
            Assert.IsFalse(string.IsNullOrEmpty(md));
            Console.WriteLine(md);
        }

        [TestMethod]
        public void CanShowTdrRushUnitsForAce()
        {
            var md = SeasonHelper.RushUnitsTdrDistributionToMarkdown(
                season: new NflSeason("2024"),
                teamCode: "DB");
            Assert.IsFalse(string.IsNullOrEmpty(md));
            Console.WriteLine(md);
        }

        [TestMethod]
        public void CanShowTdrRushUnitsForCommittee()
        {
            var md = SeasonHelper.RushUnitsTdrDistributionToMarkdown(
                season: new NflSeason("2024"),
                teamCode: "CH");
            Assert.IsFalse(string.IsNullOrEmpty(md));
            Console.WriteLine(md);
        }

        [TestMethod]
        public void CanShowTdrRushUnitsForTwin()
        {
            var md = SeasonHelper.RushUnitsTdrDistributionToMarkdown(
                season: new NflSeason("2024"),
                teamCode: "AF");
            Assert.IsFalse(string.IsNullOrEmpty(md));
            Console.WriteLine(md);
        }

        [TestMethod]
        public void CanShowTdrRushUnitsForStandard()
        {
            var md = SeasonHelper.RushUnitsTdrDistributionToMarkdown(
                season: new NflSeason("2024"),
                teamCode: "KC");
            Assert.IsFalse(string.IsNullOrEmpty(md));
            Console.WriteLine(md);
        }

        [TestMethod]
        public void RunApproachServiceCanReadNames()
        {
            var sut = new RunApproachService();
            var result = sut.GetRunApproach("DL");
            Assert.AreEqual(RunApproach.Twin, result);
        }

        [TestMethod]
        public void CanLoadSingleRushUnit()
        {
            var team = new NflTeam("AC");
            team.LoadRushUnit(
                new RunApproachService());
            Assert.IsNotNull(team);
            Console.WriteLine(
                RushUnitHelper.DumpUnitByTouches(
                    team.RunUnit,
                    new TimeKeeper(clock: null)));
        }
    }
}
