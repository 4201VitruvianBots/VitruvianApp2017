using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class ColorButton:Button
	{
		public bool on = false;

		public ColorButton(string title) {
			HorizontalOptions = LayoutOptions.CenterAndExpand;
			VerticalOptions = LayoutOptions.Center;

			Text = title;
			BackgroundColor = Color.Red;

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
