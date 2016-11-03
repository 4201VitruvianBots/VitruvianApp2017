using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class CollapsibleListContainer: StackLayout
	{
		double totalHeight;
		public CollapsibleListContainer() {
			totalHeight = Height;

			HorizontalOptions = LayoutOptions.Fill;
			Spacing = 0;
			//BackgroundColor = Color.Green;
		}

		public void AddList(CollapsibleList list) {
			Children.Add(list);
			list.tHeight = totalHeight;
		}
	}
}
