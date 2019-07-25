using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AirportScoreboard
{
	/// <summary>
	/// Логика взаимодействия для SetSpeed.xaml
	/// </summary>
	public partial class SetSpeed : Window
	{
		public int Speed { private set; get; }
		public SetSpeed()
		{
			InitializeComponent();
			Accept.Click += (sender, args) =>
			{
				int speed;
				if (Int32.TryParse(DesiredSpeed.Text, out speed) && speed > 0 && speed <= 10000)
				{
					Speed = speed;
				}
				else MessageBox.Show("Введённые данные некорректны.");
			};
		}
	}
}
