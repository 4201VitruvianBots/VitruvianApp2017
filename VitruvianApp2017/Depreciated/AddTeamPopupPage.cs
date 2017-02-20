using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

namespace VitruvianApp2017
{
	public class AddTeamPopupPage:PopupPage
	{
		LineEntry teamNo = new LineEntry("Team Number:");
		LineEntry teamNa = new LineEntry("Team Name:");

		public AddTeamPopupPage()
		{
			teamNo.inputEntry.Keyboard = Keyboard.Numeric;

			Button addTeamBtn = new Button()
			{
				Text = "Add Team"
			};
			addTeamBtn.Clicked += (object sender, EventArgs e) =>
			{
				if (string.IsNullOrEmpty(teamNa.data) || string.IsNullOrEmpty(teamNo.data))
					DisplayAlert("Error", "Please enter a Team Name and Number", "OK");
				else
					addTeam();
			};

			Button[] btnArray = { addTeamBtn };
			var navigationBtns = new PopupNavigationButtons(btnArray);
			navigationBtns.backBtn.Clicked += (object sender, EventArgs e) => {

			};

			Content = new Frame()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Margin = new Thickness(0, 0, 0, 0),
				BackgroundColor = Color.Gray,
				HasShadow = true,
				//Padding = new Thickness(1, 1, 1, 1),
				//Margin = new Thickness(50, 50, 50, 50),

				Content = new StackLayout()
				{
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					Margin = new Thickness(0, 0, 0, 0),

					Children = {
						teamNo,
						teamNa,
						navigationBtns
					}
				}
			};
		}

		async void addTeam()
		{
			var db = new FirebaseClient("https://vitruvianapptest.firebaseio.com/");
			var send = db
				.Child("teamData")
				.Child(teamNo.data)
				.PutAsync(new TeamData()
				{
					teamName = teamNa.data,
					teamNumber = Convert.ToDouble(teamNo.data)
				});

			// Need to refereh RobotInfoIndexPage after addteam

			popUpReturn();
		}

		async void popUpReturn()
		{
			//await Task.Yield();
			await Navigation.PopPopupAsync();
		}
	}
}
