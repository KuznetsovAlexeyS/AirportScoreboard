using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportScoreboard
{
	class Airport
	{
		public int ArrInLastFlight { private set; get; } // Arr - arrived
		public int ArrInLastDay { private set; get; }
		public int[] ArrInDayByHours { private set; get; } 
		// ArrInDayByHours[0] - arrived in recent hour, ArrInDayByHours[1] - between 1 and 2 hours, etc.
		public int ArrSummary { private set; get; }
		public int DepInLastFlight { private set; get; } // Dep - departured
		public int DepInLastDay { private set; get; }
		public int[] DepInDayByHours { private set; get; } // The same with ArrDayByHours.
		public int DepSummary { private set; get; }
		public Airplane RecentArrival { private set; get; }
		public Airplane RecentDeparture { private set; get; }
		private Queue<Airplane> airplanes;
		public DateTime currentTime { private set; get; }
		private string[] schedule;
		private int schedulePointer;

		public Airport(string[] schedule)
		{
			ArrInLastFlight = 0;
			ArrInLastDay = 0;
			ArrInDayByHours = new int[24];
			ArrSummary = 0;
			DepInLastFlight = 0;
			DepInLastDay = 0;
			DepInDayByHours = new int[24];
			DepSummary = 0;
			this.schedule = schedule;
			schedulePointer = 0;
			FlightInfoLine info = new FlightInfoLine(schedule[schedulePointer]);
			airplanes = new Queue<Airplane>();
			currentTime = info.Date;
			UpdateInfo();
		}

		public void AddTimeInMinutes(int addedTime)
		{
			currentTime = currentTime.AddMinutes(addedTime);
			UpdateInfo();
		}

		private void UpdateInfo()
		{
			while (schedulePointer < schedule.Length)
			{
				var info = new FlightInfoLine(schedule[schedulePointer]);
				if (info.Date > currentTime) break;
				var airplane = new Airplane(info.Model, info.Date, info.City, info.Direction);
				airplanes.Enqueue(airplane);
				UpdateSumPassengers(airplane.Direction, airplane.Passengers);
				UpdateRecentFlight(airplane);
				schedulePointer++;
			}

			while (airplanes.Count > 0 && airplanes.Peek().Time.AddDays(1) < currentTime) // AddDays(1) because "airplanes" embraces recent 24 hours.
			{
				airplanes.Dequeue();
			}
			UpdateTempPassengers();
		}

		private void UpdateSumPassengers(Direction direction, int passengers)
		{
			if (direction == Direction.In)
			{
				ArrSummary += passengers;
			}
			else
			{
				DepSummary += passengers;
			}
		}

		private void UpdateRecentFlight(Airplane airplane)
		{
			if (airplane.Direction == Direction.In)
			{
				RecentArrival = airplane;
			}
			else
			{
				RecentDeparture = airplane;
			}
		}

		private void UpdateTempPassengers()
		{
			if (RecentArrival != null)
			{
				ArrInLastFlight = RecentArrival.Passengers;
				ArrInLastDay = airplanes
					.Where(a => a.Direction == Direction.In)
					.Sum(b => b.Passengers);
				for (int i = 1; i < 25; i++)
				{
					ArrInDayByHours[i-1] = airplanes
						.Where(a => a.Direction == Direction.In)
						.Where(b => b.Time.AddHours(i) > currentTime && b.Time.AddHours(i - 1) <= currentTime)
						.Sum(c => c.Passengers);
				}
			}
			if (RecentDeparture != null)
			{
				DepInLastFlight = RecentDeparture.Passengers;
				DepInLastDay = airplanes
					.Where(a => a.Direction == Direction.Away)
					.Sum(b => b.Passengers);
				for (int i = 1; i < 25; i++)
				{
					DepInDayByHours[i - 1] = airplanes
						.Where(a => a.Direction == Direction.Away)
						.Where(b => b.Time.AddHours(i) > currentTime && b.Time.AddHours(i - 1) <= currentTime)
						.Sum(c => c.Passengers);
				}
			}
		}
	}
}
