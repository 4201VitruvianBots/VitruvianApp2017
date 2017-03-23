using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class ColumnHeaderCell: ContentView
	{
		public Label header = new Label();

		public ColumnHeaderCell() {
			HeightRequest = 10;
			header.FontSize = GlobalVariables.sizeSmall;
			header.BackgroundColor = Color.Black;
			header.TextColor = Color.White;
			header.FontAttributes = FontAttributes.Bold;
			header.HorizontalOptions = LayoutOptions.FillAndExpand;

			Content = header;
		}
	}
}