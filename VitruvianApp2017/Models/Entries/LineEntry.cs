using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class LineEntry:StackLayout
	{
		Label entryLbl = new Label();
		public Entry inputEntry = new Entry();
		public string data;

		public LineEntry(String entryTitle) : this(entryTitle, null)
		{
		}

		public LineEntry(String entryTitle, String entryPlaceholder)
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;
			entryLbl.Text = entryTitle;
			entryLbl.FontSize = GlobalVariables.sizeSmall;
			entryLbl.FontAttributes = FontAttributes.Bold;

			inputEntry.Placeholder = entryPlaceholder;
			inputEntry.TextChanged += (object sender, TextChangedEventArgs e) => {
				if (!string.IsNullOrEmpty(inputEntry.Text))
					data = inputEntry.Text;
				else
					data = null;
			};
			Children.Add(entryLbl);
			Children.Add(inputEntry);
		}
	}
}
