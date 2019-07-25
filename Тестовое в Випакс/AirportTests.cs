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
			var path = "Schedule.txt";
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
		public void FarEndOfSchedule()
		{
			var path = "Schedule.txt";
			string[] strs = File.ReadAllLines(path);
			Airport airport = new Airport(strs);
			airport.AddTimeInMinutes(10000000);
			Assert.IsTrue(airport.DepInLastDay == 0 && airport.ArrInLastDay == 0);
		}

		[Test]
		public void SchedulePointer()
		{
			var path = "Schedule.txt";
			string[] strs = File.ReadAllLines(path);
			Airport airport = new Airport(strs);
			airport.AddTimeInMinutes(1);
			airport.AddTimeInMinutes(1);
			airport.AddTimeInMinutes(1);
			airport.AddTimeInMinutes(1);
			Assert.IsTrue(airport.RecentArrival.Time.Minute == 7 && airport.RecentArrival.Time.Hour == 7);
		}

		[Test]
		public void EdgeIsAdded()
		{
			string[] strs = new string[3];
			strs[0] = "2019 7 22 7 7 Arrival Minsk Boeing 738";
			strs[1] = "2019 7 22 9 8 Arrival Moscow Sukhoi Superjet 100";
			strs[2] = "2019 7 22 11 47 Departure Kiev Boeing 739";
			Airport airport = new Airport(strs);
			airport.AddTimeInMinutes(280);
			Assert.IsTrue(airport.RecentArrival.City == "Moscow" &&
				airport.RecentArrival.Model == "Sukhoi Superjet 100" &&
				airport.RecentDeparture.Model == "Boeing 739" &&
				airport.RecentArrival.Time.Minute == 8);
		}

		[Test]
		public void Dequeueing() // We cant make correct Asserts, because passengers amount depends on random. So just watch debugging.
		{
			string[] strs = new string[3];
			strs[0] = "2019 7 22 7 7 Arrival Minsk Boeing 739";
			strs[1] = "2019 7 23 5 7 Arrival Moscow Sukhoi Superjet 100";
			strs[2] = "2019 7 24 3 7 Arrival Kiev Boeing 739";
			Airport airport = new Airport(strs);
			airport.AddTimeInMinutes(24 * 60 + 1);
			airport.AddTimeInMinutes(24 * 60 + 1);
		}

		[Test] 
		public void AddOneMin()
		{
			string[] strs = new string[2];
			strs[0] = "2019 7 22 7 7 Arrival Minsk Boeing 739";
			strs[1] = "2019 7 22 7 8 Arrival Moscow Boeing 739";
			Airport airport = new Airport(strs);
			var recArr = airport.RecentArrival.Passengers;
			System.Threading.Thread.Sleep(17);
			airport.AddTimeInMinutes(1);
			Assert.IsTrue(airport.RecentArrival.Passengers != recArr);
		}
	}
}
