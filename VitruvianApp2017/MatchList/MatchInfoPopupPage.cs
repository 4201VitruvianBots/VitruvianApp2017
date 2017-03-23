using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class MatchInfoPopupPage:PopupPage
	{
		Grid layoutGrid = new Grid() {
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,
			RowSpacing = 0,
			ColumnSpacing = 0,


			ColumnDefinitions = {
				new ColumnDefinition() { Width = GridLength.Auto },
				/*
				new ColumnDefinition() { Width = GridLength.Auto },
				new ColumnDefinition() { Width = GridLength.Auto },
				new ColumnDefinition() { Width = GridLength.Auto },
				new ColumnDefinition() { Width = GridLength.Auto },
				new ColumnDefinition() { Width = GridLength.Auto },
				new ColumnDefinition() { Width = GridLength.Auto },
				*/
			},

			RowDefinitions = {
				new RowDefinition() { Height = GridLength.Auto },
				new RowDefinition() { Height = GridLength.Auto },
				new RowDefinition() { Height = GridLength.Auto },
				new RowDefinition() { Height = GridLength.Auto },
				new RowDefinition() { Height = GridLength.Auto },
				new RowDefinition() { Height = GridLength.Star },
			}
		};


		static int gridWidth = 20;
		Grid dataGrid = new Grid() {
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,
			RowSpacing = 1,
			ColumnSpacing = 1,
			Margin = new Thickness(1),
			BackgroundColor = Color.Black,

			ColumnDefinitions = {
				new ColumnDefinition() { Width = GridLength.Auto },
				/*
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				new ColumnDefinition() { Width = new GridLength(gridWidth, GridUnitType.Absolute) },
				*/
			},

			RowDefinitions = {
				
			}
		};

		EventMatchData matchData;
		TeamData[] teams = new TeamData[6];

		int rowHeadersIndex = 0;
		Label[] rowHeaderLabels = new Label[19];

		ColumnHeaderCell[,] cHeaderCells = new ColumnHeaderCell[6,9];
		DataCell[,,] gridData = new DataCell[6, 12, 3];
		CachedImage[] robotImages = new CachedImage[6];
		bool semaphore = false;

		public MatchInfoPopupPage(EventMatchData data) {
			matchData = data;

			awaitTeamData();

			Title = "Match: " + data.matchNumber;

			var titleLbl = new Label() {
				Text = "Match: " + data.matchNumber,
				FontSize = GlobalVariables.sizeMedium,
				HorizontalTextAlignment = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold
			};
			layoutGrid.Children.Add(titleLbl, 1, 7, 0, 1);

			var blankHeader = new Label() {
				TextColor = Color.Gray,
				Text = "Gears - D, T"
			};
			layoutGrid.Children.Add(blankHeader, 0, 1);

			// Add Top Data (?)
			initializeHeaders();
			initializeAllianceData();
			initializeTeamData();

			initializeData();
			layoutGrid.Children.Add(dataGrid, 0, 7, 5, 6);

			var navigationBtns = new PopupNavigationButtons(true);

			Content = new Frame()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(50, 50),
				Padding = new Thickness(2),

				BackgroundColor = Color.Gray,
				HasShadow = true,

				Content = new StackLayout() {

					Children = {
						layoutGrid,
						navigationBtns
					}
				}
			};
		}

		void initializeData() {
			for (int i = 0; i < 6; i++)
				for (int j = 0; j < 12; j++)
					for (int k = 0; k < 3; k++) {
						gridData[i,j,k] = new DataCell();
						if (i < 3)
							gridData[i, j, k].BackgroundColor = Color.Red;
						else
							gridData[i, j, k].BackgroundColor = Color.Blue;
					}

			for (int i = 0; i < 6; i++){
				int y = 0;
				int x = (i * 3) + 1;

				// Add data headers
				cHeaderCells[i, 0] = new ColumnHeaderCell();
				cHeaderCells[i, 0].header.Text = "Successes";
				dataGrid.Children.Add(cHeaderCells[i, 0], x , 3);
				cHeaderCells[i, 1] = new ColumnHeaderCell();
				cHeaderCells[i, 1].header.Text = "Attempts";
				dataGrid.Children.Add(cHeaderCells[i, 1], x + 1, 3);
				cHeaderCells[i, 2] = new ColumnHeaderCell();
				cHeaderCells[i, 2].header.Text = "Drops";
				dataGrid.Children.Add(cHeaderCells[i, 2], x + 2, 3);

				cHeaderCells[i, 3] = new ColumnHeaderCell();
				cHeaderCells[i, 3].header.Text = "Avg.";
				dataGrid.Children.Add(cHeaderCells[i, 3], x, 6);

				cHeaderCells[i, 4] = new ColumnHeaderCell();
				cHeaderCells[i, 4].header.Text = "High";
				dataGrid.Children.Add(cHeaderCells[i, 4], x + 1, 6);

				cHeaderCells[i, 5] = new ColumnHeaderCell();
				cHeaderCells[i, 5].header.Text = "Avg.";
				dataGrid.Children.Add(cHeaderCells[i, 5], x, 9);
				cHeaderCells[i, 6] = new ColumnHeaderCell();
				cHeaderCells[i, 6].header.Text = "High";
				dataGrid.Children.Add(cHeaderCells[i, 6], x + 1, 9);

				cHeaderCells[i, 7] = new ColumnHeaderCell();
				cHeaderCells[i, 7].header.Text = "Successes";
				dataGrid.Children.Add(cHeaderCells[i, 7], x, 15);
				cHeaderCells[i, 8] = new ColumnHeaderCell();
				cHeaderCells[i, 8].header.Text = "Attempts";
				dataGrid.Children.Add(cHeaderCells[i, 8], x + 1, 15);

				// Add Data
				gridData[i, 0, 0].data.Text = teams[i].matchCount.ToString();
				dataGrid.Children.Add(gridData[i, 0, 0], x, y++);
				gridData[i, 1, 0].data.Text = teams[i].maxFuelCapacity.ToString();
				dataGrid.Children.Add(gridData[i, 1, 0], x, y++);
				y++;
				y++;
				gridData[i, 2, 0].data.Text = teams[i].totalAutoCrossSuccesses.ToString();
				dataGrid.Children.Add(gridData[i, 2, 0], x, y++);
				gridData[i, 3, 0].data.Text = teams[i].avgAutoGearScored.ToString();
				dataGrid.Children.Add(gridData[i, 3, 0], x, y);
				gridData[i, 3, 1].data.Text = teams[i].avgAutoGearsDelivered.ToString();
				dataGrid.Children.Add(gridData[i, 3, 1], x + 1, y);
				gridData[i, 3, 2].data.Text = teams[i].avgAutoGearsDropped.ToString();
				dataGrid.Children.Add(gridData[i, 3, 2], x + 2, y++);
				y++;
				gridData[i, 4, 0].data.Text = teams[i].avgAutoPressure.ToString();
				dataGrid.Children.Add(gridData[i, 4, 0], x, y);
				gridData[i, 4, 1].data.Text = teams[i].autoPressureHigh.ToString();
				dataGrid.Children.Add(gridData[i, 4, 1], x + 1, y++);
				y++;
				y++;
				gridData[i, 5, 0].data.Text = teams[i].avgTeleOpActions.ToString();
				dataGrid.Children.Add(gridData[i, 5, 0], x, y);
				gridData[i, 5, 1].data.Text = teams[i].teleOpActionsHigh.ToString();
				dataGrid.Children.Add(gridData[i, 5, 1], x + 1, y++);
				gridData[i, 6, 0].data.Text = teams[i].avgTeleOpGearsScored.ToString();
				dataGrid.Children.Add(gridData[i, 6, 0], x, y);
				gridData[i, 6, 1].data.Text = teams[i].teleOpGearsScoredHigh.ToString();
				dataGrid.Children.Add(gridData[i, 6, 1], x + 1, y++);
				gridData[i, 7, 0].data.Text = teams[i].avgTeleOpGearsStationDropped.ToString();
				dataGrid.Children.Add(gridData[i, 7, 0], x, y);
				gridData[i, 7, 1].data.Text = teams[i].teleOpGearsStationDroppedHigh.ToString();
				dataGrid.Children.Add(gridData[i, 7, 1], x + 1, y++);
				gridData[i, 8, 0].data.Text = teams[i].avgTeleOpGearsTransitDropped.ToString();
				dataGrid.Children.Add(gridData[i, 8, 0], x, y);
				gridData[i, 8, 1].data.Text = teams[i].teleOpGearsTransitDroppedHigh.ToString();
				dataGrid.Children.Add(gridData[i, 8, 1], x + 1, y++);
				gridData[i, 9, 0].data.Text = teams[i].avgTeleOpPressure.ToString();
				dataGrid.Children.Add(gridData[i, 9, 0], x, y);
				gridData[i, 9, 1].data.Text = teams[i].teleOpPressureHigh.ToString();
				dataGrid.Children.Add(gridData[i, 9, 1], x + 1, y++);
				y++;
				gridData[i, 10, 0].data.Text = teams[i].successfulClimbCount.ToString();
				dataGrid.Children.Add(gridData[i, 10, 0], x, y);
				gridData[i, 10, 1].data.Text = teams[i].attemptedClimbCount.ToString();
				dataGrid.Children.Add(gridData[i, 10, 1], x + 1, y++);
				gridData[i, 11, 0].data.Text = teams[i].foulCount.ToString();
				dataGrid.Children.Add(gridData[i, 11, 0], x, y++);
			}
		}

		void initializeAllianceData() {
			// Calculate Alliance Potential & display it
			int rScore = 0, rAutoGears = 0, rTeleOpGears = 0, rAutoPressure = 0, rTeleOpPressure = 0, rTotalPressure = 0;
			for (int i = 0; i < 3; i++) {
				if (teams[i].avgAutoGearScored > 0.5)
					rAutoGears++;
				rAutoPressure += (int)Math.Round(teams[i].avgAutoPressure, 0);
				rTeleOpPressure += (int)Math.Round(teams[i].avgTeleOpPressure, 0);
				rTeleOpGears += (int)Math.Round(teams[i].avgTeleOpGearsScored, 0);
			}
			rTotalPressure += rAutoPressure + rTeleOpPressure;
			if (rAutoGears == 3) {
				rScore += 120;
				if (rTeleOpGears >= 10)
					rScore += 80;
				else if (rTeleOpGears >= 4)
					rScore += 40;
			} else if (rAutoGears > 0) {
				int tGear = 0;
				rScore += 60;
				if (rAutoGears == 2)
					tGear = 1;
				
				if (rTeleOpGears + tGear >= 12)
					rScore += 120;
				else if (rTeleOpGears + tGear >= 6)
					rScore += 80;
				else if (rTeleOpGears+ tGear  >= 2)
					rScore += 40;
			}
			rScore += rTotalPressure;

			var rAllianceScoreLbl = new Label() {
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Score: " + rScore,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			var rAllianceGearLbl = new Label() {
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Gears: " + (rAutoGears + rTeleOpGears),
				HorizontalTextAlignment = TextAlignment.Center,
			};

			var rAlliancePressureLbl = new Label() {
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Pressure: " + rTotalPressure,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			layoutGrid.Children.Add(rAllianceScoreLbl, 1, 4, 1, 2);
			layoutGrid.Children.Add(rAllianceGearLbl, 1, 2);
			layoutGrid.Children.Add(rAlliancePressureLbl, 3, 2);

			int bScore = 0, bAutoGears = 0, bTeleOpGears = 0, bAutoPressure = 0, bTeleOpPressure = 0, bTotalPressure = 0;
			for (int i = 3; i < 6; i++) {
				if (teams[i].avgAutoGearScored > 0.5)
					bAutoGears++;
				bAutoPressure += (int)Math.Round(teams[i].avgAutoPressure, 0);
				bTeleOpPressure += (int)Math.Round(teams[i].avgTeleOpPressure, 0);
				bTeleOpGears += (int)Math.Round(teams[i].avgTeleOpGearsScored, 0);
			}
			bTotalPressure += bAutoPressure + bTeleOpPressure;
			if (bAutoGears == 3) {
				bScore += 120;
				if (bTeleOpGears >= 10)
					rScore += 80;
				else if (bTeleOpGears >= 4)
					rScore += 40;
			} else if (bAutoGears > 0) {
				int tGear = 0;
				bScore += 60;
				if (bAutoGears == 2)
					tGear = 1;
				
				if (bTeleOpGears + tGear >= 12)
					rScore += 120;
				else if (bTeleOpGears + tGear >= 6)
					rScore += 80;
				else if (bTeleOpGears + tGear >= 2)
					rScore += 40;
			}

			bScore += bTotalPressure;

			var bAllianceScoreLbl = new Label() {
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Score: " + bScore,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			var bAllianceGearLbl = new Label() {
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Gears: " + (bAutoGears + bTeleOpGears),
				HorizontalTextAlignment = TextAlignment.Center,
			};

			var bAlliancePressureLbl = new Label() {
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Pressure: " + bTotalPressure,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			layoutGrid.Children.Add(bAllianceScoreLbl, 4, 7, 1, 2);
			layoutGrid.Children.Add(bAllianceGearLbl, 4, 2);
			layoutGrid.Children.Add(bAlliancePressureLbl, 6, 2);
		}

		void initializeTeamData() {
			for (int i = 0; i < 6; i++) {
				robotImages[i] = new CachedImage() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HeightRequest = 85,
					//WidthRequest = 120,
					DownsampleToViewSize = true,
					//Aspect = Aspect.AspectFit,
					LoadingPlaceholder = "Loading_image_placeholder.png",
					ErrorPlaceholder = "Placeholder_image_placeholder.png"
				};
				getTeamImage(teams[i], robotImages[i]);
				layoutGrid.Children.Add(robotImages[i], i + 1, 3);

				var teamNoLbl = new Label() {
					FontSize = GlobalVariables.sizeSmall,
					Text = teams[i].teamNumber.ToString(),
					TextColor = Color.White,
					HorizontalTextAlignment = TextAlignment.Center,
					FontAttributes = FontAttributes.Bold
				};
				layoutGrid.Children.Add(teamNoLbl, i + 1, 4);
			}
		}

		async Task getTeamImage(TeamData d, CachedImage img) {
			try {
				string imageURL = d.imageURL;

				img.Source = new Uri(imageURL);

				var tap = new TapGestureRecognizer();

				tap.Tapped += (s, e) => {
					// Create a gesture recognizer to display the popup image
					Navigation.PushPopupAsync(new ImagePopupPage(d));
				};
				img.GestureRecognizers.Add(tap);

			} catch (Exception ex) {
				Console.WriteLine("getTeamImage Error: " + ex.Message);
			}
		}

		void initializeHeaders() {
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

			dataGrid.Children.Add(rowHeaderLabels[rowHeadersIndex], 0, rowHeadersIndex);
			dataGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) });
			rowHeadersIndex++;
		}

		async Task awaitTeamData() {
			//teams = await FetchTeamData();
			Console.WriteLine("Start Data Grab");
			for (int i = 0; i < 6; i++) {
				var task = Task.Factory.StartNew(() => FetchTeamData(i));
				task.Wait();
			}
			while (semaphore == false) {

			}
			Console.WriteLine("Stop");
		}

		async Task FetchTeamData(int i) {
			Console.WriteLine("Start");

			if (CheckInternetConnectivity.InternetStatus()) {
				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				teams[i] = new TeamData();

				if (i < 3) {
					var fbTeams = db
									.Child(GlobalVariables.regionalPointer)
									.Child("teamData")
									.Child(matchData.Red[i].ToString())
									.OnceSingleAsync<TeamData>().ContinueWith((arg) => {
										teams[i] = arg.Result;
									});

				} else {
					var fbTeams = db
								.Child(GlobalVariables.regionalPointer)
								.Child("teamData")
								.Child(matchData.Blue[i % 3].ToString())
								.OnceSingleAsync<TeamData>().ContinueWith((arg) => {
									teams[i] = arg.Result;
									if (i == 5)
										semaphore = true;
								});
				}
				Console.WriteLine("Team: " + teams[i].teamNumber);
			}
		}
	}
}
