using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class ColorButtonStackArray:Grid
	{
		ColorButton[] btnArray;
		double divisions;
		double halfDivision;
		int buttonCount;

		public ColorButtonStackArray(string title, int buttons) {
			btnArray = new ColorButton[buttons];
			buttonCount = buttons;
			divisions = 1 / buttons;
			halfDivision = divisions / 2;


			Label titleLbl = new Label() {
				Text = title,
				TextColor = Color.Black,
				FontAttributes = FontAttributes.Bold,
				FontSize = GlobalVariables.sizeMedium,
				HorizontalTextAlignment = TextAlignment.Center
			};

			for (int i = 0; i < buttons; i++) {
				btnArray[i] = new ColorButton((i * divisions) + "% - " + (i * divisions + divisions) + "%");
				btnArray[i].PropertyChanged += (sender, e) => {
					setButtonBackground(i);
				};
			}

			Children.Add(titleLbl, 0, 1);
			int j = 0;
			foreach (var btn in btnArray)
				Children.Add(btn, 0, j++);
		}

		void setButtonBackground(int index) {
			for (int i = 0; i < buttonCount; i++) {
				if (i != index){
					btnArray[i].BackgroundColor = Color.Red;
					btnArray[i].on = false;
				} else {
					btnArray[i].BackgroundColor = Color.Green;
					btnArray[i].on = true;
				}
			}
		}

		public double getAvgPercentage() {
			int index = 0;
			for (int i = 0; i < buttonCount; i++)
				if (btnArray[i].on)
					index = i;

			return index * divisions + halfDivision;
		}
	}
}
