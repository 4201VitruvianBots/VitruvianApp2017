using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using TheBlueAlliance;
using TheBlueAlliance.Models;

namespace VitruvianApp2017
{
	public class AdminPage:ContentPage
	{
		/*
		Grid grid = new Grid() {


		}
		*/

		public AdminPage() {
			Title = "Admin Page";
			var titleLbl = new Label() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Admin Page",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("1B5E20"),
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			var updateTeamListBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Update Team List",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			updateTeamListBtn.Clicked += (sender, e) => {
				UpdateTeamList();
			};

			var logoutBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Logout",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			logoutBtn.Clicked += (sender, e) => {
				logout();
			};
			Button[] btnArray = { logoutBtn };
			var navigationBtns = new NavigationButtons(false, btnArray);

			Content = new StackLayout() {
				Children = {
					titleLbl,
					updateTeamListBtn,
					navigationBtns
				}
			};
		}

		void logout() {
			AppSettings.SaveSettings("AdminLogin", "false");

			Navigation.PopModalAsync();
		}

		public async Task UpdateTeamList() {
			if (CheckInternetConnectivity.InternetStatus()) {
				var teamList = await EventsHttp.GetEventTeamsListHttp(GlobalVariables.regionalPointer);

				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				//var tbaTeams = Events.GetEventTeamsListHttp("2017calb");
				var fbTeams = await db
						.Child(GlobalVariables.regionalPointer)
						.Child("teamData")
						.OnceAsync<TeamData>();
				//var sorted = fbTeams.OrderByDescending((arg) => arg.Key("team_number"));

				foreach (var team in teamList) {
					foreach (var fbteam in fbTeams) {
						if (team.team_number == (int)fbteam.Object.teamNumber)
							break;
						else {
							var send = db
								.Child(GlobalVariables.regionalPointer)
								.Child("teamData")
								.Child(team.team_number.ToString())
								.PutAsync(new TeamData() {
									teamName = team.nickname,
									teamNumber = Convert.ToDouble(team.team_number)
								});
						}
					}
				}
			}
		}
	}
}
