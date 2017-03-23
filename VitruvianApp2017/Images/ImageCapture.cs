using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Media;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Firebase;
using Firebase.Storage;

namespace VitruvianApp2017
{
	public class ImageCapture
	{
		public static async Task ImagePicker(TeamData data, int imageInt) {
			MediaFile robotImageFile;
			String filePath = null;
			string fileName = null;
			//It works? Don't use gallery
			var robotImagePicker = new MediaPicker(Forms.Context);
			/*
			var intent = robotImagePicker.GetTakePhotoUI(new StoreCameraMediaOptions {
				Name = data.teamNumber.ToString() + ".jpg",
				Directory = "Robot Images"
			}).getre.ContinueWith(t => {
				robotImageFile = t.Result;
				filePath = robotImageFile.Path;
				Console.WriteLine("Robot Image Path: " + filePath);
			}, TaskScheduler.FromCurrentSynchronizationContext());
			//*/
			await robotImagePicker.TakePhotoAsync(new StoreCameraMediaOptions {
				Name = data.teamNumber + "_IMG" + imageInt + ".jpg",
				Directory = "Robot Images"
			}).ContinueWith(t => {
				fileName = data.teamNumber.ToString() + "_IMG" + imageInt + ".jpg";
				Console.WriteLine("Filename: " + fileName);
				robotImageFile = t.Result;
				filePath = robotImageFile.Path;
				Console.WriteLine("Robot Image Path: " + filePath);
			}, TaskScheduler.FromCurrentSynchronizationContext());

			try {
				fileName = data.teamNumber + "_IMG" + imageInt + ".jpg";
				var stream = new MemoryStream(ImageToBinary(filePath));

				var storage = new FirebaseStorage(GlobalVariables.firebaseStorageURL);

				var send = storage.Child(GlobalVariables.regionalPointer)
				             .Child(data.teamNumber.ToString())
				             .Child(fileName)
				             .PutAsync(stream);

				var downloadURL = await send;

				Console.WriteLine("URL: " + downloadURL);
				/*
				var db = new FirebaseClient(GlobalVariables.firebaseURL);

				var saveURL = db
							.Child(GlobalVariables.regionalPointer)
							.Child("teamData")
							.Child(data.teamNumber.ToString())
							.Child("imageURL")
							.PutAsync(downloadURL);
				*/
			} catch (Exception ex) {
				Console.WriteLine("Image Upload Image: " + ex);
			}
		}

		private static byte[] ImageToBinary(string imagePath) {
			FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[fileStream.Length];
			fileStream.Read(buffer, 0, (int)fileStream.Length);
			fileStream.Close();
			return buffer;
		}

		/*
		static void ImageBackup(StorageReference imageRef, TeamData data) {
			string imageRefName = data.teamNumber.ToString() + ".jpg";
			int counter = 1;

			StorageReference backupImageRef = FBApp.robotImageStorageRefOld.Child(imageRefName);
			while (backupImageRef.Name.Equals(imageRefName)) {
				if (counter < 10)
					imageRefName = data.teamNumber.ToString() + "_0" + counter + ".jpg";
				else
					imageRefName = data.teamNumber.ToString() + "_" + counter + ".jpg";

				backupImageRef = FBApp.robotImageStorageRefOld.Child(imageRefName);
				counter++;
			}
			//var filePath = ;
			//UploadTask upload = backupImageRef.PutBytes(ImageToBinary(filePath));
		}

		public static async Task<string> getImageURL(TeamData data) {
			// Temporary workaround until Xamarin.Firebase.Storage implements an easier way to get image URLs
			FBApp.InitializeFirebaseStorage();
			StorageReference cloudImageRef = FBApp.robotImageStorageRef.Child(data.teamNumber.ToString() + ".jpg");
			var listen = new CompleteListener();
			cloudImageRef.DownloadUrl.AddOnCompleteListener(listen);

			Console.WriteLine("Awaiting Signal");
			await listen.signal.WaitAsync();

			return listen.data;
		}
		*/
	}
}
