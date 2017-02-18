using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class MatchInfoPopupPage:PopupPage
	{
		Grid displayGrid = new Grid() {
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,

			ColumnDefinitions = {
				new ColumnDefinition {Width = GridLength.Auto},
				new ColumnDefinition {Width = GridLength.Auto},
				new ColumnDefinition {Width = GridLength.Auto},
				new ColumnDefinition {Width = GridLength.Auto},
				new ColumnDefinition {Width = GridLength.Auto},
				new ColumnDefinition {Width = GridLength.Auto},
				new ColumnDefinition {Width = GridLength.Auto}
			},

			RowDefinitions = {
				
			}
		};

		static EventMatchData matchData;
		static TeamData[] teams = new TeamData[6];
		TeamData[] blue = new TeamData[3];
		TeamData[] red = new TeamData[3];

		public MatchInfoPopupPage(EventMatchData data) {
			matchData = data;

			var getTeams = Task.Run(()=>FetchTeamData());
			getTeams.Wait();

			// Add Side Data

			// Add Top Data (?)

			for (int i = 0; i < 6; i++) {
				var layout = populateTeamData(teams[i]);
				displayGrid.Children.Add(layout, i + 1, i + 2, 1, 2);
			}

			var navigationBtns = new PopupNavigationButtons(true);

			var displayGridScroll = new ScrollView() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				//Orientation = ScrollOrientation.Horizontal,

				Content = displayGrid
			};

			Content = new Frame() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(50, 80),
				Padding = new Thickness(5),

				BackgroundColor = Color.Gray,
				HasShadow = true,

				Content = new StackLayout() {
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,

					Children = {
						displayGridScroll,
						navigationBtns
					}
				}
			};
		}

		StackLayout populateTeamData(TeamData tData) {
			Label teamNumber = new Label() {
				HorizontalOptions = LayoutOptions.Center,
				Text = tData.teamNumber.ToString(),
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			Label teamName = new Label() {
				HorizontalOptions = LayoutOptions.Center,
				Text = tData.teamName.ToString(),
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			return new StackLayout() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,

				Children = {
					teamNumber,
					teamName,
				}
			};
		}

		static async Task FetchTeamData() {
			var db = new FirebaseClient(GlobalVariables.firebaseURL);

			for (int i = 0; i < 3; i++) {
				var team = await db
							.Child(GlobalVariables.regionalPointer)
							.Child("teamData")
							.Child(matchData.Red[i].ToString())
							.OnceSingleAsync<TeamData>();
				teams[i] = team;
				Console.WriteLine("Team Fetch: " + team.teamNumber);
			}

			for (int i = 0; i < 3; i++) {
				var team = await db
							.Child(GlobalVariables.regionalPointer)
							.Child("teamData")
							.Child(matchData.Blue[i].ToString())
							.OnceSingleAsync<TeamData>();
				teams[i + 3] = team;
				Console.WriteLine("Team Fetch: " + team.teamNumber);
			}
		}
	}
}
