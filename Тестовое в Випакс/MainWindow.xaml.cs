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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirportScoreboard
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Start();
		}

		private void Start()
		{
			var i = 0;
			InterfaceLogicConnector ILC = new InterfaceLogicConnector("Schedule.txt", 1000);
			while (i<10)
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
				ILC.Iterate();
				UpdateLayout();
				i++;
			}
			UpdateLayout();
		}
	}
}
