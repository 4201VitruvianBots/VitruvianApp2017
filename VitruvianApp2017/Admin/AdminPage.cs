using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using TheBlueAlliance;
using TheBlueAlliance.Models;

namespace VitruvianApp2017
{
	public class AdminPage:ContentPage
	{
		/*
		Grid grid = new Grid() {


		}
		*/

		ActivityIndicator busyIcon = new ActivityIndicator() {
			IsVisible = false,
			IsRunning = false
		};

		public AdminPage() {
			Title = "Admin Page";

			var updateMatchListBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Update Match List",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			updateMatchListBtn.Clicked += (sender, e) => {
				UpdateMatchList();
			};

			var updateTeamListBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Update Team List",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			updateTeamListBtn.Clicked += (sender, e) => {
				UpdateTeamList();
			};

			var logoutBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Logout",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			logoutBtn.Clicked += (sender, e) => {
				logout();
			};
			Button[] btnArray = { logoutBtn };
			var navigationBtns = new NavigationButtons(false, btnArray);

			Content = new StackLayout() {
				Children = {
					busyIcon,
					updateTeamListBtn,
					updateMatchListBtn,
					navigationBtns
				}
			};
			Console.WriteLine("Regional Pointer: " + GlobalVariables.regionalPointer);
		}

		void logout() {
			AppSettings.SaveSettings("AdminLogin", "false");

			Navigation.PopToRootAsync();
		}

		public async Task UpdateTeamList() {
			busyIcon.IsRunning = true;
			busyIcon.IsVisible = true;
			if (CheckInternetConnectivity.InternetStatus()) {
				var teamList = await EventsHttp.GetEventTeamsListHttp(GlobalVariables.regionalPointer);
				var sorted = from Teams in teamList orderby Teams.team_number select Teams;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				
				foreach (var team in sorted) {
					var fbTeam = await db
							.Child(GlobalVariables.regionalPointer)
							.Child("teamData")
							.Child(team.team_number.ToString())
							.OnceSingleAsync<TeamData>();
					if (fbTeam == null) {
						var send = db
							.Child(GlobalVariables.regionalPointer)
							.Child("teamData")
							.Child(team.team_number.ToString())
							.PutAsync(new TeamData() {
								teamName = team.nickname,
								teamNumber = team.team_number
							});
					}
					Console.WriteLine("Team Added: " + team.team_number);
				}
				await DisplayAlert("Done", "Team List Successfully Updated", "OK");
				busyIcon.IsRunning = false;
				busyIcon.IsVisible = false;
			}
		}

		public async Task UpdateMatchList() {
			if (CheckInternetConnectivity.InternetStatus()) {
				busyIcon.IsRunning = true;
				busyIcon.IsVisible = true;

				var matchList = await EventsHttp.GetEventMatchesHttp(GlobalVariables.regionalPointer);
				var sorted = from Match in matchList orderby Match.time select Match;


				foreach (var match in sorted)
					Console.WriteLine("Match: " + match.match_number);
				
				var db = new FirebaseClient(GlobalVariables.firebaseURL);
 
				foreach (var match in sorted) {
					int n = 0, m = 0;
					int[] blueA = new int[3], redA = new int[3];

					foreach (var team in match.alliances.blue.teams) {
						blueA[n] = Convert.ToInt32(match.alliances.blue.teams[n].Substring(3));
						//Console.WriteLine("Blue: " + blueA[n]);
						n++;
					}
					foreach (var team in match.alliances.red.teams) {
						redA[m] = Convert.ToInt32(match.alliances.red.teams[m].Substring(3));
						//Console.WriteLine("Red: " + redA[m]);
						m++;;
					}

					var fbMatch = await db
							.Child(GlobalVariables.regionalPointer)
							.Child("matchList")
							.Child(match.match_number.ToString())
							.OnceSingleAsync<EventMatchData>();

					var send = db
						.Child(GlobalVariables.regionalPointer)
						.Child("matchList")
						.Child("QM" + ((match.match_number < 10) ? "0" + match.match_number.ToString() : match.match_number.ToString()))
						.PutAsync(new EventMatchData() {
							matchNumber = "QM" + ((match.match_number < 10)? "0" + match.match_number.ToString():match.match_number.ToString()),
							Blue = blueA,
							Red = redA,
							matchTime = match.time
						});

					Console.WriteLine("Completed Match: " + match.match_number);
				}
				await DisplayAlert("Done", "Match List Successfully Updated", "OK");
				busyIcon.IsRunning = false;
				busyIcon.IsVisible = false;
			}
		}
	}
}
