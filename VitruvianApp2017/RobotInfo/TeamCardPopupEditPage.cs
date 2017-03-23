using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using FFImageLoading.Forms;

namespace VitruvianApp2017
{
	public class TeamCardPopupEditPage:PopupPage
	{
		Grid topGrid = new Grid() {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			Margin = new Thickness(0, 0, 0, 0),
			Padding = 0,

			RowDefinitions ={
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = GridLength.Star },
			},
			ColumnDefinitions = {
				//new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
				//new ColumnDefinition {Width = GridLength.Auto},
				//new ColumnDefinition {Width = GridLength.Auto}
			}
		};

		TeamData data;

		public TeamCardPopupEditPage(TeamData team)
		{
			data = team;

			var teamNo = new Label() {
				Text = data.teamNumber.ToString(),
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			var teamNa = new Label() {
				Text = data.teamName,
				FontSize = GlobalVariables.sizeMedium
			};

			Button saveDataBtn = new Button(){
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Save",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			saveDataBtn.Clicked += (object sender, EventArgs e) =>
			{
				saveData();
				Navigation.PopAllPopupAsync();
			};

			Button[] btnArray = { saveDataBtn };
			var navigationBtns = new PopupNavigationButtons(false, btnArray);

			//teamGrid.Children.Add(new RobotImageLayout(data), 0, 1, gridYIndex, gridYIndex + 2);
			topGrid.Children.Add(teamNo, 1, 0);
			topGrid.Children.Add(teamNa, 1, 1);

			// teamGrid.Children.Add(navigationBtns, 0, 2, gridYIndex, gridYIndex++ + 1);

			Content = new Frame() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(50, 50),
				Padding = new Thickness(5),

				BackgroundColor = Color.Gray,
				HasShadow = true,

				Content = new StackLayout() {

					Children = {
						topGrid,
						new ScrollView(){
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.FillAndExpand,

							Content = new StackLayout(){
								HorizontalOptions = LayoutOptions.FillAndExpand,
								VerticalOptions = LayoutOptions.FillAndExpand,

								Children = {

								}
							}
						},
						navigationBtns
					}
				}
			};
		}

		void saveData() {

		}

		async void popUpPage(CachedImage rImage)
		{
			//await Task.Yield();
			//await Navigation.PushPopupAsync(new ImagePopupPage(rImage));
		}
	}
}
