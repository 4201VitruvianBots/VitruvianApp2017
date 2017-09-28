using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class TitledColorButton:StackLayout
	{
		ColorButton colorBtn;
		Label titleLbl;

		public TitledColorButton(string labelTitle, string buttonTitle) {
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;

			titleLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = labelTitle,
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			colorBtn = new ColorButton(buttonTitle);

			Children.Add(titleLbl);
			Children.Add(colorBtn);
		}

		public bool getBtnStatus() {
			return colorBtn.on;
		}
	}
}
