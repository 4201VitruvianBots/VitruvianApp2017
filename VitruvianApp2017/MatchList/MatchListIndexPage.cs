using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using TheBlueAlliance;

namespace VitruvianApp2017
{
	public class MatchListIndexPage : ContentPage
	{


		ScrollView matchIndex;
		StackLayout matchStack = new StackLayout() {
			Spacing = 1,
			BackgroundColor = Color.Silver
		};

		ActivityIndicator busyIcon = new ActivityIndicator();

		public MatchListIndexPage() {
			Title = "Match List";
			Label titleLbl = new Label() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Match List",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("1B5E20"),
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};


			matchIndex = new ScrollView() {
				Content = matchIndex
			};

			var navigationBtns = new NavigationButtons(true);
			navigationBtns.refreshBtn.Clicked += (object sender, EventArgs e) => {
				UpdateMatchList();
			};

			this.Appearing += (object sender, EventArgs e) => {
				UpdateMatchList();
			};

			this.Content = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					titleLbl,
					busyIcon,
					matchIndex,
					navigationBtns
				}
			};

			BackgroundColor = Color.White;
		}

		public async Task UpdateMatchList() {
			if (CheckInternetConnectivity.InternetStatus()) {
				busyIcon.IsVisible = true;
				busyIcon.IsRunning = true;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				//var tbaTeams = Events.GetEventTeamsListHttp("2017calb");
				var fbMatches = await db
						.Child(GlobalVariables.regionalPointer)
						.Child("matchList")
						.OnceAsync<EventMatchData>();
				//var sorted = fbTeams.OrderByDescending((arg) => arg.Key("team_number"));

				matchStack.Children.Clear();

				foreach (var match in fbMatches) {
					/*
					TeamListCell cell = new TeamListCell();
					cell.teamName.Text = "Team " + team.Object.teamNumber.ToString();
					matchStack.Children.Add(cell);
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

					if (match != null) {
						tap.Tapped += (object sender, EventArgs e) => {
							Navigation.PushPopupAsync(new MatchInfoPopupPage(match.Object));
						};
					} else {
						tap.Tapped += (object sender, EventArgs e) => {
							DisplayAlert("Error:", "No Match Data found!", "OK");
						};
					}
					cell.GestureRecognizers.Add(tap);
					//*/
				}

				busyIcon.IsVisible = false;
				busyIcon.IsRunning = false;
			}
		}
	}
}
