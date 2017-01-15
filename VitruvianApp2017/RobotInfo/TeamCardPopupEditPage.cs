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
		Grid teamGrid = new Grid()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			Margin = new Thickness(0, 0, 0, 0),
			Padding = 0,

			RowDefinitions ={
				new RowDefinition {Height = GridLength.Auto},
				new RowDefinition {Height = GridLength.Auto},
				new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
				new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
				new RowDefinition {Height = GridLength.Auto}
			},
			ColumnDefinitions = {
				new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
				new ColumnDefinition {Width = GridLength.Auto}
			}
		};

		TeamData data;

		public TeamCardPopupEditPage(TeamData team)
		{
			data = team;

			var teamNo = new Label()
			{
				Text = data.teamNumber.ToString(),
				FontSize = 22,
				FontAttributes = FontAttributes.Bold
			};

			var teamNa = new Label()
			{
				Text = data.teamName,
				FontSize = 18
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
			};

			Button[] btnArray = { saveDataBtn };
			var navigationBtns = new PopupNavigationButtons(false, btnArray);

			int gridYIndex = 0;
			//teamGrid.Children.Add(new RobotImageLayout(data), 0, 1, gridYIndex, gridYIndex + 2);
			teamGrid.Children.Add(teamNo, 1, gridYIndex++);
			teamGrid.Children.Add(teamNa, 1, gridYIndex++);
			//
			teamGrid.Children.Add(new Label() { Text = "test" }, 0, 2, gridYIndex, gridYIndex++ + 1);

			//
			teamGrid.Children.Add(navigationBtns, 0, 2, gridYIndex, gridYIndex++ + 1);

			Content = new Frame()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(50, 80),
				Padding = new Thickness(5),

				BackgroundColor = Color.Gray,
				HasShadow = true,

				Content = teamGrid
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
