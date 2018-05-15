using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class CreateProfilePage : ContentPage
	{
		private Profile TemporaryProfile;      

		public CreateProfilePage()
		{
			TemporaryProfile = new Profile();

			InitializeComponent();

			ActivationPanel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			ActivationPanel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			ActivationPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

			var selectionStyle = new Style(typeof(Button))
			{
				Setters = 
				{
					new Setter { Property = Button.TextColorProperty, Value=Color.Black},
					new Setter { Property = Button.BorderColorProperty, Value = Color.Black },
					new Setter { Property = Button.BorderWidthProperty , Value = 1 },               
					new Setter { Property = Button.BorderRadiusProperty, Value = 0 }
				}
			};
            
			var selectMonday = new Button { Text="MO" , Style=selectionStyle};
			var selectTuesday = new Button { Text="DI", Style = selectionStyle };
			var selectWednesday = new Button { Text="MI", Style = selectionStyle };
			var selectThursday = new Button { Text="DO", Style = selectionStyle };
			var selectFriday = new Button { Text="FR", Style = selectionStyle };
			var selectSaturday = new Button { Text="SA", Style = selectionStyle };
			var selectSunday = new Button { Text="SO", Style = selectionStyle};
			var activateMonday = new Button();
			var activateTuesday = new Button();
			var activateWednesday = new Button();
			var activateThursday = new Button();
			var activateFriday = new Button();
			var activateSaturday = new Button();
			var activateSunday = new Button();
			Button[] selectionRow = { selectMonday, selectTuesday, selectWednesday, selectThursday, selectFriday, selectSaturday, selectSunday };
			Button[] activationRow = { activateMonday,activateTuesday,activateWednesday,activateThursday,activateFriday,activateSaturday,activateSunday };

			for (int currSelectionColumn = 0; currSelectionColumn < selectionRow.Length;currSelectionColumn++)
			{
				int selectedColumn = currSelectionColumn;
				selectionRow[currSelectionColumn].Clicked += (sender, e) =>
				{
					selectionRow[selectedColumn].BackgroundColor = Color.Orange;
					System.Console.WriteLine("You have clicked the " + selectedColumn + " column!");
					String start = TemporaryProfile.StartTimes[selectedColumn].ToString();
					String end = TemporaryProfile.EndTimes[selectedColumn].ToString();
					System.Console.WriteLine("{" + start + "," + end + "}");
					StartTime.Text = "VON: " + TemporaryProfile.StartTimes[selectedColumn].ToString();
					EndTime.Text = "BIS: " + TemporaryProfile.EndTimes[selectedColumn].ToString();
					StartTime.IsVisible = true;
					EndTime.IsVisible = true;
				};
			}

			ActivationPanel.Children.Add(selectMonday, 0, 0);
			ActivationPanel.Children.Add(selectTuesday, 1, 0);
			ActivationPanel.Children.Add(selectWednesday, 2, 0);
			ActivationPanel.Children.Add(selectThursday, 3, 0);
			ActivationPanel.Children.Add(selectFriday, 4, 0);
			ActivationPanel.Children.Add(selectSaturday, 5, 0);
			ActivationPanel.Children.Add(selectSunday, 6, 0);
        }

		public void selectWeekDay(int weekDay)
        {
			
        }
    }

   
}
