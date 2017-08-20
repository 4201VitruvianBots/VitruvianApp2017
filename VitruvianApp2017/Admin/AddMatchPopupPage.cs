using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class AddMatchPopupPage:PopupPage
	{
		Grid pageGrid = new Grid() {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,

		};

		Entry matchNoEntry;
		TimePicker timePicker = new TimePicker();
		Entry[] red = new Entry[3];
		Entry[] blue = new Entry[3];

		long laFinals = 1490511600;
		long lvFinals = 1491634800;
		long txFinals = 1492844400;
		long matchTime;

		public AddMatchPopupPage() {

			 matchNoEntry = new Entry() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Placeholder = "Enter Match No.",
				HorizontalTextAlignment = TextAlignment.Center,
			};

			timePicker.HorizontalOptions = LayoutOptions.CenterAndExpand;
			timePicker.Time = new TimeSpan(13, 0 , 0);
			matchTime = (long)timePicker.Time.TotalSeconds;
			timePicker.PropertyChanged += (sender, e) => {
				if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
					matchTime = (long)timePicker.Time.TotalSeconds;
			};

			for (int i = 0; i < 3; i++) {
				red[i] = new Entry() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Placeholder = "Red " + (i + 1),
					HorizontalTextAlignment = TextAlignment.Center,
					Keyboard = Keyboard.Numeric
				};
				blue[i] = new Entry() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Placeholder = "Blue " + (i + 1),
					HorizontalTextAlignment = TextAlignment.Center,
					Keyboard = Keyboard.Numeric
				};
			}

			pageGrid.Children.Add(matchNoEntry, 0, 2, 0, 1);
			pageGrid.Children.Add(timePicker, 0, 2, 1, 2);
			pageGrid.Children.Add(red[0], 0, 2);
			pageGrid.Children.Add(red[1], 0, 3);
			pageGrid.Children.Add(red[2], 0, 4);
			pageGrid.Children.Add(blue[0], 1, 2);
			pageGrid.Children.Add(blue[1], 1, 3);
			pageGrid.Children.Add(blue[2], 1, 4);

			var addMatchBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Add Match",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			addMatchBtn.Clicked += (sender, e) => {
				addMatch();
			};

			Button[] btnArray = { addMatchBtn };
			var navigationBtns = new PopupNavigationButtons(false, btnArray);

			Content = new Frame() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Margin = new Thickness(400, 50),
				Padding = new Thickness(5),

				BackgroundColor = Color.Gray,
				HasShadow = true,

				Content = new StackLayout() {

					Children = {
						new ScrollView(){
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.FillAndExpand,

							Content = pageGrid
						},
						navigationBtns
					}
				}
			};
		}

		async Task addMatch() {
			try {
				var match = new EventMatchData();
				match.Red = new int[3];
				match.Blue = new int[3];
				for (int i = 0; i < 3; i++) {
					match.Red[i] = Convert.ToInt32(red[i].Text);
					match.Blue[i] = Convert.ToInt32(blue[i].Text);
				}
				match.matchNumber = matchNoEntry.Text;
				if (GlobalVariables.regionalPointer == "2017calb")
					match.matchTime = laFinals + matchTime;
				else if (GlobalVariables.regionalPointer == "2017nvlv")
					match.matchTime = lvFinals + matchTime;
				else if (GlobalVariables.regionalPointer == "2017cmptx")
					match.matchTime = txFinals + matchTime;
				else
					match.matchTime = txFinals + matchTime;

				Console.WriteLine("Match Time: " + match.matchTime);

				var db = new FirebaseClient(GlobalVariables.firebaseURL);

				var upload = db
							.Child(GlobalVariables.regionalPointer)
							.Child("matchList")
							.Child(matchNoEntry.Text)
							.PutAsync(match);
				
				await DisplayAlert("Success", "Match Successfully Added", "OK").ContinueWith((a) => {
					Navigation.PopPopupAsync();
				});
			} catch (Exception ex) {
				Console.WriteLine("addMatch Error: " + ex.Message);
			}

		}
	}
}
