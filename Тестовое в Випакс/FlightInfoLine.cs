using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportScoreboard
{
	class FlightInfoLine
	{
		public DateTime Date { get; }
		public Direction Direction { get; }
		public string City { get; }
		public string Model { get; }

		public FlightInfoLine(string line)
		{
			CheckLineCorrectness(line);
			Date = ExtractDate(line);
			Direction = ExtractDirection(line);
			City = ExtractCity(line);
			Model = ExtractModel(line);
		}

		private void CheckLineCorrectness(string line)
		{
			var splitedLine = line.Split(' ');
			if (splitedLine.Length < 8) throw new ArgumentException("Data format is incorrect");
		}

		private DateTime ExtractDate(string line)
		{
			var splitedLine = line.Split(' ');
			int year;
			if (!Int32.TryParse(splitedLine[0], out year) || year < 1903) throw new ArgumentException("Date format is incorrect");
			int month;
			if (!Int32.TryParse(splitedLine[1], out month) || month < 1 || month > 12) throw new ArgumentException("Date format is incorrect");
			int day;
			if (!Int32.TryParse(splitedLine[2], out day) ||
				day < 1 || day > DateTime.DaysInMonth(year, month)) throw new ArgumentException("Date format is incorrect");
			int hour;
			if (!Int32.TryParse(splitedLine[3], out hour) || hour < 0 || hour >= 24) throw new ArgumentException("Date format is incorrect");
			int minute;
			if (!Int32.TryParse(splitedLine[4], out minute) || minute < 0 || minute >= 60) throw new ArgumentException("Date format is incorrect");
			return new DateTime(year, month, day, hour, minute, 0, 0); // Seconds and milliseconds arn't matter in flight schedules.
		}

		private Direction ExtractDirection(string line)
		{
			var splitedLine = line.Split(' ');
			if (splitedLine[5] == "Arrival") return Direction.In;
			if (splitedLine[5] == "Departure") return Direction.Away;
			throw new ArgumentException("Direction format is incorrect");
		}

		private string ExtractCity(string line)
		{
			var splitedLine = line.Split(' ');
			return splitedLine[6];
		}

		private string ExtractModel(string line)
		{
			var model = new StringBuilder();
			var splitedLine = line.Split(' ');
			for (int i = 7; i < splitedLine.Length - 1; i++) // -1 because we dont need excess space at the end of the line.
			{
				model.Append(splitedLine[i] + " ");
			}
			model.Append(splitedLine[splitedLine.Length - 1]);
			return model.ToString();
		}
	}
}
