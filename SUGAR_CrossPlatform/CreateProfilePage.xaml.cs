using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
	public partial class CreateProfilePage : ContentPage, IOnTimeChangedListener
	{
		private Profile TemporaryProfile;
		private int selectDay;
		private ProfileManager ProfManager;

		public CreateProfilePage(Profile passedProfile)
		{
			TemporaryProfile = new Profile();
			selectDay = 0;
			ProfManager = new ProfileManager();

			InitializeComponent();

			NameLabel.TextChanged += (sender, e) =>
			{
				TemporaryProfile.Name = NameLabel.Text;
				Console.WriteLine(TemporaryProfile.Name);
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
            
			for (int currSelectionColumn = 0; currSelectionColumn < selectionRow.Length;currSelectionColumn++)
			{
				int selectedColumn = currSelectionColumn;
				selectionRow[currSelectionColumn].Clicked += (sender, e) =>
				{
					if (selectDay != -1)
                    {
                        selectionRow[selectDay].BackgroundColor = Color.White;
                    }
					if (TemporaryProfile.Days[selectedColumn])
					{
						selectDay = selectedColumn;
						selectionRow[selectedColumn].BackgroundColor = Color.Orange;
						StartTime.Text = "VON: " + TemporaryProfile.StartTimes[selectedColumn].ToString();
						EndTime.Text = "BIS: " + TemporaryProfile.EndTimes[selectedColumn].ToString();
						StartTime.IsVisible = true;
						EndTime.IsVisible = true;
					} else { 
						selectionRow[selectDay].BackgroundColor = Color.White;
						selectionRow[selectedColumn].BackgroundColor = Color.Orange;
						StartTime.IsVisible = false;
                        EndTime.IsVisible = false;
						selectDay = selectedColumn;
                    }
				};
			}

			for (int currActivationColumn = 0; currActivationColumn < activationRow.Length;currActivationColumn++)
			{
				int selectedColumn = currActivationColumn;
				activationRow[selectedColumn].Clicked += (sender, e) =>
				{
					if (!TemporaryProfile.Days[selectedColumn])
					{
						activationRow[selectedColumn].BackgroundColor = Color.Green;
						if (selectDay == selectedColumn)
						{
							StartTime.Text = "VON: " + TemporaryProfile.StartTimes[selectedColumn].ToString();
							EndTime.Text = "BIS: " + TemporaryProfile.EndTimes[selectedColumn].ToString();
							StartTime.IsVisible = true;
							EndTime.IsVisible = true;
						}
						TemporaryProfile.Days[selectedColumn] = true;
					}
					else if(TemporaryProfile.Days[selectedColumn])
					{
						activationRow[selectedColumn].BackgroundColor = Color.White;
						StartTime.IsVisible = false;
						EndTime.IsVisible = false;
						TemporaryProfile.Days[selectedColumn] = false;
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
            
			Save.Clicked += (sender, e) =>
			{
				TemporaryProfile.Name = (String)NameLabel.Text;
				Console.WriteLine(TemporaryProfile.Name);
				if (TemporaryProfile.Name == "")
				{
					DisplayAlert("Achtung", "Ihr Profil enthält keinen Namen!", "OK");
				}
				ProfManager.SaveProfile(TemporaryProfile);
				Application.Current.MainPage.Navigation.PopAsync();
				Application.Current.MainPage.Navigation.PopAsync();
				Application.Current.MainPage.Navigation.PushAsync(new OverviewPage());
			};

			Cancel.Clicked += (sender, e) =>
			{
				Application.Current.MainPage.Navigation.PopAsync();
				DisplayAlert("Achtung", "Die Profilerstellung wurde abgebrochen!", "OK");
			};
        }

		public void OnTimeChanged() {
			
		}
    }

   
}
