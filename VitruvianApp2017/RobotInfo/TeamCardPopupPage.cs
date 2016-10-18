using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using FFImageLoading.Forms;

namespace VitruvianApp2017
{
	public class TeamCardPopupPage:PopupPage
	{
		Grid teamGrid = new Grid()
		{
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center,
			Margin = new Thickness(0, 0, 0, 0),

			RowDefinitions ={
				new RowDefinition {Height = GridLength.Auto},
				new RowDefinition {Height = GridLength.Auto},
				new RowDefinition {Height = GridLength.Star},
				new RowDefinition {Height = GridLength.Star},
				new RowDefinition {Height = GridLength.Auto}
			},
			ColumnDefinitions = {
				new ColumnDefinition {Width = GridLength.Auto},
				new ColumnDefinition {Width = GridLength.Star}
			}
		};

		Image robotImage = new Image();
		TeamData data;

		public TeamCardPopupPage(TeamData team)
		{
			data = team;
			addRobotImage();

			var teamNo = new Label()
			{
				Text = data.teamNumber.ToString(),
				FontSize = 22,
				FontAttributes = FontAttributes.Bold
			};

			var teamNa = new Label()
			{
				Text = data.teamName,
				FontSize = 18
			};

			Button editTeamBtn = new Button()
			{
				Text = "Edit Info"
			};
			editTeamBtn.Clicked += (object sender, EventArgs e) =>
			{
				editTeam();
			};

			Button[] btnArray = { editTeamBtn };
			var navigationBtns = new PopupNavigationButtons(true,btnArray);
			navigationBtns.backBtn.Clicked += (object sender, EventArgs e) =>
			{
				popUpReturn();
			};



			//teamGrid.Children.Add(robotImage, 0, 1, 0, 2); //robotImage
			teamGrid.Children.Add(teamNo, 1, 0);
			teamGrid.Children.Add(teamNa, 1, 1);
			//teamGrid.Children.Add(0, 2, 2, 3); //
			//teamGrid.Children.Add(0, 2, 3, 4); //
			teamGrid.Children.Add(navigationBtns, 0, 2, 4, 5);

			Content = new Frame()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Margin = new Thickness(5, 5, 5, 5),
				BackgroundColor = Color.Gray,
				HasShadow = true,
				//Padding = new Thickness(1, 1, 1, 1),
				//Margin = new Thickness(50, 50, 50, 50),

				Content = teamGrid
			};
		}

		void editTeam()
		{
			//Navigation.PushPopupAsync(new editTeamInfoPage());
		}

		async void popUpReturn()
		{
			//await Task.Yield();
			await Navigation.PopPopupAsync();
		}

		void addRobotImage()
		{
			try
			{
				CachedImage robotImage;
				CachedImage robotImageFull;

				if (data.imageURL != null)
				{
					//ParseFile robotImageURL = (ParseFile)data["robotImage"];
					//Gets the image from parse and converts it to ParseFile
					robotImage = new CachedImage()
					{
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.Center,
						Source = new UriImageSource
						{
							Uri = new Uri(data.imageURL),
							CachingEnabled = true,
							CacheValidity = new TimeSpan(7, 0, 0, 0) //Caches Images onto your device for a week
						},
						HeightRequest = 120,
						DownsampleToViewSize = true
					};
					robotImageFull = new CachedImage()
					{
						Source = new UriImageSource
						{
							Uri = new Uri(data.imageURL),
							CachingEnabled = true,
							CacheValidity = new TimeSpan(7, 0, 0, 0) //Caches Images onto your device for a week
						}
					};

					TapGestureRecognizer tap = new TapGestureRecognizer();
					tap.Tapped += (object sender, EventArgs e) =>
					{
						// Create a gesture recognizer to display the popup image
						popUpPage(robotImageFull);
					};
					robotImage.GestureRecognizers.Add(tap);
					robotImage.Aspect = Aspect.AspectFit;
					teamGrid.Children.Add(robotImage, 0, 1, 0, 2);
				}
				else { }
			}
			catch
			{
				Image robotImage = new Image();
				robotImage.Source = "Placeholder_image_placeholder.png";
				robotImage.Aspect = Aspect.AspectFit;
				teamGrid.Children.Add(robotImage, 0, 1, 0, 2);
			}
			//Need better way to scale an image while keeping aspect ratio, but not overflowing everything else
			//robotImage.GestureRecognizers.Add (imageTap);
		}

		async void popUpPage(CachedImage rImage)
		{
			//await Task.Yield();
			await Navigation.PushPopupAsync(new ImagePopupPage(rImage));
		}
	}
}
