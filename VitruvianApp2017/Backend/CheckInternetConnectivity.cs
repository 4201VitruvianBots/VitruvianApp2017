using System;
using Plugin.Connectivity;
             
namespace VitruvianApp2017
{
	public class CheckInternetConnectivity
	{
		public static bool InternetStatus()
		{
			return CrossConnectivity.Current.IsConnected;
		}
	}
}
