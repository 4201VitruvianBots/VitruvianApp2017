using System;
using Xamarin.Forms;
using Firebase;

namespace VitruvianApp2017
{
	public static class GlobalVariables
	{
		public static double sizeTitle = Device.GetNamedSize(NamedSize.Large, typeof(Label));
		public static double sizeMedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
		public static double sizeSmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));

		public static FirebaseOptions firebaseSettings;
		//public static string [] parseStrings = { };
	}
}
