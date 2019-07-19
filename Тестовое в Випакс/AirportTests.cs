using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;

namespace AirportScoreboard
{
	[TestFixture]
	class AirportTests
	{
		[Test]
		public void DefaultValues()
		{
			var path = Directory.GetCurrentDirectory() + "\\Example.txt";
			string[] strs = File.ReadAllLines(path);
			Airport airport = new Airport(strs);
			airport.AddTimeInMinutes(600);
			Assert.IsTrue(
				airport.currentTime.Hour == 17 &&
				airport.DepInLastDay == 0 &&
				airport.DepInLastFlight == 0 &&
				airport.RecentArrival.City == "Moscow" &&
				airport.RecentArrival.Model == "Boeing 738" &&
				airport.ArrSummary > -1
				);
		}

		[Test]
		public void ZeroValues()
		{
			string[] strs = new string[0];
			Assert.Throws<IndexOutOfRangeException>(() => new Airport(strs));
		}

		[Test]
		public void EndOfSchedule()
		{
			var path = Directory.GetCurrentDirectory() + "\\Example.txt";
			string[] strs = File.ReadAllLines(path);
			Airport airport = new Airport(strs);
			airport.AddTimeInMinutes(10000000);
			Assert.IsTrue(airport.DepInLastDay == 0 && airport.ArrInLastDay == 0);
		}

		[Test]
		public void SchedulePointer()
		{
			var path = Directory.GetCurrentDirectory() + "\\Example.txt";
			string[] strs = File.ReadAllLines(path);
			Airport airport = new Airport(strs);
			airport.AddTimeInMinutes(1);
			airport.AddTimeInMinutes(1);
			airport.AddTimeInMinutes(1);
			airport.AddTimeInMinutes(1);
			Assert.IsTrue(airport.RecentArrival.Time.Minute == 7 && airport.RecentArrival.Time.Hour == 7);
		}
	}
}
