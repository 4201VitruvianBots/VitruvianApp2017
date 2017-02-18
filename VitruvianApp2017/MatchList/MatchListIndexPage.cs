using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using TheBlueAlliance;

namespace VitruvianApp2017
{
	public class MatchListIndexPage : ContentPage
	{
		ScrollView matchIndex;
		StackLayout listStack;
		StackLayout matchStack = new StackLayout() {
			Spacing = 1,
			BackgroundColor = Color.Silver
		};
		StackLayout pastMatchStack = new StackLayout() {
			Spacing = 1,
			BackgroundColor = Color.Silver
		};

		ActivityIndicator busyIcon = new ActivityIndicator();

		public MatchListIndexPage() {
			Title = "Match List";

			matchIndex = new ScrollView() {
				Content = matchStack
			};

			var navigationBtns = new NavigationButtons(true);
			navigationBtns.refreshBtn.Clicked += (object sender, EventArgs e) => {
				UpdateMatchList();
			};

			this.Appearing += (object sender, EventArgs e) => {
				UpdateMatchList();
			};

			this.Content = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					busyIcon,
					matchIndex,
					navigationBtns
				}
			};

			BackgroundColor = Color.White;
		}

		public async Task UpdateMatchList() {
			if (CheckInternetConnectivity.InternetStatus()) {
				busyIcon.IsVisible = true;
				busyIcon.IsRunning = true;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				//var tbaTeams = Events.GetEventTeamsListHttp("2017calb");
				var fbMatches = await db
						.Child(GlobalVariables.regionalPointer)
						.Child("matchList")
						.OnceAsync<EventMatchData>();
				//var sorted = fbTeams.OrderByDescending((arg) => arg.Key("team_number"));

				matchStack.Children.Clear();

				foreach (var match in fbMatches) {
					
					MatchListCell cell = new MatchListCell();
					cell.matchNumber.Text = "Q" + match.Object.matchNumber.Remove(0, 2);
					matchStack.Children.Add(cell);
					TapGestureRecognizer tap = new TapGestureRecognizer();


					if (match != null) {
						tap.Tapped += (object sender, EventArgs e) => {
							Navigation.PushPopupAsync(new MatchInfoPopupPage(match.Object));
						};
					} else {
						tap.Tapped += (object sender, EventArgs e) => {
							DisplayAlert("Error:", "No Match Data found!", "OK");
						};
					}
					cell.GestureRecognizers.Add(tap);

				}

				busyIcon.IsVisible = false;
				busyIcon.IsRunning = false;
			}
		}
	}
}
