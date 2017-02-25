using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using FFImageLoading.Forms;

namespace VitruvianApp2017
{
	public class TeamCardPopupPage:PopupPage
	{
		Grid teamGrid = new Grid()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			Margin = new Thickness(0, 0, 0, 0),
			Padding = 0,

			RowDefinitions ={
				new RowDefinition {Height = GridLength.Auto},
				new RowDefinition {Height = GridLength.Auto},
				new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
				new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
				new RowDefinition {Height = GridLength.Auto}
			},
			ColumnDefinitions = {
				new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
				new ColumnDefinition {Width = GridLength.Auto}
			}
		};

		TeamData data;
		string[] pitdataTitles = { "Volume Configuration:", "Max Fuel Capacity:", "Ground Intake:"};
		string[] pitdata = new string[3];

		public TeamCardPopupPage(TeamData team)
		{
			data = team;

			var teamNo = new Label()
			{
				Text = data.teamNumber.ToString(),
				FontSize = 22,
				FontAttributes = FontAttributes.Bold
			};

			var teamNa = new Label()
			{
				Text = data.teamName,
				FontSize = 18
			};

			var teamOPR = new Label() {
				Text = data.tbaOPR.ToString(),
				FontSize = 14
			};

			Button editTeamBtn = new Button(){
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Edit Info",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			editTeamBtn.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushPopupAsync(new TeamCardPopupEditPage(data));
			};

			Button[] btnArray = { editTeamBtn };
			var navigationBtns = new PopupNavigationButtons(true, btnArray);

			int gridYIndex = 0;
			//teamGrid.Children.Add(new RobotImageLayout(data), 0, 2, gridYIndex, gridYIndex + 2);
			teamGrid.Children.Add(teamNo, 1, gridYIndex++);
			teamGrid.Children.Add(teamNa, 1, gridYIndex++);
			teamGrid.Children.Add(teamOPR, 1, gridYIndex++);

			//teamGrid.Children.Add(teamOPR, 0, 2, gridYIndex, gridYIndex++ + 1);
			//teamGrid.Children.Add(teamOPR, 0, 2, gridYIndex, gridYIndex++ + 1);

			//
			teamGrid.Children.Add(navigationBtns, 0, 2, gridYIndex, gridYIndex++ + 1);

			Content = new Frame()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(50, 80),
				Padding = new Thickness(5),

				BackgroundColor = Color.Gray,
				HasShadow = true,

				Content = teamGrid
			};
		}

		async void popUpPage(CachedImage rImage)
		{
			//await Task.Yield();
			//await Navigation.PushPopupAsync(new ImagePopupPage(rImage));
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			getFirebaseData();
			setPitData();
			//setMatchData();
		}

		async Task getFirebaseData() {
			var db = new FirebaseClient(GlobalVariables.firebaseURL);
			var fbTeam = await db
					.Child(GlobalVariables.regionalPointer)
					.Child("teamData")
					.Child(data.teamNumber.ToString())
					.OnceSingleAsync<TeamData>();

			data = fbTeam;
		}

		void setPitData() {
			pitdata[0] = data.volumeConfig;
			pitdata[1] = data.maxFuelCapacity.ToString();
			pitdata[2] = data.groundIntake.ToString();

			//populate dynamic list
		}

		void setMatchData() {

		}
	}
}
