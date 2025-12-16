namespace RosterLib.Tests
{
    [TestClass]
    public class NflGameTests
    {
        [TestMethod]
        public void TestFantasyHomePlayers()
        {
            var testGame = new NFLGame("2025:14-B");
            var playerList = testGame.LoadAllFantasyHomePlayers(
                date: null,
                catFilter: "1");
            Assert.IsTrue(playerList.Count > 0);
            playerList.ForEach(p => Console.WriteLine(p.ToString()));
        }
    }
}
