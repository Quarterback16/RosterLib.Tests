namespace RosterLib.Tests
{
    [TestClass]
    public class StartersTests
    {
        [TestMethod]
        public void StartersRunsOk()
        {
            var report = new Starters(
                new TimeKeeper(null));

            report.DoReport();

            Assert.IsNotNull(report);
        }
    }
}
