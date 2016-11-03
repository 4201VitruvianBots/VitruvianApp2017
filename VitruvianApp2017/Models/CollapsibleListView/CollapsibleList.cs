using System;
using System.Collections.Generic;
using Xamarin.Forms;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
             
namespace VitruvianApp2017
{
	public class CollapsibleList : Grid
	{
		public bool expanded;
		ListView dataView;
		List<PitData> dataList = new List<PitData>();
		ScrollView datascroll;
		Grid headergrid;
		TapGestureRecognizer tap;
		Label lbl;
		CachedImage img;
		public double tHeight;

		public CollapsibleList() : this("title", 0) {

		}

		public CollapsibleList(string Title) : this(Title, 0) {

		}
		public CollapsibleList(string Title, double totalHeight) {
			tHeight = totalHeight;

			HorizontalOptions = LayoutOptions.Fill;
			RowSpacing = 0;
			RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
			RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });

			dataView = new ListView() {
				ItemsSource = dataList,
				ItemTemplate = new DataTemplate(() => {
					var cell = new PitDataCell();
					cell.textLbl.SetBinding(Label.TextProperty, "dataHeader");
					cell.detailLbl.SetBinding(Label.TextProperty, "data");

					return cell;
				}),
				HorizontalOptions = LayoutOptions.Fill,
			};
			dataView.ItemSelected += (sender, e) => {
				 ((ListView)sender).SelectedItem = null;
			 };

			datascroll = new ScrollView() {
				HorizontalOptions = LayoutOptions.Fill,
				Content = dataView
			};

			headergrid = new Grid() {
				BackgroundColor = Color.White,

				ColumnDefinitions = {
					new ColumnDefinition(){ Width = GridLength.Star },
					new ColumnDefinition(){ Width = GridLength.Auto }
				},

				RowDefinitions = {
					new RowDefinition(){ Height = GridLength.Auto }
				}
			};
			lbl = new Label() {
				Text = Title,
				FontSize = GlobalVariables.sizeMedium,
				BackgroundColor = Color.White
			};
			img = new CachedImage() {
				Source = "Menu_Icon.png",
				WidthRequest = GlobalVariables.sizeMedium,
				HeightRequest = GlobalVariables.sizeMedium,
				DownsampleToViewSize = true,
			};
			img.RotateTo(180, 0);
			   
			tap = new TapGestureRecognizer();
			//tap.NumberOfTapsRequired = 2;
			tap.Tapped += (sender, e) => {
				tapAction();
			};

			headergrid.GestureRecognizers.Add(tap);
			headergrid.Children.Add(lbl, 0, 0);
			headergrid.Children.Add(img, 1, 0);
			Children.Add(datascroll, 0, 1);
			Children.Add(headergrid, 0, 0);

			IsClippedToBounds = true;
		}

		public void addData(string header, string value) {
			var cell = new PitData() {
				dataHeader = header,
				data = value
			};
			dataList.Add(cell);
		}

		public void addData(PitData cell) {
			dataList.Add(cell);
		}

		void tapAction() {
			var length = tHeight;
			if (expanded == true) {
				img.RotateTo(360, 250, Easing.Linear);
				dataView.Animate("expand", (arg) => {
					dataView.TranslateTo(0, 0, 250);
					dataView.HeightRequest = length;
				}, 0, 250, Easing.Linear, (arg1, arg2) => {
					expanded = !expanded;
					dataView.IsVisible = true;
					dataView.IsEnabled = true;
				});
			} else if (expanded == false) {
				img.RotateTo(180, 250, Easing.Linear);
				dataView.Animate("collapse", (arg) => {
					dataView.TranslateTo(0, -length, 250);
					dataView.HeightRequest = 0;
				}, 0, 250, Easing.Linear, (arg1, arg2) => {
					expanded = !expanded;
					dataView.IsVisible = false;
					dataView.IsEnabled = false;
				});
			}
		}
	}
}
