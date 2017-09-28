using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class MultiCounter:Grid
	{
		public event PropertyChangingEventHandler ValueChanged;
		int val = 0;

		public MultiCounter(String mainTitle) : this(mainTitle, 0) {

		}

		public MultiCounter(String mainTitle, int value)
		{
			val = value;

			var titleLbl = new Label() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = mainTitle,
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};

			var valueLbl = new Label() {
				Text = val.ToString(),
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = GlobalVariables.sizeMedium
			};

			var decrement1 = new Button() {
				Text = "-1",
				BackgroundColor = Color.Red,
			};
			decrement1.Clicked += (object sender, EventArgs e) => {
				if (val > 0){
					val--;
					valueLbl.Text = val.ToString();
					this.OnPropertyChanged();
				}
			};

			var decrement2 = new Button() {
				Text = "-2",
				BackgroundColor = Color.Red,
			};
			decrement2.Clicked += (object sender, EventArgs e) => {
				if (val > 1) {
					val -= 2;
					valueLbl.Text = val.ToString();
					this.OnPropertyChanged();
				}
			};

			var decrement5 = new Button() {
				Text = "-5",
				BackgroundColor = Color.Red,
			};
			decrement5.Clicked += (object sender, EventArgs e) => {
				if (val > 4) {
					val -= 5;
					valueLbl.Text = val.ToString();
					this.OnPropertyChanged();
				}
			};

			var increment1 = new Button()
			{
				Text = "+1",
				BackgroundColor = Color.Green,
			};
			increment1.Clicked += (object sender, EventArgs e) =>
			{
				val++;
				valueLbl.Text = val.ToString();
				this.OnPropertyChanged();
			};

			var increment2 = new Button()
			{
				Text = "+2",
				BackgroundColor = Color.Green,
			};
			increment2.Clicked += (object sender, EventArgs e) =>
			{
				val += 2;
				valueLbl.Text = val.ToString();
				this.OnPropertyChanged();
			};

			var increment5 = new Button() {
				Text = "+5",
				BackgroundColor = Color.Green,
			};
			increment5.Clicked += (object sender, EventArgs e) => {
				val += 5;
				valueLbl.Text = val.ToString();
				this.OnPropertyChanged();
			};

			this.Children.Add(titleLbl, 0, 2, 0, 1);
			this.Children.Add(valueLbl, 0, 2, 1, 2);
			this.Children.Add(decrement1, 0, 2);
			this.Children.Add(increment1, 1, 2);
			this.Children.Add(decrement2, 0, 3);
			this.Children.Add(increment2, 1, 3);
			this.Children.Add(decrement5, 0, 4);
			this.Children.Add(increment5, 1, 4);
		}

		public int getValue()
		{
			return val;
		}

		protected void OnValueChanged(string i) {
			PropertyChangingEventHandler c = this.ValueChanged;

			if (c != null) {
				c(this, new PropertyChangingEventArgs(i));
			}
		}
	}
}
