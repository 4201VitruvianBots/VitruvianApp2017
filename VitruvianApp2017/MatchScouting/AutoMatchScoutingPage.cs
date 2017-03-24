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
		ColorButton[] inputs = new ColorButton[4];
		SingleCounter highGoalHits, lowGoalHits;
		Label autoGearLbl, autoPressureLbl;
		int autoGears = 0, autoPressure = 0;
		int mType;
		TeamMatchData matchData;

		public AutoMatchScoutingPage(TeamMatchData data, int matchType) {
			matchData = data;
			mType = matchType;

			Title = "Autonomous Mode";

			Label teamNumberLbl = new Label() {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Text = "Team: " + matchData.teamNumber.ToString(),
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			autoPressureLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Pressure: " + autoPressure,
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			autoGearLbl = new Label() {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Text = "Gears: " + autoGears,
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			Label crossingLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Crossing",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			Label fuelLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = "Fuel",
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
			inputs[1] = new ColorButton("Gear Scored");
			inputs[2] = new ColorButton("Gear Delivered");
			inputs[3] = new ColorButton("Gear Dropped");
			
			inputs[1].Clicked += (sender, e) => {
				inputs[2].on = false;
				inputs[2].BackgroundColor = Color.Red;
				inputs[3].on = false;
				inputs[3].BackgroundColor = Color.Red;
			};
			inputs[2].Clicked += (sender, e) => {
				inputs[1].on = false;
				inputs[1].BackgroundColor = Color.Red;
				inputs[3].on = false;
				inputs[3].BackgroundColor = Color.Red;
			};
			inputs[3].Clicked += (sender, e) => {
				inputs[1].on = false;
				inputs[1].BackgroundColor = Color.Red;
				inputs[2].on = false;
				inputs[2].BackgroundColor = Color.Red;
			};

			foreach (var input in inputs) 
				input.Clicked += (sender, e) => { calcScore(); };

			highGoalHits = new SingleCounter("High Goal Hits");
			highGoalHits.PropertyChanged += (sender, e) => {
				calcScore();
			};

			lowGoalHits = new SingleCounter("Low Goal Hits");
			lowGoalHits.PropertyChanged += (sender, e) => {
				calcScore();
			};

			var teleOpBtn = new Button() {
				Text = "TELEOP",
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				FontSize = GlobalVariables.sizeTitle,
				BackgroundColor = Color.Yellow
			};

			teleOpBtn.Clicked += (sender, e) => {
				saveData();
				Navigation.PushAsync(new TeleOpMatchScoutingPage(matchData, mType));
			};


			var topBar = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Spacing = 0,
			};
			topBar.Children.Add(teamNumberLbl);
			topBar.Children.Add(autoPressureLbl);
			topBar.Children.Add(autoGearLbl);

			pageLayout.Children.Add(crossingLbl, 0, 0);
			pageLayout.Children.Add(inputs[0], 0, 1);
			pageLayout.Children.Add(fuelLbl, 1, 0);
			pageLayout.Children.Add(highGoalHits, 1, 2, 1, 3);
			pageLayout.Children.Add(lowGoalHits, 1, 2, 3, 5);
			pageLayout.Children.Add(gearsLbl, 2, 0);
			pageLayout.Children.Add(inputs[1], 2, 1);
			pageLayout.Children.Add(inputs[2], 2, 2);
			pageLayout.Children.Add(inputs[3], 2, 3);
			pageLayout.Children.Add(teleOpBtn, 1, 2, 6, 7);

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
						IsClippedToBounds = true,

						Content = pageLayout
					}
				}
			};
		}

		protected override void OnAppearing() {
			base.OnAppearing();
			inputs[1].WidthRequest = inputs[2].Width;
			inputs[3].WidthRequest = inputs[2].Width;
		}

		void calcScore() {
			autoGears = 0;
			autoPressure = 0;
			if (inputs[1].on)
				autoGears = 1;

			autoPressure += highGoalHits.getValue();
			autoPressure += lowGoalHits.getValue();

			autoGearLbl.Text = "Gears: " + autoGears;
			autoPressureLbl.Text = "Pressure: " + autoPressure;
		}

		async Task saveData() {
			if (CheckInternetConnectivity.InternetStatus()) {
			calcScore();

			matchData.autoCross = inputs[0].on;
			matchData.autoGearScored = inputs[1].on;
			matchData.autoGearDelivered = inputs[2].on;
			matchData.autoGearDropped = inputs[3].on;
			matchData.autoLowHits = lowGoalHits.getValue();
			matchData.autoHighHits = highGoalHits.getValue();
			matchData.autoPressure = lowGoalHits.getValue() + highGoalHits.getValue();

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
	}
}
