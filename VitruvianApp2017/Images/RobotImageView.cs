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
	public class RobotImageView:ContentView
	{
		public CachedImage robotImage;
		TapGestureRecognizer tap = new TapGestureRecognizer();
		StackLayout loadingStack, errorStack, placeholderStack;
		Label errorMessageLbl;
		Button retryBtn;
		string imageURL;
		bool link;
		ActivityIndicator busyIcon;

		public RobotImageView(TeamData data)
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;

			busyIcon = new ActivityIndicator() {
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			tap = new TapGestureRecognizer();
			tap.Tapped += (s, e) => {
				// Create a gesture recognizer to display the popup image
				Navigation.PushPopupAsync(new ImagePopupPage(data));
			};

			robotImage = new CachedImage() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 120,
				//WidthRequest = 120,
				DownsampleToViewSize = true,
				Aspect = Aspect.AspectFit,
				CacheDuration = new TimeSpan(7, 0, 0, 0),
			};
			robotImage.GestureRecognizers.Add(tap);

			loadingStack = new StackLayout() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 120, WidthRequest = 120,
				BackgroundColor = Color.Green,

				Children = {
					busyIcon,
					new Label(){
						Text = "Loading Image...",
						HorizontalTextAlignment = TextAlignment.Center
					}
				}
			};
			/*
			placeholderStack = new StackLayout() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 120, WidthRequest = 120,
				BackgroundColor = Color.Green,

				Children = {
					busyIcon,
					new Label(){
						Text = "[Placeholder Image]",
						HorizontalTextAlignment = TextAlignment.Center
					}
				}
			};
			placeholderStack.GestureRecognizers.Add(tap);
			*/
			errorMessageLbl = new Label() {
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = GlobalVariables.sizeTiny
			};

			retryBtn = new Button() {
				Text = "Retry",
			};
			retryBtn.Clicked += (sender, e) => {
				addRobotImage(data);
			};

			errorStack = new StackLayout() {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 120, WidthRequest = 120,
				BackgroundColor = Color.Yellow,

				Children = {
					new Label(){
						Text = "Error:",
						HorizontalTextAlignment = TextAlignment.Center
					},
					errorMessageLbl,
					retryBtn
				}
			};

			addRobotImage(data);
		}

		public async Task addRobotImage(TeamData data)
		{
			if (data.imageURL != null) {
				try {
					busyIcon.IsRunning = true;
					busyIcon.IsVisible = true;

					imageURL = data.imageURL;

					robotImage.Source = new Uri(imageURL);

					robotImage.Success += (sender, ea) => {
						Console.WriteLine("Image source: " + robotImage.Source);
						busyIcon.IsRunning = false;
						busyIcon.IsVisible = false;

						Console.WriteLine(robotImage.Source);
						robotImage.GestureRecognizers.Add(tap);
					};
					Content = robotImage;
				} catch (Exception ex) {
					Console.WriteLine("Error: " + ex.Message);
					busyIcon.IsRunning = false;
					busyIcon.IsVisible = false;

					errorMessageLbl.Text = ex.Message;
					
					Content = errorStack;
				}
			} else {
				robotImage.Source = "placeholder_image_placeholder.png";
				Content = robotImage;
			}
		}
	}
}
