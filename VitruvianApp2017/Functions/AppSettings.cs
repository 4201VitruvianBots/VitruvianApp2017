using System;
using Android.Content;
using Xamarin.Forms;

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
			var read = settings.GetString(code, null);

			return read;
		}
	}
}
