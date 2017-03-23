using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace VitruvianApp2017
{
	public class AutoCompleteTest:ContentPage
	{
		Entry entryTest = new Entry();
		ListView list = new ListView();
		List<TeamData> teamData = new List<TeamData>();
		AutoCompleteView autoComplete = new AutoCompleteView();
		bool semaphore = true;

		public AutoCompleteTest() {
			getTeamList();

			var navigationBtns = new NavigationButtons(false);

			Content = new StackLayout() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,

				Children = {
					entryTest,
					autoComplete,
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

			var testList = new List<int>();
			foreach (var team in teamList)
				testList.Add(team.Object.teamNumber);

			autoComplete.Suggestions = {testList, };=
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
