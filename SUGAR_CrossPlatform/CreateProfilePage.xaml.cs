using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
	public partial class CreateProfilePage : ContentPage, IOnTimeChangedListener
	{
		private Profile CreatedProfile;
		private int selectDay;
		private ProfileManager ProfManager;

		public CreateProfilePage(OverviewPage parentPage)
		{
			CreatedProfile = new Profile();
			selectDay = 0;
			ProfManager = new ProfileManager();
            
			InitializeComponent();

			NameEntry.TextChanged += (sender, e) =>
			{
				CreatedProfile.Name = NameEntry.Text;
				Console.WriteLine(CreatedProfile.Name);
			};

			ActivationPanel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			ActivationPanel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

			var selectionStyle = new Style(typeof(Button))
			{
				Setters = 
				{
					new Setter { Property = Button.TextColorProperty, Value=Color.Black},
					new Setter { Property = Button.BorderColorProperty, Value = Color.Black },
					new Setter { Property = Button.BorderWidthProperty , Value = 1 },               
					new Setter { Property = Button.BorderRadiusProperty, Value = 0 },
					new Setter { Property = Button.FontSizeProperty, Value = 13 }
				}
			};
            
			var selectMonday = new Button { Text="MO" , Style=selectionStyle};
			var selectTuesday = new Button { Text="DI", Style = selectionStyle };
			var selectWednesday = new Button { Text="MI", Style = selectionStyle };
			var selectThursday = new Button { Text="DO", Style = selectionStyle };
			var selectFriday = new Button { Text="FR", Style = selectionStyle };
			var selectSaturday = new Button { Text="SA", Style = selectionStyle };
			var selectSunday = new Button { Text="SO", Style = selectionStyle};
			var activateMonday = new Button() { Style = selectionStyle };
			var activateTuesday = new Button() { Style = selectionStyle };
			var activateWednesday = new Button() { Style = selectionStyle };
			var activateThursday = new Button() { Style = selectionStyle };
			var activateFriday = new Button() { Style = selectionStyle };
			var activateSaturday = new Button() { Style = selectionStyle };
			var activateSunday = new Button() { Style = selectionStyle };
			Button[] selectionRow = { selectMonday, selectTuesday, selectWednesday, selectThursday, selectFriday, selectSaturday, selectSunday };
			Button[] activationRow = { activateMonday,activateTuesday,activateWednesday,activateThursday,activateFriday,activateSaturday,activateSunday };

			for (int currentColumn = 0; currentColumn < activationRow.Length; currentColumn++) {
				if(CreatedProfile.Days[currentColumn]) {
					activationRow[currentColumn].BackgroundColor = Color.Green;
				}
			}

			for (int currSelectionColumn = 0; currSelectionColumn < selectionRow.Length;currSelectionColumn++)
			{
				int selectedColumn = currSelectionColumn;
				selectionRow[currSelectionColumn].Clicked += (sender, e) =>
				{
					if (selectDay != -1)
                    {
                        selectionRow[selectDay].BackgroundColor = Color.White;
                    }
					if (CreatedProfile.Days[selectedColumn])
					{
						selectDay = selectedColumn;
						selectionRow[selectedColumn].BackgroundColor = Color.Orange;
						StartTimeButton.Text = "VON: " + CreatedProfile.StartTimes[selectedColumn].ToString();
						EndTimeButton.Text = "BIS: " + CreatedProfile.EndTimes[selectedColumn].ToString();
						StartTimeButton.IsVisible = true;
						EndTimeButton.IsVisible = true;
					} else { 
						selectionRow[selectDay].BackgroundColor = Color.White;
						selectionRow[selectedColumn].BackgroundColor = Color.Orange;
						StartTimeButton.IsVisible = false;
						EndTimeButton.IsVisible = false;
						selectDay = selectedColumn;
                    }
				};
			}

			for (int currActivationColumn = 0; currActivationColumn < activationRow.Length;currActivationColumn++)
			{
				int selectedColumn = currActivationColumn;
				activationRow[selectedColumn].Clicked += (sender, e) =>
				{
					if (!CreatedProfile.Days[selectedColumn])
					{
						activationRow[selectedColumn].BackgroundColor = Color.Green;
						if (selectDay == selectedColumn)
						{
							StartTimeButton.Text = "VON: " + CreatedProfile.StartTimes[selectedColumn].ToString();
							EndTimeButton.Text = "BIS: " + CreatedProfile.EndTimes[selectedColumn].ToString();
							StartTimeButton.IsVisible = true;
							EndTimeButton.IsVisible = true;
						}
						CreatedProfile.Days[selectedColumn] = true;
					}
					else if(CreatedProfile.Days[selectedColumn])
					{
						activationRow[selectedColumn].BackgroundColor = Color.White;
						if (selectDay == selectedColumn)
						{
							StartTimeButton.IsVisible = false;
							EndTimeButton.IsVisible = false;
						}
						CreatedProfile.Days[selectedColumn] = false;
					}
				};
			}

			ActivationPanel.Children.Add(selectMonday, 0, 0);
			ActivationPanel.Children.Add(selectTuesday, 1, 0);
			ActivationPanel.Children.Add(selectWednesday, 2, 0);
			ActivationPanel.Children.Add(selectThursday, 3, 0);
			ActivationPanel.Children.Add(selectFriday, 4, 0);
			ActivationPanel.Children.Add(selectSaturday, 5, 0);
			ActivationPanel.Children.Add(selectSunday, 6, 0);
			ActivationPanel.Children.Add(activateMonday, 0, 1);
			ActivationPanel.Children.Add(activateTuesday, 1, 1);
			ActivationPanel.Children.Add(activateWednesday, 2, 1);
			ActivationPanel.Children.Add(activateThursday, 3, 1);
			ActivationPanel.Children.Add(activateFriday, 4, 1);
			ActivationPanel.Children.Add(activateSaturday, 5, 1);
			ActivationPanel.Children.Add(activateSunday, 6, 1);
            
			StartTimeButton.Clicked += (sender, args) =>
            {
                Navigation.PushAsync(new ChangeTimePage(selectDay, CreatedProfile.StartTimes[selectDay], true, CreatedProfile, this));
            };

            EndTimeButton.Clicked += (sender, args) =>
            {
                Navigation.PushAsync(new ChangeTimePage(selectDay, CreatedProfile.EndTimes[selectDay], false, CreatedProfile, this));
            };

			Save.Clicked += (sender, e) =>
			{
				Console.WriteLine(CreatedProfile.Name);
				if (CreatedProfile.Name == "")
				{
					DisplayAlert("Achtung", "Bitte geben Sie einen Namen für das Profil ein.", "OK");
					return;
				}
				ProfManager.SaveProfile(CreatedProfile);
                parentPage.UpdateList();
				Navigation.PopAsync();
			};

			Cancel.Clicked += (sender, e) =>
			{
				Application.Current.MainPage.Navigation.PopAsync();
			};
        }

		public void OnTimeChanged() {
			StartTimeButton.Text = "VON: " + CreatedProfile.StartTimes[selectDay];
			EndTimeButton.Text = "BIS: " + CreatedProfile.EndTimes[selectDay];
		}
    }

   
}
