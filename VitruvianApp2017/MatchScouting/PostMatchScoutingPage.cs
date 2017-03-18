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

		public PostMatchScoutingPage(TeamMatchData data) {
			Title = "Post Match";
			matchData = data;

			var goodCheck = new CheckBox() {
				DefaultText = "Did this team perform well?"
			};

			var finishMatchBtn = new Button() {
				Text = "Save Match Data",
				FontSize = GlobalVariables.sizeMedium,
				BackgroundColor = Color.Green,
			};
			finishMatchBtn.Clicked += (sender, e) => {
				//
				saveData();

				//Console.WriteLine("Nav Count: " + Navigation.NavigationStack.Count);
				//Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
				//Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

				Navigation.PushAsync(new PreMatchScoutingPage(matchData.scouterName));
				//Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
				//Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
			};

			Content = new StackLayout() {
				VerticalOptions = LayoutOptions.EndAndExpand,

				Children = {
					goodCheck,

					finishMatchBtn
				}
			};
		}

		async Task saveData() {


			var db = new FirebaseClient(GlobalVariables.firebaseURL);

			var uploadData = db
							.Child(GlobalVariables.regionalPointer)
							.Child("teamData")
							.Child(matchData.teamNumber.ToString())
							.Child("Matches")
							.Child(matchData.matchNumber)
							.PutAsync(matchData);
		}

		public async Task updateAvgTeamData() {


			var db = new FirebaseClient(GlobalVariables.firebaseURL);

			var fbTeam = await db
						.Child(GlobalVariables.regionalPointer)
						.Child("teamData")
						.Child(matchData.teamNumber.ToString())
						.OnceSingleAsync<TeamData>();
			/*
			int count = 0, totalAutoGearDeposits = 0, totalAutoGearDropped = 0 totalAutoCross = 0, 
				totalAutoLowFuelScore = 0,
				totalAutoHighFuelScore = 0, totalCycles = 0, totalEstScore = 0, totalGearDeposits = 0,
				totalGearsDropped = 0,
				totalLowScore = 0, totalLowScoreMatch = 0,
				totalHighAcc = 0, totalHighAccMatch = 0,
				totalHighScore = 0, totalHighScoreMatch = 0,
				totalFouls = 0, totalGoodScore = 0, successfulClimbCount = 0;

			foreach (var match in fbTeam.Matches) {
				totalAutoGearDeposits += match.autoGearDeposit ? 1 : 0;
				totalAutoGearDropped += match.autoGearDropped ? 1 : 0;
				totalAutoCross += match.autoCross ? 1 : 0;
				totalAutoLowFuelScore += match.autoLowHits;
				totalAutoHighFuelScore += match.autoHighHits;
				totalCycles += match.cycleCount;
				totalGearDeposits += match.teleopGearsDeposit;
				totalGearsDropped += match.teleOpGearsDropped;

				foreach(var lowScore in match.teleOpLowScore)
					+

				count++;
			}
			//*/
			var uploadData = db
						.Child(GlobalVariables.regionalPointer)
						.Child("teamData")
						.Child(fbTeam.teamNumber.ToString())
						.PutAsync(fbTeam);
		}
	}
}
