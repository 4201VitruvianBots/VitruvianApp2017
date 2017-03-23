using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class ColorButton:Button
	{
		public bool on = false;

		public ColorButton(string title) {
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;

			Text = title;
			BackgroundColor = Color.Red;
			FontSize = GlobalVariables.sizeSmall;

			Clicked += (sender, e) => {
				if (!on) {
					BackgroundColor = Color.Green;
					on = !on;
				} else {
					BackgroundColor = Color.Red;
					on = !on;
				}
			};
		}
	}
}
