using System;
using Android.Net;
using Xamarin.Forms;
             
namespace VitruvianApp2017
{
	public class CheckInternetConnectivity:ContentPage
	{
		public CheckInternetConnectivity()
		{
		}

		public bool InternetStatus()
		{
			ConnectivityManager test = (ConnectivityManager)Android.App.Application.Context.GetSystemService(
										   Android.App.Activity.ConnectivityService);
			NetworkInfo test2 = test.ActiveNetworkInfo;
			if (test2 != null && test2.IsConnected)
				return true;
			else {
				DisplayAlert("Error:", "No Internet Connection", "OK");
				return false;
			}
		}
	}
}
