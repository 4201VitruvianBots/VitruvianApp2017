using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace VitruvianApp2017
{
	public class AdminLoginPopup:PopupPage
	{
		public AdminLoginPopup() {

			var pageGrid = new Grid() {

				RowDefinitions = {
					new RowDefinition() { Height = GridLength.Auto },
					new RowDefinition() { Height = GridLength.Auto },
					new RowDefinition() { Height = GridLength.Auto }
				}
			};

			var pw = new LineEntry("Enter Password:");
			pw.inputEntry.IsPassword = true;

			var checkBox = new CheckBox() {
				DefaultText = "Remember Login"
			};
			var loginBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Login",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			loginBtn.Clicked += (sender, e) => {
				if (pw.inputEntry.Text == "DaV1nc1") {
					if (checkBox.Checked)
						AppSettings.SaveSettings("AdminLogin", "true");
						
					pw.inputEntry.Text = null;
					Navigation.PopPopupAsync();
					Navigation.PushModalAsync(new AdminPage());
				} else
					DisplayAlert("Error", "Incorrect password", "OK");
			};

			Button[] btnArray = { loginBtn };
			var navigationBtns = new PopupNavigationButtons(false, btnArray);


			pageGrid.Children.Add(pw, 0, 0);
			pageGrid.Children.Add(checkBox, 0, 1);
			pageGrid.Children.Add(navigationBtns, 0, 2);

			Content = new Frame() {
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Margin = new Thickness(50),
				Padding = new Thickness(5),

				BackgroundColor = Color.Gray,
				HasShadow = true,
		
				Content = pageGrid
			};
		}
	}
}
