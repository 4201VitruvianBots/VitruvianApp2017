using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class TitledColorButton:StackLayout
	{
		public event PropertyChangingEventHandler ValueChanged;
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
			colorBtn.Clicked += (sender, e) => {
				this.OnPropertyChanged();
			};
			Children.Add(titleLbl);
			Children.Add(colorBtn);
		}

		public bool getBtnStatus() {
			return colorBtn.on;
		}

		protected void OnValueChanged(string i) {
			PropertyChangingEventHandler c = this.ValueChanged;

			if (c != null) {
				c(this, new PropertyChangingEventArgs(i));
			}
		}
	}
}
