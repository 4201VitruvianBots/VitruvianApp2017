using System;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class TeleOpMatchScoutingPage: ContentPage
	{
		Grid pageLayout = new Grid() {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.White,

			RowDefinitions = {

			},
			ColumnDefinitions = {

			}
		};
		ColorButton[] inputs = new ColorButton[3];
		SingleCounter highGoalHits, lowGoalHits;
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
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Robot Max Fuel Capacity: " + robotMaxCapacity,
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			Label cycleCountLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Current Cycle: " + cycleCount,
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			Label gearsLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Gears",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			inputs[0] = new ColorButton("Crossed");
			inputs[1] = new ColorButton("Gear Delivered");
			inputs[2] = new ColorButton("Gear Dropped");

			foreach (var input in inputs)
				input.Clicked += (sender, e) => { calcScore(); };

			highGoalHits = new SingleCounter("High Goal Hits");
			highGoalHits.PropertyChanged += (sender, e) => {
				calcScore();
			};
			lowGoalHits = new SingleCounter("Low Goal Hits");
			lowGoalHits.upperLimit = 3;
			lowGoalHits.PropertyChanged += (sender, e) => {
				calcScore();
			};

			var teleOpBtn = new Button() {
				Text = "TELEOP",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				BackgroundColor = Color.Yellow
			};

			teleOpBtn.Clicked += (sender, e) => {
				saveData();
				Navigation.PushAsync(new TeleOpMatchScoutingPage(matchData));
			};


			var topBar = new Grid() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Green
			};
			topBar.Children.Add(teamNumberLbl, 0, 0);
			topBar.Children.Add(teleOpScoreLbl, 1, 0);

			pageLayout.Children.Add(topBar, 0, 3, 0, 1);
			//pageLayout.Children.Add(crossingLbl, 0, 1);
			pageLayout.Children.Add(inputs[0], 0, 2);
			pageLayout.Children.Add(highGoalHits, 1, 2, 1, 3);
			pageLayout.Children.Add(lowGoalHits, 1, 2, 3, 5);
			pageLayout.Children.Add(gearsLbl, 2, 1);
			pageLayout.Children.Add(inputs[1], 2, 2);
			pageLayout.Children.Add(inputs[2], 2, 3);
			pageLayout.Children.Add(teleOpBtn, 0, 3, 6, 7);

			Content = new ScrollView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = pageLayout
			};
		}

		void calcScore() {
			teleOpScore = 0 + matchData.autoScore;
			if (inputs[1].on)
				teleOpScore += 60;

			teleOpScore += highGoalHits.value();
			teleOpScore += lowGoalHits.value();

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
			matchData.gearDeposit = inputs[1].on;
			matchData.droppedGears = inputs[2].on ? 1 : 0;
			matchData.autoLowHit = lowGoalHits.value();
			matchData.autoHighHit = highGoalHits.value();
			matchData.teleOpScore = teleOpScore;

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
}
