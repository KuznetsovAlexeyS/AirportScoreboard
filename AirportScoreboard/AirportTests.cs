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
			string[] strs = new string[]
			{
				"2019 7 22 7 7 Arrival Minsk Boeing 739",
				"2019 7 22 8 8 Arrival Kiev Boeing 739",
				"2019 7 22 9 9 Arrival Minsk Sukhoi Superjet 100",
				"2019 7 22 10 10 Arrival Moscow Boeing 738",
				"2019 7 22 11 11 Arrival Minsk Boeing 739",
			};
			Airport airport = new Airport(strs);
			airport.AddTimeInMinutes(600);
			Assert.IsTrue(
				airport.currentTime.Hour == 17 &&
				airport.DepInLastDay == 0 &&
				airport.DepInLastFlight == 0 &&
				airport.RecentArrival.City == "Minsk" &&
				airport.RecentArrival.Model == "Boeing 739" &&
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
		public void Dequeueing() // Общий случай удаления из очереди, см. отладку.
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
			System.Threading.Thread.Sleep(17); // Иначе случайно сгенерированное количество пассажиров будет одинаково в обоих случаях. 
			airport.AddTimeInMinutes(1);
			Assert.IsTrue(airport.RecentArrival.Passengers != recArr);
		}

		[Test]
		public void ScheduleCorrectness()
		{
			string[] strs = new string[2];
			strs[0] = "2019 7 22 7 7 Arrival Minsk Boeing 739";
			strs[1] = "2019 7 22 7 5 Arrival Moscow Boeing 739";
			Assert.Throws<ArgumentException>(() => new Airport(strs));
		}

		[Test]
		public void PassangeersByHours() // Число пассажиров в самолёте может быть и 0, так что если тест красный, попробуйте запустить его ещё раз.
		{
			string[] strs = new string[8];
			strs[0] = "2019 7 22 7 7 Arrival Minsk Boeing 739";
			strs[1] = "2019 7 22 8 6 Arrival Moscow Sukhoi Superjet 100";
			strs[2] = "2019 7 22 9 5 Arrival Kiev Boeing 739";
			strs[3] = "2019 7 22 10 4 Arrival Kiev Boeing 739";
			strs[4] = "2019 7 22 11 3 Departure Minsk Boeing 739";
			strs[5] = "2019 7 22 12 2 Departure Moscow Sukhoi Superjet 100";
			strs[6] = "2019 7 22 13 1 Departure Kiev Boeing 739";
			strs[7] = "2019 7 22 14 0 Departure Kiev Boeing 739";
			Airport airport = new Airport(strs);
			for(int i=0; i<7 * 60; i++)
			{
				airport.AddTimeInMinutes(1);
				System.Threading.Thread.Sleep(3);
			}
			Assert.IsTrue(
				airport.ArrInDayByHours[0] == 0 &&
				airport.ArrInDayByHours[1] == 0 &&
				airport.ArrInDayByHours[2] == 0 &&
				airport.ArrInDayByHours[3] == 0 &&
				airport.ArrInDayByHours[4] != 0 &&
				airport.ArrInDayByHours[5] != 0 &&
				airport.ArrInDayByHours[6] != 0 &&
				airport.ArrInDayByHours[7] != 0 &&
				airport.DepInDayByHours[0] != 0 &&
				airport.DepInDayByHours[1] != 0 &&
				airport.DepInDayByHours[2] != 0 &&
				airport.DepInDayByHours[3] != 0
				);
		}
	}
}
