namespace RosterLib.Tests
{
    [TestClass]
    public class StrengthOfScheduleTests
    {
        private StrengthOfSchedule? _sut;

        [TestInitialize]
        public void Setup()
        {
            _sut = new StrengthOfSchedule(
                new TimeKeeper(clock:null));
        }

        [TestMethod]
        public void StrengthOfSchedule_SendsMarkdown()
        {
            var md = _sut?.RenderAsMarkdown();
            Assert.IsFalse(
                string.IsNullOrWhiteSpace(md), 
                "Markdown should not be empty or null.");
            Console.WriteLine(md);
        }

    }
}
