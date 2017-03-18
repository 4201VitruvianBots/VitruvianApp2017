using System;
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
				/*
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Star},
				new RowDefinition() { Height = GridLength.Auto},
				*/
			},
			ColumnDefinitions = {
			}
		};
		ColorButton[] inputs = new ColorButton[3];
		ColorButton gearScored, climbSuccessBtn, climbAttemptBtn;
		Button cycleUndo, lowGoalDumpBtn;
		SingleCounter cycleCounter, gearsStationDropped, gearsTransitDropped;
		ColorButtonStackArray hopperCapacity, highGoalAcc;
		bool lowDumpOn = false;
		Label teleOpPressureLbl, teleOpGearLbl, actionCounter;
		int teleOpGears = 0, teleOpPressure = 0, cPressure = 0;
		int robotMaxCapacity = 0;
		ActionData[] matchActions = new ActionData[999];
		int actionCount = 0;

		TeamMatchData matchData;

		public TeleOpMatchScoutingPage(TeamMatchData data) {
			matchData = data;
			getMaxCapacity();

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
				Text = "Pressure: " + (matchData.autoPressure + teleOpPressure),
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			teleOpGearLbl = new Label() {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Text = "Gears: " + ((matchData.autoGearScored ? 1 : 0) + teleOpGears),
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			Label robotFuelCapacityLbl = new Label() {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Text = "Max Fuel Capacity: " + robotMaxCapacity,
				TextColor = Color.White,
				FontSize = GlobalVariables.sizeTiny,
				FontAttributes = FontAttributes.Bold,
			};

			Label actionCountLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Actions:",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			actionCounter = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "0",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			cycleCounter = new SingleCounter("Cycle Counter");

			var fuelLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Fuel",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};
			hopperCapacity = new ColorButtonStackArray("Capacity", 5);
			highGoalAcc = new ColorButtonStackArray("High Accuracy", 5);
			lowGoalDumpBtn = new Button() {
				Text = "Low Goal Dump",
				BackgroundColor = Color.Yellow,
				FontAttributes = FontAttributes.Bold
			};
			lowGoalDumpBtn.Clicked += (sender, e) => {
				highGoalAcc.setAllFalse();

				lowDumpOn = true;
				lowGoalDumpBtn.BackgroundColor = Color.Orange;
			};
			highGoalAcc.valueChanged += (sender, e) => {
				lowDumpOn = false;
				lowGoalDumpBtn.BackgroundColor = Color.Yellow;

			};

			var fuelScoreBtn = new Button() {
				Text = "Fuel Score",
				BackgroundColor = Color.Green,
				FontAttributes = FontAttributes.Bold
			};
			fuelScoreBtn.Clicked += (sender, e) => {
				addAction();
			};

			gearScored = new ColorButton("Gear Scored");
			gearScored.Clicked += (sender, e) => {
				addAction();
			};

			var droppedLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Gears Dropped:",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center
			};
			gearsStationDropped = new SingleCounter("At Station");
			gearsTransitDropped = new SingleCounter("In Transit");

			var finishBtn = new Button() {
				Text = "FINISH",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				BackgroundColor = Color.Yellow
			};

			finishBtn.Clicked += (sender, e) => {
				saveData();
				Navigation.PushAsync(new PostMatchScoutingPage(matchData));
			};


			var topBar = new Grid() {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Green,
				RowSpacing = 0,
				ColumnSpacing = 0,

				RowDefinitions = {
					new RowDefinition() { Height = GridLength.Auto},
					new RowDefinition() { Height = GridLength.Auto},
				}
			};
			topBar.Children.Add(teamNumberLbl, 0, 0);
			topBar.Children.Add(robotFuelCapacityLbl, 0, 1);
			topBar.Children.Add(teleOpGearLbl, 2, 0);
			topBar.Children.Add(teleOpPressureLbl, 1, 0);

			pageLayout.Children.Add(actionCountLbl, 0, 0);
			pageLayout.Children.Add(actionCounter, 0, 1);

			pageLayout.Children.Add(fuelLbl, 1, 3, 0, 1);
			pageLayout.Children.Add(hopperCapacity, 1, 2, 1, 6);
			pageLayout.Children.Add(highGoalAcc, 2, 3, 1, 6);
			pageLayout.Children.Add(lowGoalDumpBtn, 2, 3, 6, 7);
			pageLayout.Children.Add(fuelScoreBtn, 1, 3, 7, 8);

			pageLayout.Children.Add(gearScored, 3, 4, 0, 1);
			pageLayout.Children.Add(droppedLbl, 3, 4, 1, 2);
			pageLayout.Children.Add(gearsStationDropped, 3, 4, 2, 4);
			pageLayout.Children.Add(gearsTransitDropped, 3, 4, 4, 6);
			pageLayout.Children.Add(finishBtn, 0, 4, 9, 10);

			Content = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 0,

				Children = {
					topBar,
					new ScrollView(){
						Content = pageLayout
					}
				}
			};
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			foreach (var btn in hopperCapacity.btnArray)
				btn.WidthRequest = lowGoalDumpBtn.Width;

			foreach (var btn in highGoalAcc.btnArray)
				btn.WidthRequest = lowGoalDumpBtn.Width;
		}

		void addAction() {
			matchActions[actionCount].hopperCapacity = hopperCapacity.getAvgPercentage();
			matchActions[actionCount].highGoalAccuracy = hopperCapacity.getAvgPercentage();
			matchActions[actionCount].lowGoalDump = lowDumpOn;
			matchActions[actionCount].gearsStationDrop = gearsStationDropped.value();
			matchActions[actionCount].gearsTransitDrop = gearsTransitDropped.value();
			matchActions[actionCount].gearSet = gearScored.on;
			matchActions[actionCount].cyclePressure = cPressure;

			if (gearScored.on) {
				gearScored.on = false;
				gearScored.BackgroundColor = Color.Red;
			}

				
			actionCount++;
			actionCounter.Text = actionCount.ToString();
		}

		void calcScore() {
			teleOpGears = matchData.autoGearScored ? 1 : 0;
			teleOpPressure = matchData.autoPressure;

			for (int i = 0; i < actionCount; i++) {
				teleOpGears += matchActions[i].gearSet ? 1 : 0;
				teleOpPressure = matchActions[i].cyclePressure;
			}
			if (lowDumpOn)
				cPressure += (int)Math.Floor(hopperCapacity.getAvgPercentage() * robotMaxCapacity);
			else
				cPressure += (int)Math.Floor((hopperCapacity.getAvgPercentage() * robotMaxCapacity * highGoalAcc.getAvgPercentage()) / 3);

			teleOpPressureLbl.Text = "Pressure: " + teleOpPressure + cPressure;
			teleOpGearLbl.Text = "Gears: " + teleOpGears;
		}

		async Task getMaxCapacity() {
			var db = new FirebaseClient(GlobalVariables.firebaseURL);

			var teamData = await db
					.Child(GlobalVariables.regionalPointer)
					.Child("teamData")
					.Child(matchData.teamNumber.ToString())
					.OnceSingleAsync<TeamData>();

			robotMaxCapacity = teamData.maxFuelCapacity;
		}

		async Task saveData() {
			calcScore();

			/*
			matchData.autoCross = inputs[0].on;
			matchData.teleopGearsDeposit = gearsScored.value();
			matchData.teleOpLowScore = inputs[2].on ? 1 : 0;
			//matchData.autoLowHit = lowGoalHits.value();
			//matchData.autoHighHit = highGoalHits.value();
			matchData.teleOptotalScore = teleOpScore;
			*/

			var db = new FirebaseClient(GlobalVariables.firebaseURL);

			var fbTeam = db
					.Child(GlobalVariables.regionalPointer)
					.Child("teamData")
					.Child(matchData.teamNumber.ToString())
					.Child("Matches")
					.Child(matchData.matchNumber)
					.PutAsync(matchData);
		}
	}

	public class ActionData{
		public int matchGears { get; set; }
		public int matchFuel { get; set; }
		public double hopperCapacity { get; set; }
		public double highGoalAccuracy { get; set; }
		public bool lowGoalDump { get; set; }
		public int cyclePressure { get; set; }
		public bool gearSet { get; set; }
		public int gearsStationDrop { get; set; }
		public int gearsTransitDrop { get; set; }
	}
}
