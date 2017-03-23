using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public static class GlobalVariables {
		// Modifiable per competition/year
		public static string regionalPointer { get; set; } = "NULL";
		public static string firebaseApplicationID = "1:222280551868:android:051a820459657e45\n";
		public static string firebaseAPIKey = "AIzaSyDeNEa_Dgr7AfDfUyBWX6D5Le80H9t26es";
		public static string firebaseURL = "x";
		public static string firebaseStorageURL = "vitruvianapptest.appspot.com";

		// Don't modify these
		public static double sizeTitle = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 1.5;
		public static double sizeMedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) * 1.5;
		public static double sizeSmall = Device.GetNamedSize(NamedSize.Small, typeof(Label)) * 1.5;
		public static double sizeTiny = Device.GetNamedSize(NamedSize.Small, typeof(Label));
		//public static Page rootPage { get; set; }

		//public static string [] parseStrings = { };
	}
}
