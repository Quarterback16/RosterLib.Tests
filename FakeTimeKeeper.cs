﻿using RosterLib.Interfaces;
using System.Globalization;

namespace RosterLib.Tests
{

    public class FakeTimeKeeper : IKeepTheTime
    {
        public string Season { get; set; }

        public string Week { get; set; }

        public bool _isItPreseason { get; set; }

        public bool _isPeakTime { get; set; }

        public bool _isPostSeason { get; set; }

        public DateTime TheDateTime { get; set; }

        public FakeTimeKeeper()
        {
            Season = "2017";
            _isItPreseason = true;
            _isPostSeason = true;
            _isPeakTime = false;
            TheDateTime = DateTime.Now;
        }

        public FakeTimeKeeper(DateTime theDate)
        {
            Season = "2017";
            _isItPreseason = true;
            _isPeakTime = false;
            TheDateTime = theDate;
            Week = "00";
        }

        public FakeTimeKeeper(
            bool isPreSeason,
            bool isPeakTime)
        {
            Season = "2017";
            Week = isPreSeason ? "00" : "01";

            _isItPreseason = isPreSeason;
            _isPeakTime = isPeakTime;

            TheDateTime = DateTime.Now;
        }

        public FakeTimeKeeper(
            string season,
            string week,
            DateTime theDate)
        {
            Season = season;
            Week = week;
            TheDateTime = theDate;
        }


        public FakeTimeKeeper(
            string season)
        {
            Season = season;
            Week = "00";
            TheDateTime = DateTime.Now;
        }

        public FakeTimeKeeper(
            string season,
            string week)
        {
            Season = season;
            Week = week;
            TheDateTime = DateTime.Now;
        }

        public bool IsItMondayMorning()
        {
            var isIt = false;
            var currTime = CurrentDateTime();
            if (currTime.DayOfWeek == DayOfWeek.Monday && currTime.Hour < 12)
            {
                isIt = true;
            }
            return isIt;
        }

        public bool IsItWednesdayOrThursday(DateTime focusDate)
        {
            return false;
        }

        public bool IsItSaturdayOrSunday(
            DateTime focusDate)
        {
            return false;
        }

        public bool IsItFriday(DateTime focusDate)
        {
            return false;
        }

        public bool IsItSaturday(DateTime focusDate)
        {
            return false;
        }

        public bool IsItWednesday(DateTime focusDate)
        {
            return false;
        }

        public bool IsItOffSeason()
        {
            return true;
        }

        public bool IsItPreseason()
        {
            return _isItPreseason;
        }

        public bool IsItPostSeason()
        {
            var nWeek = Int32.Parse(Week);
            return nWeek > 17;
        }

        public bool IsItRegularSeason()
        {
            var nWeek = Int32.Parse(Week);
            return nWeek > 0 && nWeek < 18;
        }

        public bool IsItQuietTime()
        {
            return false;
        }

        public bool IsItPeakTime()
        {
            return _isPeakTime;
        }

        public DateTime GetDateUsa()
        {
            return TheDateTime;
        }

        public bool IsDateDaysOld(int daysOld, DateTime theDate)
        {
            return true;
        }

        public string CurrentSeason(DateTime focusDate)
        {
            return Season;
        }

        public string CurrentSeason()
        {
            return Season;
        }

        public string PreviousWeek()
        {
            if (Week.Equals("00"))
                return "17";
            var currentWeek = CurrentWeek(CurrentDateTime());

            var previousWeek = currentWeek - 1;
            return string.Format("{0:0#}", previousWeek);
        }

        public string PreviousSeason(DateTime focusDate)
        {
            throw new NotImplementedException();
        }

        public string PreviousSeason()
        {
            var ps = Int32.Parse(Season) - 1;
            return ps.ToString(CultureInfo.InvariantCulture);
        }

        public int CurrentWeek(DateTime focusDate)
        {
            return Int32.Parse(Week);
        }

        public bool IsItFridaySaturdayOrSunday(DateTime focusDate)
        {
            return focusDate.DayOfWeek == DayOfWeek.Friday || focusDate.DayOfWeek == DayOfWeek.Saturday || focusDate.DayOfWeek == DayOfWeek.Sunday;
        }

        public DateTime CurrentDateTime()
        {
            return TheDateTime;
        }

        public bool IsItMonday()
        {
            var focusDate = CurrentDateTime();
            return focusDate.DayOfWeek == DayOfWeek.Monday;
        }

        public bool IsItSaturday()
        {
            var focusDate = CurrentDateTime();
            return focusDate.DayOfWeek == DayOfWeek.Saturday;
        }

        public bool IsItSunday()
        {
            var focusDate = CurrentDateTime();
            return focusDate.DayOfWeek == DayOfWeek.Sunday;
        }

        public bool IsItTuesday()
        {
            var focusDate = CurrentDateTime();
            return focusDate.DayOfWeek == DayOfWeek.Tuesday;
        }

        public DateTime GetSundayFor(DateTime when)
        {
            var theSeason = Utility.SeasonFor(when);
            var theSunday = Utility.TflWs.GetSeasonStartDate(theSeason);
            if (when <= theSunday)
                return theSunday;
            for (var i = 1; i < 16; i++)
            {
                var sunday = theSunday.AddDays(i * 7);
                if (when > sunday) continue;
                theSunday = sunday;
                break;
            }
            return theSunday;
        }

        public int CurrentWeek()
        {
            return CurrentWeek(GetDateUsa());
        }

        public bool IsItThursday(DateTime focusDate)
        {
            return false;
        }

        public bool IsItThursday()
        {
            var focusDate = CurrentDateTime();
            return focusDate.DayOfWeek == DayOfWeek.Thursday;
        }

        public DateTime NflStartDate() =>

            new DateTime(
                Int32.Parse(CurrentSeason()),
                3,
                1,
                0,
                0,
                0,
                DateTimeKind.Unspecified);


        public DateTime NflEndDate() =>

            new DateTime(
                int.Parse(NextSeason()),
                28,
                2,
                0,
                0,
                0,
                DateTimeKind.Unspecified);

        public string NextSeason() =>

            (Int32.Parse(CurrentSeason()) + 1).ToString();

        public bool IsItStartOfSeason()
        {
            throw new NotImplementedException();
        }

        public DateTime GetRatingSundayFor(DateTime when)
        {
            throw new NotImplementedException();
        }
    }
}
