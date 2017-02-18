using System;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class AutoMatchScoutingPage:ContentPage
	{
		Grid pageLayout = new Grid() {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.Teal,

			RowDefinitions = {
				
			},
			ColumnDefinitions = {

			}
		};
		ColorButton[] inputs = new ColorButton[3];
		SingleCounter highGoalHits, lowGoalHits;
		Label autoScoreLbl;
		int autoScore = 0;

		TeamMatchData matchData;

		public AutoMatchScoutingPage(TeamMatchData data) {
			matchData = data;

			Title = "Autonomous Mode";

			Label teamNumberLbl = new Label() {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Text = "Team: " + matchData.teamNumber.ToString(),
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			autoScoreLbl = new Label() {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Text = "Est. Score: " + autoScore,
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			Label crossingLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Crossing",
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


			var topBar = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Spacing = 0,
			};
			topBar.Children.Add(teamNumberLbl);
			topBar.Children.Add(autoScoreLbl);

			pageLayout.Children.Add(crossingLbl, 0, 0);
			pageLayout.Children.Add(inputs[0], 0, 1);
			pageLayout.Children.Add(highGoalHits, 1, 2, 0, 2);
			pageLayout.Children.Add(lowGoalHits, 1, 2, 2, 4);
			pageLayout.Children.Add(gearsLbl, 2, 0);
			pageLayout.Children.Add(inputs[1], 2, 1);
			pageLayout.Children.Add(inputs[2], 2, 2);
			pageLayout.Children.Add(teleOpBtn, 0, 3, 5, 6);

			BackgroundColor = Color.Teal;

			Content = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				//Spacing = 0,

				Children = {
					topBar,
					new ScrollView(){
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,

						Content = pageLayout
					}
				}
			};
		}

		void calcScore() {
			autoScore = 0;
			if (inputs[1].on)
				autoScore += 60;

			autoScore += highGoalHits.value();
			autoScore += lowGoalHits.value();

			autoScoreLbl.Text = "Est. Score: " + autoScore;
		}

		async Task saveData() {
			calcScore();

			matchData.autoCross = inputs[0].on;
			matchData.gearDeposit = inputs[1].on;
			matchData.droppedGears = inputs[2].on ? 1 : 0;
			matchData.autoLowHit = lowGoalHits.value();
			matchData.autoHighHit = highGoalHits.value();
			matchData.autoScore = autoScore;

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
