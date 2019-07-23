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
		public void ZeroValues()
		{
			ExampleGenerator.GenerateExample(0);
			var path = "Example.txt";
			string[] data = File.ReadAllLines(path);
			Assert.IsTrue(data.Length == 0);
		}

		[Test]
		public void GeneralRunning()
		{
			ExampleGenerator.GenerateExample(5);
			var path ="Example.txt";
			string[] data = File.ReadAllLines(path);
			Assert.IsTrue(data.Length == 5);
		}

		[Test]
		public void HugeValues()
		{
			var someHugeValue = 3000;
			ExampleGenerator.GenerateExample(someHugeValue);
			var path = "Example.txt";
			string[] data = File.ReadAllLines(path);
			Assert.IsTrue(data.Length == someHugeValue);
		}
	}
}