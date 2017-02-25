using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Media;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Firebase.Xamarin;

using Firebase;
using Firebase.Storage;
using FFImageLoading;
using FFImageLoading.Forms;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

namespace VitruvianApp2017
{
	public class RobotImageLayout:Grid
	{
		TapGestureRecognizer tap = new TapGestureRecognizer();
		CachedImage robotImageFull = new CachedImage();
		CachedImage robotImage;
		string imageURL;

		ActivityIndicator busyIcon;

		public RobotImageLayout(TeamData data)
		{
			busyIcon = new ActivityIndicator()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			robotImage = new CachedImage()
			{
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 120,
				WidthRequest = 120,
				DownsampleToViewSize = true,
				// ErrorPlaceholder = 
			};

			addRobotImage(data);

			Children.Add(robotImage);
			Children.Add(busyIcon);
		}

		public async void addRobotImage(TeamData data)
		{
			busyIcon.IsRunning = true;
			busyIcon.IsVisible = true;
			try
			{
				imageURL = await ImageCapture.getImageURL(data);

				robotImage.Source = new Uri(imageURL);

				robotImage.Success += (sender, ea) => {
					Console.WriteLine("Image source: " + robotImage.Source);

					robotImage.Aspect = Aspect.AspectFit;
					robotImageFull.Source = robotImage.Source;

					tap.Tapped += (s, e) => {
						// Create a gesture recognizer to display the popup image
						Navigation.PushPopupAsync(new ImagePopupPage(robotImageFull, data));
					};
					GestureRecognizers.Add(tap);
					busyIcon.IsRunning = false;
					busyIcon.IsVisible = false;

					Console.WriteLine(robotImage.Source);
				};
			}
			catch(Exception ex) {
				robotImage.Source = "Placeholder_image_placeholder.png";
				robotImage.Aspect = Aspect.AspectFill;
				Console.WriteLine("Error: " + ex.Message);

				tap.Tapped += (object sender, EventArgs e) =>
				{
					Console.WriteLine("Taking Image...");
					// Create a gesture recognizer to display the popup image
					ImageCapture.ImagePicker(data);
				};
				GestureRecognizers.Add(tap);
				busyIcon.IsRunning = false;
				busyIcon.IsVisible = false;
			}
		}
	}
}
