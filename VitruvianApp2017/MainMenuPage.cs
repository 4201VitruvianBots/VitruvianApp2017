using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace VitruvianApp2017
{
	public class MainMenuPage:ContentPage
	{
		ContentPage[] mainMenuLinks = { new RobotInfoIndexPage(), new PitScoutingIndexPage() };
		String[] mainMenuPageTitles = { "Robot Information", "Pit Scouting" };

		public MainMenuPage()
		{
			Title = "Team 4201 Scouting App";
			Label titleLbl = new Label(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Team 4201 Scouting App",
				TextColor = Color.White,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			StackLayout pageStack = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 20,

				Children = {
					titleLbl
				}
			};

			Console.WriteLine("Test: " + mainMenuPageTitles.Length);

			foreach (String pageTitle in mainMenuPageTitles){
				int index = Array.IndexOf(mainMenuPageTitles, pageTitle);
				var btn = new Button(){
					Text = mainMenuPageTitles[index],
					BackgroundColor = Color.Green,
					TextColor = Color.White
				};
				btn.Clicked	+= (object sender, EventArgs e) => {
					Navigation.PushModalAsync(mainMenuLinks[index]);
				};
				pageStack.Children.Add(btn);
			}

			this.Content = new ScrollView(){
				Content = pageStack
			};

			BackgroundColor = Color.White;

		}
	}
}
