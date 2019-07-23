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

namespace AirportScoreboard
{
	public partial class MainWindow : Window
	{
		private Thread thread;
		private int speed = 50000;

		public MainWindow()
		{
			InitializeComponent();
		}


		protected override void OnContentRendered(EventArgs e)
		{
			base.OnContentRendered(e);
			thread = new Thread(() =>
			{
				try
				{
					foreach (var moment in Iterations())
					{
						var invokation = this.Dispatcher.BeginInvoke((Action)(() => UpdateInfo(moment)));
						Thread.Sleep(100000 / this.speed);
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
			
		private IEnumerable<InterfaceLogicConnector> Iterations()
		{
			InterfaceLogicConnector ILC = new InterfaceLogicConnector("Schedule.txt", 10000);
			while (true)
			{
				ILC.Iterate();
				yield return ILC;
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
			UpdateLayout();
		}
	}
}
