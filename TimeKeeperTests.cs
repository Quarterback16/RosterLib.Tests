namespace RosterLib.Tests
{
    [TestClass]
    public class TimeKeeperTests
    {
        [TestMethod]
        public void TestWhatWeekItIs()
        {
            var sut = new TimeKeeper(clock: null);
            Console.WriteLine($"Season : {sut.Season} Week {sut.Week}");
            Console.WriteLine($"Schedule Available :{sut.ScheduleAvailable}");
            Console.WriteLine($"IsItPreseason      :{sut.IsItPreseason()}");
            Console.WriteLine($"IsItRegularSeason  :{sut.IsItRegularSeason()}");
            Console.WriteLine($"IsItPostSeason     :{sut.IsItPostSeason()}");
            Console.WriteLine($"IsItQuietTime      :{sut.IsItQuietTime()}");
            Console.WriteLine($"IsItPeakTime       :{sut.IsItPeakTime()}");
            Console.WriteLine($"IsItWednesday      :{sut.IsItWednesday(DateTime.Now)}");
            Console.WriteLine($"GetDateUsa         :{sut.GetDateUsa()}");
            Console.WriteLine($"CurrentSeason      :{sut.CurrentSeason()}");
            Console.WriteLine($"CurrentWeek        :{sut.CurrentWeek()}");
            Console.WriteLine($"PreviousWeek       :{sut.PreviousWeek()}");
            Console.WriteLine($"CurrentDateTime    :{sut.CurrentDateTime()}");
            Console.WriteLine($"PreviousSeason     :{sut.PreviousSeason()}");
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void TestActualCurrentSeason()
        {
            var sut = new TimeKeeper(
                clock: null);
            Assert.AreEqual(
                expected: "2024",
                actual: sut.CurrentSeason());
        }

        [TestMethod]
        public void TestCurrentSeason()
        {
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(2023, 06, 10,
                        0, 0, 0, DateTimeKind.Unspecified)));
            Assert.AreEqual(
                expected: "2023",
                actual: sut.CurrentSeason());
        }

        [TestMethod]
        public void TestWeekCutsOverOnUSTuesday()
        {
            int lastWeek = 0;
            for (int day = 1; day < 8; day++)
            {
                var testDate = new DateTime(2016, 12, day,
                    0, 0, 0, DateTimeKind.Unspecified);
                var sut = new TimeKeeper(
                    new FakeClock(testDate));
                Console.WriteLine(
                    "Our date {0,10:dddd} {0,10:d} {1} {2}",
                    testDate,
                    sut.Season,
                    sut.Week);
                if (testDate.ToString("dddd").Equals("Wednesday"))
                    Assert.IsTrue(Int32.Parse(sut.Week) > lastWeek);
                lastWeek = Int32.Parse(sut.Week);
            }
        }

        [TestMethod]
        public void TestWeekCutsOverOnUSTuesdayOurWed()
        {
            // 4th 12 2016 is Sunday
            var ourDate = new DateTime(2016, 12, 7); // equates to 6th US
            var sut = new TimeKeeper(
                    new FakeClock(ourDate));
            Console.WriteLine(
                    "{0,10:dddd} {0,10:d} {1} {2}",
                    ourDate,
                    sut.Season, // 2016
                    sut.Week);  // 14
            Assert.AreEqual("14", sut.Week);
        }

        [TestMethod]
        public void TestCurrentWeekIsZeroInMarch()
        {
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(2015, 03, 16)));  // set clock to March
            Assert.AreEqual(0, sut.CurrentWeek());
        }

        [TestMethod]
        public void TestGetUsDate()
        {
            var sut = new TimeKeeper(
                null);
            var result = sut.GetUsDate();
            Assert.AreEqual(
                DateTime.Now.AddDays(-1).Day,
                result.Day);
        }

        [TestMethod]
        public void TestCurrentSeasonPost()
        {
            var sut = new TimeKeeper(new FakeClock(new DateTime(2016, 02, 15)));  // set clock to Feb-2016
            Assert.AreEqual(sut.CurrentSeason(), "2015");
        }

        [TestMethod]
        public void TestCurrentSeason2016()
        {
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(2016, 08, 25)));  // set clock to Apr-2016
            Assert.AreEqual(
                "2016",
                sut.CurrentSeason());
        }

        [TestMethod]
        public void TestMondayMorning()
        {
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(2017, 10, 23, 2, 0, 0)));  // set clock to 2am on a Monday
            Assert.IsTrue(sut.IsItMondayMorning());
        }

        [TestMethod]
        public void TestTuesday()
        {
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(2017, 10, 24, 2, 0, 0)));  // set clock to 2am on a Tuesday
            Assert.IsTrue(sut.IsItTuesday());
        }

        [TestMethod]
        public void TestNotMondayMorning()
        {
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(2017, 10, 23, 14, 0, 0)));  // set clock to 2pm on a Monday
            Assert.IsFalse(sut.IsItMondayMorning());
        }

        [TestMethod]
        public void TestStartOfSeason2017()
        {
            var sut = new TimeKeeper(new FakeClock(new DateTime(2017, 08, 04)));
            var nextSunday = sut.GetSundayFor(new DateTime(2017, 8, 4));
            Assert.AreEqual(expected: new DateTime(2017, 9, 10), actual: nextSunday);
        }

        [TestMethod]
        public void TestPreseason()
        {
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(2015, 03, 16, 12, 0, 0)));  // set clock to March
            Assert.IsFalse(sut.IsItPreseason());
            Assert.IsFalse(sut.IsItRegularSeason());
            Assert.IsFalse(sut.IsItPostSeason());
        }

        [TestMethod]
        public void TestPreseasonLastSeason()
        {
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(2014, 03, 16)));  // set clock to March
            Assert.IsFalse(
                sut.IsItPreseason(),
                "Preseason is basically August");
            Assert.IsFalse(
                sut.IsItRegularSeason(),
                "March is not regular season");
            Assert.IsFalse(
                sut.IsItPostSeason(),
                "Post season is determined by Week");
        }

        [TestMethod]
        public void TestRegularSeason()
        {
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(
                        2015, 
                        10, 
                        16, 
                        12, 
                        0, 
                        0, 
                        DateTimeKind.Unspecified))); 
            Assert.IsFalse(sut.IsItPreseason());
            Assert.IsTrue(sut.IsItRegularSeason());
            Assert.IsFalse(sut.IsItPostSeason());
        }

        [TestMethod]
        public void TestRegularSeasonNow()
        {
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(
                        2023, 
                        6, 
                        10, 
                        12, 
                        0, 
                        0, 
                        DateTimeKind.Unspecified)));
            Assert.IsTrue(
                sut.IsItPreseason(),
                "Preseason starts in August");
            Assert.IsFalse(
                sut.IsItRegularSeason(),
                "Regular Season hasnt started yet");
            Assert.IsFalse(
                sut.IsItPostSeason(),
                "Post season is over");
        }

        [TestMethod]
        public void TestPostSeason()
        {
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(
                        2026, 
                        02, 
                        03, 
                        12, 
                        0, 
                        0,
                        DateTimeKind.Unspecified)));
            Assert.IsFalse(
                sut.IsItPreseason());
            Assert.IsFalse(
                sut.IsItRegularSeason());
            Assert.IsTrue(
                sut.IsItPostSeason(),
                "It should be the Post Season");
        }


        [TestMethod]
        public void TestPeakTime()
        {
            var testDateTime = new DateTime(
                2014, 
                09, 
                04, 
                3, 
                42, 
                0,
                DateTimeKind.Unspecified);
            var sut = new TimeKeeper(null);
            Assert.IsFalse(sut.IsItPeakTime(testDateTime));
        }

        [TestMethod]
        public void TestPeakTimeWhen6to1()
        {
            var testDateTime = new DateTime(
                2014, 
                09, 
                04, 
                13, 
                42, 
                0, 
                DateTimeKind.Unspecified);
            var sut = new TimeKeeper(null);
            Assert.IsTrue(sut.IsItPeakTime(testDateTime));
        }

        [TestMethod]
        public void TestTimekeeperKnowslastweek()
        {
            var testDate = new DateTime(
                2015, 
                11, 
                17, 
                12, 
                0, 
                0,
                DateTimeKind.Unspecified);
            var sut = new TimeKeeper(
                new FakeClock(testDate));
            Console.WriteLine(
                $"As of {testDate:g}");
            Console.WriteLine(
                "This week is {0}:{1}",
                sut.CurrentSeason(),
                sut.CurrentWeek());
            Console.WriteLine(
                "Last week is {0}:{1}",
                sut.CurrentSeason(),
                sut.PreviousWeek());
            Assert.IsTrue(
                sut.PreviousWeek().Equals("09"));
        }

        [TestMethod]
        public void TestSundayDump()
        {
            // Sat
            var sut = new TimeKeeper(
                new FakeClock(
                    new DateTime(
                        2026, 
                        12, 
                        31, 
                        0, 
                        0, 
                        0, 
                        DateTimeKind.Unspecified)));
            var result = sut.DumpSeasonSundays();
            Assert.IsTrue(result.Equals(22));
        }
    }
}
