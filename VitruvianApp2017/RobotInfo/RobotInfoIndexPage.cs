using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using TheBlueAlliance;

namespace VitruvianApp2017
{
	public class RobotInfoIndexPage : ContentPage
	{
		ListView teamListView;
		List<TeamData> teamList;
		ScrollView teamIndex;
		StackLayout teamStack = new StackLayout()
		{
			Spacing = 1,
			BackgroundColor = Color.Silver
		};

		ActivityIndicator busyIcon = new ActivityIndicator();

		public RobotInfoIndexPage() {
			Title = "Robot Info";

			teamListView = new ListView() {
				ItemsSource = teamList,
				ItemTemplate = new DataTemplate(() => {

					var teamNumber = new Label();
					teamNumber.SetBinding(Label.TextProperty, new Binding( "teamNumber", BindingMode.Default, null, null, "Team {0}"));
					teamNumber.TextColor = Color.Black;
					teamNumber.VerticalOptions = LayoutOptions.CenterAndExpand;

					return new ViewCell() {
						View = new StackLayout() {
							BackgroundColor = Color.White,

							Children = {
								teamNumber
							}
						}
					};
				})
			};
			teamListView.ItemSelected += (sender, e) => {
				((ListView)sender).SelectedItem = null;
			};
			teamListView.ItemTapped += (sender, e) => {
				Navigation.PushPopupAsync(new TeamCardPopupPage((TeamData)sender));
			};

			teamIndex = new ScrollView()
			{
				Content = teamListView
			};

			var navigationBtns = new NavigationButtons(true);
			navigationBtns.refreshBtn.Clicked += (object sender, EventArgs e) =>
			{
				UpdateTeamList();
			};

			this.Appearing += (object sender, EventArgs e) =>
			{
				UpdateTeamList();
			};

			this.Content = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					busyIcon,
					teamIndex,
					navigationBtns
				}
			};

			BackgroundColor = Color.White;
		}

		public async Task UpdateTeamList()
		{
			if (CheckInternetConnectivity.InternetStatus()) {
				busyIcon.IsVisible = true;
				busyIcon.IsRunning = true;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				//var tbaTeams = Events.GetEventTeamsListHttp("2017calb");
				var fbTeams = await db
						.Child(GlobalVariables.regionalPointer)
						.Child("teamData")
						.OnceAsync<TeamData>();
				//var sorted = fbTeams.OrderByDescending((arg) => arg.Key("team_number"));

				teamStack.Children.Clear();

				foreach (var team in fbTeams) {
					teamList.Add(team.Object);
					TeamListCell cell = new TeamListCell();
					cell.teamName.Text = "Team " + team.Object.teamNumber.ToString();
					teamStack.Children.Add(cell);
					TapGestureRecognizer tap = new TapGestureRecognizer();

					if (team != null) {
						tap.Tapped += (object sender, EventArgs e) => {
							Navigation.PushPopupAsync(new TeamCardPopupPage(team.Object));
						};
					} else {
						tap.Tapped += (object sender, EventArgs e) => {
							DisplayAlert("Error:", "No Team Data found!", "OK");
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
