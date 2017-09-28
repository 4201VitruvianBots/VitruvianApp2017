using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace VitruvianApp2017 {
	public class TeleOpMatchScoutingPage : ContentPage {
		Grid pageLayout = new Grid() {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.White,

			RowDefinitions = {
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Star},
				new RowDefinition() { Height = GridLength.Auto},
			},
			ColumnDefinitions = {
			}
		};

		MatchData matchData;
		int mType;
		Label teleOpPressureLbl, teleOpGearLbl;
		SingleCounter actionCounter, gearCounter;
		MultiCounter pressureCounter;


		public TeleOpMatchScoutingPage(MatchData data, int matchType) {
			mType = matchType;
			matchData = data;

			Title = "TeleOp Mode";

			Label teamNumberLbl = new Label() {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Text = "Team: " + matchData.teamNumber.ToString(),
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			teleOpPressureLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Pressure: " + (matchData.autoPressure),
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			teleOpGearLbl = new Label() {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Text = "Gears: " + ((matchData.autoGearScored ? 1 : 0)),
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			actionCounter = new SingleCounter("Actions");

			pressureCounter = new MultiCounter("Pressure");
			pressureCounter.PropertyChanged += (sender, e) => {
				teleOpPressureLbl.Text = "Total Pressure: " + (matchData.autoPressure + pressureCounter.getValue());
			};

			gearCounter = new SingleCounter("Gears");
			gearCounter.PropertyChanged += (sender, e) => {
				actionCounter.addToLowerLimit(gearCounter.getValue());
			};

			var finishBtn = new Button() {
				Text = "FINISH",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Yellow
			};

			finishBtn.Clicked += (sender, e) => {
				saveData();
				Navigation.PushModalAsync(new PostMatchScoutingPage(matchData, mType));
			};

			var topBar = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Spacing = 0,
			};

			topBar.Children.Add(teamNumberLbl);
			topBar.Children.Add(teleOpPressureLbl);
			topBar.Children.Add(teleOpGearLbl);

			pageLayout.Children.Add(actionCounter, 0, 0);
			pageLayout.Children.Add(pressureCounter, 1, 2, 0, 2);
			pageLayout.Children.Add(gearCounter, 2, 0);

			pageLayout.Children.Add(finishBtn, 1, 2, 3, 4);

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

		/*
		void addAction(int v) {
			bool t = false;
			mActions[aIndex] = new ActionData();
			try {
				foreach (var hopper in hopperCapacity.on)
					if (hopper == true) {
						t = true;
						break;
					}
				if (t)
					mActions[aIndex].hopperCapacity = hopperCapacity.getAvgPercentage() * robotMaxCapacity;
				else
					mActions[aIndex].hopperCapacity = 0;
			}
			catch{
				mActions[aIndex].hopperCapacity = 0;
			}

			if (v == 0) // gear scored
				mActions[aIndex].cyclePressure = 0;
			else if (v == 1) // high shots
				mActions[aIndex].cyclePressure = (int)Math.Floor((robotMaxCapacity * hopperCapacity.getAvgPercentage() * goalAccuracy.getAvgPercentage()) / 3);
			else if (v == 2) { // low shots
				mActions[aIndex].lowGoalDump = true;
				mActions[aIndex].cyclePressure = (int)Math.Floor((robotMaxCapacity * hopperCapacity.getAvgPercentage()) / 9);
			} else // no complete scoring action
				mActions[aIndex].lowGoalDump = false;

			mActions[aIndex].gearsStationDrop = gearsStationDropped.getValue();
			mActions[aIndex].gearsTransitDrop = gearsTransitDropped.getValue();
			mActions[aIndex].gearSet = gearScoredBtn.on;

			if (gearScoredBtn.on) {
				gearScoredBtn.on = false;
				gearScoredBtn.BackgroundColor = Color.Red;
			}

			lastActionLabels[0].Text = "Hopper Capacity: " + mActions[aIndex].hopperCapacity;
			lastActionLabels[1].Text = "High Accuracy: " + mActions[aIndex].highGoalAccuracy;
			lastActionLabels[2].Text = "Low Goal Dump: " + mActions[aIndex].lowGoalDump;
			lastActionLabels[3].Text = "Cycle Pressure: " + mActions[aIndex].cyclePressure;
			lastActionLabels[4].Text = "Gear Set: " + mActions[aIndex].gearSet;
			lastActionLabels[5].Text = "Gear Station Drops: " + mActions[aIndex].gearsStationDrop;
			lastActionLabels[6].Text = "Gear Transit Drops: " + mActions[aIndex].gearsTransitDrop;
			clearValues();

			if (v != 3) // aCount != aIndex in the event that the last 'action' the robot performs yeilds no score
				aCount++;
			aIndex++;
			actionCounter.Text = aIndex.ToString();
			actionCounter.BackgroundColor = Color.Transparent;
			cycleUndo.BackgroundColor = Color.Yellow;
		}

		void undoAction() {
			if (aIndex > 1) {
				lastActionLabels[0].Text = "Hopper Capacity: " + mActions[aIndex - 1].hopperCapacity;
				lastActionLabels[1].Text = "High Accuracy: " + mActions[aIndex - 1].highGoalAccuracy;
				lastActionLabels[2].Text = "Low Goal Dump: " + mActions[aIndex - 1].lowGoalDump;
				lastActionLabels[3].Text = "Cycle Pressure: " + mActions[aIndex - 1].cyclePressure;
				lastActionLabels[4].Text = "Gear Set: " + mActions[aIndex - 1].gearSet;
				lastActionLabels[5].Text = "Gear Station Drops: " + mActions[aIndex - 1].gearsStationDrop;
				lastActionLabels[6].Text = "Gear Transit Drops: " + mActions[aIndex - 1].gearsTransitDrop;
				aIndex--;
			} else {
				lastActionLabels[0].Text = "Hopper Capacity: " + 0;
				lastActionLabels[1].Text = "High Accuracy: " + 0;
				lastActionLabels[2].Text = "Low Goal Dump: " + false;
				lastActionLabels[3].Text = "Cycle Pressure: " + 0;
				lastActionLabels[4].Text = "Gear Set: " + false;
				lastActionLabels[5].Text = "Gear Station Drops: " + 0;
				lastActionLabels[6].Text = "Gear Transit Drops: " + 0;
			}

			actionCounter.Text = aCount.ToString();
			actionCounter.BackgroundColor = Color.Orange;
			lastActionView.BackgroundColor = Color.Orange;
		}

		*/

		async Task saveData() {
			matchData.actionCount = actionCounter.getValue();
			matchData.teleOpPressure = pressureCounter.getValue();
			matchData.teleOpGearsScored = gearCounter.getValue();

			if (CheckInternetConnectivity.InternetStatus()) {
				/*
				matchData.actionCount = aCount;
				matchData.teleOpTotalPressure = teleOpPressure;
				matchData.teleOpGearsDeposit = teleOpGears;

				int stationDropped = 0, transitDropped = 0;
				double hAcc = 0;
				for (int i = 0; i < aIndex; i++) {
					hAcc += mActions[i].highGoalAccuracy;
					stationDropped += mActions[i].gearsStationDrop;
					transitDropped += mActions[i].gearsTransitDrop;
				}
				matchData.teleOpHighAcc = hAcc / aCount;
				matchData.teleOpGearsStationDropped = stationDropped;
				matchData.teleOpGearsTransitDropped = transitDropped;
				*/

				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				string path = "ERROR";

				if (mType == -1)
					path = "practiceMatchData/" + matchData.matchID;
				else {
					path = "matchData/" + matchData.matchID;
					FirebaseAccess.saveData(db, "matchActionData/" + matchData.matchID, matchData);
				}

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
					var matchBreakDown = db
										.Child(GlobalVariables.regionalPointer)
										.Child("teamMatchActionData")
										.Child(matchData.teamNumber.ToString())
										.Child(matchData.matchNumber.ToString())
										.PutAsync(mActions);

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
	}
}
