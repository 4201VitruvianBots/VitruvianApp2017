using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using FFImageLoading.Forms;
using TheBlueAlliance;
using TheBlueAlliance.Models;
using XLabs.Forms.Controls;

namespace VitruvianApp2017
{
	public class TeamCardPopupPage:PopupPage
	{
		Grid topGrid = new Grid()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			Margin = new Thickness(0, 0, 0, 0),
			Padding = 0,

			RowDefinitions = {
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = new GridLength(130, GridUnitType.Absolute) },
			},
			ColumnDefinitions = {
				//new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
				//new ColumnDefinition {Width = GridLength.Auto},
				//new ColumnDefinition {Width = GridLength.Auto}
			}
		};

		Grid dataGrid = new Grid() {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			Margin = new Thickness(0, 0, 0, 0),
			Padding = 0,

			ColumnDefinitions = {
				new ColumnDefinition {Width = GridLength.Star},
				new ColumnDefinition {Width = GridLength.Star}
			},

			RowDefinitions = {
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = GridLength.Star },
			}
		};

		Grid matchDataGrid = new Grid() {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			RowSpacing = 1,
			ColumnSpacing = 1,
			Margin = new Thickness(1),
			BackgroundColor = Color.Black
		};
		StackLayout pitDataLayout, matchDataLayout;
		CheckBox[] pitCheckBoxes = new CheckBox[10];
		int rowHeadersIndex = 0;
		Label[] rowHeaderLabels = new Label[19];
		ColumnHeaderCell[] cHeaderCells = new ColumnHeaderCell[9];
		DataCell[,] gridData = new DataCell[12, 3];

		TeamData data;
		string[] pitdataTitles = { "Volume Configuration:", "Max Fuel Capacity:", "Ground Intake:"};
		string[] pitdata = new string[5];

		public TeamCardPopupPage(TeamData team)
		{
			data = team;

			var teamNo = new Label()
			{
				Text = data.teamNumber.ToString(),
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			var teamNa = new Label()
			{
				Text = data.teamName,
				FontSize = GlobalVariables.sizeMedium
			};

			var teamOPR = new Label() {
				Text = data.tbaOPR.ToString(),
				FontSize = 14
			};

			setPitData();
			setMatchData();

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

			var robotImage = new RobotImageLayout(data);
			topGrid.Children.Add(robotImage, 0, 1, 0, 2);
			topGrid.Children.Add(teamNo, 1, 0);
			topGrid.Children.Add(teamNa, 1, 1);

			//teamGrid.Children.Add(teamOPR, 0, 2, gridYIndex, gridYIndex++ + 1);
			//teamGrid.Children.Add(teamOPR, 0, 2, gridYIndex, gridYIndex++ + 1);

			//topGrid.Children.Add(navigationBtns, 0, 2, gridYIndex, gridYIndex++ + 1);

			Content = new Frame()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(50, 50),
				Padding = new Thickness(5),

				BackgroundColor = Color.Gray,
				HasShadow = true,

				Content = new StackLayout() {

					Children = {
						topGrid,
						dataGrid,
						navigationBtns
					}
				}
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

		async Task getTBARatingData() {
			/*
			var opr = await TheBlueAlliance.Teams.GetTeamInformation(
			var fbTeam = await db
					.Child(GlobalVariables.regionalPointer)
					.Child("teamData")
					.Child(data.teamNumber.ToString())
					.OnceSingleAsync<TeamData>();

			data = fbTeam;
			*/
		}

		void setPitData() {
			pitDataLayout = new StackLayout() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Gray,
			};
			var pitDataLbl = new Label() {
				Text = "Pit Scouting:",
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
			};

			var mechanismsLbl = new Label() {
				Text = "Mechanisms:",
				FontSize = GlobalVariables.sizeMedium,
				TextColor = Color.White,
			};

			var configurationLbl = new Label() {
				Text = "Configuration: " + data.volumeConfig,
				FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.White
			};

			var fuelCapacityLabel = new Label() {
				Text = "Max Fuel Capacity: " + data.maxFuelCapacity,
				FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.White
			};

			var autoActionsLbl = new Label() {
				Text = "Auto Actions:",
				FontSize = GlobalVariables.sizeMedium,
				TextColor = Color.White,
			};

			var notesLbl = new Label() {
				Text = "Additional Notes:",
				FontSize = GlobalVariables.sizeMedium,
				TextColor = Color.White,
			};

			var notesText = new Label() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Text = data.notes,
				FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.White,
			};

			pitDataLayout.Children.Add(pitDataLbl);
			pitDataLayout.Children.Add(mechanismsLbl);
			pitDataLayout.Children.Add(configurationLbl);
			pitDataLayout.Children.Add(fuelCapacityLabel);
			generateDataCheckBox(pitCheckBoxes[0], "Gear Delivery", data.gearMechanism);
			generateDataCheckBox(pitCheckBoxes[1], "Fuel Deilvery - Low", data.fuelLowMechanism);
			generateDataCheckBox(pitCheckBoxes[2], "Fuel Deilvery - High", data.fuelHighMechanism);
			generateDataCheckBox(pitCheckBoxes[3], "Climbing", data.climbingMechanism);
			generateDataCheckBox(pitCheckBoxes[4], "Ground Intake - Gears", data.gearGroundIntakeMechanism);
			generateDataCheckBox(pitCheckBoxes[5], "Ground Intake - Fuel", data.fuelGroundIntakeMechanism);
			pitDataLayout.Children.Add(autoActionsLbl);
			generateDataCheckBox(pitCheckBoxes[6], "Cross", data.pitAutoCross);
			generateDataCheckBox(pitCheckBoxes[7], "Gear Delivery", data.pitAutoGear);
			generateDataCheckBox(pitCheckBoxes[8], "Fuel Delivery - Low", data.pitAutoFuelLow);
			generateDataCheckBox(pitCheckBoxes[9], "Fuel Delivery - High", data.pitAutoFuelHigh);
			pitDataLayout.Children.Add(notesLbl);
			pitDataLayout.Children.Add(notesText);

			dataGrid.Children.Add(new ScrollView(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Gray,
				IsClippedToBounds = true,

				Content = pitDataLayout }, 0, 1, 0, 2);
		}

		void generateDataCheckBox(CheckBox box, string title, bool matchBool) {
			box = new CheckBox() {
				DefaultText = title,
				FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.White,
			};
			box.Checked = matchBool;
			box.CheckedChanged += (sender, e) => {
				box.Checked = matchBool;
			};
			pitDataLayout.Children.Add(box);
		}

		void setMatchData() {
			matchDataLayout = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			var pitDataLbl = new Label() {
				Text = "Match Scouting:",
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
			};

			initializeMatchData();

			dataGrid.Children.Add(pitDataLbl, 1, 0);
			dataGrid.Children.Add(matchDataGrid, 1, 1);

			/*
			dataGrid.Children.Add(new ScrollView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = matchDataLayout }, 1, 0);
				*/
		}

		void initializeMatchData() {
			initializeRowHeaders();
			for (int j = 0; j < 12; j++)
				for (int k = 0; k < 3; k++)
					gridData[j, k] = new DataCell();

			int y = 0;
			// Add data headers
			cHeaderCells[0] = new ColumnHeaderCell();
			cHeaderCells[0].header.Text = "Successes";
			matchDataGrid.Children.Add(cHeaderCells[0], 1, 3);
			cHeaderCells[1] = new ColumnHeaderCell();
			cHeaderCells[1].header.Text = "Attempts";
			matchDataGrid.Children.Add(cHeaderCells[1], 2, 3);
			cHeaderCells[2] = new ColumnHeaderCell();
			cHeaderCells[2].header.Text = "Drops";
			matchDataGrid.Children.Add(cHeaderCells[2], 3, 3);

			cHeaderCells[3] = new ColumnHeaderCell();
			cHeaderCells[3].header.Text = "Avg.";
			matchDataGrid.Children.Add(cHeaderCells[3], 1, 6);

			cHeaderCells[4] = new ColumnHeaderCell();
			cHeaderCells[4].header.Text = "High";
			matchDataGrid.Children.Add(cHeaderCells[4], 2, 6);

			cHeaderCells[5] = new ColumnHeaderCell();
			cHeaderCells[5].header.Text = "Avg.";
			matchDataGrid.Children.Add(cHeaderCells[5], 1, 9);
			cHeaderCells[6] = new ColumnHeaderCell();
			cHeaderCells[6].header.Text = "High";
			matchDataGrid.Children.Add(cHeaderCells[6], 2, 9);

			cHeaderCells[7] = new ColumnHeaderCell();
			cHeaderCells[7].header.Text = "Successes";
			matchDataGrid.Children.Add(cHeaderCells[7], 1, 15);
			cHeaderCells[8] = new ColumnHeaderCell();
			cHeaderCells[8].header.Text = "Attempts";
			matchDataGrid.Children.Add(cHeaderCells[8], 2, 15);

			// Add Data
			gridData[0, 0].data.Text = data.matchCount.ToString();
			matchDataGrid.Children.Add(gridData[0, 0], 1, y++);
			gridData[1, 0].data.Text = data.maxFuelCapacity.ToString();
			matchDataGrid.Children.Add(gridData[1, 0], 1, y++);
			y++;
			y++;
			gridData[2, 0].data.Text = data.totalAutoCrossSuccesses.ToString();
			matchDataGrid.Children.Add(gridData[2, 0], 1, y++);
			gridData[3, 0].data.Text = data.avgAutoGearScored.ToString();
			matchDataGrid.Children.Add(gridData[3, 0], 1, y);
			gridData[3, 1].data.Text = data.avgAutoGearsDelivered.ToString();
			matchDataGrid.Children.Add(gridData[3, 1], 2, y);
			gridData[3, 2].data.Text = data.avgAutoGearsDropped.ToString();
			matchDataGrid.Children.Add(gridData[3, 2], 3, y++);
			y++;
			gridData[4, 0].data.Text = data.avgAutoPressure.ToString();
			matchDataGrid.Children.Add(gridData[4, 0], 1, y);
			gridData[4, 1].data.Text = data.autoPressureHigh.ToString();
			matchDataGrid.Children.Add(gridData[4, 1], 1 + 1, y++);
			y++;
			y++;
			gridData[5, 0].data.Text = data.avgTeleOpActions.ToString();
			matchDataGrid.Children.Add(gridData[5, 0], 1, y);
			gridData[5, 1].data.Text = data.teleOpActionsHigh.ToString();
			matchDataGrid.Children.Add(gridData[5, 1], 2, y++);
			gridData[6, 0].data.Text = data.avgTeleOpGearsScored.ToString();
			matchDataGrid.Children.Add(gridData[6, 0], 1, y);
			gridData[6, 1].data.Text = data.teleOpGearsScoredHigh.ToString();
			matchDataGrid.Children.Add(gridData[6, 1], 2, y++);
			gridData[7, 0].data.Text = data.avgTeleOpGearsStationDropped.ToString();
			matchDataGrid.Children.Add(gridData[7, 0], 1, y);
			gridData[7, 1].data.Text = data.teleOpGearsStationDroppedHigh.ToString();
			matchDataGrid.Children.Add(gridData[7, 1], 2, y++);
			gridData[8, 0].data.Text = data.avgTeleOpGearsTransitDropped.ToString();
			matchDataGrid.Children.Add(gridData[8, 0], 1, y);
			gridData[8, 1].data.Text = data.teleOpGearsTransitDroppedHigh.ToString();
			matchDataGrid.Children.Add(gridData[8, 1], 2, y++);
			gridData[9, 0].data.Text = data.avgTeleOpPressure.ToString();
			matchDataGrid.Children.Add(gridData[9, 0], 1, y);
			gridData[9, 1].data.Text = data.teleOpPressureHigh.ToString();
			matchDataGrid.Children.Add(gridData[9, 1], 2, y++);
			y++;
			gridData[10, 0].data.Text = data.successfulClimbCount.ToString();
			matchDataGrid.Children.Add(gridData[10, 0], 1, y);
			gridData[10, 1].data.Text = data.attemptedClimbCount.ToString();
			matchDataGrid.Children.Add(gridData[10, 1], 2, y++);
			gridData[11, 0].data.Text = data.foulCount.ToString();
			matchDataGrid.Children.Add(gridData[11, 0], 1, y++);
		}

		void initializeRowHeaders() {
			addRowHeaders("Matches");
			addRowHeaders("Capcity");
			addRowHeaders("");
			addRowHeaders("Auto");
			addRowHeaders("Crosses");
			addRowHeaders("Gears");
			addRowHeaders("");
			addRowHeaders("Pressure");
			addRowHeaders(""); // spacer
			addRowHeaders("TeleOp");
			addRowHeaders("Actions");
			addRowHeaders("Gears - S");
			addRowHeaders("Gears - D, S");
			addRowHeaders("Gears - D, T");
			addRowHeaders("Pressure");
			addRowHeaders(""); // spacer
			addRowHeaders("Climbs");
			addRowHeaders("Fouls");
		}

		void addRowHeaders(string description) {
			rowHeaderLabels[rowHeadersIndex] = new Label() {
				FontSize = GlobalVariables.sizeTiny,
				TextColor = Color.White,
				Text = description
			};
			if (rowHeadersIndex == 3 || rowHeadersIndex == 9)
				rowHeaderLabels[rowHeadersIndex].FontAttributes = FontAttributes.Bold;

			matchDataGrid.Children.Add(rowHeaderLabels[rowHeadersIndex], 0, rowHeadersIndex);
			matchDataGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) });
			rowHeadersIndex++;
		}
	}
}
