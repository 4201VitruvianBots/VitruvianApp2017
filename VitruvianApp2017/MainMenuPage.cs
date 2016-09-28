using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace VitruvianApp2017
{
	public class MainMenuPage:ContentPage
	{
		ContentPage[] mainMenuLinks = { new RobotInfoIndexPage() };
		String[] mainMenuPageTitles = { "Robot Information" };
		Button[] btnArray;
		int index = 0;
		public MainMenuPage()
		{
			Title = "Team 4201 Scouting App";
			Label title = new Label(){
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
				Spacing = 20
			};

			foreach (String pageTitle in mainMenuPageTitles){
				btnArray[index] = new Button();
				btnArray[index].Clicked	+= (object sender, EventArgs e) => {
					Navigation.PushModalAsync(mainMenuLinks[index]);
				};
				pageStack.Children.Add(btnArray[index]);
				index++;
			}

			this.Content = new ScrollView(){
				Content = pageStack
			};

			BackgroundColor = Color.White;
		}
	}
}
