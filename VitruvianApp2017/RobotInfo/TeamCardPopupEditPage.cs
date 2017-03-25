using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using FFImageLoading.Forms;
using XLabs.Forms.Controls;

namespace VitruvianApp2017
{
	public class TeamCardPopupEditPage : PopupPage
	{
		Grid topGrid = new Grid() {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			Margin = new Thickness(0, 0, 0, 0),
			Padding = 0,

			RowDefinitions ={
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = GridLength.Star },
			},
			ColumnDefinitions = {
				//new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
				//new ColumnDefinition {Width = GridLength.Auto},
				//new ColumnDefinition {Width = GridLength.Auto}
			}
		};

		TeamData data;

		StackLayout dataLayout = new StackLayout() {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
		};
		int cIndex = 0;
		Entry fuelCapacityEntry;
		CheckBox[] checkBoxes = new CheckBox[10];
		Picker configurationPicker;
		string config;
		Editor notesEditor;

		public TeamCardPopupEditPage(TeamData team) {
			data = team;

			var teamNo = new Label() {
				Text = data.teamNumber.ToString(),
				FontSize = GlobalVariables.sizeMedium,
			};

			var teamNa = new Label() {
				Text = data.teamName,
				FontSize = GlobalVariables.sizeMedium
			};

			initalizeData();

			Button saveDataBtn = new Button() {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Save",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			saveDataBtn.Clicked += (object sender, EventArgs e) => {
				saveData();
			};

			Button[] btnArray = { saveDataBtn };
			var navigationBtns = new PopupNavigationButtons(false, btnArray);

			//teamGrid.Children.Add(new RobotImageLayout(data), 0, 1, gridYIndex, gridYIndex + 2);
			topGrid.Children.Add(teamNo, 1, 0);
			topGrid.Children.Add(teamNa, 1, 1);

			// teamGrid.Children.Add(navigationBtns, 0, 2, gridYIndex, gridYIndex++ + 1);

			CloseWhenBackgroundIsClicked = false;
			Content = new Frame() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(50, 50),
				Padding = new Thickness(5),

				BackgroundColor = Color.Gray,
				HasShadow = true,

				Content = new StackLayout() {

					Children = {
						topGrid,
						new ScrollView(){
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.FillAndExpand,

							Content = dataLayout
						},
						navigationBtns
					}
				}
			};
		}

		async Task saveData() {
			if (CheckInternetConnectivity.InternetStatus()){
				data.volumeConfig = config;
				data.maxFuelCapacity = Convert.ToInt32(fuelCapacityEntry.Text);
				data.gearMechanism = checkBoxes[0].Checked;
				data.fuelLowMechanism = checkBoxes[1].Checked;
				data.fuelHighMechanism = checkBoxes[2].Checked;
				data.climbingMechanism = checkBoxes[3].Checked;
				data.gearGroundIntakeMechanism = checkBoxes[4].Checked;
				data.fuelGroundIntakeMechanism = checkBoxes[5].Checked;

				data.pitAutoCross = checkBoxes[6].Checked;
				data.pitAutoGear = checkBoxes[7].Checked;
				data.pitAutoFuelLow = checkBoxes[8].Checked;
				data.pitAutoFuelHigh = checkBoxes[9].Checked;
				data.notes = notesEditor.Text;

				var db = new FirebaseClient(GlobalVariables.firebaseURL);

				var upload = db.Child(GlobalVariables.regionalPointer)
							   .Child("teamData")
							   .Child(data.teamNumber.ToString())
							   .PutAsync(data);
			
				await DisplayAlert("Success", "Data Saved", "OK").ContinueWith((a) => {
					Navigation.PopAllPopupAsync();	
				});
			}
		}

		async void popUpPage(CachedImage rImage) {
			//await Task.Yield();
			//await Navigation.PushPopupAsync(new ImagePopupPage(rImage));
		}

		void initalizeData() {
			var mechanismsLbl = new Label() {
				Text = "Mechanisms:",
				FontSize = GlobalVariables.sizeMedium,
				TextColor = Color.White,
			};

			var configLbl = new Label() {
				Text = "Volume Configuration:",
				FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.White
			};

			configurationPicker = new Picker(){
				Title = "Select a Configuration",
				Items = {
					"Configuartion A",
					"Configuration B"
				},
			};
			configurationPicker.SelectedIndexChanged += (sender, e) => {
				config = configurationPicker.Items[configurationPicker.SelectedIndex];
			};
			if (data.volumeConfig == "Configuartion A")
				configurationPicker.SelectedIndex = 0;
			else
				configurationPicker.SelectedIndex = 1;

			var fuelCapacityLbl = new Label() {
				Text = "Fuel Capacity:",
				FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.White,
			};

			fuelCapacityEntry = new Entry() {
				Placeholder = "[Enter Fuel Capcity]",
				FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.White,
				Keyboard = Keyboard.Numeric,
				Text = data.maxFuelCapacity.ToString()
			};

			var autoActionsLbl = new Label() {
				Text = "Auto Actions:",
				FontSize = GlobalVariables.sizeMedium,
				TextColor = Color.White,
			};

			var notesEditorLbl = new Label() {
				Text = "Additional Notes:",
				FontSize = GlobalVariables.sizeMedium,
				TextColor = Color.White,
			};

			 notesEditor = new Editor() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 160,
				BackgroundColor = Color.White,
				TextColor = Color.Black,
				Text = data.notes
			};

			dataLayout.Children.Add(mechanismsLbl);
			dataLayout.Children.Add(configLbl);
			dataLayout.Children.Add(configurationPicker);
			dataLayout.Children.Add(fuelCapacityLbl);
			dataLayout.Children.Add(fuelCapacityEntry);
			addCheckBox("Gear Delivery", data.gearMechanism);
			addCheckBox("Fuel Deilvery - Low", data.fuelLowMechanism);
			addCheckBox("Fuel Deilvery - High", data.fuelHighMechanism);
			addCheckBox("Climbing", data.climbingMechanism);
			addCheckBox("Ground Intake - Gears", data.gearGroundIntakeMechanism);
			addCheckBox("Ground Intake - Fuel", data.fuelGroundIntakeMechanism);
			dataLayout.Children.Add(autoActionsLbl);
			addCheckBox("Cross", data.pitAutoCross);
			addCheckBox("Gear Delivery", data.pitAutoGear);
			addCheckBox("Fuel Deilvery - Low", data.pitAutoFuelLow);
			addCheckBox("Fuel Deilvery - High", data.pitAutoFuelHigh);

			dataLayout.Children.Add(notesEditorLbl);
			dataLayout.Children.Add(notesEditor);
		}

		void addCheckBox(string description, bool dataBool) {
			checkBoxes[cIndex] = new CheckBox {
				DefaultText = description,
				FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.White,
				Checked = dataBool
			};
			dataLayout.Children.Add(checkBoxes[cIndex++]);
		}
	}
}
