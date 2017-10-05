using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Firebase;
using Firebase.Storage;

namespace VitruvianApp2017
{
	public class ImageCapture
	{
		public static async Task ImagePicker(TeamData data, int imageInt) {
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
				Console.WriteLine("No Camera");
			else {
				try {
					
					var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions {
					});

					var fileName = data.teamNumber + "_IMG" + imageInt + ".jpg";
					var filePath = file.Path;

					var stream = new MemoryStream(ImageToBinary(filePath));

					var storage = new FirebaseStorage(GlobalVariables.firebaseStorageURL);

					var send = storage.Child(GlobalVariables.regionalPointer)
								 .Child(data.teamNumber.ToString())
								 .Child(fileName)
								 .PutAsync(stream);

					var downloadURL = await send;

					Console.WriteLine("URL: " + downloadURL);

					var db = new FirebaseClient(GlobalVariables.firebaseURL);

					FirebaseAccess.saveData(db, "teamData/" + data.teamNumber + "/imageURL", downloadURL);
				}
				catch (Exception ex) {
					Console.WriteLine("Image Upload Image: " + ex);
				}
			}
		}
		/*
		public static async Task ImagePicker(TeamData data, int imageInt) {
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
				Console.WriteLine("No Camera");
			else {
				var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions {
					Directory = "RobotImages",
					Name = data.teamNumber +"_IMG" + imageInt+ ".jpg"
				});

				try {
					var fileName = data.teamNumber + "_IMG" + imageInt + ".jpg";
					var filePath = file.AlbumPath;

					var stream = new MemoryStream(ImageToBinary(filePath));

					var storage = new FirebaseStorage(GlobalVariables.firebaseStorageURL);

					var send = storage.Child(GlobalVariables.regionalPointer)
								 .Child(data.teamNumber.ToString())
								 .Child(fileName)
								 .PutAsync(stream);

					var downloadURL = await send;

					Console.WriteLine("URL: " + downloadURL);
				}
				catch (Exception ex) {
					Console.WriteLine("Image Upload Image: " + ex);
				}
			}
		}
		*/
		private static byte[] ImageToBinary(string imagePath) {
			FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[fileStream.Length];
			fileStream.Read(buffer, 0, (int)fileStream.Length);
			fileStream.Close();
			return buffer;
		}
	}
}
