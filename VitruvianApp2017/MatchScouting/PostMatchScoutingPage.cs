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
		TeamMatchData matchData;
		int mType;

		ColorButton climbAttemptBtn, climbSuccessBtn;
		SingleCounter foulCounter;
		CheckBox goodCheck;

		public PostMatchScoutingPage(TeamMatchData data, int matchType) {
			Title = "Post Match";
			matchData = data;
			mType = matchType;

			goodCheck = new CheckBox() {
				DefaultText = "Did this team perform well?",
				FontSize = GlobalVariables.sizeSmall
			};

			var disabledBtn = new ColorButton("Disabled");
			var climbingLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Climbing",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold

			};
			climbAttemptBtn = new ColorButton("Attempt");
			climbSuccessBtn = new ColorButton("Success");
			climbAttemptBtn.Clicked += (sender, e) => {
				if (climbSuccessBtn.on) {
					climbSuccessBtn.on = false;
					climbSuccessBtn.BackgroundColor = Color.Red;
				}
			};
			climbSuccessBtn.Clicked += (sender, e) => {
				if (climbAttemptBtn.on) {
					climbAttemptBtn.on = false;
					climbAttemptBtn.BackgroundColor = Color.Red;
				}
			};

			foulCounter = new SingleCounter("Fouls");

			var finishMatchBtn = new Button() {
				Text = "Save Match Data",
				FontSize = GlobalVariables.sizeMedium,
				BackgroundColor = Color.Green,
			};
			finishMatchBtn.Clicked += (sender, e) => {
				saveData();
				updateAvgTeamData();

				//Console.WriteLine("Nav Count: " + Navigation.NavigationStack.Count);
				//Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
				//Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

				Navigation.PushAsync(new PreMatchScoutingPage(matchData.scouterName));
				//Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
				//Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
			};

			Content = new StackLayout() {
				Children = {
					new ScrollView(){
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,

						Content = new StackLayout(){
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.FillAndExpand,

							Children = {
								disabledBtn,
								climbingLbl,
								climbAttemptBtn,
								climbSuccessBtn,
								foulCounter,
								goodCheck
							}
						}
					},
					finishMatchBtn
				}
			};
		}
		async Task saveData() {
			if (CheckInternetConnectivity.InternetStatus()) {
				matchData.attemptedClimb = climbAttemptBtn.on;
				matchData.successfulClimb = climbSuccessBtn.on;
				matchData.fouls = foulCounter.getValue();
				matchData.good = goodCheck.Checked;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);

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
			}
		}

		public async Task updateAvgTeamData() {
			if (CheckInternetConnectivity.InternetStatus()) {
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
							.Child(matchData.teamNumber.ToString())
							.PutAsync(fbTeamData);
			}
		}
	}
}
