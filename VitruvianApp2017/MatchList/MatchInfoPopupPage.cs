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


			ColumnDefinitions = {

			},

			RowDefinitions = {

			}
		};

		Grid dataGrid = new Grid() {
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,
			RowSpacing = 1,
			ColumnSpacing = 1,
			BackgroundColor = Color.Black,

			ColumnDefinitions = {
				
			},

			RowDefinitions = {
				
			}
		};

		EventMatchData matchData;
		TeamData[] teams = new TeamData[6];
		TeamData[] blue = new TeamData[3];
		TeamData[] red = new TeamData[3];

		int rowHeadersIndex = 0;
		Label[] rowHeaderLabels = new Label[15];

		ColumnHeaderCell[,] cHeaderCells = new ColumnHeaderCell[6,9];
		DataCell[,,] gridData = new DataCell[6, 12, 3];

		public MatchInfoPopupPage(EventMatchData data) {
			matchData = data;

			awaitTeamData();

			Title = "Match: " + data.matchNumber;

			// Add Top Data (?)
			initializeAllianceData();
			initializeTeamData();

			initializeData();
			layoutGrid.Children.Add(dataGrid, 0, 7, 4, 5);

			var navigationBtns = new PopupNavigationButtons(true);

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
				dataGrid.Children.Add(cHeaderCells[i, 0], x + 1, 3);
				cHeaderCells[i, 1] = new ColumnHeaderCell();
				cHeaderCells[i, 1].header.Text = "Attempts";
				dataGrid.Children.Add(cHeaderCells[i, 1], x + 2, 3);
				cHeaderCells[i, 2] = new ColumnHeaderCell();
				cHeaderCells[i, 2].header.Text = "Drops";
				dataGrid.Children.Add(cHeaderCells[i, 2], x + 3, 3);

				cHeaderCells[i, 3] = new ColumnHeaderCell();
				cHeaderCells[i, 3].header.Text = "Avg.";
				dataGrid.Children.Add(cHeaderCells[i, 3], x + 1, 5);

				cHeaderCells[i, 4] = new ColumnHeaderCell();
				cHeaderCells[i, 4].header.Text = "High";
				dataGrid.Children.Add(cHeaderCells[i, 4], x + 2, 5);

				cHeaderCells[i, 5] = new ColumnHeaderCell();
				cHeaderCells[i, 5].header.Text = "Avg.";
				dataGrid.Children.Add(cHeaderCells[i, 5], x + 1, 8);
				cHeaderCells[i, 6] = new ColumnHeaderCell();
				cHeaderCells[i, 6].header.Text = "High";
				dataGrid.Children.Add(cHeaderCells[i, 6], x + 2, 8);

				cHeaderCells[i, 7] = new ColumnHeaderCell();
				cHeaderCells[i, 7].header.Text = "Successes";
				dataGrid.Children.Add(cHeaderCells[i, 7], x + 1, 14);
				cHeaderCells[i, 8] = new ColumnHeaderCell();
				cHeaderCells[i, 8].header.Text = "Attempts";
				dataGrid.Children.Add(cHeaderCells[i, 8], x + 2, 14);

				// Add Data
				gridData[i, 0, 0].data.Text = teams[i].matchCount.ToString();
				dataGrid.Children.Add(gridData[i, 0, 0], x, x + 2, y, y++ + 1);
				gridData[i, 1, 0].data.Text = teams[i].maxFuelCapacity.ToString();
				dataGrid.Children.Add(gridData[i, 1, 0], x, x + 2, y, y++ + 1);
				y++;
				gridData[i, 2, 0].data.Text = teams[i].totalAutoCrossSuccesses.ToString();
				dataGrid.Children.Add(gridData[i, 2, 0], x, x + 2, y, y++ + 1);
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
				dataGrid.Children.Add(gridData[i, 11, 0], x, x + 2, y, y++ + 1);
			}
		}

		void initializeAllianceData() {
			// Calculate Alliance Potential & display it
			int rScore = 0, rAutoGears = 0, rTeleOpGears = 0, rAutoPressure = 0, rTeleOpPressure = 0, rTotalPressure = 0;
			foreach (var rTeam in red) {
				if (rTeam.avgAutoGearScored > 0.5)
					rAutoGears++;
				rAutoPressure += (int)Math.Round(rTeam.avgAutoPressure, 0);
				rTeleOpPressure += (int)Math.Round(rTeam.avgTeleOpPressure, 0);
				rTeleOpGears += (int)Math.Round(rTeam.avgTeleOpGearsScored, 0);
			}
			rTotalPressure += rAutoPressure + rTeleOpPressure;
			if (rAutoGears == 3)
				rScore += 120;
			else if (rAutoGears > 0)
				rScore += 60;
			rScore += rTotalPressure;

			var rAllianceScoreLbl = new Label() {
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Score: " + rScore,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			var rAllianceGearLbl = new Label() {
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Gears: " + (rAutoGears + rTeleOpGears),
				HorizontalTextAlignment = TextAlignment.Center,
			};

			var rAlliancePressureLbl = new Label() {
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Pressure: " + rTotalPressure,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			layoutGrid.Children.Add(rAllianceScoreLbl, 1, 4, 0, 1);
			layoutGrid.Children.Add(rAllianceGearLbl, 1, 1);
			layoutGrid.Children.Add(rAlliancePressureLbl, 3, 1);

			int bScore = 0, bAutoGears = 0, bTeleOpGears = 0, bAutoPressure = 0, bTeleOpPressure = 0, bTotalPressure = 0;
			foreach (var bTeam in blue) {
				if (bTeam.avgAutoGearScored > 0.5)
					bAutoGears++;
				bAutoPressure += (int)Math.Round(bTeam.avgAutoPressure, 0);
				bTeleOpPressure += (int)Math.Round(bTeam.avgTeleOpPressure, 0);
				bTeleOpGears += (int)Math.Round(bTeam.avgTeleOpGearsScored, 0);
			}
			bTotalPressure += bAutoPressure + bTeleOpPressure;
			if (bAutoGears == 3)
				bScore += 120;
			else if (bAutoGears > 0)
				bScore += 60;
			bScore += bTotalPressure;

			var bAllianceScoreLbl = new Label() {
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Score: " + bScore,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			var bAllianceGearLbl = new Label() {
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Gears: " + (bAutoGears + bTeleOpGears),
				HorizontalTextAlignment = TextAlignment.Center,
			};

			var bAlliancePressureLbl = new Label() {
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				Text = "Avg. Pressure: " + bTotalPressure,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			layoutGrid.Children.Add(bAllianceScoreLbl, 4, 7, 0, 1);
			layoutGrid.Children.Add(bAllianceGearLbl, 4, 1);
			layoutGrid.Children.Add(bAlliancePressureLbl, 6, 1);
		}

		void initializeTeamData() {
			for (int i = 0; i < 6; i++) {
				var robotImage = new CachedImage() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HeightRequest = 120,
					//WidthRequest = 120,
					DownsampleToViewSize = true,
					//Aspect = Aspect.AspectFit,
					LoadingPlaceholder = "Loading_image_placeholder.png",
					ErrorPlaceholder = "Placeholder_image_placeholder.png"
				};
				getTeamImage(teams[i], robotImage);
				layoutGrid.Children.Add(robotImage, i + 1, 2);
				var teamNoLbl = new Label() {
					FontSize = GlobalVariables.sizeSmall,
					Text = teams[i].teamNumber.ToString(),
					TextColor = Color.White,
					HorizontalTextAlignment = TextAlignment.Center
				};
				layoutGrid.Children.Add(teamNoLbl, i + 1, 3);

				var teamNotes = new Label() {
					FontSize = GlobalVariables.sizeTiny,
					TextColor = Color.White,
				};
				if (string.IsNullOrEmpty(teams[i].notes))
					teamNotes.Text = "No Notes Recorded";
				else
					teamNotes.Text = teams[i].notes;

				layoutGrid.Children.Add(new ScrollView() {
					Content = new StackLayout() {
						Children = {
							teamNotes
						}
					}
				}, i + 1, 5);
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
			addRowHeaders("");
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
				TextColor = Color.White
			};
			if (rowHeadersIndex == 3 || rowHeadersIndex == 10)
				rowHeaderLabels[rowHeadersIndex].FontAttributes = FontAttributes.Bold;

			dataGrid.Children.Add(rowHeaderLabels[rowHeadersIndex], 0, rowHeadersIndex);
			dataGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) });
			rowHeadersIndex++;
		}

		async Task awaitTeamData() {
			//teams = await FetchTeamData();
			for (int i = 0; i < 6; i++)
				await FetchTeamData(i);

			/*
			for (int i = 0; i < 3; i++){
				// var task =  Task.Factory.StartNew(() => FetchTeamData(matchData.Red[i]));
				// teams[i] = await task.Result;
				teams[i] = await FetchTeamData(matchData.Red[i]);
			} 
			for (int i = 0; i < 3; i++) {
				//var task = Task.Factory.StartNew(() => FetchTeamData(matchData.Blue[i]));
				// teams[i + 3] = await task.Result;
				teams[i + 3] = await FetchTeamData(matchData.Blue[i]);
			}
			*/
			Console.WriteLine("Stop");
		}

		async Task FetchTeamData(int i) {
			Console.WriteLine("Start");

			if (CheckInternetConnectivity.InternetStatus()) {
				try {
					var db = new FirebaseClient(GlobalVariables.firebaseURL);
					teams[i] = new TeamData();

					if (i < 3) {
						var fbTeams = await db
										.Child(GlobalVariables.regionalPointer)
										.Child("teamData")
										.Child(matchData.Red[i].ToString())
										.OnceSingleAsync<TeamData>();

						teams[i] = fbTeams;
						red[i] = teams[i];
					} else {
						var fbTeams = await db
									.Child(GlobalVariables.regionalPointer)
									.Child("teamData")
									.Child(matchData.Blue[i % 3].ToString())
									.OnceSingleAsync<TeamData>();

						teams[i] = fbTeams;
						blue[i % 3] = teams[i];
					}

					Console.WriteLine("Team: " + teams[i].teamNumber);
				} catch (Exception ex) {
					Console.WriteLine("FetchTeamData Error: " + ex.Message);
				}
			}
		}
	}
}
