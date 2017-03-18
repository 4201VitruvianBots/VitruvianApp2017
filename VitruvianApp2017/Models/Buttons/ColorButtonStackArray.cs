using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class ColorButtonStackArray:StackLayout
	{
		public event PropertyChangingEventHandler valueChanged;	
		public Button[] btnArray;
		public bool[] on;
		double divisions;
		double halfDivision;
		int buttonCount;

		public ColorButtonStackArray(string title, int buttons) {
			HorizontalOptions = LayoutOptions.StartAndExpand;
			Spacing = 5;
			btnArray = new Button[buttons];
			on = new bool[buttons];
			buttonCount = buttons;
			divisions = 1 / Convert.ToDouble(buttons);
			halfDivision = (divisions / 2);

			Console.WriteLine("Divisions: " + divisions);

			Label titleLbl = new Label() {
				Text = title,
				TextColor = Color.Black,
				FontAttributes = FontAttributes.Bold,
				FontSize = GlobalVariables.sizeSmall,
				HorizontalTextAlignment = TextAlignment.Center
			};

			for (int i = 0; i < buttons; i++) {
				btnArray[i] = new Button() {
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					Text = string.Format("{0:0%} - {1:0%}", i * divisions, i * divisions + divisions),
					BackgroundColor = Color.Red
				};
			}

			foreach (var btn in btnArray)
				btn.Clicked += (sender, e) => {
					setButtonBackground(btn);
					this.OnValueChanged("t");
				};

			Children.Add(titleLbl);
			foreach (var btn in btnArray)
				Children.Add(btn);
		}

		public void setAllFalse(){
			setButtonBackground(null);
		}

		void setButtonBackground(Button btn) {
			//Console.WriteLine("Button: " + index);
			for (int i = 0; i < buttonCount; i++) {
				if (btnArray[i] != btn){
					btnArray[i].BackgroundColor = Color.Red;
					on[i] = false;
				} else {
					btnArray[i].BackgroundColor = Color.Green;
					on[i] = true;
				}
			}
		}

		public double getAvgPercentage() {
			int index = 0;
			for (int i = 0; i < buttonCount; i++)
				if (on[i])
					index = i;

			return index * divisions + halfDivision;
		}

		protected void OnValueChanged(string i) {
			PropertyChangingEventHandler c = this.valueChanged;

			if (c != null) {
				c(this, new PropertyChangingEventArgs(i));
			}
		}
	}
}
