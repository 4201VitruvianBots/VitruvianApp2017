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

		public NavigationButtons(bool toogleRefreshBtn) : this(toogleRefreshBtn, null)
		{

		}

		public NavigationButtons(Button[] array) : this(false, array)
		{

		}

		public NavigationButtons(bool toogleRefreshBtn, Button[] array)
		{
			arrayOfBtn = array;


			//Back Button
			backBtn = new Button()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			backBtn.Clicked += (object sender, EventArgs e) =>
			{
				try
				{
					try
					{
						Navigation.PopModalAsync();
					}
					catch
					{
						popUpReturn();
					}
				}
				catch
				{

				}
			};

			//Refresh Button
			refreshBtn = new Button()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};

			HorizontalOptions = LayoutOptions.FillAndExpand;
			Orientation = StackOrientation.Horizontal;
			BackgroundColor = Color.Green;
			Padding = 5;

			Children.Add(backBtn);
			if(arrayOfBtn != null)
				foreach (Button btn in arrayOfBtn)
					Children.Add(btn);
			if (toogleRefreshBtn)
				Children.Add(refreshBtn);
		}

		async void popUpReturn()
		{
			await Task.Yield();
			await PopupNavigation.PopAsync();
		}
	}
}
