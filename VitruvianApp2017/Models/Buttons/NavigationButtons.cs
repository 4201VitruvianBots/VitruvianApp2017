using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class NavigationButtons:StackLayout
	{
		public Button refreshBtn;
		public Button[] arrayOfBtn;
		public Button backBtn;

		public NavigationButtons() : this(false, null)
		{

		}

		public NavigationButtons(bool toggleRefreshBtn) : this(toggleRefreshBtn, null)
		{

		}

		public NavigationButtons(Button[] array) : this(false, array)
		{

		}

		public NavigationButtons(bool toggleRefreshBtn, Button[] array){
			HorizontalOptions = LayoutOptions.Fill;
			Orientation = StackOrientation.Horizontal;
			BackgroundColor = Color.Green;
			Padding = 5;

			arrayOfBtn = array;
			//Back Button
			backBtn = new Button(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				//VerticalOptions = LayoutOptions.FillAndExpand,
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			backBtn.Clicked += (object sender, EventArgs e) =>{
				returnPage();
			};

			//Refresh Button
			refreshBtn = new Button(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				//VerticalOptions = LayoutOptions.FillAndExpand,
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};

			Children.Add(backBtn);
			if(arrayOfBtn != null)
				foreach (Button btn in arrayOfBtn)
					Children.Add(btn);
			if (toggleRefreshBtn)
				Children.Add(refreshBtn);
		}

		public virtual async void returnPage(){
			await Navigation.PopAsync();
		}
	}

}
