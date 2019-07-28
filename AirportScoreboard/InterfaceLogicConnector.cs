using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportScoreboard
{
	class InterfaceLogicConnector
	{
		private static string airportCity = "Perm";
		private Airplane recentFlight;
		public string TimeOfRecentFlight { private set; get; }
		public string RouteOfRecentFlight { private set; get; }
		public int ArrInRecentFlight { private set; get; }
		public int ArrInRecentDay { private set; get; }
		public int[] ArrInDayByHours { private set; get; }
		public int ArrSummary { private set; get; }
		public int DepInRecentFlight { private set; get; }
		public int DepInRecentDay { private set; get; }
		public int[] DepInDayByHours { private set; get; }
		public int DepSummary { private set; get; }
		public Direction RecentFlightDirection { private set; get; }
		public DateTime CurrentTime { private set; get; }
		private Airport airport;
		private DateTime endTime;

		public InterfaceLogicConnector(string path)
		{
			string[] strs = File.ReadAllLines(path);
			airport = new Airport(strs);
			FlightInfoLine endInfo = new FlightInfoLine(strs[strs.Length - 1]);
			endTime = endInfo.Date;
			UpdateInfo();
		}

		public void Iterate()
		{
			airport.AddTimeInMinutes(1);
			UpdateInfo();
		}

		private void UpdateInfo()
		{
			ArrInRecentFlight = airport.ArrInRecentFlight;
			ArrInRecentDay = airport.ArrInRecentDay;
			ArrSummary = airport.ArrSummary;
			DepInRecentDay = airport.DepInRecentDay;
			DepInRecentFlight = airport.DepInRecentFlight;
			DepSummary = airport.DepSummary;
			CurrentTime = airport.currentTime;
			ArrInDayByHours = airport.ArrInDayByHours;
			DepInDayByHours = airport.DepInDayByHours;
			UpdateRecentFlight();
			RecentFlightDirection = recentFlight.Direction;
		}

		private void UpdateRecentFlight()
		{
			if (airport.RecentArrival != null && airport.RecentDeparture != null)
			{
				if (airport.RecentArrival.Time > airport.RecentDeparture.Time)
					recentFlight = airport.RecentArrival;
				else recentFlight = airport.RecentDeparture;
			}
			else
			{
				if (airport.RecentArrival == null && airport.RecentDeparture != null)
					recentFlight = airport.RecentDeparture;
				else recentFlight = airport.RecentArrival;
			}
			UpdateRecentFlightInfo();
		}

		private void UpdateRecentFlightInfo()
		{
			TimeOfRecentFlight = recentFlight.Time.Hour.ToString() + ":" + recentFlight.Time.Minute.ToString();
			if (recentFlight.Direction == Direction.In)
			{
				RouteOfRecentFlight = recentFlight.City + "-" + airportCity;
			}
			else RouteOfRecentFlight = airportCity + "-" + recentFlight.City;
		}
	}
}
