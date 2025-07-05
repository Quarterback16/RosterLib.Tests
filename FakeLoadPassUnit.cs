using RosterLib.Interfaces;

namespace RosterLib.Tests
{
    //  creates Passing units based on a dummy teamCode
    public class FakeLoadPassUnit : ILoadPassUnit
    {
        public List<NFLPlayer> Load(
            string teamCode,
            string playerCat)
        {
            if (teamCode.Equals("NE"))
            {
                if (playerCat.Equals(Constants.K_QUARTERBACK_CAT))
                {
                    // standard formation
                    var playerList = new List<NFLPlayer>
                    {
                        new FakeNflPlayer( "QB01", "S", "QB", "Quincy the QB" ),
                        new FakeNflPlayer( "QB02", "B", "QB", "Rod the Ready" ),
                        new FakeNflPlayer( "QB03", "R", "QB", "Clipboard holder" )
                    };
                    return playerList;
                }
                else
                {
                    var playerList = new List<NFLPlayer>
                    {
                        new FakeNflPlayer( "WR01", "S", "WR,W1", "NE-Wideout 1" ),
                        new FakeNflPlayer( "WR02", "S", "WR,W2", "NE-Wideout 2" ),
                        new FakeNflPlayer( "WR03", "S", "WR,W3", "NE-Wideout 3" ),
                        new FakeNflPlayer( "TE01", "S", "TE", "NE-TE Starter" ),
                        new FakeNflPlayer( "TE02", "B", "TE", "NE-TE Backup" )
                    };
                    return playerList;
                }
            }
            else if (teamCode.Equals("AF"))
            {
                if (playerCat.Equals(Constants.K_QUARTERBACK_CAT))
                {
                    // standard formation
                    var playerList = new List<NFLPlayer>
                    {
                        new FakeNflPlayer( "QB04", "S", "QB", "Quard the QB" ),
                        new FakeNflPlayer( "QB05", "B", "QB", "Roopert the Ready" ),
                        new FakeNflPlayer( "QB06", "R", "QB", "Clipboard holder #2" )
                    };
                    return playerList;
                }
                else
                {
                    var playerList = new List<NFLPlayer>
                    {
                        new FakeNflPlayer( "WR06", "S", "WR,W1", "AF-Wideout 1" ),
                        new FakeNflPlayer( "WR07", "S", "WR,W2", "AF-Wideout 2" ),
                        new FakeNflPlayer( "WR08", "S", "WR,W3", "AF-Wideout 3" ),
                        new FakeNflPlayer( "TE03", "S", "TE", "AF-TE Starter" ),
                        new FakeNflPlayer( "TE04", "B", "TE", "AF-TE Backup" )
                    };
                    return playerList;
                }
            }
            else
            {
                return new List<NFLPlayer>();
            }
        }
    }
}
