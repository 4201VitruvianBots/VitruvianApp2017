using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class RobotInfoIndexPage:ContentPage
	{
		public RobotInfoIndexPage()
		{
			int i = 0, j = 0, k = 0, l = 0;
			String test = "Title", test2 = "Title2";

			var singleCounterTest = new SingleCounter(test, i);
			var singleCounterTest2 = new SingleCounter(test2, j);
			var doubleCounterTest = new DoubleCounter("Main Title", test, test2, k, l);

			var testGrid = new Grid();
			testGrid.Children.Add(singleCounterTest, 0, 0);
			testGrid.Children.Add(singleCounterTest2, 1, 0);
			testGrid.Children.Add(doubleCounterTest, 0, 2, 1, 2);

			this.Content = testGrid;

			BackgroundColor = Color.Lime;
		}
	}
}
