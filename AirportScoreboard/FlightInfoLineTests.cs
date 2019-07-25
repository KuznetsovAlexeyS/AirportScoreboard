using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
namespace AirportScoreboard
{
	[TestFixture]
	public class InfoLineTests
	{
		[Test]
		public void DefaultValues()
		{
			string str = "2019 7 21 7 14 Departure Moscow Boeing 737";
			DateTime date = new DateTime(2019, 7, 21, 7, 14, 0);
			FlightInfoLine infoLine = new FlightInfoLine(str);
			Assert.IsTrue(
				infoLine.Date == date &&
				infoLine.City == "Moscow" &&
				infoLine.Direction == Direction.Away &&
				infoLine.Model == "Boeing 737"
				);
		}

		[Test]
		public void EmptyString()
		{
			string str = "";
			Assert.Throws<ArgumentException>(() => new FlightInfoLine(str));
		}

		[Test]
		public void IncorrectDate()
		{
			string str = "1902 7 21 7 14 Departure Moscow Boeing 737";
			Assert.Throws<ArgumentException>(() => new FlightInfoLine(str));
		}

		[Test]
		public void IncorrectDirection()
		{
			string str = "2019 7 21 7 14 JustStaying Moscow Boeing 737";
			Assert.Throws<ArgumentException>(() => new FlightInfoLine(str));
		}

		[Test]
		public void WrongFormat()
		{
			string str = "Arrival 2019 7 21 7 14 Boeing 737 Moscow";
			Assert.Throws<ArgumentException>(() => new FlightInfoLine(str));
		}
	}
}