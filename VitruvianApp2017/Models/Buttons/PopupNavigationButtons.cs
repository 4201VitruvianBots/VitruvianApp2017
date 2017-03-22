using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class PopupNavigationButtons:NavigationButtons
	{
		public PopupNavigationButtons(bool toggleRefreshBtn):base(toggleRefreshBtn){

		}

		public PopupNavigationButtons(Button[] array):base(array){

		}

		public PopupNavigationButtons(bool toggleRefreshBtn, Button[] array):base(toggleRefreshBtn, array){
			Margin = new Thickness(-5, 0, -5, -5);
		}

		public override async void returnPage()
		{
			await Navigation.PopPopupAsync();
		}
	}
}
