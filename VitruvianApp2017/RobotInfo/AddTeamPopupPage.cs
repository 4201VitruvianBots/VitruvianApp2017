using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Firebase.Xamarin.Database;

namespace VitruvianApp2017
{
	public class AddTeamPopupPage:PopupPage
	{
		LineEntry teamNo = new LineEntry("Team Number:");
		LineEntry teamNa = new LineEntry("Team Name:");

		public AddTeamPopupPage()
		{
			

			Button addTeamBtn = new Button()
			{
				Text = "Add Team"
			};
			addTeamBtn.Clicked += (object sender, EventArgs e) =>
			{
				addTeam();
			};

			Button[] btnArray = { addTeamBtn };
			var navigationBtns = new NavigationButtons(btnArray);

			Content = new StackLayout()
			{
				Padding = new Thickness(50, 50, 50, 50),

				Children = {
					teamNo,
					teamNa,
					navigationBtns
				}
			};
			BackgroundColor = Color.Gray;
		}

		void addTeam()
		{
			try
			{
				var db = new FirebaseClient("https://vitruvianapptest.firebaseio.com/teamData")
					.Child(teamNo.data)
					.PostAsync(new TeamData()
					{
						teamName = teamNa.data,
						teamNumber = Convert.ToDouble(teamNo.data)
					});

				// Need refresh
				popUpReturn();

			}
			catch
			{

			}
		}

		async void popUpReturn()
		{
			await Task.Yield();
			await PopupNavigation.PopAsync();
		}
	}
}
