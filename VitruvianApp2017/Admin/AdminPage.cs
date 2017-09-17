using System;
using System.Collections.Generic;
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
				//getEventStats();
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
				//updateAvgTeamData(Convert.ToInt32(updateTeamStats.data), true);
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
				//allTeamsStatsUpdateGrab();
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

			var updateTeamOPRS = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Update OPRS",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			updateTeamOPRS.Clicked += (sender, e) => {
				getEventOprs();
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
								//ingleTeamStatUpdateView,
								//allTeamStatUpdateBtn,
								addMatchBtn,
								updateTeamOPRS
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

				string matchNumber;
				foreach (var match in sorted.ToArray())
					Console.WriteLine("Match: " + match.match_number);
				
				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				foreach (var match in sorted) {
					if (match.comp_level == "qm") {
						if (match.match_number < 10)
							matchNumber = match.comp_level.ToUpper() + "0" + match.match_number;
						else
							matchNumber = match.comp_level.ToUpper() + match.match_number;
					}
					else
						matchNumber = match.comp_level.ToUpper() + match.set_number + "M" + match.match_number;

					int m = 0, n = 0;
					int[] blueA = new int[match.alliances.blue.team_keys.Length + match.alliances.blue.surrogate_team_keys.Length], 
					       redA = new int[match.alliances.red.team_keys.Length + match.alliances.red.surrogate_team_keys.Length];

					// remove the frc label from each team
					foreach (var team in match.alliances.blue.team_keys)
						blueA[m++] = Convert.ToInt32(team.Remove(0, 3));
					foreach (var team in match.alliances.blue.surrogate_team_keys)
						blueA[m++] = Convert.ToInt32(team.Remove(0, 3));
					foreach (var team in match.alliances.red.team_keys)
						redA[n++] = Convert.ToInt32(team.Remove(0, 3));
					foreach (var team in match.alliances.red.surrogate_team_keys)
						redA[n++] = Convert.ToInt32(team.Remove(0, 3));

					int i = 1;
					string matchID;
					foreach (var robot in blueA) {
						matchID = robot + "-" + matchNumber;
						await db
								.Child(GlobalVariables.regionalPointer)
								.Child("tableauMatchSchedule")
								.Child(matchID)
								.OnceSingleAsync<TableauMatchShedule>();

						await db
								.Child(GlobalVariables.regionalPointer)
								.Child("tableauMatchSchedule")
								.Child(matchID)
								.PutAsync(new TableauMatchShedule() {
									matchID = matchID,
									matchNumber = matchNumber,
									alliance = "Blue",
									alliancePos = i++,
									teamNumber = robot,
									matchTime = match.time
								});
					}

					i = 1;
					foreach (var robot in redA) {
						matchID = robot + "-" + matchNumber;
						await db
								.Child(GlobalVariables.regionalPointer)
								.Child("tableauMatchSchedule")
								.Child(matchID)
								.OnceSingleAsync<TableauMatchShedule>();

						await db
								.Child(GlobalVariables.regionalPointer)
								.Child("tableauMatchSchedule")
								.Child(matchID)
								.PutAsync(new TableauMatchShedule() {
									matchID = matchID,
									matchNumber = matchNumber,
									alliance = "Red",
									alliancePos = i++,
									teamNumber = robot,
									matchTime = match.time
								});
					}
					// Why is this needed?

					var fbMatch = await db
							.Child(GlobalVariables.regionalPointer)
							.Child("matchList")
							.Child(matchNumber)
							.OnceSingleAsync<EventMatchData>();

					var send = db
						.Child(GlobalVariables.regionalPointer)
						.Child("matchList")
						.Child(matchNumber)
						.PutAsync(new EventMatchData() {
							matchNumber = matchNumber,
							Blue = blueA,
							Red = redA,
							matchTime = match.time
						});
					Console.WriteLine("Completed Match: " + matchNumber);
				}
				await DisplayAlert("Done", "Match List Successfully Updated", "OK");
				busyIcon.IsRunning = false;
				busyIcon.IsVisible = false;
			}
		}

		public async Task getEventOprs() {
			var data = await EventsHttp.GetEventTeamsOprsHttp(GlobalVariables.regionalPointer);
			SortedDictionary<int, double> oprs = new SortedDictionary<int, double>();
			SortedDictionary<int, double> dprs = new SortedDictionary<int, double>();
			SortedDictionary<int, double> ccwms = new SortedDictionary<int, double>();

			foreach (var i in data.oprs)
				oprs.Add(Convert.ToInt32(i.Key.Remove(0, 3)), i.Value);
			foreach (var i in data.dprs)
				dprs.Add(Convert.ToInt32(i.Key.Remove(0, 3)), i.Value);
			foreach (var i in data.ccwms)
				ccwms.Add(Convert.ToInt32(i.Key.Remove(0, 3)), i.Value);

			var db = new FirebaseClient(GlobalVariables.firebaseURL);
			var teamData = await db
								.Child(GlobalVariables.regionalPointer)
								.Child("teamData")
								.OnceAsync<TeamData>();
			
			foreach (var i in teamData) {
				double result;

				if (oprs.TryGetValue(i.Object.teamNumber, out result))
					i.Object.tbaOPR = result;
				if (dprs.TryGetValue(i.Object.teamNumber, out result))
					i.Object.tbaDPR = result;
				if (ccwms.TryGetValue(i.Object.teamNumber, out result))
					i.Object.tbaCCWM = result;

				await db.Child(GlobalVariables.regionalPointer)
						.Child("teamData")
						.Child(i.Object.teamNumber.ToString())
						.PutAsync(i.Object);
			}
		}

		/*
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

				busyIcon.IsRunning = false;
				busyIcon.IsVisible = false;
			}
		}
		
		public async Task updateAvgTeamData(int teamNumber, bool awaiter) {
			if (CheckInternetConnectivity.InternetStatus()) {
				busyIcon.IsRunning = true;
				busyIcon.IsVisible = true;

				try {
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
					int autoPressureCount = 0, autoHighCount = 0, teleOpPressureCount = 0, teleOpHighCount = 0;
					int climbCount = 0;

					foreach (var match in fbMatches) {
						autoCrosses += match.Object.autoCross ? 1 : 0;
						autoGearsScored += match.Object.autoGearScored ? 1 : 0;
						autoGearsDelivered += match.Object.autoGearDelivered ? 1 : 0;
						autoGearsDropped += match.Object.autoGearDropped ? 1 : 0;
						if (match.Object.autoHighHits > 0) {
							autoHighHits += match.Object.autoHighHits;
							autoHighCount++;
						}
						if (match.Object.autoPressure > 0) {
							autoPressure += match.Object.autoPressure;
							autoPressureCount++;
						}

						teleopActions += match.Object.actionCount;
						if (match.Object.teleOpHighAcc > 0) {
							teleOpHighAcc += match.Object.teleOpHighAcc;
							teleOpHighCount++;
						}
						if (match.Object.teleOpTotalPressure > 0) {
							teleOpPressure += match.Object.teleOpTotalPressure;
							teleOpHighAcc += match.Object.teleOpHighAcc;
							teleOpPressureCount++;
						}
						teleOpGearsScored += match.Object.teleOpGearsDeposit;
						teleOpGearsTransitDropped += match.Object.teleOpGearsTransitDropped;
						teleOpGearsStationDropped += match.Object.teleOpGearsStationDropped;

						successfulClimbs += match.Object.successfulClimb ? 1 : 0;
						attemptedClimbs += match.Object.attemptedClimb ? 1 : 0;
						if (match.Object.successfulClimb != false || match.Object.attemptedClimb != false)
							climbCount++;
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
					fbTeamData.avgAutoPressure = autoPressureCount > 0 ? autoPressure / autoPressureCount : 0;
					fbTeamData.avgAutoHighHits = autoHighCount > 0 ? autoHighHits / autoHighCount : 0;
					fbTeamData.avgAutoGearScored = autoGearsScored / mCount;
					fbTeamData.avgAutoGearsDelivered = autoGearsDelivered / mCount;
					fbTeamData.avgAutoGearsDropped = autoGearsDropped / mCount;

					fbTeamData.avgTeleOpActions = teleopActions / mCount;
					fbTeamData.avgTeleOpPressure = teleOpPressureCount > 0 ? teleOpPressure / teleOpPressureCount : 0;
					fbTeamData.avgTeleOpHighAccuracy = teleOpHighCount > 0 ? teleOpHighAcc / teleOpHighCount : 0;
					fbTeamData.avgTeleOpGearsScored = teleOpGearsScored / mCount;
					fbTeamData.avgTeleOpGearsTransitDropped = teleOpGearsTransitDropped / mCount;
					fbTeamData.avgTeleOpGearsStationDropped = teleOpGearsStationDropped / mCount;

					fbTeamData.successfulClimbCount = successfulClimbs;
					fbTeamData.attemptedClimbCount = attemptedClimbs;
					fbTeamData.climbSuccessRate = climbCount > 0 ? successfulClimbs / climbCount : 0;
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
				} catch (Exception ex) {
					Console.WriteLine("avgTeamData error: " + ex.Message);
					Console.WriteLine("Bad data Team No.: " + teamNumber);
				}
				busyIcon.IsRunning = false;
				busyIcon.IsVisible = false;
			}
		}
		*/
	}
}
