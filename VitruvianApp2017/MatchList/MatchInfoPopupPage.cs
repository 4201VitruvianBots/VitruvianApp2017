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
				//new ColumnDefinition() { Width = GridLength.Auto },
				new ColumnDefinition() { Width = GridLength.Auto }
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

		Grid rowHeaderGrid = new Grid() {
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,
			RowSpacing = 1,
			BackgroundColor = Color.Black,

			ColumnDefinitions = {
				new ColumnDefinition() { Width = GridLength.Auto }
			}
		};
		/*
		{
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,
			RowSpacing = 1,
			ColumnSpacing = 1,
			Margin = new Thickness(1),
			BackgroundColor = Color.Black,

			ColumnDefinitions = {
				new ColumnDefinition() { Width = GridLength.Auto },
			},

			RowDefinitions = {
				
			}
		};
		*/

		EventMatchData matchData;
		TeamData[] teams = new TeamData[6];
		//StackLayout[] teamHeaderLayout = new StackLayout[6];
		Button[] teamNoBtn = new Button[6];
		Grid[] teamDataGrid = new Grid[6];
		ContentView[] teamDataView = new ContentView[6];
		StackLayout[] loadingStack = new StackLayout[6];
		StackLayout[] errorStack = new StackLayout[6];
		ActivityIndicator[] busyIcon = new ActivityIndicator[6];
		Button[] retryDataBtn = new Button[6];
		bool[] semaphore = { false, false, false, false, false, false };

		int rowHeadersIndex = 0;
		Label[] rowHeaderLabels = new Label[19];

		ColumnHeaderCell[,] cHeaderCells = new ColumnHeaderCell[6,9];
		DataCell[,,] gridData = new DataCell[6, 12, 3];
		CachedImage[] robotImages = new CachedImage[6];
		TapGestureRecognizer[] tap = new TapGestureRecognizer[6];

		public MatchInfoPopupPage(EventMatchData data) {
			matchData = data;

			Title = "Match: " + data.matchNumber;

			var titleLbl = new Label() {
				Text = "Match: " + data.matchNumber,
				FontSize = GlobalVariables.sizeMedium,
				HorizontalTextAlignment = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold
			};
			layoutGrid.Children.Add(titleLbl, 1, 7, 0, 1);

			for (int i = 0; i < 6; i++) {
				teamDataView[i] = new ContentView() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Margin = 0,
					Padding = 0,
				};
				busyIcon[i] = new ActivityIndicator();
				loadingStack[i] = new StackLayout() {
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,

					Children = {
						busyIcon[i],
						new Label(){
							Text = "Loading Data...",
							FontSize = GlobalVariables.sizeTiny,
							HorizontalTextAlignment = TextAlignment.Center
						}
					}
				};
				retryDataBtn[i] = new Button() {
					FontSize = GlobalVariables.sizeSmall,
					Text = "Retry",
					TextColor = Color.Black,
					FontAttributes = FontAttributes.Bold
				};
				retryDataBtn[i].Clicked += (sender, e) => {
					awaitTeamData(i);
				};
				errorStack[i] = new StackLayout() {
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,

					Children = {
						new Label(){
							Text = "Error:",
							FontSize =GlobalVariables.sizeTiny,
							HorizontalTextAlignment = TextAlignment.Center
						},
						new Label(){
							Text = "Data not Retieved",
							FontSize = GlobalVariables.sizeTiny,
							HorizontalTextAlignment = TextAlignment.Center
						},
						retryDataBtn[i]
					}
				};
				layoutGrid.Children.Add(teamDataView[i], i + 1, 5);
			}

			initializeDataRowHeaders();

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

		protected override void OnAppearing() {
			base.OnAppearing();

			for (int i = 0; i < 6; i++) {
				teamDataView[i].Content = loadingStack[i];
				var task1 = Task.Factory.StartNew(() => awaitTeamData(i));
				task1.Wait();
				var task2 = Task.Factory.StartNew(() => initializeTeamHeaderData(i));
				task2.Wait();
				var task3 = Task.Factory.StartNew(() => initializeTeamData(i));
				task3.Wait();
				var task4 = Task.Factory.StartNew(() => getTeamImage(i));
				task4.Wait();

				busyIcon[i].IsRunning = false;
				busyIcon[i].IsEnabled = false;
			}
			initializeAllianceData();
		}

		async Task awaitTeamData(int i) {
			//teams = await FetchTeamData();
			Console.WriteLine("Start Data Grab");
			busyIcon[i].IsRunning = true;
			busyIcon[i].IsEnabled = true;
			var task1 = Task.Factory.StartNew(() => FetchTeamData(i));
			//task1.Wait();

			Console.WriteLine("Stop");
		}

		async Task FetchTeamData(int i) {
			Console.WriteLine("Start");

			if (CheckInternetConnectivity.InternetStatus()) {
				var db = new FirebaseClient(GlobalVariables.firebaseURL);
				teams[i] = new TeamData();

				if (i < 3) {
					try {
						var fbTeams = await db
										.Child(GlobalVariables.regionalPointer)
										.Child("teamData")
										.Child(matchData.Red[i].ToString())
										.OnceSingleAsync<TeamData>();
						teams[i] = fbTeams;} 
					catch (Exception ex) {
						Console.Write("Fetch Error: " + ex.Message);
					}
				} 
				else {
					try {
						var fbTeams = await db
										.Child(GlobalVariables.regionalPointer)
										.Child("teamData")
										.Child(matchData.Blue[i % 3].ToString())
										.OnceSingleAsync<TeamData>();
						teams[i] = fbTeams;
					} 
					catch (Exception ex) {
						Console.Write("Fetch Error: " + ex.Message);
						teamDataView[i].Content = errorStack[i];
					}
				}
				Console.WriteLine("Team: " + teams[i].teamNumber);
			}
			semaphore[i] = true;
		}

		async Task initializeTeamData(int i) {
			while (semaphore[i] == false) {

			}

			try {
				for (int j = 0; j < 12; j++)
					for (int k = 0; k < 3; k++) {
						gridData[i, j, k] = new DataCell();
						if (i < 3)
							gridData[i, j, k].BackgroundColor = Color.Red;
						else
							gridData[i, j, k].BackgroundColor = Color.Blue;
					}
				int y = 0;

				teamDataGrid[i] = new Grid() {
					BackgroundColor = Color.Black,
					ColumnSpacing = 1,
					RowSpacing = 1,
				};
				// Add data headers
				cHeaderCells[i, 0] = new ColumnHeaderCell();
				cHeaderCells[i, 0].header.Text = "Successes";
				teamDataGrid[i].Children.Add(cHeaderCells[i, 0], 0, 3);
				cHeaderCells[i, 1] = new ColumnHeaderCell();
				cHeaderCells[i, 1].header.Text = "Attempts";
				teamDataGrid[i].Children.Add(cHeaderCells[i, 1], 1, 3);
				cHeaderCells[i, 2] = new ColumnHeaderCell();
				cHeaderCells[i, 2].header.Text = "Drops";
				teamDataGrid[i].Children.Add(cHeaderCells[i, 2], 2, 3);

				cHeaderCells[i, 3] = new ColumnHeaderCell();
				cHeaderCells[i, 3].header.Text = "Avg.";
				teamDataGrid[i].Children.Add(cHeaderCells[i, 3], 0, 6);

				cHeaderCells[i, 4] = new ColumnHeaderCell();
				cHeaderCells[i, 4].header.Text = "High";
				teamDataGrid[i].Children.Add(cHeaderCells[i, 4], 1, 6);

				cHeaderCells[i, 5] = new ColumnHeaderCell();
				cHeaderCells[i, 5].header.Text = "Avg.";
				teamDataGrid[i].Children.Add(cHeaderCells[i, 5], 0, 9);
				cHeaderCells[i, 6] = new ColumnHeaderCell();
				cHeaderCells[i, 6].header.Text = "High";
				teamDataGrid[i].Children.Add(cHeaderCells[i, 6], 1, 9);

				cHeaderCells[i, 7] = new ColumnHeaderCell();
				cHeaderCells[i, 7].header.Text = "Successes";
				teamDataGrid[i].Children.Add(cHeaderCells[i, 7], 0, 15);
				cHeaderCells[i, 8] = new ColumnHeaderCell();
				cHeaderCells[i, 8].header.Text = "Attempts";
				teamDataGrid[i].Children.Add(cHeaderCells[i, 8], 1, 15);

				// Add Data
				gridData[i, 0, 0].data.Text = teams[i].matchCount.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 0, 0], 0, y++);
				gridData[i, 1, 0].data.Text = teams[i].maxFuelCapacity.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 1, 0], 0, y++);
				y++;
				y++;
				gridData[i, 2, 0].data.Text = teams[i].totalAutoCrossSuccesses.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 2, 0], 0, y++);
				gridData[i, 3, 0].data.Text = String.Format("{0:N2}", teams[i].avgAutoGearScored);
				teamDataGrid[i].Children.Add(gridData[i, 3, 0], 0, y);
				gridData[i, 3, 1].data.Text = String.Format("{0:N2}", teams[i].avgAutoGearsDelivered);
				teamDataGrid[i].Children.Add(gridData[i, 3, 1], 1, y);
				gridData[i, 3, 2].data.Text = String.Format("{0:N2}", teams[i].avgAutoGearsDropped);
				teamDataGrid[i].Children.Add(gridData[i, 3, 2], 2, y++);
				y++;
				gridData[i, 4, 0].data.Text = String.Format("{0:N2}", teams[i].avgAutoPressure);
				teamDataGrid[i].Children.Add(gridData[i, 4, 0], 0, y);
				gridData[i, 4, 1].data.Text = teams[i].autoPressureHigh.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 4, 1], 1, y++);
				y++;
				y++;
				gridData[i, 5, 0].data.Text = String.Format("{0:N2}", teams[i].avgTeleOpActions);
				teamDataGrid[i].Children.Add(gridData[i, 5, 0], 0, y);
				gridData[i, 5, 1].data.Text = teams[i].teleOpActionsHigh.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 5, 1], 1, y++);
				gridData[i, 6, 0].data.Text = String.Format("{0:N2}", teams[i].avgTeleOpGearsScored);
				teamDataGrid[i].Children.Add(gridData[i, 6, 0], 0, y);
				gridData[i, 6, 1].data.Text = teams[i].teleOpGearsScoredHigh.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 6, 1], 1, y++);
				gridData[i, 7, 0].data.Text = String.Format("{0:N2}", teams[i].avgTeleOpGearsStationDropped);
				teamDataGrid[i].Children.Add(gridData[i, 7, 0], 0, y);
				gridData[i, 7, 1].data.Text = teams[i].teleOpGearsStationDroppedHigh.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 7, 1], 1, y++);
				gridData[i, 8, 0].data.Text = String.Format("{0:N2}", teams[i].avgTeleOpGearsTransitDropped);
				teamDataGrid[i].Children.Add(gridData[i, 8, 0], 0, y);
				gridData[i, 8, 1].data.Text = teams[i].teleOpGearsTransitDroppedHigh.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 8, 1], 1, y++);
				gridData[i, 9, 0].data.Text = String.Format("{0:N2}", teams[i].avgTeleOpPressure);
				teamDataGrid[i].Children.Add(gridData[i, 9, 0], 0, y);
				gridData[i, 9, 1].data.Text = teams[i].teleOpPressureHigh.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 9, 1], 1, y++);
				y++;
				gridData[i, 10, 0].data.Text = teams[i].successfulClimbCount.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 10, 0], 0, y);
				gridData[i, 10, 1].data.Text = teams[i].attemptedClimbCount.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 10, 1], 1, y++);
				gridData[i, 11, 0].data.Text = teams[i].foulCount.ToString();
				teamDataGrid[i].Children.Add(gridData[i, 11, 0], 0, y++);

				teamDataView[i].Content = teamDataGrid[i];
			} catch (Exception ex) {
				Console.WriteLine("initializeTeamData Error: " + ex.Message);
			}
		}

		void initializeAllianceData() {
			// Calculate Alliance Potential & display it
			int rScore = 0, rAutoGears = 0, rTeleOpGears = 0, rAutoPressure = 0, rTeleOpPressure = 0, rTotalPressure = 0,
				rTotalClimbs = 0;
			for (int i = 0; i < 3; i++) {
				if (teams[i] != null) {
					if (teams[i].avgAutoGearScored > 0.5)
						rAutoGears++;
					rAutoPressure += (int)Math.Round(teams[i].avgAutoPressure, 0);
					rTeleOpPressure += (int)Math.Round(teams[i].avgTeleOpPressure, 0);
					rTeleOpGears += (int)Math.Round(teams[i].avgTeleOpGearsScored, 0);
					if(teams[i].matchCount != 0)
						rTotalClimbs += (int)Math.Round(Convert.ToDouble(teams[i].successfulClimbCount / teams[i].matchCount), 0);
				}
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

			var rAllianceClimbLbl = new Label() {
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Climbs: " + rTotalClimbs,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			layoutGrid.Children.Add(rAllianceScoreLbl, 1, 4, 1, 2);
			layoutGrid.Children.Add(rAllianceGearLbl, 1, 2);
			layoutGrid.Children.Add(rAlliancePressureLbl, 2, 2);
			layoutGrid.Children.Add(rAllianceClimbLbl, 3, 2);

			int bScore = 0, bAutoGears = 0, bTeleOpGears = 0, bAutoPressure = 0, bTeleOpPressure = 0, bTotalPressure = 0,
				bTotalClimbs = 0;
			for (int i = 3; i < 6; i++) {
				if (teams[i] != null) {
					if (teams[i].avgAutoGearScored > 0.5)
						bAutoGears++;
					bAutoPressure += (int)Math.Round(teams[i].avgAutoPressure, 0);
					bTeleOpPressure += (int)Math.Round(teams[i].avgTeleOpPressure, 0);
					bTeleOpGears += (int)Math.Round(teams[i].avgTeleOpGearsScored, 0);
					if(teams[i].matchCount != 0)
						bTotalClimbs += (int)Math.Round(Convert.ToDouble(teams[i].successfulClimbCount / teams[i].matchCount), 0);
				}
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

			var bAllianceClimbLbl = new Label() {
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Climbs: " + bTotalClimbs,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			layoutGrid.Children.Add(bAllianceScoreLbl, 4, 7, 1, 2);
			layoutGrid.Children.Add(bAllianceGearLbl, 4, 2);
			layoutGrid.Children.Add(bAlliancePressureLbl, 5, 2);
			layoutGrid.Children.Add(bAllianceClimbLbl, 6, 2);
		}

		async Task initializeTeamHeaderData(int i) {
			while (semaphore[i] == false) {

			}

			teamNoBtn[i] = new Button() {
				FontSize = GlobalVariables.sizeSmall,
				Text = teams[i].teamNumber.ToString(),
				TextColor = Color.Black,
				FontAttributes = FontAttributes.Bold
			};
			teamNoBtn[i].Clicked += (sender, e) => {
				Navigation.PushPopupAsync(new TeamCardPopupPage(teams[i]));
			};

			layoutGrid.Children.Add(teamNoBtn[i], i + 1, 4);
		}

		async Task getTeamImage(int i) {
			while (semaphore[i] == false) {

			}
			robotImages[i] = new CachedImage() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 120,
				//WidthRequest = 120,
				DownsampleToViewSize = true,
				Aspect = Aspect.AspectFit,
				CacheDuration = new TimeSpan(7, 0, 0, 0),
			};
			tap[i] = new TapGestureRecognizer();
			tap[i].Tapped += (sender, e) => {
				Navigation.PushPopupAsync(new ImagePopupPage(teams[i]));
			};
			robotImages[i].GestureRecognizers.Add(tap[i]);
			layoutGrid.Children.Add(robotImages[i], i + 1, 3);

			try {
				robotImages[i].Source = new Uri(teams[i].imageURL);
				layoutGrid.Children.Add(robotImages[i], i + 1, 3);
			} catch (Exception ex) {
				Console.WriteLine("initializeTeamHeader Error: " + ex.Message);
				robotImages[i].Source = "placeholder_image_placeholder.png";
			}
		}

		void initializeDataRowHeaders() {
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

			layoutGrid.ColumnDefinitions[0] = rowHeaderGrid.ColumnDefinitions[0];
			layoutGrid.Children.Add(rowHeaderGrid, 0, 5);
		}

		void addRowHeaders(string description) {
			rowHeaderLabels[rowHeadersIndex] = new Label() {
				FontSize = GlobalVariables.sizeTiny,
				TextColor = Color.White,
				Text = description
			};
			if (rowHeadersIndex == 3 || rowHeadersIndex == 9)
				rowHeaderLabels[rowHeadersIndex].FontAttributes = FontAttributes.Bold;

			rowHeaderGrid.Children.Add(rowHeaderLabels[rowHeadersIndex], 0, rowHeadersIndex);
			rowHeaderGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) });
			rowHeadersIndex++;
		}
	}
}
