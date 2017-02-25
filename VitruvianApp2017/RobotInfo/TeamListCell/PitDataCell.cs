using System;
using System.Collections.Generic;
using Xamarin.Forms;
namespace VitruvianApp2017
{
	public class PitDataCell : ViewCell
	{
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Data", typeof(List<PitData>), typeof(ListView), new List<PitData>());

		public List<PitData> Data {
			get { return (List<PitData>)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		string dataHeader;
		string data;
		public Label textLbl;
		public Label detailLbl;

		public PitDataCell() : this(null, null) {
		}

		public PitDataCell(string header, string value) {
			dataHeader = header;
			data = value;

			textLbl = new Label() {
				Text = dataHeader,
				FontSize = GlobalVariables.sizeMedium,
				TextColor = Color.White
			};

			detailLbl = new Label() {
				Text = data,
				FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.Gray
			};

			var grid = new Grid() {
				ColumnSpacing = 0,
				RowSpacing = 0,

				RowDefinitions = {
					new RowDefinition() { Height = GridLength.Auto },
					new RowDefinition() { Height = GridLength.Auto }
				},
				ColumnDefinitions = {
					new ColumnDefinition() { Width = GridLength.Auto },
					new ColumnDefinition() { Width = GridLength.Star },
				}
			};

			grid.Children.Add(textLbl, 0, 0);
			grid.Children.Add(detailLbl, 0, 1);

			//Height = 40;
			View = grid;
		}
	}
}
