using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class App : Application
	{
		public App()
		{
			//ParseClient.Initialize("efyheG3BwD7TagqRBxCvq377InnwhThDxrzo5iNS","znyJSWdN3xaV3ifTZLjSrNCy10vkJzkm3sv0v25Q");
			/*
			//Firebase Initialization
			var options = new FirebaseOptions.Builder()
											 .SetApplicationId("1:222280551868:android:051a820459657e45\n")
											 .SetApiKey("AIzaSyDeNEa_Dgr7AfDfUyBWX6D5Le80H9t26es")
											 .SetDatabaseUrl("https://vitruvianapptest.firebaseio.com/")
											 .Build();

			var firebaseApp = FirebaseApp.InitializeApp(Forms.Context, options);
			var database = FirebaseDatabase.GetInstance(firebaseApp);
			database.GetReference("test3").SetValue("Hello world!");
			*/

			MainPage = new MainMenuPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
