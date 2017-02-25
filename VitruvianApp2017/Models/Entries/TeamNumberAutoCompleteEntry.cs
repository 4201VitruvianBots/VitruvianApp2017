using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class TeamNumberAutoComplete:StackLayout
	{
		Entry lineEntry = new Entry();
		ListView list = new ListView();
		List<TeamData> teamData = new List<TeamData>();
		ScrollView listScroll = new ScrollView();
		bool semaphore = true;
		public int teamNo;
		double initialHieght;

		public TeamNumberAutoComplete() {
			getTeamList();
			initialHieght = Height;

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
				listScroll.IsVisible = false;
				listScroll.IsEnabled = false;
				HeightRequest = initialHieght;
				teamNo = ((TeamData)list.SelectedItem).teamNumber;
				lineEntry.Text = teamNo.ToString();
			};
			lineEntry.TextChanged += (sender, e) => {
				if (semaphore)
					autoCompleteOptions();
				else
					semaphore = true;
				if(lien
			};

			listScroll.Content = list;

			var relLayout = new RelativeLayout() { 
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			relLayout.Children.Add(listScroll, Constraint.RelativeToParent((parent) => {
				return parent.X;
			}), Constraint.RelativeToParent((parent) => {
				return parent.Y;
			}), Constraint.RelativeToParent((parent) => {
				return parent.Width;
			}), Constraint.RelativeToParent((parent) => {
				return parent.Height;
			}));

			listScroll.IsVisible = false;
			listScroll.IsEnabled = false;

			Children.Add(entryLbl);
			Children.Add(lineEntry);
			Children.Add(listScroll);
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
			int height = 0;
			foreach (var item in teamData)
				if (item.teamNumber.ToString().StartsWith(lineEntry.Text)) {
					filtered.Add(item);
					height += 40;
				}
			
			list.ItemsSource = filtered;
			listScroll.IsVisible = true;
			listScroll.IsEnabled = true;
			semaphore = false;
		}

	}
}
