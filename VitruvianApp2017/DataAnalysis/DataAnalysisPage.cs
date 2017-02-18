using System;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class DataAnalysisPage:ContentPage
	{
		ActivityIndicator busyIcon;
		//enum LogicOperators =  { <, ==, > };

		Grid dataGrid = new Grid() {
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand

		};

		public DataAnalysisPage() {
			Title = "Data Analysis";



			var navigationBtns = new NavigationButtons(true); //, {} );

			Content = new StackLayout(){
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			
				Children = {
					dataGrid,
					navigationBtns
				}	
			};
		}

		public async Task UpdateTeamList() {
			if (CheckInternetConnectivity.InternetStatus()) {
				busyIcon.IsVisible = true;
				busyIcon.IsRunning = true;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);;
				var fbTeams = await db
						.Child(GlobalVariables.regionalPointer)
						.Child("teamData")
						.OnceAsync<TeamData>();
				//var sorted = fbTeams.OrderByDescending((arg) => arg.Key("team_number"));

				foreach (var team in fbTeams) {
					
				}

				busyIcon.IsVisible = false;
				busyIcon.IsRunning = false;
			}
		}
	}
}
