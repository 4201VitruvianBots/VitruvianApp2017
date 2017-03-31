using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
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

			var eventStatTestBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Test OPR Grab",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};

			eventStatTestBtn.Clicked += (sender, e) => {
				getEventStats();
			};

			var updateTeamStats = new LineEntry("Update Single Team Stats");
			updateTeamStats.inputEntry.Placeholder = "Enter Team No.";
			updateTeamStats.inputEntry.Keyboard = Keyboard.Numeric;
			var singleTeamStatUpdateBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Update",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			singleTeamStatUpdateBtn.Clicked += (sender, e) => {
				updateAvgTeamData(Convert.ToInt32(updateTeamStats.data), true);
			};

			var singleTeamStatUpdateView = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				Spacing = 1,
				Children = {
					updateTeamStats,
					singleTeamStatUpdateBtn
				}
			};

			var allTeamStatUpdateBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Update All Team Stats",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			allTeamStatUpdateBtn.Clicked += (sender, e) => {
				allTeamsStatsUpdateGrab();
			};

			var addMatchBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Add Match",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			addMatchBtn.Clicked += (sender, e) => {
				Navigation.PushPopupAsync(new AddMatchPopupPage());
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
					new ScrollView(){
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,

						Content = new StackLayout(){
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.FillAndExpand,

							Children = {
								updateTeamListBtn,
								updateMatchListBtn,
								singleTeamStatUpdateView,
								allTeamStatUpdateBtn,
								addMatchBtn
								//eventStatTestBtn,
							}
						}
					},
					navigationBtns
				}
			};
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
						.Child(((match.match_number < 10) ? "0" + match.match_number.ToString() : match.match_number.ToString()))
						.PutAsync(new EventMatchData() {
							matchNumber = ((match.match_number < 10)? "0" + match.match_number.ToString():match.match_number.ToString()),
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

		public async Task getEventStats() {
			var test = await EventsHttp.GetEventStatsHttp(GlobalVariables.regionalPointer);
		}

		public async Task allTeamsStatsUpdateGrab() {
			if (CheckInternetConnectivity.InternetStatus()) {
				busyIcon.IsRunning = true;
				busyIcon.IsVisible = true;

				var teamList = await EventsHttp.GetEventTeamsListHttp(GlobalVariables.regionalPointer);
				var sorted = from Teams in teamList orderby Teams.team_number select Teams;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);

				foreach (var team in sorted)
					await updateAvgTeamData(team.team_number, false);

				await DisplayAlert("Success", "Team Stats Updated", "OK");

				busyIcon.IsRunning = true;
				busyIcon.IsVisible = true;
			}
		}

		public async Task updateAvgTeamData(int teamNumber, bool awaiter) {
			if (CheckInternetConnectivity.InternetStatus()) {
				busyIcon.IsRunning = true;
				busyIcon.IsVisible = true;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);

				var fbMatches = await db
									.Child(GlobalVariables.regionalPointer)
									.Child("teamMatchData")
									.Child(teamNumber.ToString())
									.OnceAsync<TeamMatchData>();

				var fbTeamData = await db
									.Child(GlobalVariables.regionalPointer)
									.Child("teamData")
									.Child(teamNumber.ToString())
									.OnceSingleAsync<TeamData>();

				int mCount = 0, successfulClimbs = 0, attemptedClimbs = 0, totalFouls = 0, totalGood = 0;
				int autoCrosses = 0;
				double autoGearsScored = 0, autoGearsDelivered = 0, autoGearsDropped = 0, autoHighHits = 0, autoPressure = 0;
				double teleopActions = 0, teleOpPressure = 0, teleOpHighAcc = 0, teleOpGearsScored = 0,
					teleOpGearsTransitDropped = 0, teleOpGearsStationDropped = 0;
				int autoPressureHigh = 0, teleOpActionsHigh = 0, teleOpGearsScoredHigh = 0, teleOpGearsStationDroppedHigh = 0,
					teleOpGearsTransitDroppedHigh = 0;
				double teleOpPressureHigh = 0;

				foreach (var match in fbMatches) {
					autoCrosses += match.Object.autoCross ? 1 : 0;
					autoGearsScored += match.Object.autoGearScored ? 1 : 0;
					autoGearsDelivered += match.Object.autoGearDelivered ? 1 : 0;
					autoGearsDropped += match.Object.autoGearDropped ? 1 : 0;
					autoHighHits += match.Object.autoHighHits;
					autoPressure += match.Object.autoPressure;

					teleopActions += match.Object.actionCount;
					teleOpPressure += match.Object.teleOpTotalPressure;
					teleOpHighAcc += match.Object.teleOpHighAcc;
					teleOpGearsScored += match.Object.teleOpGearsDeposit;
					teleOpGearsTransitDropped += match.Object.teleOpGearsTransitDropped;
					teleOpGearsStationDropped += match.Object.teleOpGearsStationDropped;

					successfulClimbs += match.Object.successfulClimb ? 1 : 0;
					attemptedClimbs += match.Object.attemptedClimb ? 1 : 0;
					totalFouls += match.Object.fouls;
					totalGood += match.Object.good ? 1 : 0;

					if (match.Object.autoPressure > autoPressureHigh)
						autoPressureHigh = match.Object.autoPressure;
					if (match.Object.actionCount > teleOpActionsHigh)
						teleOpActionsHigh = match.Object.actionCount;
					if (match.Object.teleOpGearsDeposit > teleOpGearsScoredHigh)
						teleOpGearsScoredHigh = match.Object.teleOpGearsDeposit;
					if (match.Object.teleOpGearsTransitDropped > teleOpGearsTransitDroppedHigh)
						teleOpGearsTransitDroppedHigh = match.Object.teleOpGearsTransitDropped;
					if (match.Object.teleOpGearsStationDropped > teleOpGearsStationDroppedHigh)
						teleOpGearsStationDroppedHigh = match.Object.teleOpGearsStationDropped;
					if (match.Object.teleOpTotalPressure > teleOpPressureHigh)
						teleOpPressureHigh = match.Object.teleOpTotalPressure;

					mCount++;
				}

				fbTeamData.matchCount = mCount;
				fbTeamData.autoPressureHigh = autoPressureHigh;
				fbTeamData.teleOpActionsHigh = teleOpActionsHigh;
				fbTeamData.teleOpGearsScoredHigh = teleOpGearsScoredHigh;
				fbTeamData.teleOpGearsTransitDroppedHigh = teleOpGearsTransitDroppedHigh;
				fbTeamData.teleOpGearsStationDroppedHigh = teleOpGearsStationDroppedHigh;
				fbTeamData.teleOpPressureHigh = teleOpPressureHigh;

				fbTeamData.totalAutoCrossSuccesses = autoCrosses;
				fbTeamData.avgAutoPressure = autoPressure / mCount;
				fbTeamData.avgAutoHighHits = autoHighHits / mCount;
				fbTeamData.avgAutoGearScored = autoGearsScored / mCount;
				fbTeamData.avgAutoGearsDelivered = autoGearsDelivered / mCount;
				fbTeamData.avgAutoGearsDropped = autoGearsDropped / mCount;

				fbTeamData.avgTeleOpActions = teleopActions / mCount;
				fbTeamData.avgTeleOpPressure = teleOpPressure / mCount;
				fbTeamData.avgTeleOpHighAccuracy = teleOpHighAcc / mCount;
				fbTeamData.avgTeleOpGearsScored = teleOpGearsScored / mCount;
				fbTeamData.avgTeleOpGearsTransitDropped = teleOpGearsTransitDropped / mCount;
				fbTeamData.avgTeleOpGearsStationDropped = teleOpGearsStationDropped / mCount;

				fbTeamData.successfulClimbCount = successfulClimbs;
				fbTeamData.attemptedClimbCount = attemptedClimbs;
				fbTeamData.foulCount = totalFouls;
				fbTeamData.goodCount = totalGood;

				var fbTeam = db
							.Child(GlobalVariables.regionalPointer)
							.Child("teamData")
							.Child(teamNumber.ToString())
					 		.PutAsync(fbTeamData);

				if (awaiter)
					await DisplayAlert("Success", "Team Stat Updated", "OK");

				Console.WriteLine("Updated Team Stats: " + fbTeamData.teamNumber);

				busyIcon.IsRunning = false;
				busyIcon.IsVisible = false;
			}
		}
	}
}
