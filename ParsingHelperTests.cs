using RosterLib.Helpers;

namespace RosterLib.Tests
{
    [TestClass]
    public class ParsingHelperTests
    {
        [TestMethod]
        public void SafeToInt_ReturnsCorrectValue()
        {
            Assert.AreEqual(3, ParsingHelper.SafeToInt("3"));
        }
    }
}
