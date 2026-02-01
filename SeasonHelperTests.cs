using RosterLib.Helpers;

namespace RosterLib.Tests
{
    [TestClass]
    public class SeasonHelperTests
    {
        [TestMethod]
        public void TestGetWeekWorksInThePostSeason()
        {
            var sundayOfWeek21 = new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Unspecified);
            var firstSundayOfSeason = new DateTime(2025, 9, 7, 0, 0, 0, DateTimeKind.Unspecified);
            var result = SeasonHelper.GetWeekOf(
                whenSunday: sundayOfWeek21,
                firstSundayOfSeason: firstSundayOfSeason,
                season: "2025");
            Assert.AreEqual(21, result);
        }
    }
}
