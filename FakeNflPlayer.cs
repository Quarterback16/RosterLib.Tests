namespace RosterLib.Tests
{
    public class FakeNflPlayer : NFLPlayer
    {
        public FakeNflPlayer(
            string id, string role, string posDesc, string name,
            string injury = "0")
        {
            PlayerCode = id;
            PlayerName = name;
            PlayerRole = role;
            PlayerPos = posDesc;
            Injury = injury;
        }
    }
}
