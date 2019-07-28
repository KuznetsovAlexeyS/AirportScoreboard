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
		public int ArrInRecentFlight { private set; get; } // Arr - arrived
		public int ArrInRecentDay { private set; get; }
		public int[] ArrInDayByHours { private set; get; } 
		// ArrInDayByHours[0] - прибыло в последний час, ArrInDayByHours[1] - между 1 и 2 часами, и т. д.
		public int ArrSummary { private set; get; }
		public int DepInRecentFlight { private set; get; } // Dep - departured
		public int DepInRecentDay { private set; get; }
		public int[] DepInDayByHours { private set; get; } // Аналогично с ArrDayByHours.
		public int DepSummary { private set; get; }
		public Airplane RecentArrival { private set; get; }
		public Airplane RecentDeparture { private set; get; }
		private Queue<Airplane> airplanes;
		public DateTime currentTime { private set; get; }
		private string[] schedule;
		private int schedulePointer;

		public Airport(string[] schedule)
		{
			ArrInRecentFlight = 0;
			ArrInRecentDay = 0;
			ArrInDayByHours = new int[24];
			ArrSummary = 0;
			DepInRecentFlight = 0;
			DepInRecentDay = 0;
			DepInDayByHours = new int[24];
			DepSummary = 0;
			this.schedule = schedule;
			schedulePointer = 0;
			FlightInfoLine info = new FlightInfoLine(schedule[schedulePointer]);
			airplanes = new Queue<Airplane>();
			currentTime = info.Date;
			if (!IsScheduleCorrect())
				throw new ArgumentException("В расписании есть ситуация, когда в одной из строк стоит более ранняя дата, чем в предыдущей.");
			UpdateInfo();
		}

		private bool IsScheduleCorrect() // Проверяет, нет ли в расписании ситуации, когда следующая строка имеет более раннюю дату.
		{
			if (schedule.Length == 1) return true;
			for(int i=1; i<schedule.Length; i++)
			{
				var firstLine = new FlightInfoLine(schedule[i-1]);
				var secondLine = new FlightInfoLine(schedule[i]);
				if (firstLine.Date > secondLine.Date) return false;
			}
			return true;
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
				ArrInRecentFlight = RecentArrival.Passengers;
				ArrInRecentDay = airplanes
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
				DepInRecentFlight = RecentDeparture.Passengers;
				DepInRecentDay = airplanes
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
