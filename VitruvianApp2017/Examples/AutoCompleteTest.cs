using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class AutoCompleteTest:ContentPage
	{
		Entry entryTest = new Entry();
		ListView list = new ListView();
		List<TeamData> teamData = new List<TeamData>();
		bool semaphore = true;

		public AutoCompleteTest() {
			getTeamList();

			list.ItemTemplate = new DataTemplate(() => {
				var teamNumber = new Label();
				teamNumber.SetBinding(Label.TextProperty, "teamNumber");

				return new ViewCell() {
					View = new StackLayout() {
						Children = {
							teamNumber
						}
					}
				};
			});
			list.ItemSelected += (sender, e) => {
				list.IsVisible = false;
				entryTest.Text = ((TeamData)list.SelectedItem).teamNumber.ToString();
			};
			entryTest.TextChanged += (sender, e) => {
				if (semaphore)
					autoCompleteOptions();
				else
					semaphore = true;
			};

			var listScroll = new ScrollView() {
				Content = list
			};

			list.IsVisible = false;

			var navigationBtns = new NavigationButtons(false);

			Content = new StackLayout() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,

				Children = {
					entryTest,
					list,
					navigationBtns
				}
			};
		}

		async Task getTeamList() {
			var db = new FirebaseClient(GlobalVariables.firebaseURL);

			var teamList = await db
							.Child(GlobalVariables.regionalPointer)
							.Child("teamData")
							.OnceAsync<TeamData>();

			foreach (var team in teamList)
				teamData.Add(team.Object);

			list.ItemsSource = teamData;
		}

		void autoCompleteOptions() {
			var filtered = new List<TeamData>();

			foreach (var item in teamData)
				if (item.teamNumber.ToString().StartsWith(entryTest.Text))
					filtered.Add(item);
			
			list.ItemsSource = filtered;
  			list.IsVisible = true;
			semaphore = false;
		}

	}
}
