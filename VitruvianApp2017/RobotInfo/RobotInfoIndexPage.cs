using System;
using System.Threading.Tasks;
using System.Linq;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using TheBlueAlliance;

namespace VitruvianApp2017
{
	public class RobotInfoIndexPage : ContentPage
	{
		ScrollView teamIndex;
		StackLayout teamStack = new StackLayout()
		{
			Spacing = 1,
			BackgroundColor = Color.Silver
		};

		ActivityIndicator busyIcon = new ActivityIndicator();

		public RobotInfoIndexPage()
		{
			Title = "Robot Info";
			Label titleLbl = new Label()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Robot Info",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("1B5E20"),
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
				UpdateTeamList();
			};

			this.Appearing += (object sender, EventArgs e) =>
			{
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
					navigationBtns
				}
			};

			BackgroundColor = Color.White;
		}

		public async Task UpdateTeamList()
		{
			if (CheckInternetConnectivity.InternetStatus()) {
				busyIcon.IsVisible = true;
				busyIcon.IsRunning = true;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				//var tbaTeams = Events.GetEventTeamsListHttp("2017calb");
				var fbTeams = await db
						.Child(GlobalVariables.regionalPointer)
						.Child("teamData")
						.OnceAsync<TeamData>();
				//var sorted = fbTeams.OrderByDescending((arg) => arg.Key("team_number"));

				teamStack.Children.Clear();

				foreach (var team in fbTeams) {
					TeamListCell cell = new TeamListCell();
					cell.teamName.Text = "Team " + team.Object.teamNumber.ToString();
					teamStack.Children.Add(cell);
					TapGestureRecognizer tap = new TapGestureRecognizer();

					/*
					var data = await db
						.Child(GlobalVariables.regionalPointer)
						.Child("teamData")
						.Child(team.team_number.ToString())
						.OnceSingleAsync<TeamData>();

					if (data == null)
					{
						var send = db
							.Child(GlobalVariables.regionalPointer)
							.Child("teamData")
							.Child(team.team_number.ToString())
							.PutAsync(new TeamData()
							{
								teamName = team.nickname,
								teamNumber = Convert.ToDouble(team.team_number)
							});

						data = await db
							.Child(GlobalVariables.regionalPointer)
							.Child("teamData")
							.Child(team.team_number.ToString())
							.OnceSingleAsync<TeamData>();
					}
					*/

					if (team != null) {
						tap.Tapped += (object sender, EventArgs e) => {
							Navigation.PushPopupAsync(new TeamCardPopupPage(team.Object));
						};
					} else {
						tap.Tapped += (object sender, EventArgs e) => {
							DisplayAlert("Error:", "No Team Data found!", "OK");
						};
					}
					cell.GestureRecognizers.Add(tap);
				}

				busyIcon.IsVisible = false;
				busyIcon.IsRunning = false;
			}
		}
	}
}
