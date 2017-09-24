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
		string[] startPos = { "Loading Side", "Center", "Boiler Side" },
				 alliances = { "Red", "Blue" },
				 matchPhase = { "P", "QM", "QF", "SF", "F" };
		ScouterNameAutoComplete scouts = new ScouterNameAutoComplete();
		LineEntry matchNoLineEntry = new LineEntry("Match Number:"),
				  teamNoLineEntry;
		Picker alliancePicker = new Picker();
		Picker teamNoPicker = new Picker();
		Picker setNoPicker = new Picker();
		StackLayout teamNoPickerLayout,
					matchNoLayout;
		ContentView teamNoView, matchNoView;
		Picker positionPicker = new Picker();
		int teamNumber;
		string matchNumber;
		int setNumber;
		string competitionPhase;
		Label setNoLbl;
		CheckBox[] matchPhaseCheckboxes = new CheckBox[5];
		bool semaphore = false;

		MatchData matchData = new MatchData();

		ActivityIndicator busyIcon = new ActivityIndicator() {
			IsVisible = false,
			IsRunning = false
		};

		public PreMatchScoutingPage() : this(null) {

		}

		public PreMatchScoutingPage(string scouterName) {
			//Page Title
			Title = "Match Scouting";

			if (!string.IsNullOrEmpty(scouts.scouterName)) {
				scouts.lineEntry.Text = scouterName;
				matchNoLineEntry.inputEntry.Text = "";
				matchNumber = "";
			}

			var checkBoxLayout = new StackLayout() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal
			};
			for (int i = 0; i < matchPhaseCheckboxes.Length; i++)
				matchPhaseCheckboxes[i] = new CheckBox() {
					DefaultText = matchPhase[i],
					FontSize = GlobalVariables.sizeSmall
				};

			foreach (var box in matchPhaseCheckboxes) {
				box.CheckedChanged += (sender, e) => {
					if (!semaphore)
						checkBoxChanged(box);
				};
				checkBoxLayout.Children.Add(box);
			}
			getDefaultMatchType();

			setNoLbl = new Label {
				Text = "Set Number:",
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold,
			};

			setNoPicker.Title = "[Select Set No.]";
			setNoPicker.SelectedIndexChanged += (sender, e) => {
				try {
					setNumber = Convert.ToInt32(setNoPicker.Items[setNoPicker.SelectedIndex]);
				}
				catch (Exception ex) {
					Console.WriteLine("Error: " + ex.Message);
				}
				setMatchNumber();
			};

			matchNoLineEntry = new LineEntry("Match Number:");
			matchNoLineEntry.inputEntry.Keyboard = Keyboard.Numeric;
			matchNoLineEntry.inputEntry.TextChanged += (sender, e) => {
				setMatchNumber();
			};

			matchNoLayout = new StackLayout() {
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,

				Children = {
					new StackLayout(){

						Children = {
							setNoLbl,
							setNoPicker
						}
					},
					matchNoLineEntry
				}
			};

			matchNoView = new ContentView() {
				Content = matchNoLineEntry
			};

			Label allianceLabel = new Label {
				Text = "Alliance:",
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			alliancePicker = new Picker();
			alliancePicker.Title = "[Choose an Option]";
			foreach (var i in alliances)
				alliancePicker.Items.Add(i);
			alliancePicker.SelectedIndexChanged += (sender, e) => {
				setMatchNumber();
			};

			var teamNoLbl = new Label {
				Text = "Team Number:",
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			teamNoPicker = new Picker();
			teamNoPicker.Title = "[Select Match No. and Alliance first]";

			teamNoPicker.SelectedIndexChanged += (sender, e) => {
				try {
					teamNumber = Convert.ToInt32(teamNoPicker.Items[teamNoPicker.SelectedIndex]);
				}
				catch (Exception ex) {
					Console.WriteLine("Error: " + ex.Message);
				}
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

			teamNoLineEntry = new LineEntry("Team Number:");
			teamNoLineEntry.inputEntry.Placeholder = "[Enter Team Number]";
			teamNoLineEntry.inputEntry.Keyboard = Keyboard.Numeric;
			teamNoLineEntry.inputEntry.TextChanged += (sender, e) => {
				try {
					teamNumber = Convert.ToInt32(teamNoLineEntry.data);
				}
				catch {
					teamNumber = 0;
				}
			};

			Label positionLabel = new Label {
				Text = "Starting Position:",
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			positionPicker = new Picker();
			positionPicker.Title = "Choose an Option";
			foreach (var i in startPos)
				positionPicker.Items.Add(i);

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
				if (string.IsNullOrEmpty(matchNoLineEntry.inputEntry.Text))
					inputFlag = true;
				if ((teamNoPicker.SelectedIndex == -1 && (competitionPhase == "F" || competitionPhase == "QM")) ||
				   (teamNoPicker.SelectedIndex == -1 && setNoPicker.SelectedIndex == -1 && (competitionPhase == "QF" || competitionPhase == "SF")) ||
				   (string.IsNullOrEmpty(teamNoLineEntry.inputEntry.Text) && competitionPhase == "P"))
					inputFlag = true;

				if (positionPicker.SelectedIndex == -1 || alliancePicker.SelectedIndex == -1 || inputFlag) {
					DisplayAlert("Error", "Fill out all inputs", "OK");
				}
				else {
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
					matchNoView,
					allianceLabel,
					alliancePicker,
					teamNoView,
					positionLabel,
					positionPicker
				}
			};

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

		async Task getDefaultMatchType() {
			if (CheckInternetConnectivity.InternetStatus()) {
				var db = new FirebaseClient(GlobalVariables.firebaseURL);

				var phaseGet = await db
								.Child(GlobalVariables.regionalPointer)
								.Child("competitionPhase")
								.OnceSingleAsync<string>();

				int i = 0;
				foreach (var phase in matchPhase) {
					if (phase == phaseGet)
						break;
					i++;
				}
				checkBoxChanged(matchPhaseCheckboxes[i]);
			}
		}

		void checkBoxChanged(CheckBox c) {
			semaphore = true;
			for (int i = 0; i < matchPhaseCheckboxes.Length; i++) {
				if (matchPhaseCheckboxes[i] != c)
					matchPhaseCheckboxes[i].Checked = false;
				else {
					matchPhaseCheckboxes[i].Checked = true;
					competitionPhase = matchPhase[i];
				}
			}
			setMatchTeamNumberLayout();

			if (competitionPhase != "QF" || competitionPhase != "SF") {
				teamNoPicker.Items.Clear();
				teamNoPicker.SelectedIndex = -1;
				teamNoPicker.Title = "[Select A Team]";
			}
			else
				setMatchNumber();

			semaphore = false;
		}

		void setMatchTeamNumberLayout() {
			if (competitionPhase == "P") {
				teamNoView.Content = teamNoLineEntry;
				matchNoLayout.Children.Remove(matchNoLineEntry);
			}
			else {
				teamNoView.Content = teamNoPickerLayout;

				if (competitionPhase == "QM" || competitionPhase == "F") {
					matchNoView.Content = matchNoLineEntry;
					matchNoLayout.Children.Remove(matchNoLineEntry);

				}
				else {
					matchNoView.Content = matchNoLayout;
					matchNoLayout.Children.Add(matchNoLineEntry);
					setNoPicker.Items.Clear();

					int items = 0;
					if (competitionPhase == "QF")
						items = 4;
					else
						items = 2;

					for (int i = 1; i <= items; i++)
						setNoPicker.Items.Add(i.ToString());
				}
			}
		}

		async Task setMatchNumber() {
			if (competitionPhase == "QF" || competitionPhase == "SF")
				matchNumber = competitionPhase + (setNoPicker.SelectedIndex + 1) + "M" + matchNoLineEntry.inputEntry.Text;
			else if (competitionPhase == "F")
				matchNumber = competitionPhase + matchNoLineEntry.inputEntry.Text;
			else
				matchNumber = competitionPhase + (Convert.ToInt64(matchNoLineEntry.inputEntry.Text) < 10 ? "0" + matchNoLineEntry.inputEntry.Text : matchNoLineEntry.inputEntry.Text);

			if (!matchPhaseCheckboxes[0].Checked)
				await getTeamNoPickerOptions();
		}

		async Task getTeamNoPickerOptions() {
			if (CheckInternetConnectivity.InternetStatus()) {
				try {
					busyIcon.IsVisible = true;
					busyIcon.IsRunning = true;

					var db = new FirebaseClient(GlobalVariables.firebaseURL);
					EventMatchData matchGet = new EventMatchData();

					matchGet = await db
									.Child(GlobalVariables.regionalPointer)
									.Child("matchList")
									.Child(matchNumber.ToString())
									.OnceSingleAsync<EventMatchData>();

					Console.WriteLine("Match: " + matchNoLineEntry.inputEntry.Text + " , Alliance: " + alliancePicker.Items[alliancePicker.SelectedIndex]);

					teamNoPicker.Items.Clear();
					if (string.IsNullOrEmpty(matchGet.matchNumber))
						teamNoPicker.Title = "[Select Match No. and Alliance first]";
					else {
						teamNoPicker.Title = "[Select A Team]";

						if (alliancePicker.SelectedIndex == 0)
							foreach (var item in matchGet.Red)
								teamNoPicker.Items.Add(item.ToString());
						else if (alliancePicker.SelectedIndex == 1)
							foreach (var item in matchGet.Blue)
								teamNoPicker.Items.Add(item.ToString());
					}

					busyIcon.IsVisible = false;
					busyIcon.IsRunning = false;
				}  catch (Exception e) {
					Console.WriteLine("Error: " + e.Message);
				}
			}
		}

		async Task initializeTeamData() {
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;
			int matchType = 0;

			matchData.scouterName = scouts.lineEntry.Text;;
			matchData.matchNumber = matchNumber;
			matchData.teamNumber = teamNumber;
			matchData.matchID = teamNumber + "-" + matchNumber;
			matchData.alliance = alliancePicker.Items[alliancePicker.SelectedIndex];
			matchData.startPos = positionPicker.Items[positionPicker.SelectedIndex];

			if (CheckInternetConnectivity.InternetStatus()) {
				bool test = true;
				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				string path = "NULL";


				if (competitionPhase != "P") {
					path = "matchData/" + matchData.matchID;
					matchType = 0;
				}
				else {
					path = "practiceMatchData/" + matchData.matchID;
					matchType = -1;
				}
				
				if (await FirebaseAccess.checkExistingMatchData(db, path))
					if (!await DisplayAlert("Error", "Match Data already exists for this team-match. Do you want to overwrite it?", "OK", "Cancel"))
						test = false;

				if (test) {
					FirebaseAccess.saveData(db, path, matchData);

					await Navigation.PushAsync(new AutoMatchScoutingPage(matchData, matchType));
				}
					
				/*
				if (checkValue == 1) {
					
					var dataCheck = await db
									.Child(GlobalVariables.regionalPointer)
									.Child("teamMatchData")
									.Child(matchData.teamNumber.ToString())
									.Child(matchData.matchNumber)
									.OnceSingleAsync<TeamMatchData>();

					if (dataCheck != null)
						if (!await DisplayAlert("Error", "Match Data already exists for this team. Do you want to overwrite it?", "OK", "Cancel"))
							test = false;

					if(test){
						var send = db
								.Child(GlobalVariables.regionalPointer)
								.Child("teamMatchData")
								.Child(matchData.teamNumber.ToString())
								.Child(matchData.matchNumber)
								.PutAsync(matchData);
						
						matchType = 0;

						await Navigation.PushAsync(new AutoMatchScoutingPage(matchData, matchType));
					}
				} else {
					var dataCheck = await db
									.Child(GlobalVariables.regionalPointer)
									.Child("PracticeMatches")
									.Child(matchData.teamNumber.ToString())
									.Child(matchData.matchNumber)
									.OnceSingleAsync<TeamMatchData>();

					if (dataCheck != null)
						if (!await DisplayAlert("Error", "Match Data already exists for this team. Do you want to overwrite it?", "OK", "Cancel"))
							test = false;
					
					if(test) {
						var send = db
								.Child(GlobalVariables.regionalPointer)
								.Child("PracticeMatches")
								.Child(matchData.teamNumber.ToString())
								.Child(matchData.matchNumber)
								.PutAsync(matchData);

						matchType = -1;

						await Navigation.PushAsync(new AutoMatchScoutingPage(matchData, matchType));
					}

				}
				*/
			} 
			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}
