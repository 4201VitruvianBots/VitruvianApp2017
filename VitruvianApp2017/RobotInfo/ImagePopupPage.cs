using System;
using System.IO;
using Xamarin.Media;
using Xamarin.Forms;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using FFImageLoading.Forms;

namespace VitruvianApp2017
{
	public class ImagePopupPage:PopupPage
	{
		MediaFile robotImageFile;
		CachedImage robotImage = new CachedImage ();
		Button[] btnArray;
		TeamData team;

		Grid layoutGrid = new Grid (){													// Creates a grid to organize how the page is arranged
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,

			ColumnDefinitions = {
				new ColumnDefinition{ Width = GridLength.Auto },
				new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },	
			},
			RowDefinitions = {		
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },		// robotImage
				new RowDefinition{ Height = GridLength.Auto },							// backBtn
			}
		};
		public ImagePopupPage(CachedImage robotImage) : this(robotImage, null)								// Takes an image input to be displayed as a popup
		{
			
		}

		public ImagePopupPage (CachedImage robotImage, TeamData data)
		{
			team = data;
			robotImage.Aspect = Aspect.Fill;

			if (team != null)
			{
				Button retakeImageBtn = new Button()
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Text = "Retake Image",
					TextColor = Color.Green,
					BackgroundColor = Color.Black
				};
				retakeImageBtn.Clicked += (object sender, EventArgs e) =>
				{
					OpenImagePicker();
				};

				btnArray = new Button[] { retakeImageBtn };
			}
			var navigationBtns = new PopupNavigationButtons(true, btnArray);

			layoutGrid.Children.Add(robotImage, 0, 2, 0, 1);
			layoutGrid.Children.Add(navigationBtns, 0, 2, 1, 2);

			this.Content = layoutGrid;
		}

		async Task OpenImagePicker()
		{
			ImageCapture.capture(team);
			await Navigation.PopPopupAsync();
		}
	}
}

