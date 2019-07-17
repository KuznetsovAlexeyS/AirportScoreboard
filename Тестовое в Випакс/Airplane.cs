using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AirportScoreboard
{
	public enum Direction { In, Away }

	class Airplane
	{
		public string Model { get; }
		private int capacity = -1;
		public int Passengers { get; }
		public DateTime Time { get; }
		public string City { get; }
		public Direction Direction { get; }

		public Airplane(string model, DateTime time, string city, Direction direction)
		{
			this.Model = model;
			GetCapacity();
			Random rnd = new Random();
			Passengers = rnd.Next(capacity+1); // +1 because airplane can be full.
			this.Time = time;
			this.City = city;
			this.Direction = direction;
		}

		private void GetCapacity()
		{
			var path = Directory.GetCurrentDirectory() + "\\Models.txt";
			IEnumerable<string> data = File.ReadAllLines(path).Skip(1); // First string is a title. 
			foreach(var str in data)
			{
				var nameAndCapacity = str.Split(' ');
				if (nameAndCapacity.Length < 2) throw new ArgumentException("File with models is incorrect"); 
					// <2 because at least you must have model name and capacity.
				var capacity = -1;
				if (!Int32.TryParse(nameAndCapacity[nameAndCapacity.Length - 1], out capacity))
					throw new ArgumentException("Something is wrong with capacity");
				var name = GetName(str);
				if (this.Model == name) this.capacity = capacity;
			}
			if (this.capacity < 0) throw new ArgumentException("This model wasn't find in the file with models"); 
		}

		private string GetName(string str)
		{
			var cutTo = str.LastIndexOf(' ');
			var model = str.Substring(0, cutTo);
			return model;
		}
	}
}