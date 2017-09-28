using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;


namespace VitruvianApp2017 
{
	
	public class MatchHeaderLists:ContentView
	{
		ListView upcomingMatchView = new ListView();
		List<EventMatchData> upcomingMatchList = new List<EventMatchData>();
		ListView pastMatchView = new ListView();
		List<EventMatchData> pastMatchList = new List<EventMatchData>();
		ListView ourMatchView = new ListView();
		List<EventMatchData> ourMatchList = new List<EventMatchData>();
		double height;

		Grid searchBar;
		Entry searchEntry;

		public MatchHeaderLists() {
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
				Placeholder = "Search matches by team",
				Keyboard = Keyboard.Numeric,

				MinimumWidthRequest = Width
			};
			var filterBtn = new Button() {
				Text = "Search",
				TextColor = Color.White,
				FontSize = GlobalVariables.sizeMedium,
				BackgroundColor = Color.Green,
			};
			filterBtn.Clicked += (sender, e) => {
				autoCompleteOptions();
			};
			searchBar.Children.Add(searchEntry, 1, 0);
			searchBar.Children.Add(filterBtn, 2, 0);

			var upcomingMatchHeader = new ContentView() {
				Content = new Frame() {
					OutlineColor = Color.Gray,
					Padding = new Thickness(5),

					Content = new Label() {
						Text = "Upcoming Matches",
						FontSize = GlobalVariables.sizeMedium,
						FontAttributes = FontAttributes.Bold,
						TextColor = Color.Black
					}
				}
			};
			var upcomingMatchHeaderTap = new TapGestureRecognizer();
			upcomingMatchHeaderTap.Tapped += (sender, e) => {
				Console.WriteLine("test");
				if (pastMatchView.IsEnabled) {
					upcomingMatchView.IsEnabled = !upcomingMatchView.IsEnabled;
					setListHieght();
				}
				AppSettings.SaveSettings("UpcomingMatchListEn", upcomingMatchView.IsEnabled.ToString());
			};
			upcomingMatchHeader.GestureRecognizers.Add(upcomingMatchHeaderTap);

			upcomingMatchView.ItemTemplate = new DataTemplate(() => {
				var matchLbl = new Label() {
					TextColor = Color.Black,
					FontSize = GlobalVariables.sizeMedium
				};
				matchLbl.SetBinding(Label.TextProperty, "matchNumber");

				var cell = new ViewCell() {
					View = new StackLayout() {
						Children = {
							matchLbl
						}
					}
				};

				return cell;
			});

			upcomingMatchView.ItemSelected += (sender, e) => {
				((ListView)sender).SelectedItem = null;
			};

			upcomingMatchView.ItemTapped += (sender, e) => {
				//Navigation.PushPopupAsync(new MatchInfoPopupPage((EventMatchData)e.Item));
			};

			var pastMatchHeader = new ContentView() {
				Content = new Frame() {
					OutlineColor = Color.Gray,
					Padding = new Thickness(5),

					Content = new Label() {
						Text = "Past Matches",
						FontSize = GlobalVariables.sizeMedium,
						FontAttributes = FontAttributes.Bold,
						TextColor = Color.Black
					}
				}
			};
			var pastMatchHeaderTap = new TapGestureRecognizer();
			pastMatchHeaderTap.Tapped += (sender, e) => {
				if (upcomingMatchView.IsEnabled) {
					pastMatchView.IsEnabled = !pastMatchView.IsEnabled;
					setListHieght();
				}
				AppSettings.SaveSettings("PastMatchListEn", pastMatchView.IsEnabled.ToString());
			};
			pastMatchHeader.GestureRecognizers.Add(pastMatchHeaderTap);

			pastMatchView.ItemTemplate = new DataTemplate(() => {
				var matchLbl = new Label() {
					TextColor = Color.Black,
					FontSize = GlobalVariables.sizeMedium
				};
				matchLbl.SetBinding(Label.TextProperty, "matchNumber");
				var cell = new ViewCell() {
					View = new StackLayout() {
						Children = {
							matchLbl
						}
					}
				};

				return cell;
			});

			pastMatchView.ItemSelected += (sender, e) => {
				((ListView)sender).SelectedItem = null;
			};
			pastMatchView.ItemTapped += (sender, e) => {
				//Navigation.PushPopupAsync(new MatchInfoPopupPage((EventMatchData)e.Item));
			};

			var ourMatchHeader = new ContentView() {
				Content = new Frame() {
					OutlineColor = Color.Gray,
					Padding = new Thickness(5),

					Content = new Label() {
						Text = "Our Matches",
						FontSize = GlobalVariables.sizeMedium,
						FontAttributes = FontAttributes.Bold,
						TextColor = Color.Black
					}
				}
			};

			ourMatchView.ItemTemplate = new DataTemplate(() => {
				var matchLbl = new Label() {
					TextColor = Color.Black,
					FontSize = GlobalVariables.sizeMedium
				};
				matchLbl.SetBinding(Label.TextProperty, "matchNumber");
				var cell = new ViewCell() {
					View = new StackLayout() {
						Children = {
							matchLbl
						}
					}
				};

				return cell;
			});

			ourMatchView.ItemSelected += (sender, e) => {
				((ListView)sender).SelectedItem = null;
			};
			ourMatchView.ItemTapped += (sender, e) => {
				//Navigation.PushPopupAsync(new MatchInfoPopupPage((EventMatchData)e.Item));
			};

