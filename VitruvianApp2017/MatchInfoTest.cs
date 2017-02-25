using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class MatchInfoTest:ContentPage
	{
		EventMatchData matchData = new EventMatchData() {
			Blue = new int[] { 207, 368, 508 },
			Red = new int[] { 4201, 330, 294 },
		};

		TeamData[] teams = new TeamData[6];
		TeamData[] red = new TeamData[3];
		TeamData[] blue = new TeamData[3];

		public MatchInfoTest() {


		}


		protected override void OnAppearing() {
			base.OnAppearing();

			FetchTeamData();
			Console.WriteLine("Stop");

			foreach (var team in teams)
				Console.WriteLine("Team: " + team.teamNumber);
		}

		async Task awaitTeamData() {
			//teams = await FetchTeamData();
			var task = Task.Factory.StartNew(() => FetchTeamData());
			task.Wait();

			/*
			for (int i = 0; i < 3; i++){
				// var task =  Task.Factory.StartNew(() => FetchTeamData(matchData.Red[i]));
				// teams[i] = await task.Result;
				teams[i] = await FetchTeamData(matchData.Red[i]);
			} 
			for (int i = 0; i < 3; i++) {
				//var task = Task.Factory.StartNew(() => FetchTeamData(matchData.Blue[i]));
				// teams[i + 3] = await task.Result;
				teams[i + 3] = await FetchTeamData(matchData.Blue[i]);
			}
			*/
			Console.WriteLine("Stop");
		}

		async Task FetchTeamData() {
			Console.WriteLine("Start");


			for (int i = 0; i < 1; i++) {
				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				var fbTeams = await db
							.Child("2017calb")
							.Child("teamData")
							.Child("4201")
							//.Child(matchData.Red[i].ToString())
							.OnceSingleAsync<TeamData>();
				
				teams[i] = new TeamData();
				teams[i] = fbTeams;

				Console.WriteLine("Team: " + teams[i].teamNumber);
			}
		}
	}
}
