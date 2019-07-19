using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AirportScoreboard
{
	class ExampleGenerator
	{
		public static void GenerateExample(int length)
		{
			List<string> cities = new List<string>();
			cities.Add("Moscow");
			cities.Add("Kiev");
			cities.Add("Minsk");
			Random rnd = new Random();
			DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, rnd.Next(29), rnd.Next(24), rnd.Next(60), rnd.Next(60));
			List<string> fullInfo = new List<string>();
			for (int i=0; i<length; i++)
			{
				var str = new StringBuilder();
				var numberOfModel = rnd.Next(GetAmountOfModels());
				str.Append(date.Year.ToString() + " " + date.Month.ToString() + " " + date.Day.ToString() 
					+ " " + date.Hour.ToString() + " " + date.Minute.ToString() + " ");
				date = date.AddMinutes(rnd.Next(10, 300));
				if (rnd.Next(2) == 0)
				{
					str.Append("Arrival ");
				}
				else
				{
					str.Append("Departure ");
				};
				var city = cities[rnd.Next(3)];
				str.Append(city + " ");
				str.Append(GetModel(numberOfModel));
				fullInfo.Add(str.ToString());
			}
			var path = Directory.GetCurrentDirectory() + "\\Example.txt";
			File.WriteAllLines(path, fullInfo);
		}

		private static int GetAmountOfModels()
		{
			var path = Directory.GetCurrentDirectory() + "\\Models.txt";
			string[] data = File.ReadAllLines(path);
			return data.Length - 1; // First string is a title, so -1.
		}

		private static string GetModel(int number)
		{
			var path = Directory.GetCurrentDirectory() + "\\Models.txt";
			string[] data = File.ReadAllLines(path)
				.Skip(1)
				.ToArray();
			var cutTo = data[number].LastIndexOf(' ');
			var model = data[number].Substring(0, cutTo);
			return model;
		}
	}
}