			height = Height;

			Content = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					searchBar,
					new StackLayout(){
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						Orientation = StackOrientation.Horizontal,

						Children = {
							new StackLayout(){
								HorizontalOptions = LayoutOptions.FillAndExpand,
								VerticalOptions = LayoutOptions.FillAndExpand,

								Children = {
									upcomingMatchHeader,
									upcomingMatchView,
									pastMatchHeader,
									pastMatchView
								}
							},
							new StackLayout(){
								HorizontalOptions = LayoutOptions.FillAndExpand,
								VerticalOptions = LayoutOptions.FillAndExpand,

								Children = {
									ourMatchHeader,
									ourMatchView
								}
							}

						}
					}
				}
			};
		}

		void setListHieght() {
			if (upcomingMatchView.IsEnabled && pastMatchView.IsEnabled) {
				upcomingMatchView.HeightRequest = height;
                pastMatchView.HeightRequest = height;
			} else if (upcomingMatchView.IsEnabled && !pastMatchView.IsEnabled) {
				upcomingMatchView.HeightRequest = height;
				pastMatchView.HeightRequest = 0;
			} else if (!upcomingMatchView.IsEnabled && pastMatchView.IsEnabled) {
				upcomingMatchView.HeightRequest = 0;
				pastMatchView.HeightRequest = height;
			} else if (!upcomingMatchView.IsEnabled && !pastMatchView.IsEnabled) {
				upcomingMatchView.HeightRequest = 0;
				pastMatchView.HeightRequest = 0;
			}

			Console.WriteLine("Hieght: " + height);
			Console.WriteLine("Hieght: " + upcomingMatchView.Height);
			Console.WriteLine("Hieght: " + upcomingMatchView.HeightRequest);
			Console.WriteLine("Hieght: " + pastMatchView.Height);
			Console.WriteLine("Hieght: " + pastMatchView.HeightRequest);
			Console.WriteLine(upcomingMatchView.IsEnabled);
			Console.WriteLine(pastMatchView.IsEnabled);
		}

		public async Task updateMatchSchedule() {
			searchEntry.Text = null;
			searchEntry.Placeholder = "Search matches by team";
			await Task.Run(() => getMatchSchedule());

			upcomingMatchView.ItemsSource = upcomingMatchList;
			pastMatchView.ItemsSource = pastMatchList;
			ourMatchView.ItemsSource = ourMatchList;
			Console.WriteLine("test done");
		}

		async Task getMatchSchedule() {
			var s1 = AppSettings.RetrieveSettings("UpcomingMatchListEn");
			if (s1 == "false")
				upcomingMatchView.IsEnabled = false;
			var s2 = AppSettings.RetrieveSettings("PastMatchListEn");
			if (s2 == "false")
				pastMatchView.IsEnabled = false;
			
			var l1 = new List<EventMatchData>();
			var l2 = new List<EventMatchData>();
			var l3 = new List<EventMatchData>();

			var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			var db = new FirebaseClient(GlobalVariables.firebaseURL);
			var matches = await db
				.Child(GlobalVariables.regionalPointer)
				.Child("matchList")
				.OnceAsync<EventMatchData>();

			foreach (var match in matches) {
				if (match.Object.matchTime >= currentTime)
					l1.Add(match.Object);
				else
					l2.Add(match.Object);
				
				foreach (var blue in match.Object.Blue)
					if (blue == 4201)
						l3.Add(match.Object);
				foreach (var red in match.Object.Red)
					if (red == 4201)
						l3.Add(match.Object);
			}

			l1.Sort((x, y) => x.matchTime.CompareTo(y.matchTime));
			l2.Sort((x, y) => y.matchTime.CompareTo(x.matchTime));
			l3.Sort((x, y) => x.matchTime.CompareTo(y.matchTime));

			// Needed to avoid Java.lang.IllegalStateException
			upcomingMatchList = l1;
			pastMatchList = l2;
			ourMatchList = l3;
		}

		void autoCompleteOptions() {
			var oldMatchFilter = new List<EventMatchData>();
			var newMatchFilter = new List<EventMatchData>();
			var ourMatchFilter = new List<EventMatchData>();

			foreach (var match in upcomingMatchList) {
				foreach (var blue in match.Blue)
					if (blue.ToString() == searchEntry.Text) {
						newMatchFilter.Add(match);
						break;
					}
				foreach (var red in match.Red)
					if (red.ToString() == searchEntry.Text) {
						newMatchFilter.Add(match);
						break;
					}
			}
			foreach (var match in pastMatchList) {
				foreach (var blue in match.Blue)
					if (blue.ToString() == searchEntry.Text) {
						oldMatchFilter.Add(match);
						break;
					}
				foreach (var red in match.Red)
					if (red.ToString() == searchEntry.Text) {
						oldMatchFilter.Add(match);
						break;
					}
			}

			foreach (var match in ourMatchList) {
				foreach (var blue in match.Blue)
					if (blue.ToString() == searchEntry.Text) {
						ourMatchFilter.Add(match);
						break;
					}
				foreach (var red in match.Red)
					if (red.ToString() == searchEntry.Text) {
						ourMatchFilter.Add(match);
						break;
					}
			}

			upcomingMatchView.ItemsSource = newMatchFilter;
			pastMatchView.ItemsSource = oldMatchFilter;
			ourMatchView.ItemsSource = ourMatchFilter;
		}
	}
}
