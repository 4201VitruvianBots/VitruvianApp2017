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
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Auto},
				new RowDefinition() { Height = GridLength.Star},
				new RowDefinition() { Height = GridLength.Auto},
			},
			ColumnDefinitions = {

			}
		};
		ColorButton[] inputs = new ColorButton[3];
		Button cycleIncrement, cycleUndo;
		SingleCounter cycleCounter, gearsScored, gearsDropped;
		ColorButtonStackArray hopperCapacity, highGoalAcc;
		Label teleOpScoreLbl;
		int teleOpScore = 0;
		int robotMaxCapacity;
		int cycleCount = 0;

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
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			teleOpScoreLbl = new Label() {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Text = "Est. Score: " + matchData.autoScore + teleOpScore,
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			Label robotFuelCapacityLbl = new Label() {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Text = "Max Fuel Capacity: " + robotMaxCapacity,
				TextColor = Color.White,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold,
			};

			Label cycleCountLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Cycle Count:",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			Label cycleCount = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "0",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			cycleCounter = new SingleCounter("Cycle Counter");
			cycleIncrement = new Button() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Add Cycle",
				TextColor = Color.Black,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};
			cycleIncrement.Clicked += (sender, e) => {

			};
			cycleUndo = new Button() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Undo Cycle Add",
				TextColor = Color.Black,
				BackgroundColor = Color.Yellow,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			hopperCapacity = new ColorButtonStackArray("Hopper Capacity", 5);
			hopperCapacity.PropertyChanged += (sender, e) => {
				calcScore();
			};
			highGoalAcc = new ColorButtonStackArray("High Goal Accuracy", 5);
			highGoalAcc.PropertyChanged += (sender, e) => {
				calcScore();
			};
			var lowGoalDumpBtn = new Button() {
				Text = "Low Goal Dump",
				BackgroundColor = Color.Yellow,
				FontAttributes = FontAttributes.Bold
			};

			gearsScored = new SingleCounter("Gears Scored");
			gearsDropped = new SingleCounter("Gears Dropped");

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
			topBar.Children.Add(teleOpScoreLbl, 1, 0);

			pageLayout.Children.Add(cycleCountLbl, 0, 0);
			pageLayout.Children.Add(cycleCount, 0, 1);
			pageLayout.Children.Add(cycleIncrement, 0, 2);
			pageLayout.Children.Add(cycleUndo, 0, 3);

			pageLayout.Children.Add(hopperCapacity, 1, 2, 0, 5);
			pageLayout.Children.Add(highGoalAcc, 2, 3, 0, 5);
			pageLayout.Children.Add(lowGoalDumpBtn, 2, 3, 5, 6);
			pageLayout.Children.Add(gearsScored, 3, 4, 0, 3);
			pageLayout.Children.Add(gearsDropped, 3, 4, 3, 5);
			pageLayout.Children.Add(finishBtn, 0, 4, 7, 8);

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

		void calcScore() {
			teleOpScore = 0 + matchData.autoScore;

			teleOpScore += (int)Math.Floor((hopperCapacity.getAvgPercentage() * robotMaxCapacity * highGoalAcc.getAvgPercentage()) / 3);

			teleOpScoreLbl.Text = "Est. Score: " + teleOpScore + matchData.autoScore;
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

			matchData.autoCross = inputs[0].on;
			matchData.teleopGearsDeposit = gearsScored.value();
			matchData.teleOpLowScore = inputs[2].on ? 1 : 0;
			//matchData.autoLowHit = lowGoalHits.value();
			//matchData.autoHighHit = highGoalHits.value();
			matchData.teleOptotalScore = teleOpScore;

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

	public class CycleData{
		public int cycleScore { get; set; }
		public int cycleCount { get; set; }
		public double hopperCapacity { get; set; }
		public double highGoalAccuracy { get; set; }
		public int lowGoalDump { get; set; }
		public int gearsSet { get; set; }
		public int gearsDropped { get; set; }
	}
}
