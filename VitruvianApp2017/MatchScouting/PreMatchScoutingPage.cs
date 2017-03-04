using System;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class PreMatchScoutingPage : ContentPage
	{
		enum startPos { Far, Center, Near };
		enum alliances { Blue, Red };
		ScouterNameAutoComplete scouts = new ScouterNameAutoComplete();
		LineEntry matchNo = new LineEntry("Match Number:");
		Picker alliancePicker = new Picker();
		Picker teamNoPicker = new Picker();
		Picker positionPicker = new Picker();
		int teamNumber;

		TeamMatchData matchData = new TeamMatchData();

		ActivityIndicator busyIcon = new ActivityIndicator() {
			IsVisible = false,
			IsRunning = false
		};

		public PreMatchScoutingPage() : this(null) {

		}

		public PreMatchScoutingPage(string scouterName) {
			//Page Title
			Title = "Match Scouting";

			if (!string.IsNullOrEmpty(scouts.scouterName))
				scouts.lineEntry.Text = scouterName;

			matchNo = new LineEntry("Match Number:");
			matchNo.inputEntry.Keyboard = Keyboard.Numeric;
			matchNo.inputEntry.TextChanged += (sender, e) => {
				getTeamNoPickerOptions();
			};

			Label allianceLabel = new Label {
				Text = "Alliance:",
				FontAttributes = FontAttributes.Bold
			};

			alliancePicker = new Picker();
			alliancePicker.Title = "Choose an Option";
			foreach (var item in Enum.GetValues(typeof(alliances)))
				alliancePicker.Items.Add(item.ToString());
			alliancePicker.SelectedIndexChanged += (sender, e) => {
				alliancePicker.Title = alliancePicker.Items[alliancePicker.SelectedIndex];
				getTeamNoPickerOptions();
			};

			Label teamNoLbl = new Label {
				Text = "Team Number:",
				FontAttributes = FontAttributes.Bold
			};

			teamNoPicker = new Picker();
			teamNoPicker.Title = "[Select Match No. and Alliance first]";

			teamNoPicker.SelectedIndexChanged += (sender, e) => {
				teamNoPicker.Title = teamNoPicker.Items[teamNoPicker.SelectedIndex];
				if(teamNoPicker.IsEnabled)
					teamNumber = Convert.ToInt32(teamNoPicker.Title);
			};

			Label positionLabel = new Label {
				Text = "Starting Position:",
				FontAttributes = FontAttributes.Bold
			};

			// Temp for practice matches
			var teamNoEntry = new Entry() {

			};
			teamNoEntry.TextChanged += (sender, e) => {
				if (teamNoEntry.IsEnabled)
					teamNumber = Convert.ToInt32(teamNoEntry.Text);
			};

			positionPicker = new Picker();
			positionPicker.Title = "Choose an Option";
			foreach (var item in Enum.GetValues(typeof(startPos)))
				positionPicker.Items.Add(item.ToString());

			positionPicker.SelectedIndexChanged += (sender, e) => {
				positionPicker.Title = positionPicker.Items[positionPicker.SelectedIndex];
			};

			var beginMatchBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Begin Match",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			beginMatchBtn.Clicked += (sender, e) => {
				bool inputFlag = false;
				if (string.IsNullOrEmpty(matchNo.inputEntry.Text))
					inputFlag = true;
				if (string.IsNullOrEmpty(scouts.lineEntry.Text))
					inputFlag = true;

				if (positionPicker.Title == "Choose an Option" || alliancePicker.Title == "Choose an Option" || teamNoPicker.Title == "Choose an Option" || string.IsNullOrEmpty(teamNoPicker.Title) || inputFlag) {
					DisplayAlert("Error", "Fill out all inputs", "OK");
				} else {
					initializeTeamData();
				}
			};

			var navigationBtns = new NavigationButtons(false, new Button[] { beginMatchBtn });
			navigationBtns.backBtn.Clicked += (sender, e) => {
				Navigation.PopToRootAsync();
			};

			var pageLayout = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			pageLayout.Children.Add(busyIcon);
			pageLayout.Children.Add(scouts);
			pageLayout.Children.Add(matchNo);
			pageLayout.Children.Add(allianceLabel);
			pageLayout.Children.Add(alliancePicker);
			pageLayout.Children.Add(teamNoLbl);
			pageLayout.Children.Add(teamNoPicker);
        	pageLayout.Children.Add(positionLabel);
          	pageLayout.Children.Add(positionPicker);

			var dataScroll = new ScrollView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = pageLayout
			};

			var grid = new Grid() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				RowDefinitions = {
					new RowDefinition { Height = GridLength.Star },
					new RowDefinition { Height = GridLength.Auto }
				}
			};
			grid.Children.Add(dataScroll, 0, 0);
			grid.Children.Add(navigationBtns, 0, 1);

			Content = grid;
		}

		async Task getTeamNoPickerOptions() {
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;

			var db = new FirebaseClient(GlobalVariables.firebaseURL);

			var matchGet = await db
							.Child(GlobalVariables.regionalPointer)
							.Child("matchList")
							.Child("QM" + (matchNo.inputEntry.Text.Length < 2 ? "0" + matchNo.inputEntry.Text : matchNo.inputEntry.Text))
							.OnceSingleAsync<EventMatchData>();
			Console.WriteLine("Match: " + matchNo.inputEntry.Text + " , Alliance: " + alliancePicker.Title);
			Console.WriteLine("MatchGet: " + matchGet.matchNumber + " " + matchGet.Blue[0]);

			if (matchGet == null)
				teamNoPicker.Title = "[Select Match No. and Alliance first]";
			else {
				teamNoPicker.Items.Clear();
				if(alliancePicker.Title == "Blue")
					foreach (var item in matchGet.Blue)
						teamNoPicker.Items.Add(item.ToString());
				else if(alliancePicker.Title == "Red")
					foreach (var item in matchGet.Red)
						teamNoPicker.Items.Add(item.ToString());
			}

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}

		async Task initializeTeamData() {
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;
			
			matchData.scouterName = scouts.lineEntry.Text;;
			matchData.matchNumber = "QM" + matchNo.inputEntry.Text;
			matchData.teamNumber = teamNumber;
			matchData.alliance = alliancePicker.Title;
			matchData.startPos = positionPicker.Title;

			if (CheckInternetConnectivity.InternetStatus()) {
				var db = new FirebaseClient(GlobalVariables.firebaseURL);

				var dataCheck = await db
								.Child(GlobalVariables.regionalPointer)
								.Child("teamData")
								.Child(matchData.teamNumber.ToString())
								.Child("Matches")
								.Child(matchData.matchNumber)
								.OnceSingleAsync<TeamMatchData>();

				if (await DisplayAlert("Error", "Match Data already exists for this team. Do you want to overwrite it?", "OK", "Cancel")) {

					var db2 = new FirebaseClient(GlobalVariables.firebaseURL);

					var send = db
							.Child(GlobalVariables.regionalPointer)
							.Child("teamData")
							.Child(matchData.teamNumber.ToString())
							.Child("Matches")
							.Child(matchData.matchNumber)
							.PutAsync(matchData);

					await Navigation.PushAsync(new AutoMatchScoutingPage(matchData));
				}
			} 
			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}
