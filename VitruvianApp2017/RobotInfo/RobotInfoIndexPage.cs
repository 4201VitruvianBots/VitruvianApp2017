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
		ListView teamListView = new ListView();
		List<TeamData> teamList = new List<TeamData>();

		Grid searchBar;
		Entry searchEntry;
		Label searchCancel;

		ActivityIndicator busyIcon = new ActivityIndicator();

		public RobotInfoIndexPage() {
			Title = "Robot Info";

			searchBar = new Grid() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,

				ColumnDefinitions = {
					new ColumnDefinition() { Width = GridLength.Auto },
					new ColumnDefinition() { Width = GridLength.Star },
					new ColumnDefinition() { Width = GridLength.Auto },
					new ColumnDefinition() { Width = 5 },
				},
				RowDefinitions = {
					new RowDefinition() { Height = GridLength.Auto }
				}
			};
			searchEntry = new Entry() {
				Placeholder = "Search team",
				MinimumWidthRequest = Width,
				FontSize = GlobalVariables.sizeMedium
			};
			searchEntry.TextChanged += (sender, e) => {
				autoCompleteOptions();
				searchCancel.IsVisible = true;
				searchCancel.IsEnabled = true;

				if (string.IsNullOrEmpty(searchEntry.Text)){
					searchEntry.Text = "";
					searchEntry.Placeholder = "Search team";
					searchCancel.IsVisible = false;
					searchCancel.IsEnabled = false;
				}
			};
			searchCancel = new Label() {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Text = "x",
				FontSize = GlobalVariables.sizeTitle,
				TextColor = searchEntry.PlaceholderColor,
				IsVisible = false,
				IsEnabled = false,
			};
			var tap = new TapGestureRecognizer();
			tap.Tapped += (sender, e) => {
				searchEntry.Text = "";
			};
			var searchCell = new ContentView() {
				Content = searchCancel
			};
			searchCell.GestureRecognizers.Add(tap);
			searchBar.Children.Add(searchEntry, 1, 0);
			searchBar.Children.Add(searchCell, 2, 0);

			teamListView.MinimumHeightRequest = Height;
			teamListView.ItemTemplate = new DataTemplate(() => {
				var teamNumber = new Label() { 
					TextColor = Color.Black,
					FontSize = GlobalVariables.sizeMedium,
					VerticalOptions = LayoutOptions.CenterAndExpand
				};
				teamNumber.SetBinding(Label.TextProperty, "teamNumber");
				 
				var cell = new ViewCell() {
					View = new StackLayout() {
						BackgroundColor = Color.White,

						Children = {
							teamNumber
						}
					}
				};

				return cell;
			});
			teamListView.ItemSelected += (sender, e) => {
				((ListView)sender).SelectedItem = null;
			};
			teamListView.ItemTapped += (sender, e) => {
				Navigation.PushPopupAsync(new TeamCardPopupPage((TeamData)e.Item));
			};

			var navigationBtns = new NavigationButtons(true);
			navigationBtns.refreshBtn.Clicked += (object sender, EventArgs e) =>
			{
				updateMatchLists();
			};

			this.Content = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 1,
				Children = {
					busyIcon,
					searchBar,
					teamListView,
					navigationBtns
				}
			};

			BackgroundColor = Color.White;
		}

		protected override void OnAppearing() {
			base.OnAppearing();
			updateMatchLists();
		}

		public async Task updateMatchLists() {
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;
			await Task.Run(() => getTeamList());
			//getTeamList();

			Console.WriteLine("Continue");
			teamListView.ItemsSource = teamList;

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}

		async Task getTeamList()
		{
			Console.WriteLine("Begin");
			if (CheckInternetConnectivity.InternetStatus()) {
				try {
					var list = new List<TeamData>();

					var db = new FirebaseClient(GlobalVariables.firebaseURL);
					//var tbaTeams = Events.GetEventTeamsListHttp("2017calb");
					var fbTeams = await db
									.Child(GlobalVariables.regionalPointer)
									.Child("teamData")
									.OnceAsync<TeamData>();

					foreach (var team in fbTeams) {
						Console.WriteLine("TeamNo: " + team.Object.teamNumber);
						list.Add(team.Object);
					}
					teamList = list;
				} catch (Exception ex) {
					Console.WriteLine("Error: " + ex.Message);
				}	
			}
			Console.WriteLine("End");
		}

		void autoCompleteOptions() {
			var filtered = new List<TeamData>();

			foreach (var team in teamList)
				if (team.teamNumber.ToString().StartsWith(searchEntry.Text.ToLower()))
					filtered.Add(team);

			teamListView.ItemsSource = filtered;
		}
	}
}
