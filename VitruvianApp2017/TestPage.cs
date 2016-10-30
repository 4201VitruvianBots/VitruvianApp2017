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

			RowDefinitions ={
					new RowDefinition {Height = GridLength.Auto},
					new RowDefinition {Height = GridLength.Auto},
					new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
					new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
					new RowDefinition {Height = GridLength.Auto}
				},

		};

		ActivityIndicator busyIcon = new ActivityIndicator();

		public TestPage()
		{
			var navigationBtns = new NavigationButtons();

			layoutTest.Children.Add(new Image()
			{
				Source = "Placeholder_image_placeholder.png"
			}, 0, 1, 0, 2);
			layoutTest.Children.Add(new Label() { Text = "test1" }, 1, 0);
			layoutTest.Children.Add(new Label() { Text = "test2" }, 1, 1);
			layoutTest.Children.Add(new Label() { Text = "test3" }, 0, 2, 2, 3);
			layoutTest.Children.Add(new Label() { Text = "test4" }, 0, 2, 3, 4);
			layoutTest.Children.Add(navigationBtns, 0, 2, 4, 5);

			Content = layoutTest;
		}
	}
}
