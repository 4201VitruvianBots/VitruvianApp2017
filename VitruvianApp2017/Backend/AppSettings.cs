using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

namespace VitruvianApp2017
{
	public class AppSettings
	{

		public static void SaveSettings(String code, string value)
		{
			var settings = Forms.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
			var edit = settings.Edit();
			edit.PutString(code, value);
			edit.Commit();
		}

		public static string RetrieveSettings(String code)
		{
			var settings = Forms.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
			var data = settings.GetString(code, null);
			return data;
		}

		public static async Task<string> getRegionalPointer() {
			var db = new FirebaseClient(GlobalVariables.firebaseURL);
			string pointer = string.Empty;
			var task = db
						.Child("regionalPointer")
						.OnceSingleAsync<string>()
						.ContinueWith((arg) => {
							pointer = arg.Result;
							Console.WriteLine("Pointer: " + pointer);
							GlobalVariables.regionalPointer = pointer;
						});
			task.Wait();

			return pointer;
		}
	}
}
