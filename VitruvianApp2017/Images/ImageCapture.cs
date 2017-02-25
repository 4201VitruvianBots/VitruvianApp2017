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
		public static async Task ImagePicker(TeamData data) {
			MediaFile robotImageFile;
			String filePath = null;
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
				Name = data.teamNumber.ToString() + ".jpg",
				Directory = "Robot Images"
			}).ContinueWith(t => {
				robotImageFile = t.Result;
				filePath = robotImageFile.Path;
				Console.WriteLine("Robot Image Path: " + filePath);
			}, TaskScheduler.FromCurrentSynchronizationContext());

			try {
				Console.WriteLine("Begin Firebase Initialization");
				FBApp.InitializeFirebaseStorage();

				var imageRefName = data.teamNumber.ToString() + ".jpg";
				StorageReference imageRef = FBApp.robotImageStorageRef.Child(imageRefName);

				Console.WriteLine(imageRef.Path.ToString());

				// Attempt Image Backup -> Not pursuing due to complexity of implementation
				//if (imageRef.Name.Equals(imageRefName))
					//ImageBackup(imageRef, data);

				var db = new FirebaseClient(GlobalVariables.firebaseURL);

				var send = db.Child(GlobalVariables.regionalPointer)
							 .Child("teamData")
				             .Child(data.teamNumber.ToString())
							 .Child("imageWrite")
							 .PutAsync(false);

				UploadTask upload = imageRef.PutBytes(ImageToBinary(filePath));
				
				/*
				Task uploadWait = Task.Factory.StartNew(() => upload.IsSuccessful).ContinueWith(t =>
				{
					Console.WriteLine("Testing Cast");
					//var test = JavaObjectCast.CastToUrl<UploadTask.TaskSnapshot>(upload.Snapshot);
					Console.WriteLine("Download URL: " + imageref.DownloadUrl.ToString());
				});
				// Check if upload is successful?
				//upload.AddOnCompleteListener(new Android.Gms.Tasks.IOnCompleteListener()<UploadTask.TaskSnapshot>{ });
				*/
			} catch (Exception ex) {
				Console.WriteLine("Error: " + ex);
			}
		}

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

		private static byte[] ImageToBinary(string imagePath) {
			FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[fileStream.Length];
			fileStream.Read(buffer, 0, (int)fileStream.Length);
			fileStream.Close();
			return buffer;
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
	}
}
