using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;

namespace VitruvianApp2017
{
	public class MainMenuPage:ContentPage
	{
		ContentPage[] mainMenuLinks = { new RobotInfoIndexPage(), new PreMatchScoutingPage(), new MatchListIndexPage(), new AdminPage() };//, new AutoMatchScoutingPage(new TeamMatchData(), -1)};
		String[] mainMenuPageTitles = { "Robot Info", "Match Scouting", "Match List", "Admin Page" };//, "Test"};

		public MainMenuPage()
		{
			Title = "Team 4201 Scouting App";

			Label regionalLbl = new Label()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Current Regional: " + GlobalVariables.regionalPointer,
				TextColor = Color.White,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeSmall,
				FontAttributes = FontAttributes.Bold
			};

			StackLayout pageStack = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 20,

				Children = {
					new StackLayout(){
						//HorizontalOptions = LayoutOptions.FillAndExpand,
						//VerticalOptions = LayoutOptions.FillAndExpand,
						Spacing = 0
					}
				}
			};

			foreach (String pageTitle in mainMenuPageTitles){
				int index = Array.IndexOf(mainMenuPageTitles, pageTitle);
				var btn = new Button(){
					Text = mainMenuPageTitles[index],
					FontSize = GlobalVariables.sizeMedium,
					BackgroundColor = Color.Green,
					TextColor = Color.White
				};

				btn.Clicked	+= (object sender, EventArgs e) => {
					if (mainMenuLinks[index].GetType().ToString() == "VitruvianApp2017.AdminLoginPopup") {
						var setting = AppSettings.RetrieveSettings("AdminLogin");

						if (setting == "true")
							Navigation.PushAsync(new AdminPage());
						else
							Navigation.PushPopupAsync((PopupPage)mainMenuLinks[index]);
					}
					else
						Navigation.PushAsync(mainMenuLinks[index]);
				};
				pageStack.Children.Add(btn);
			}

			this.Content = new StackLayout(){
				Children = {
					regionalLbl,
					new ScrollView(){
						IsClippedToBounds = true,

						Content = pageStack
					}
				}
			};

			BackgroundColor = Color.White;

		}
	}
}
