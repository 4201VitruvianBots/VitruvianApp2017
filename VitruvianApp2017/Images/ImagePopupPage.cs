using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Media;
using Xamarin.Forms;
using Firebase.Storage;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using FFImageLoading.Forms;

namespace VitruvianApp2017
{
	public class ImagePopupPage:PopupPage
	{
		RobotImageView robotImg;
		CachedImage[] robotImages = new CachedImage[5];
		Frame[] imageFrame = new Frame[5];
		int imageIndex = 0;
		bool[] imageSelect = new bool[5];
		Button[] btnArray;
		TeamData data;

		StackLayout imageStack = new StackLayout() {
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			                                 
			Orientation = StackOrientation.Horizontal,
		};

		public ImagePopupPage (TeamData tData)
		{
			data = tData;
			robotImg = new RobotImageView(tData);


			Button setImageDefaultBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Set Default",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			setImageDefaultBtn.Clicked += (object sender, EventArgs e) => {
				setDefaultImage();
			};


			Button retakeImageBtn = new Button()
			{
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Retake",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			retakeImageBtn.Clicked += (object sender, EventArgs e) =>
			{
				OpenImagePicker();
			};

			btnArray = new Button[] { setImageDefaultBtn, retakeImageBtn };
			var navigationBtns = new PopupNavigationButtons(true, btnArray);
			navigationBtns.refreshBtn.Clicked += (sender, e) => {
				refreshImages();
				robotImg = new RobotImageView(tData);
			};

			Content = new Frame(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = GlobalVariables.popupMargin,
				Padding = new Thickness(5),

				BackgroundColor = Color.Gray,
				HasShadow = true,

				Content = new StackLayout() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,

					Children = {
						robotImg,
						new ScrollView(){
							HorizontalOptions = LayoutOptions.FillAndExpand,
							Orientation = ScrollOrientation.Horizontal,

							Content = imageStack
						},
						navigationBtns
					}
				}
			};
		}

		protected override void OnAppearing() {
			base.OnAppearing();
			var task = Task.Factory.StartNew(() => fillImageStack());
			task.Wait();
		}

		async Task OpenImagePicker()
		{
			await ImageCapture.ImagePicker(data, imageIndex);
			refreshImages();
		}

		async Task fillImageStack() {
			string fileName;

			for (int i = 0; i < 5; i++) {
				robotImages[i] = new CachedImage() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HeightRequest = 100,
					//WidthRequest = 120,
					DownsampleToViewSize = true,
					LoadingPlaceholder	= "Loading_image_placeholder.png",
					ErrorPlaceholder = "Placeholder_image_placeholder.png",
					Aspect = Aspect.AspectFit
				};
				var imageTap = new TapGestureRecognizer();
				imageTap.Tapped += (sender, e) => {
					selectImage((CachedImage)sender);
				};

				robotImages[i].GestureRecognizers.Add(imageTap);
				robotImages[i].Source = "Placeholder_image_placeholder.png";

				imageFrame[i] = new Frame() {
					BackgroundColor = Color.Transparent,
					Padding = new Thickness(5),

					Content = robotImages[i]
				};

				fileName = data.teamNumber + "_IMG" + i + ".jpg";

				getImage(robotImages[i], fileName);
				imageStack.Children.Add(imageFrame[i]);
			}
		}

		async Task refreshImages() {
			string fileName;
			for (int i = 0; i < 5; i++) {
				fileName = data.teamNumber + "_IMG" + i + ".jpg";

				getImage(robotImages[i], fileName);
			}
		}

		async Task getImage(CachedImage img, string fileName) {
			var storage = new FirebaseStorage(GlobalVariables.firebaseStorageURL);

			try {
				var retrieve = storage
								.Child(GlobalVariables.regionalPointer)
								.Child(data.teamNumber.ToString())
								.Child(fileName)
								.GetDownloadUrlAsync().ContinueWith((arg) => {
									img.Source = new Uri(arg.Result);
									//selectImage(img);
								});
			} catch (Exception ex) {
				Console.WriteLine("ImageStack Error: " + ex.Message);
				//img.Source = "Placeholder_image_placeholder.jpg";
			}
		}

		void selectImage(CachedImage img) {
			for (int i = 0; i < 5; i++) {
				if (robotImages[i] == img) {
					imageFrame[i].BackgroundColor = Color.Red;
					imageIndex = i;
					robotImg.robotImage.Source = robotImages[i].Source;
				} else {
					imageFrame[i].BackgroundColor = Color.Transparent;
				}
			}
		}

		async Task setDefaultImage() {
			string fileName =  data.teamNumber + "_IMG" + imageIndex + ".jpg";
			
			var storage = new FirebaseStorage(GlobalVariables.firebaseStorageURL);

			var getURL = await storage.Child(GlobalVariables.regionalPointer)
						 .Child(data.teamNumber.ToString())
						 .Child(fileName)
		                 .GetDownloadUrlAsync();

			/*
			robotImage.Source = new UriImageSource() {
				Uri = new Uri(getURL)
			};
			*/

			var db = new FirebaseClient(GlobalVariables.firebaseURL);

			FirebaseAccess.saveData(db, "teamData/" + data.teamNumber + "/imageURL", getURL);

			await DisplayAlert("Success", "Default Image Changed", "OK");
			await Navigation.PopAllPopupAsync();
		}
	}
}

