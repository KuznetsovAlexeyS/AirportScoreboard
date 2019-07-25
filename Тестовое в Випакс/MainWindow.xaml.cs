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

namespace AirportScoreboard
{
	public partial class MainWindow : Window
	{
		private Thread thread;
		private int speed = 10000;

		public MainWindow()
		{
			InitializeComponent();
		}

		protected override void OnContentRendered(EventArgs e)
		{
			base.OnContentRendered(e);
			DeclareChart();
			thread = new Thread(() =>
			{
				try
				{
					foreach (var dataSlice in ExtractData())
					{
						this.Dispatcher.Invoke(() =>
						{
							UpdateInfo(dataSlice);
							UpdateCharts(dataSlice.ArrInDayByHours, dataSlice.DepInDayByHours);
						});
						Thread.Sleep(600000 / this.speed);
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

		//Chart area below

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
				Values = new ChartValues<double>()
			});

			Labels = new string[24];
			for (int i = 0; i < Labels.Length; i++)
			{
				Labels[i] = i.ToString();
			}
			Formatter = value => value.ToString("N");
			chart.DisableAnimations = true;
			DataContext = this;
		}

		private void UpdateCharts(int[] arrInDayByHours, int[] depInDayByHours)
		{
			var arrData = CastToDouble(arrInDayByHours);
			var depData = CastToDouble(depInDayByHours);

			SeriesCollection[0].Values.Clear();
			for (int i = 0; i < 24; i++)
			{
				SeriesCollection[0].Values.Add(arrData[i]);
			}

			SeriesCollection[1].Values.Clear();
			for (int i = 0; i < 24; i++)
			{
				SeriesCollection[1].Values.Add(depData[i]);
			}
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
