using RosterLib.Implementations;
using WikiPages;

namespace RosterLib.Tests
{
    [TestClass]
    public class SoccerSeasonTests
    {
        [TestMethod]
        public void TestSeasonSlate()
        {
            var season = 2025;
            SeasonSlate("Arsenal", season, "EPL");
            //	SeasonSlate("Juventus", season, "S-A");
            //	SeasonSlate("Brisbane Roar", season, "AUS");
        }

        private void SeasonSlate(
            string team,
            int season,
            string league,
            bool toObsidian = false)
        {
            var scheduleService = new ScheduleMaster();
            var games = scheduleService.GetSchedule(
                team,
                league,
                season);
            var table = new WikiTable();
            table.Columns.Add(new WikiColumn("Rd"));
            table.Columns.Add(new WikiColumn("Date"));
            table.Columns.Add(new WikiColumn("Opponent"));
            table.Columns.Add(new WikiColumn("W"));
            table.Columns.Add(new WikiColumn("R"));
            table.Columns.Add(new WikiColumn("Score"));
            table.Columns.Add(new WikiColumn("Rec"));
            table.Columns.Add(new WikiColumn("Comment"));
            table.AddRows(games.Count);

            for (int i = 1; i < games.Count + 1; i++)
            {
                var game = Week(i, games);
                table.AddCell(i, 0, $"{i:0#}");
                table.AddCell(i, 1, $"{GameDate(game)}");
                table.AddCell(i, 2, $"{GameOpponent(game, team, league)}");
                table.AddCell(i, 3, $"{GameLocation(game, team)}");
            }
            table.Render();

            Console.WriteLine();
            Console.WriteLine("---");
            Console.WriteLine($"  ⏪ [[{team} {season - 1}]] ♦ [[{team} {season + 1}]] ⏩");
            Console.WriteLine();
            LinqPadFile($"Soccer-Season");
        }

        void LinqPadFile(string filename)
        {
            var lpf = new LinqpadFile(
                filename);
            Console.WriteLine($"{lpf.FileOut()}");
        }

        string GameDate(Models.Game game)
        {
            if (game == null)
                return "BYE";
            return game.GameDate.ToString("yyyy-MM-dd HH:mm");
        }

        string GameOpponent(
            Models.Game game, string team, string league)
        {
            if (game == null)
                return "BYE";
            var oppCode = game.OpponentOf(team);
            var oppName = oppCode;
            if (league.Equals("NFL"))
            {
                oppName = ConvertNflCode(oppCode);
                oppName = $"[[{oppName}|{oppCode}]]";
            }
            return oppName;
        }

        string GameLocation(Models.Game game, string team)
        {
            if (game == null)
                return "";
            if (game.IsHomeTeam(team))
                return "H";
            return "A";
        }

        RosterLib.Models.Game Week(int w, List<RosterLib.Models.Game> games)
        {
            var query = games.Where(g => g.Round.Equals(w))
                .FirstOrDefault();
            return query;
        }



        public static string ConvertNflTeam(string teamName)
        {
            if (teamName == "San Francisco 49ers")
                return "SF";
            if (teamName == "New Orleans Saints")
                return "NO";
            if (teamName == "Green Bay Packers")
                return "GB";
            if (teamName == "Philadelphia Eagles")
                return "PE";
            if (teamName == "Kansas City Chiefs")
                return "KC";
            if (teamName == "Houston Texans")
                return "HT";
            if (teamName == "Baltimore Ravens")
                return "BR";
            if (teamName == "New England Patriots")
                return "NE";
            if (teamName == "Denver Broncos")
                return "DB";
            if (teamName == "Buffalo Bills")
                return "BB";
            if (teamName == "Tennessee Titans")
                return "TT";
            if (teamName == "Minnesota Vikings")
                return "MV";
            if (teamName == "Las Vegas Raiders")
                return "OR";
            if (teamName == "Seattle Seahawks")
                return "SS";
            if (teamName == "Pittsburgh Steelers")
                return "PS";
            if (teamName == "Dallas Cowboys")
                return "DC";
            if (teamName == "Detroit Lions")
                return "DL";
            if (teamName == "Carolina Panthers")
                return "CP";
            if (teamName == "Los Angeles Rams")
                return "LR";
            if (teamName == "New York Jets")
                return "NJ";
            if (teamName == "Cleveland Browns")
                return "CL";
            if (teamName == "Indianapolis Colts")
                return "IC";
            if (teamName == "Los Angeles Chargers")
                return "LC";
            if (teamName == "New York Giants")
                return "NG";
            if (teamName == "Chicago Bears")
                return "CH";
            if (teamName == "Atlanta Falcons")
                return "AF";
            if (teamName == "Miami Dolphins")
                return "MD";
            if (teamName == "Cincinnati Bengals")
                return "CI";
            if (teamName == "Jacksonville Jaguars")
                return "JJ";
            if (teamName == "Washington Redskins")
                return "WR";
            if (teamName == "Tampa Bay Buccaneers")
                return "TB";
            if (teamName == "Arizona Cardinals")
                return "AC";
            return teamName;
        }
        public static string ConvertNflCode(string teamCode)
        {
            if (teamCode == "SF")
                return "San Francisco 49ers";
            if (teamCode == "NO")
                return "New Orleans Saints";
            if (teamCode == "GB")
                return "Green Bay Packers";
            if (teamCode == "PE")
                return "Philadelphia Eagles";
            if (teamCode == "KC")
                return "Kansas City Chiefs";
            if (teamCode == "HT")
                return "Houston Texans";
            if (teamCode == "BR")
                return "Baltimore Ravens";
            if (teamCode == "NE")
                return "New England Patriots";
            if (teamCode == "DB")
                return "Denver Broncos";
            if (teamCode == "BB")
                return "Buffalo Bills";
            if (teamCode == "TT")
                return "Tennessee Titans";
            if (teamCode == "MV")
                return "Minnesota Vikings";
            if (teamCode == "OR")
                return "Las Vegas Raiders";
            if (teamCode == "SS")
                return "Seattle Seahawks";
            if (teamCode == "PS")
                return "Pittsburgh Steelers";
            if (teamCode == "DC")
                return "Dallas Cowboys";
            if (teamCode == "DL")
                return "Detroit Lions";
            if (teamCode == "CP")
                return "Carolina Panthers";
            if (teamCode == "LR")
                return "Los Angeles Rams";
            if (teamCode == "NJ")
                return "New York Jets";
            if (teamCode == "CL")
                return "Cleveland Browns";
            if (teamCode == "IC")
                return "Indianapolis Colts";
            if (teamCode == "LC")
                return "Los Angeles Chargers";
            if (teamCode == "NG")
                return "New York Giants";
            if (teamCode == "CH")
                return "Chicago Bears";
            if (teamCode == "AF")
                return "Atlanta Falcons";
            if (teamCode == "MD")
                return "Miami Dolphins";
            if (teamCode == "CI")
                return "Cincinnati Bengals";
            if (teamCode == "JJ")
                return "Jacksonville Jaguars";
            if (teamCode == "WR")
                return "Washington Redskins";
            if (teamCode == "TB")
                return "Tampa Bay Buccaneers";
            if (teamCode == "AC")
                return "Arizona Cardinals";
            return teamCode;
        }
    }
}
