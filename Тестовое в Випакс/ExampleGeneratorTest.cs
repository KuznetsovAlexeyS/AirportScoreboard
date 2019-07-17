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
	public class ExampleGeneratorTest
	{
		[Test]
		public void GeneralRunning()
		{
			ExampleGenerator.GenerateExample(5);
			var path = Directory.GetCurrentDirectory() + "\\Example.txt";
			string[] data = File.ReadAllLines(path);
			Assert.IsTrue(data.Length == 5);
		}
	}
}