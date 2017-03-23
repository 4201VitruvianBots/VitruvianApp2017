using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class ScouterNameAutoComplete:StackLayout
	{
		public Entry lineEntry = new Entry();
		ListView list = new ListView();
		List<ScouterName> scouterNames = new List<ScouterName>();
		ScrollView listScroll = new ScrollView();
		public string scouterName;
		double initialHieght;

		public ScouterNameAutoComplete() {
			getNameList();
			initialHieght = Height;

			var entryLbl = new Label() {
				Text = "Scouter Name:",
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold
			};
			lineEntry.FontSize = GlobalVariables.sizeSmall;

			list.ItemTemplate = new DataTemplate(() => {
				var name = new Label();
				name.SetBinding(Label.TextProperty, "scouterName");
				name.FontSize = GlobalVariables.sizeSmall;
				return new ViewCell() {
					View = new StackLayout() {
						Children = {
							name
						}
					}
				};
			});
			list.ItemTapped  += (sender, e) => {
				scouterName = ((ScouterName)list.SelectedItem).scouterName;
				lineEntry.Text = scouterName;
				list.SelectedItem = null;
			};
			list.ItemSelected += (sender, e) => {
				listScroll.IsVisible = false;
				listScroll.IsEnabled = false;
			};
			lineEntry.TextChanged += (sender, e) => {
				if (string.IsNullOrEmpty(lineEntry.Text)) {
					listScroll.IsVisible = false;
					listScroll.IsEnabled = false;
				} else
					autoCompleteOptions();
			};

			listScroll.Content = list;

			var relLayout = new RelativeLayout() { 
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			relLayout.Children.Add(listScroll, Constraint.RelativeToParent((parent) => {
				return parent.X;
			}), Constraint.RelativeToParent((parent) => {
				return parent.Y;
			}), Constraint.RelativeToParent((parent) => {
				return parent.Width;
			}), Constraint.RelativeToParent((parent) => {
				return parent.Height;
			}));

			listScroll.IsVisible = false;
			listScroll.IsEnabled = false;

			Children.Add(entryLbl);
			Children.Add(lineEntry);
			Children.Add(listScroll);
		}

		async Task getNameList() {
			var db = new FirebaseClient(GlobalVariables.firebaseURL);
			var nameList = await db
							.Child("Global")
							.OnceSingleAsync<ScouterName>();
			foreach (var name in nameList.scouterNames) {
				scouterNames.Add(new ScouterName() { scouterName = name });
			}
			list.ItemsSource = scouterNames;
		}

		void autoCompleteOptions() {
			var filtered = new List<ScouterName>();
			int height = 0;

			foreach (var name in scouterNames)
				if (name.scouterName.ToLower().Contains(lineEntry.Text.ToLower())) {
					filtered.Add(name);
					height += 40;
				}
			height = height > 0 ? height += 20 : height = 0;

			list.HeightRequest = height;
			listScroll.HeightRequest = height;
			list.ItemsSource = filtered;
			listScroll.IsVisible = true;
			listScroll.IsEnabled = true;
		}
	}
}
