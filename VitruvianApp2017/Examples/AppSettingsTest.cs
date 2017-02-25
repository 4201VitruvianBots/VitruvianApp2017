using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class TestPage:ContentPage
	{
		public TestPage()
		{
			var test = new AppSettings();
			String output;
			LineEntry input = new LineEntry("Input Setting");

			Button save = new Button()
			{
				Text = "Save"
			};
			save.Clicked += (sender, e) =>
			{
				AppSettings.SaveSettings("Test", input.data);
			};

			Button recall = new Button()
			{
				Text = "Recall"
			};
			recall.Clicked += (sender, e) =>
			{
				output = AppSettings.RetrieveSettings("Test");
				DisplayAlert("Output", output, "OK");
			};

			Content = new StackLayout()
			{
				Children = {
					input,
					save,
					recall
				}
			};
		}
	}
}
