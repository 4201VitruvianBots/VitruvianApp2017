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
				new ColumnDefinition{ Width = GridLength.Star },
			},
			RowDefinitions = {		
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },		// robotImage
				new RowDefinition{ Height = GridLength.Auto },							// backBtn
			}
		};

		public ImagePopupPage(CachedImage robotImage) : this(robotImage, null)			// Takes an image input to be displayed as a popup
		{
			
		}

		public ImagePopupPage (CachedImage robotImage, TeamData data)
		{
			team = data;
			robotImage.Aspect = Aspect.Fill;

			Button retakeImageBtn = new Button()
			{
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Retake Image",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			retakeImageBtn.Clicked += (object sender, EventArgs e) =>
			{
				if(data.imageWrite != false)
					OpenImagePicker();
			};

			btnArray = new Button[] { retakeImageBtn };
			var navigationBtns = new PopupNavigationButtons(true, btnArray);

			layoutGrid.Children.Add(robotImage, 0, 0);
			layoutGrid.Children.Add(navigationBtns, 0, 1);

			Content = new Frame(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(50, 80),
				Padding = new Thickness(5),

				BackgroundColor = Color.Gray,
				HasShadow = true,

				Content = layoutGrid
			};
		}

		async Task OpenImagePicker()
		{
			await ImageCapture.ImagePicker(team);
		}
	}
}

