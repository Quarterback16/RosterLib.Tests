namespace RosterLib.Tests
{
    [TestClass]
    public class BalanceReportTests
    {
        [TestMethod]
        public void TestBalanceReportToMarkdown()
        {
            var tk = new FakeTimeKeeper("2025");
            var sut = new BalanceReport(tk);
            Assert.IsNotNull(sut);
            sut.RenderToMarkDown_TDR();
        }

        [TestMethod]
        public void TestDoBalanceReportJobAnyYear()
        {
            var tk = new FakeTimeKeeper("2026");
            var sut = new BalanceReport(tk);
            Assert.IsNotNull(sut);
            sut.Render(mdOnly: false);
            Console.WriteLine($"file sent to {sut.OutputFilename()}");
        }

    }
}
