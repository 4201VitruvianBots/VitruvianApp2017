using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public static class GlobalVariables
	{
		public static string regionalPointer = "2017calb";
		public static string firebaseURL = "https://vitruvianapptest.firebaseio.com/";

		public static double sizeTitle = Device.GetNamedSize(NamedSize.Large, typeof(Label));
		public static double sizeMedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
		public static double sizeSmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));

		//public static string [] parseStrings = { };
	}
}
