using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class MatchScheduleCell:ContentView
	{
		public Label matchNumber;

		public MatchScheduleCell() {
			matchNumber = new Label();
			WidthRequest = 100;
			HeightRequest = 50;
			matchNumber.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
			matchNumber.TextColor = Color.Black;
			matchNumber.VerticalOptions = LayoutOptions.CenterAndExpand;
			BackgroundColor = Color.White;

			Content = matchNumber;
		}
	}
}
