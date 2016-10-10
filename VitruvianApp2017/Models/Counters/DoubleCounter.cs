using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class DoubleCounter:Grid
	{
		int i, j;

		public DoubleCounter(String mainTitle, String subTitle1, String subTitle2, int counter1, int counter2)
		{
			i = counter1;
			j = counter2;

			var titleLbl = new Label()
			{
				Text = mainTitle,
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center
			};
			var subLbl1 = new Label()
			{
				Text = subTitle1,
				HorizontalTextAlignment = TextAlignment.Center
			};
			var subLbl2 = new Label()
			{
				Text = subTitle2,
				HorizontalTextAlignment = TextAlignment.Center
			};

			var valueLbl1 = new Label()
			{
				Text = i.ToString(),
				HorizontalTextAlignment = TextAlignment.Center
			};

			var valueLbl2 = new Label()
			{
				Text = j.ToString(),
				HorizontalTextAlignment = TextAlignment.Center
			};

			var decrement1 = new Button()
			{
				Text = "-",
				BackgroundColor = Color.Red,
			};
			decrement1.Clicked += (object sender, EventArgs e) =>
			{
				if (i > 0)
				{
					i--;
					valueLbl1.Text = i.ToString();
				}
			};

			var increment1 = new Button()
			{
				Text = "+",
				BackgroundColor = Color.Green,
			};
			increment1.Clicked += (object sender, EventArgs e) =>
			{
				i++;
				valueLbl1.Text = i.ToString();
			};

			var decrement2 = new Button()
			{
				Text = "-",
				BackgroundColor = Color.Red,
			};
			decrement2.Clicked += (object sender, EventArgs e) =>
			{
				if (j > 0)
				{
					j--;
					valueLbl2.Text = j.ToString();
				}
			};

			var increment2 = new Button()
			{
				Text = "+",
				BackgroundColor = Color.Green,
			};
			increment2.Clicked += (object sender, EventArgs e) =>
			{
				j++;
				valueLbl2.Text = j.ToString();
			};

			this.Children.Add(titleLbl, 0, 3, 0, 1);
			this.Children.Add(subLbl1, 0, 3, 1, 2);
			this.Children.Add(decrement1, 0, 2);
			this.Children.Add(valueLbl1, 1, 2);
			this.Children.Add(increment1, 2, 2);
			this.Children.Add(subLbl2, 0, 3, 3, 4);
			this.Children.Add(decrement2, 0, 4);
			this.Children.Add(valueLbl2, 1, 4);
			this.Children.Add(increment2, 2, 4);
		}

		public int value1()
		{
			return i;
		}

		public int value2()
		{
			return j;
		}
	}
}
