using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Media;
using Firebase.Storage;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
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
		bool link;
		ActivityIndicator busyIcon;

		public RobotImageLayout(TeamData data)
		{
			busyIcon = new ActivityIndicator()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			robotImage = new CachedImage() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 120,
				//WidthRequest = 120,
				DownsampleToViewSize = true,
				Aspect = Aspect.AspectFit,
				LoadingPlaceholder = "Loading_image_placeholder.png",
				//ErrorPlaceholder = "Placeholder_image_placeholder.png"
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
				imageURL = data.imageURL;

				robotImage.Source = new Uri(imageURL);

				robotImage.Success += (sender, ea) => {
					Console.WriteLine("Image source: " + robotImage.Source);

					robotImageFull.Source = robotImage.Source;

					tap.Tapped += (s, e) => {
						// Create a gesture recognizer to display the popup image
						Navigation.PushPopupAsync(new ImagePopupPage(data));
					};
					GestureRecognizers.Add(tap);
					busyIcon.IsRunning = false;
					busyIcon.IsVisible = false;

					Console.WriteLine(robotImage.Source);
				};
			}
			catch(Exception ex) {
				robotImage.Source = "Placeholder_image_placeholder.png";
				Console.WriteLine("Error: " + ex.Message);

				tap.Tapped += (object sender, EventArgs e) =>
				{
					Navigation.PushPopupAsync(new ImagePopupPage(data));
				};
				GestureRecognizers.Add(tap);
				busyIcon.IsRunning = false;
				busyIcon.IsVisible = false;
			}
			if(imageURL == null)
				robotImage.Source = "Placeholder_image_placeholder.png";
		}
	}
}
