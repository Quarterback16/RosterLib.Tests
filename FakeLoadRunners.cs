using RosterLib.Interfaces;

namespace RosterLib.Tests
{
    //  Fakeloadrunner creates Rushing units based on a dummy teamCode
    public class FakeLoadRunners : ILoadRunners
    {
        public List<NFLPlayer> Load(string teamCode)
        {
            if (teamCode.Equals("NE"))
            {
                // standard 
                var playerList = new List<NFLPlayer>
                {
                    new FakeNflPlayer( "JS01", "S", "RB", "Jonny the Starter" ),
                    new FakeNflPlayer( "BB01", "B", "RB,3D", "Buddy the Backup" )
                };
                return playerList;
            }
            else if (teamCode.Equals("AF"))
            {
                // standard
                var playerList = new List<NFLPlayer>
                {
                    new FakeNflPlayer( "VV01", "S", "RB", "Vick the Vet", "3" ),
                    new FakeNflPlayer( "BB02", "B", "RB", "Bro the Backup" ),
                    new FakeNflPlayer( "VU01", "R", "RB,SH", "Vulture the Goalline specialist" ),
                };
                return playerList;
            }
            else if (teamCode.Equals("BB"))
            {
                //  Ace
                var playerList = new List<NFLPlayer>
                {
                    new FakeNflPlayer( "BM01", "S", "RB,SH", "Beast Mode", "1" ),
                    new FakeNflPlayer( "VU01", "R", "RB,3D", "Trippy the 3rd down specialist" ),
                };
                return playerList;
            }
            else if (teamCode.Equals("BR"))
            {
                //  Committe
                var playerList = new List<NFLPlayer>
                {
                    new FakeNflPlayer( "CM01", "S", "RB,SH", "Committee 1" ),
                    new FakeNflPlayer( "CM02", "S", "RB,3D", "Committee 2" ),
                };
                return playerList;
            }
            else
            {
                return new List<NFLPlayer>();
            }
        }
    }
}
