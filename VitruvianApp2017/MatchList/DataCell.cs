using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class DataCell : ContentView
	{
		public Label data = new Label();

		public DataCell() {
			WidthRequest = 30;
			HeightRequest = 10;
			BackgroundColor = Color.White;
			data.TextColor = Color.Black;
			data.FontSize = GlobalVariables.sizeTiny;
			data.HorizontalTextAlignment = TextAlignment.End;

			Content = data;
		}
	}
}
