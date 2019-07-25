using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AirportScoreboard
{
	[TestFixture]
	public class AirplaneTests
	{
		[Test]
		public void GeneralCorrectness()
		{
			DateTime date = DateTime.Now;
			var airplane = new Airplane("Sukhoi Superjet 100", date, "City", Direction.In);
			// Поменять тест, когда "Sukhoi Superjet 100" будет удалён из Models.
			Assert.IsTrue(
				airplane.Model == "Sukhoi Superjet 100" &&
				airplane.Time == date &&
				airplane.City == "City" &&
				airplane.Direction == Direction.In &&
				airplane.Passengers > 0 &&
				airplane.Passengers <= 100
				);
		}

		[Test]
		public void Passangers()
		{
			DateTime date = new DateTime(2019, 7, 17, 20, 42, 51);
			var flag = true;
			for (int i = 0; i < 1000; i++)
			{
				var airplane = new Airplane("Sukhoi Superjet 100", date, "Moscow", Direction.In);
				// Поменять тест, когда "Sukhoi Superjet 100" будет удалён из Models.
				if (airplane.Passengers < 0 || airplane.Passengers > 100)
				{
					flag = false;
					break;
				}
			}
			Assert.IsTrue(flag);
		}

		[Test]
		public void ZeroValues()
		{
			DateTime date = new DateTime(1, 1, 1, 0, 0, 0);
			Assert.Throws<ArgumentException>(() => new Airplane("", date, "", Direction.Away));
		}

	}
}