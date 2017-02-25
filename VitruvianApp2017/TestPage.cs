using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

namespace VitruvianApp2017
{
	public class TestPage:ContentPage
	{
		ScrollView teamIndex;
		Grid  layoutTest = new Grid()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			Margin = new Thickness(0, 0, 0, 0),

			RowDefinitions = {
				new RowDefinition(){ Height = GridLength.Star },
				new RowDefinition(){ Height = GridLength.Auto },
			}
		};

		ActivityIndicator busyIcon = new ActivityIndicator();

		public TestPage() {
			var list = new CollapsibleListContainer();

			var group1 = new CollapsibleList("Pit Scouting Data");
			group1.addData(new PitData() { dataHeader = "Data Header", data = "data" });
			group1.addData(new PitData() { dataHeader = "Data Header", data = "data" });
			group1.addData(new PitData() { dataHeader = "Data Header", data = "data" });

			var group2 = new CollapsibleList("Match Scouting Data");
			group2.addData(new PitData() { dataHeader = "Data Header", data = "data" });
			group2.addData(new PitData() { dataHeader = "Data Header", data = "data" });
			group2.addData(new PitData() { dataHeader = "Data Header", data = "data" });


			list.AddList(group1);
			list.AddList(group2);

			var navigationBtns = new NavigationButtons();

			layoutTest.Children.Add(list, 0, 2, 0, 1);
			layoutTest.Children.Add(navigationBtns, 0, 2, 1, 2);

			Content = layoutTest;
		}
	}
}
