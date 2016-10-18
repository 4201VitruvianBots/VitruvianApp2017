using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Media;
using Firebase.Xamarin;
using Firebase.Xamarin.Database;
using FFImageLoading.Forms;

namespace VitruvianApp2017
{
	public class ImageCapture
	{
		MediaFile robotImageFile;
		CachedImage robotImage = new CachedImage();

		public static void capture(TeamData data)
		{
			ImagePicker(data);
		}

		async Task ImagePicker(TeamData data)
		{
			//It works? Don't use gallery
			var robotImagePicker = new MediaPicker(Forms.Context);

			await robotImagePicker.TakePhotoAsync(new StoreCameraMediaOptions
			{
				Name = data.teamNumber.ToString() + ".jpg",
				Directory = "Robot Images"
			}).ContinueWith(t =>
			{
				robotImageFile = t.Result;
				Console.WriteLine("Robot Image Path: " + robotImageFile.Path);
			}, TaskScheduler.FromCurrentSynchronizationContext());
			robotImage.Source = robotImageFile.Path;
			try
			{
				ParseFile image = new ParseFile(data["teamNumber"].ToString() + ".jpg", ImageToBinary(robotImageFile.Path));

				data["robotImage"] = image;
				await data.SaveAsync();
			}
			catch
			{
				Console.WriteLine("Image Save Error");
			}
		}

		public byte[] ImageToBinary(string imagePath)
		{
			FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[fileStream.Length];
			fileStream.Read(buffer, 0, (int)fileStream.Length);
			fileStream.Close();
			return buffer;
		}
	}
}
