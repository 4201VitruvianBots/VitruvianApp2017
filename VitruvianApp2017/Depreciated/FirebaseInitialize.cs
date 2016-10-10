using System;
//using Firebase;
//using Firebase.Database;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class FirebaseInitialize
	{
		// Old implementation of Firebase using Xamarin.Firebase
		FirebaseDatabase database;

		public FirebaseInitialize()
		{
			var options = new FirebaseOptions.Builder()
											 .SetApplicationId("1:222280551868:android:051a820459657e45\n")
											 .SetApiKey("AIzaSyDeNEa_Dgr7AfDfUyBWX6D5Le80H9t26es")
			                                 .SetDatabaseUrl("https://vitruvianapptest.firebaseio.com/")
											 .Build();

			var firebaseApp = FirebaseApp.InitializeApp(Forms.Context, options);
			database = FirebaseDatabase.GetInstance(firebaseApp);
		}

		public FirebaseDatabase get()
		{
			return database;
		}
	}
}
