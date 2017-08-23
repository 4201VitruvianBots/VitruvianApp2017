using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using TheBlueAlliance;

namespace VitruvianApp2017
{
	public class MatchScheduleIndexPage : ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator();
		MatchHeaderLists lists;

		public MatchScheduleIndexPage() {
			Title = "Match List";

			lists = new MatchHeaderLists();

			UpdateMatchSchedule();

			var navigationBtns = new NavigationButtons(true);
			navigationBtns.refreshBtn.Clicked += (object sender, EventArgs e) => {
				UpdateMatchSchedule();
			};

			this.Appearing += (object sender, EventArgs e) => {
				UpdateMatchSchedule();
			};

			this.Content = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.End,

				Children = {
					busyIcon,
					new StackLayout(){
						VerticalOptions = LayoutOptions.Start,

						Children = {
							lists
						}
					},
					new StackLayout(){
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.End,

						Children = {
							navigationBtns
						}
					}
				}
			};
			BackgroundColor = Color.White;
		}

		public async Task UpdateMatchSchedule() {
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;

			lists.updateMatchSchedule();

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}
