using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class SingleCounter:Grid
	{
		int i;

		public SingleCounter(String title, int counter)
		{
			i = counter;

			var titleLbl = new Label()
			{
				Text = title,
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center
			};

			var valueLbl = new Label()
			{
				Text = i.ToString(),
				HorizontalTextAlignment = TextAlignment.Center
			};

			var decrement = new Button()
			{
				Text = "-",
				BackgroundColor = Color.Red,
			};
			decrement.Clicked += (object sender, EventArgs e) =>
			{
				if (i > 0)
				{
					i--;
					valueLbl.Text = i.ToString();
				}
			};

			var increment = new Button()
			{
				Text = "+",
				BackgroundColor = Color.Green,
			};
			increment.Clicked += (object sender, EventArgs e) =>
			{
				i++;
				valueLbl.Text = i.ToString();
			};

			this.Children.Add(titleLbl, 0, 3, 0, 1);
			this.Children.Add(decrement, 0, 1);
			this.Children.Add(valueLbl, 1, 1);
			this.Children.Add(increment, 2, 1);
		}

		public int value()
		{
			return i;
		}
	}
}
