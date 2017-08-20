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
	public class MatchListIndexPage : ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator();
		MatchHeaderLists lists;

		public MatchListIndexPage() {
			Title = "Match List";

			lists = new MatchHeaderLists();

			UpdateMatchList();

			var navigationBtns = new NavigationButtons(true);
			navigationBtns.refreshBtn.Clicked += (object sender, EventArgs e) => {
				UpdateMatchList();
			};

			this.Appearing += (object sender, EventArgs e) => {
				UpdateMatchList();
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

		public async Task UpdateMatchList() {
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;

			lists.updateMatchLists();

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}
