namespace RosterLib.Tests
{
    [TestClass]
    public class FaMarketTests
    {
        [TestMethod]
        public void TestFaMarket()
        {
            var timekeeper = new TimeKeeper(null);
            var report = new FaMarket(timekeeper);
            report.DoReport();
            var teams = report.GetTeams();
            Assert.IsNotNull(teams);
            Assert.IsTrue(teams.Rows.Count > 0);
        }
    }
}
