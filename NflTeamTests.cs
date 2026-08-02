namespace RosterLib.Tests
{
    [TestClass]
    public class NflTeamTests
    {
        [TestMethod]
        public void TestNflTeamLoadGames()
        {
            var teamCode = "SS";
            var team = new NflTeam(teamCode);
            Assert.IsNotNull(team);

            // Call the void method (do not assign)
            team.LoadGames(teamCode, sSeason: "2025");

            // Assert on a property that LoadGames should populate, e.g. NflGameList or GameList
            Assert.IsNotNull(team.GameList);
            Assert.IsTrue(
                team.GameList.Count > 0 
                && team.GameList != null); // adjust to expected behavior
        }
    }
}
