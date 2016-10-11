using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Firebase.Xamarin;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

namespace VitruvianApp2017
{
	public class RobotInfoIndexPage : ContentPage
	{
		ScrollView teamIndex;
		StackLayout teamStack = new StackLayout()
		{
			Padding = new Thickness(0, 0, 0, 1),
			BackgroundColor = Color.Silver
		};

		ActivityIndicator busyIcon = new ActivityIndicator();

		public RobotInfoIndexPage()
		{
			Title = "Team 4201 Scouting App";
			Label titleLbl = new Label()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Team 4201 Scouting App",
				TextColor = Color.White,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};


			teamIndex = new ScrollView()
			{
				Content = teamStack
			};

			var navigationBtns = new NavigationButtons(true);
			navigationBtns.refreshBtn.Clicked += (object sender, EventArgs e) =>
			{
				if (new CheckInternetConnectivity().InternetStatus())
					UpdateTeamList();
			};

			Button addTeamBtn = new Button()
			{
				Text = "Add Team"
			};
			addTeamBtn.Clicked += (object sender, EventArgs e) =>
			{
				popUpPage();
			};

			this.Appearing += (object sender, EventArgs e) =>
			{
				if (new CheckInternetConnectivity().InternetStatus())
					UpdateTeamList();
			};

			this.Content = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					titleLbl,
					busyIcon,
					teamIndex,
					navigationBtns,
					addTeamBtn
				}
			};

			BackgroundColor = Color.White;
		}

		async void UpdateTeamList()
		{
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;
			var db = new FirebaseClient("https://vitruvianapptest.firebaseio.com/");
			var teams = await db
				.Child("teamData")
				.OrderByKey()
				.OnceAsync<TeamData>();
			
			teamStack.Children.Clear();

			foreach (var team in teams)
			{
				TeamListCell cell = new TeamListCell();
				cell.teamName.Text = "Team " + team.Object.teamNumber.ToString();
				teamStack.Children.Add(cell);
				TapGestureRecognizer tap = new TapGestureRecognizer();
				tap.Tapped += (object sender, EventArgs e) =>
				{
					//Navigation.PushModalAsync(new RobotInfoViewPage(obj));
				};
				cell.GestureRecognizers.Add(tap);
			}

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;

		}

		async void popUpPage()
		{
			await Task.Yield();
			await PopupNavigation.PushAsync(new AddTeamPopupPage(), false);
		}
	}
}
