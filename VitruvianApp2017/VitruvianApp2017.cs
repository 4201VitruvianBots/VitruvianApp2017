using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class App : Application
	{
		public App()
		{
			//ParseClient.Initialize("efyheG3BwD7TagqRBxCvq377InnwhThDxrzo5iNS","znyJSWdN3xaV3ifTZLjSrNCy10vkJzkm3sv0v25Q");

			FBApp.Initialize();
			getPtr();

			MainPage = new MainMenuPage();
		}

		void getPtr() {
			Console.WriteLine("Get Data");
			var task = Task.Factory.StartNew(() => AppSettings.getRegionalPointer());
			task.Wait();
			Console.WriteLine("Retrieved Data");
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
