using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class LineEntry:StackLayout
	{
		Label entryLbl = new Label();
		Entry inputEntry = new Entry();
		public string data;

		public LineEntry(String entryTitle) : this(entryTitle, null)
		{
		}

		public LineEntry(String entryTitle, String entryPlaceholder)
		{
			entryLbl.Text = entryTitle;
			inputEntry.Placeholder = entryPlaceholder;
			inputEntry.TextChanged += (object sender, TextChangedEventArgs e) => {
				data = entryLbl.Text;
			};
			Children.Add(entryLbl);
			Children.Add(inputEntry);
		}
	}
}
