using System;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace VitruvianApp2017
{
	public class PreMatchScoutingPage : ContentPage
	{
		enum startPos { Loading_Side, Center, Boiler_Side };
		enum alliances { Red, Blue };
		ScouterNameAutoComplete scouts = new ScouterNameAutoComplete();
		LineEntry matchNoEntry = new LineEntry("Match Number:"), teamNoEntry;
		Picker alliancePicker = new Picker();
		Picker teamNoPicker = new Picker();
		StackLayout teamNoPickerLayout;
		ContentView teamNoView;
		Picker positionPicker = new Picker();
		int teamNumber, matchNumber;
		enum matchPhase { P, QM }; //, QF, SF, F };
		int checkValue;
		CheckBox[] matchPhaseCheckboxes = new CheckBox[2];
		bool semaphore = false;

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
			
			var checkBoxLayout = new StackLayout() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal
			};
			for (int i = 0; i < 2; i++)
				matchPhaseCheckboxes[i] = new CheckBox() {
					DefaultText = Enum.GetName(typeof(matchPhase), i),
					FontSize = GlobalVariables.sizeSmall
				};

			foreach (var box in matchPhaseCheckboxes) {
				box.CheckedChanged += (sender, e) => {
					if (!semaphore)
						checkBoxChanged(box);
				};
				checkBoxLayout.Children.Add(box);
			}
			setDefaultMatchType();

			matchNoEntry = new LineEntry("Match Number:");
			matchNoEntry.inputEntry.Keyboard = Keyboard.Numeric;
			matchNoEntry.inputEntry.TextChanged += (sender, e) => {
				matchNumber = Convert.ToInt32(matchNoEntry.inputEntry.Text);
				if(!matchPhaseCheckboxes[0].Checked)
					getTeamNoPickerOptions();
			};

			Label allianceLabel = new Label {
				Text = "Alliance:",
				FontSize =GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			alliancePicker = new Picker();
			alliancePicker.Title = "Choose an Option";
			foreach (var item in Enum.GetValues(typeof(alliances)))
				alliancePicker.Items.Add(item.ToString());
			alliancePicker.SelectedIndexChanged += (sender, e) => {
				if (!matchPhaseCheckboxes[0].Checked)
					getTeamNoPickerOptions();
			};

			Label teamNoLbl = new Label {
				Text = "Team Number:",
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			teamNoPicker = new Picker();
			teamNoPicker.Title = "[Select Match No. and Alliance first]";

			teamNoPicker.SelectedIndexChanged += (sender, e) => {
				if (teamNoPicker.IsEnabled)
					teamNumber = Convert.ToInt32(teamNoPicker.Items[teamNoPicker.SelectedIndex]);
			};
			teamNoPickerLayout = new StackLayout() {
				Children = {
					teamNoLbl,
					teamNoPicker
				}
			};
			teamNoView = new ContentView() {
				Content = teamNoPickerLayout
			};

			teamNoEntry = new LineEntry("Team Number:");
			teamNoEntry.inputEntry.Placeholder = "[Enter Team Number]";
			teamNoEntry.inputEntry.Keyboard = Keyboard.Numeric;
			teamNoEntry.inputEntry.TextChanged += (sender, e) => {
				if (!string.IsNullOrEmpty(teamNoEntry.inputEntry.Text))
					teamNumber = Convert.ToInt32(teamNoEntry.data);
				else
					teamNumber = 0;
			};

			Label positionLabel = new Label {
				Text = "Starting Position:",
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
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
				if (string.IsNullOrEmpty(scouts.lineEntry.Text))
					inputFlag = true;
				if (string.IsNullOrEmpty(matchNoEntry.inputEntry.Text))
					inputFlag = true;
				if(teamNoPicker.SelectedIndex == -1 && string.IsNullOrEmpty(teamNoEntry.inputEntry.Text))
					inputFlag = true;

				if (positionPicker.SelectedIndex == -1 || alliancePicker.SelectedIndex == -1 || inputFlag) {
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
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					busyIcon,
					scouts,
					checkBoxLayout,
					matchNoEntry,
					allianceLabel,
					alliancePicker,
					teamNoView,
					positionLabel,
					positionPicker
				}
			};

			/*
			pageLayout.Children.Add(busyIcon);
			pageLayout.Children.Add(scouts);
			pageLayout.Children.Add(matchNo);
			pageLayout.Children.Add(allianceLabel);
			pageLayout.Children.Add(alliancePicker);
			pageLayout.Children.Add(teamNoLbl);
			pageLayout.Children.Add(teamNoPicker);
			pageLayout.Children.Add(positionLabel);
			pageLayout.Children.Add(positionPicker);
			*/

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

		async Task setDefaultMatchType() {
			int index = 0;

			if (CheckInternetConnectivity.InternetStatus()) {
				var db = new FirebaseClient(GlobalVariables.firebaseURL);

				var phaseGet = await db
								.Child(GlobalVariables.regionalPointer)
								.Child("competitionPhase")
								.OnceSingleAsync<string>();

				for (int i = 0; i < 2; i++) {
					if (Enum.GetName(typeof(matchPhase), i) == phaseGet)
						index = i;
				}

				checkBoxChanged(matchPhaseCheckboxes[index]);
			}
		}

		void checkBoxChanged(CheckBox c) {
			semaphore = true;
			for (int i = 0; i < 2; i++) {
				if (matchPhaseCheckboxes[i] != c)
					matchPhaseCheckboxes[i].Checked = false;
				else {
					matchPhaseCheckboxes[i].Checked = true;
					checkValue = i;
				}
			}
			if (checkValue == 0)
				teamNoView.Content = teamNoEntry;
			else
				teamNoView.Content = teamNoPickerLayout;
			semaphore = false;
		}

		async Task getTeamNoPickerOptions() {
			if (CheckInternetConnectivity.InternetStatus()) {
				busyIcon.IsVisible = true;
				busyIcon.IsRunning = true;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);

				var matchGet = await db
								.Child(GlobalVariables.regionalPointer)
								.Child("matchList")
								.Child(((matchNumber < 10) ? "0" + matchNumber.ToString() : matchNumber.ToString()))
								.OnceSingleAsync<EventMatchData>();
				Console.WriteLine("Match: " + matchNoEntry.inputEntry.Text + " , Alliance: " + alliancePicker.Items[alliancePicker.SelectedIndex]);

				if (matchGet == null)
					teamNoPicker.Title = "[Select Match No. and Alliance first]";
				else {
					teamNoPicker.Title = "[Select A Team]";
					teamNoPicker.Items.Clear();
					if (alliancePicker.SelectedIndex == 0)
						foreach (var item in matchGet.Red)
							teamNoPicker.Items.Add(item.ToString());
					else if (alliancePicker.SelectedIndex == 1)
						foreach (var item in matchGet.Blue)
							teamNoPicker.Items.Add(item.ToString());
				}

				busyIcon.IsVisible = false;
				busyIcon.IsRunning = false;
			}
		}

		async Task initializeTeamData() {
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;
			int matchType = 0;
			matchData.scouterName = scouts.lineEntry.Text;;
			matchData.matchNumber = Convert.ToInt32(matchNoEntry.inputEntry.Text);
			matchData.teamNumber = teamNumber;
			matchData.alliance = alliancePicker.Title;
			matchData.startPos = positionPicker.Title;

			if (CheckInternetConnectivity.InternetStatus()) {
				bool test = true;
				var db = new FirebaseClient(GlobalVariables.firebaseURL);

				if (checkValue == 1) {
					var dataCheck = await db
									.Child(GlobalVariables.regionalPointer)
									.Child("teamMatchData")
									.Child(matchData.teamNumber.ToString())
									.Child(matchData.matchNumber.ToString())
									.OnceSingleAsync<TeamMatchData>();

					if (dataCheck != null)
						if (!await DisplayAlert("Error", "Match Data already exists for this team. Do you want to overwrite it?", "OK", "Cancel"))
							test = false;

					if(test){
						var send = db
								.Child(GlobalVariables.regionalPointer)
								.Child("teamMatchData")
								.Child(matchData.teamNumber.ToString())
								.Child(matchData.matchNumber.ToString())
								.PutAsync(matchData);
						
						matchType = 0;

						await Navigation.PushAsync(new AutoMatchScoutingPage(matchData, matchType));
					}
				} else {
					var dataCheck = await db
									.Child(GlobalVariables.regionalPointer)
									.Child("PracticeMatches")
									.Child(matchData.teamNumber.ToString())
									.Child(matchData.matchNumber.ToString())
									.OnceSingleAsync<TeamMatchData>();

					if (dataCheck != null)
						if (!await DisplayAlert("Error", "Match Data already exists for this team. Do you want to overwrite it?", "OK", "Cancel"))
							test = false;
					
					if(test) {
						var send = db
								.Child(GlobalVariables.regionalPointer)
								.Child("PracticeMatches")
								.Child(matchData.teamNumber.ToString())
								.Child(matchData.matchNumber.ToString())
								.PutAsync(matchData);

						matchType = -1;

						await Navigation.PushAsync(new AutoMatchScoutingPage(matchData, matchType));
					}
				}
			} 
			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}
