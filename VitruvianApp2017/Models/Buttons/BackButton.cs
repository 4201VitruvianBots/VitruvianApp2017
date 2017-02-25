using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class BackButton:Button
	{
		public BackButton()
		{
			Text = "Back";
			TextColor = Color.Green;
			BackgroundColor = Color.Black;

			Clicked += (object sender, EventArgs e) =>
			{
				
			};
		}
	}
}
