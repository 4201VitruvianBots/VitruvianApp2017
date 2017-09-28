using System;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using XLabs.Forms.Controls;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class PostMatchScoutingPage:ContentPage
	{
		Grid pageLayout = new Grid() {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.White,

			RowDefinitions = {
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Star},
				new RowDefinition() { Height = GridLength.Auto},
			}
		};

		MatchData matchData;
		int mType;
		Label pressureLbl, gearLbl;

		TitledColorButton climbSuccessBtn, fullDisableBtn, pilotErrorBtn;
		SingleCounter foulCounter, partialDisablesCounter;
		CheckBox goodCheck;
		Editor foulNotes;

		public PostMatchScoutingPage(MatchData data, int matchType) {
			Title = "Post Match";
			matchData = data;
			mType = matchType;

			Label teamNumberLbl = new Label() {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Text = "Team: " + matchData.teamNumber.ToString(),
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			pressureLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Pressure: " + (matchData.autoPressure + matchData.teleOpPressure),
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			gearLbl = new Label() {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Text = "Gears: " + ((matchData.autoGearScored ? 1 : 0) + matchData.teleOpGearsScored),
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};


			climbSuccessBtn = new TitledColorButton("Climbing", "Success");

			goodCheck = new CheckBox() {
				DefaultText = "Did this team perform well?",
				FontSize = GlobalVariables.sizeSmall
			};

			fullDisableBtn = new TitledColorButton("Full Disable", "Disabled");
			partialDisablesCounter = new SingleCounter("Partial Disables");

			foulCounter = new SingleCounter("Fouls");

			var foulNotesLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Foul Notes",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			foulNotes = new Editor() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Gray
			};

			pilotErrorBtn = new TitledColorButton("Pilot Error", "Error");

			var finishMatchBtn = new Button() {
				Text = "Save Match Data",
				FontSize = GlobalVariables.sizeMedium,
				BackgroundColor = Color.Green,
			};
			finishMatchBtn.Clicked += (sender, e) => {
				finishMatch();
			};

			var topBar = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Spacing = 0,
			};

			topBar.Children.Add(teamNumberLbl);
			topBar.Children.Add(pressureLbl);
			topBar.Children.Add(gearLbl);

			pageLayout.Children.Add(climbSuccessBtn, 0, 0);
			pageLayout.Children.Add(pilotErrorBtn, 0, 1);
			pageLayout.Children.Add(goodCheck, 0, 2);
			pageLayout.Children.Add(fullDisableBtn, 1, 0);
			pageLayout.Children.Add(partialDisablesCounter, 1, 1);
			pageLayout.Children.Add(foulCounter, 2, 0);
			pageLayout.Children.Add(foulNotesLbl, 2, 1);
			pageLayout.Children.Add(foulNotes, 2, 3, 1, 4);

			pageLayout.Children.Add(finishMatchBtn, 1, 2, 5, 6);

			Content = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 0,

				Children = {
					topBar,
					new ScrollView(){
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						IsClippedToBounds = true,

						Content = pageLayout
					}
				}
			};
		}

		async Task finishMatch() {
			if(await DisplayAlert("Match Finished", "Are you sure you want to save this data?", "Yes", "No")) {
				saveData();

				while (Navigation.ModalStack.Count > 0)
					Navigation.PopModalAsync();

				Navigation.PushModalAsync(new PreMatchScoutingPage(matchData.scouterName));
			}
		}

		async Task saveData() {
			if (CheckInternetConnectivity.InternetStatus()) {
				matchData.successfulClimb = climbSuccessBtn.getBtnStatus();
				matchData.pilotError = pilotErrorBtn.getBtnStatus();
				matchData.good = goodCheck.Checked;
				matchData.fullDisable = fullDisableBtn.getBtnStatus();
				matchData.partialDisables = partialDisablesCounter.getValue();
				matchData.fouls = foulCounter.getValue();
				matchData.foulNote = foulNotes.Text;
				matchData.sendTime = DateTimeOffset.Now.ToUnixTimeSeconds();
				matchData.dataIsReady = true;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				string path = "ERROR";

				if (mType == -1)
					path = "practiceMatchData/" + matchData.matchID;
				else
					path = "matchData/" + matchData.matchID;

				FirebaseAccess.saveData(db, path, matchData);

				/*
				if (mType == -1) {
					var send = db
								.Child(GlobalVariables.regionalPointer)
								.Child("PracticeMatches")
								.Child(matchData.teamNumber.ToString())
								.Child(matchData.matchNumber.ToString())
								.PutAsync(matchData);
				} else {
					var fbTeam = db
								.Child(GlobalVariables.regionalPointer)
								.Child("teamMatchData")
								.Child(matchData.teamNumber.ToString())
								.Child(matchData.matchNumber.ToString())
								.PutAsync(matchData);
				}
				*/
			}
		}

		/*
		public async Task updateAvgTeamData() {
			if (CheckInternetConnectivity.InternetStatus()) {
				try {
					var db = new FirebaseClient(GlobalVariables.firebaseURL);

					var fbMatches = await db
										.Child(GlobalVariables.regionalPointer)
										.Child("teamMatchData")
										.Child(matchData.teamNumber.ToString())
										.OnceAsync<TeamMatchData>();

					var fbTeamData = await db
										.Child(GlobalVariables.regionalPointer)
										.Child("teamData")
										.Child(matchData.teamNumber.ToString())
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
								.Child(matchData.teamNumber.ToString())
								.PutAsync(fbTeamData);
				} catch (Exception ex) {
					Console.WriteLine("avgTeamData error: " + ex.Message);
				}
			}
		}
		*/
	}
}
