using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using FFImageLoading.Forms;
using Xamarin.Forms;
using Xamarin.Media;
using Firebase;
using Firebase.Storage;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using TheBlueAlliance;

namespace VitruvianApp2017
{
	public class TestPage2:ContentPage
	{
		ScrollView teamIndex;
		RobotImageLayout testImage;
		TeamData testdata;
		StackLayout teamStack = new StackLayout()
		{
			Spacing = 1,
			BackgroundColor = Color.Silver
		};

		ActivityIndicator busyIcon = new ActivityIndicator();

		public TestPage2()
		{
			var data = new TeamData()
			{
				teamName = "Test",
				teamNumber = 9998
			};
			testdata = data;
			testImage = new RobotImageLayout(data);

			var test = new Button()
			{
				Text = "test"
			};
			test.Clicked += (sender, e) =>
			{
				takeImage(data);
			};

			var test2 = new Button()
			{
				Text = "Test2"
			};
			test2.Clicked += (sender, e) => {
				//getImageURL(data);
			};

			var test3 = new Button() {
				Text = "Test3"
			};
			test3.Clicked += (sender, e) => {
				getTeams();
			};

			//getImageURL(data);
				
			teamStack.Children.Add(testImage);
			teamStack.Children.Add(test);
			teamStack.Children.Add(test2);
			teamStack.Children.Add(test3);
			Content = teamStack;
		}

		public async Task getTeams() {
			var teamList = await EventsHttp.GetEventTeamsListHttp("2017calb");

			foreach (var team in teamList) {
				Console.WriteLine(team.team_number);
			}

		}

		async Task takeImage(TeamData data) {
			await ImageCapture.ImagePicker(data);
		}

		protected override void OnAppearing() {
			getImage();
			base.OnAppearing();
		}

		async Task getImage() {
			testImage = new RobotImageLayout(testdata);
		}
	}
}
