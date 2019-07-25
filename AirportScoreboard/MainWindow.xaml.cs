using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;

namespace AirportScoreboard
{
	public partial class MainWindow : Window
	{
		private Thread thread;
		public int Speed { private set; get; }
		private bool isCounting = false;
		private string filePath;

		public MainWindow()
		{
			Speed = 1000;
			InitializeComponent();

			this.KeyDown += (sender, e) =>
			{
				switch (e.Key)
				{
					case Key.F1:
						var info = new Info();
						info.Show();
						break;
					case Key.F2:
						OpenFileDialog openFileDialog = new OpenFileDialog();
						if (isCounting)
						{
							MessageBox.Show("Файл уже выбран.");
						}
						if (!isCounting && openFileDialog.ShowDialog() == true)
						{
							filePath = openFileDialog.FileName;
							isCounting = true;
							RunDisplay();
						}
						break;
					case Key.F3:
						var setSpeedWindow = new SetSpeed(Speed);
						setSpeedWindow.Show();
						setSpeedWindow.Accept.Click += (send, args) =>
						{
							Speed = setSpeedWindow.Speed;
							setSpeedWindow.Close();
						};
						break;
				}
			};
		}

		private void RunDisplay()
		{
			DeclareChart();
			thread = new Thread(() =>
			{
				try
				{
					foreach (var dataSlice in ExtractData())
					{
						this.Dispatcher.BeginInvoke((Action)(() =>
						{
							UpdateInfo(dataSlice);
							UpdateCharts(dataSlice.ArrInDayByHours, dataSlice.DepInDayByHours, dataSlice.CurrentTime);
						}));
						Thread.Sleep(600000 / this.Speed);
					}
				}
				catch
				{
					MessageBox.Show("Something is wrong");
				}
			})
			{ IsBackground = true };
			thread.Start();
		}
			
		private IEnumerable<InterfaceLogicConnector> ExtractData()
		{
			InterfaceLogicConnector ILC = new InterfaceLogicConnector("Schedule.txt");
			while (true)
			{
				yield return ILC;
				ILC.Iterate();
			}
		}

		private void UpdateInfo(InterfaceLogicConnector ILC)
		{
			ArrivedInRecentFlight.Text = ILC.ArrInRecentFlight.ToString();
			ArrivedInRecentDay.Text = ILC.ArrInRecentDay.ToString();
			ArrivedSummary.Text = ILC.ArrSummary.ToString();
			DeparturedInRecentDay.Text = ILC.DepInRecentDay.ToString();
			DeparturedInRecentFlight.Text = ILC.DepInRecentFlight.ToString();
			DeparturedSummary.Text = ILC.DepSummary.ToString();
			RecentFlightRoute.Text = ILC.RouteOfRecentFlight;
			RecentFlightTime.Text = ILC.TimeOfRecentFlight;
			CurrentTime.Text = ILC.CurrentTime.ToString();
			if (ILC.RecentFlightDirection == Direction.In) RecentFlightLabel.Text = "Время прибытия";
			else RecentFlightLabel.Text = "Время вылета";
			UpdateLayout();
		}

		//Ниже - график.

		private void DeclareChart()
		{
			SeriesCollection = new SeriesCollection
			{
				new ColumnSeries
				{
					Title = "Прибыло",
					Values = new ChartValues<double>(),
				}
			};

			SeriesCollection.Add(new ColumnSeries
			{
				Title = "Вылетело",
				Values = new ChartValues<double>(),
			});

			Labels = new string[24];
			for (int i = 0; i < Labels.Length; i++)
			{
				Labels[23-i] = i.ToString();
			}
			Formatter = value => value.ToString("N");
			DataContext = this;
		}

		private void UpdateCharts(int[] arrInDayByHours, int[] depInDayByHours, DateTime currentTime)
		{
			var arrData = CastToDouble(arrInDayByHours);
			var depData = CastToDouble(depInDayByHours);

			SeriesCollection[0].Values.Clear();
			for (int i = 0; i < 24; i++)
			{
				SeriesCollection[0].Values.Add(arrData[23-i]); // Необходимо для "прожождения" колонок справа налево.
			}

			SeriesCollection[1].Values.Clear();
			for (int i = 0; i < 24; i++)
			{
				SeriesCollection[1].Values.Add(depData[23-i]);
			}

			for (int i = 0; i < Labels.Length; i++)
			{
				Labels[23-i] = currentTime.AddHours(24-i).Hour.ToString();
			}
			chart.UpdateLayout();
		}

		private double[] CastToDouble(int[] array)
		{
			var doubleArr = new double[array.Length];
			for(int i=0; i<array.Length; i++)
			{
				doubleArr[i] = array[i];
			}
			return doubleArr;
		}

		public SeriesCollection SeriesCollection { get; set; }
		public string[] Labels { get; set; }
		public Func<double, string> Formatter { get; set; }

	}
}
