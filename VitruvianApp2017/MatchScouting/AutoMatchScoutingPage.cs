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
		TitledColorButton[] inputs = new TitledColorButton[2];
		MultiCounter pressureCounter;
		Label autoGearLbl, autoPressureLbl;
		int autoGears = 0, autoPressure = 0;
		int mType;
		MatchData matchData;

		public AutoMatchScoutingPage(MatchData data, int matchType) {
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

			inputs[0] = new TitledColorButton("Crossing", "CROSSED");
			inputs[1] = new TitledColorButton("Gears", "Gear Scored");
			pressureCounter = new MultiCounter("Pressure");

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
				Navigation.PushModalAsync(new TeleOpMatchScoutingPage(matchData, mType));
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

			pageLayout.Children.Add(inputs[0], 0, 0);
			pageLayout.Children.Add(pressureCounter, 1, 0);
			pageLayout.Children.Add(inputs[1], 2, 0);
			pageLayout.Children.Add(teleOpBtn, 1, 2, 2, 3);

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
			//inputs[1].WidthRequest = inputs[2].Width;
		}

		async Task saveData() {
			if (CheckInternetConnectivity.InternetStatus()) {

				matchData.autoCross = inputs[0].getBtnStatus();
				matchData.autoGearScored = inputs[1].getBtnStatus();
				matchData.autoPressure = pressureCounter.getValue();

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
	}
}
