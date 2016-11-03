using System;
using System.IO;
using System.Threading.Tasks;
using Firebase;
using Firebase.Storage;
using Firebase.Xamarin;
using Firebase.Auth;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public static class FBApp	
	{
		static FirebaseApp firebaseApp;
		public static FirebaseStorage storage { get; set; }
		public static StorageReference baseStorageRef { get; set; }
		public static StorageReference robotImageStorageRef { get; set; }
		public static StorageReference robotImageStorageRefOld { get; set; }
		public static StorageReference drawingImageStorageRef { get; set; }
		private static FirebaseAuth auth { get; set; }

		public static void Initialize()
		{
			if (firebaseApp == null)
			{
				var options = new FirebaseOptions.Builder()
              	     .SetApplicationId(GlobalVariables.firebaseApplicationID)	
                     .SetApiKey(GlobalVariables.firebaseAPIKey)
                     .SetDatabaseUrl(GlobalVariables.firebaseURL)
					 .Build();

				firebaseApp = FirebaseApp.InitializeApp(Forms.Context, options);
				//auth = FirebaseAuth.GetInstance(firebaseApp);
				//auth.SignInAnonymously();
					
				InitializeFirebaseStorage();
			}
		}

		public static void InitializeFirebaseStorage()
		{
			storage = FirebaseStorage.GetInstance(firebaseApp);

			baseStorageRef = storage.GetReferenceFromUrl(GlobalVariables.firebaseStorageURL);
			robotImageStorageRef = storage.GetReferenceFromUrl(GlobalVariables.firebaseStorageURL + GlobalVariables.regionalPointer + "/Robot Images/");
			robotImageStorageRefOld = robotImageStorageRef.Child("Old");
		}
	}
}
