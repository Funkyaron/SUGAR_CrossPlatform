using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{

	public partial class ClosingTimesPage : ContentPage
	{
		private delegate void ChooseClosingTime(DayOfWeek day, Button sender);

        public ClosingTimesPage()
        {
            InitializeComponent();
            
			ChooseClosingTime handler = ChooseClosingTimeMethod;

			ClosingTimeManager mgr = new ClosingTimeManager();
			TimeUnit[] allClosingTimes = mgr.GetAllClosingTimes();

			MondayTimeButton.Text = allClosingTimes[0]?.ToString();
			TuesdayTimeButton.Text = allClosingTimes[1]?.ToString();
			WednesdayTimeButton.Text = allClosingTimes[2]?.ToString();
			ThursdayTimeButton.Text = allClosingTimes[3]?.ToString();
			FridayTimeButton.Text = allClosingTimes[4]?.ToString();
			SaturdayTimeButton.Text = allClosingTimes[5]?.ToString();
			SundayTimeButton.Text = allClosingTimes[6]?.ToString();

			MondayTimeButton.Clicked += (sender, args) => Navigation.PushAsync(new ModifyClosingTImePage(DayOfWeek.Monday, (Button)sender));
			TuesdayTimeButton.Clicked += (sender, args) => Navigation.PushAsync(new ModifyClosingTImePage(DayOfWeek.Tuesday, (Button)sender));
			WednesdayTimeButton.Clicked += (sender, args) => Navigation.PushAsync(new ModifyClosingTImePage(DayOfWeek.Wednesday, (Button)sender));
			ThursdayTimeButton.Clicked += (sender, args) => Navigation.PushAsync(new ModifyClosingTImePage(DayOfWeek.Thursday, (Button)sender));
			FridayTimeButton.Clicked += (sender, args) => Navigation.PushAsync(new ModifyClosingTImePage(DayOfWeek.Friday, (Button)sender));
			SaturdayTimeButton.Clicked += (sender, args) => Navigation.PushAsync(new ModifyClosingTImePage(DayOfWeek.Saturday, (Button)sender));
			SundayTimeButton.Clicked += (sender, args) => Navigation.PushAsync(new ModifyClosingTImePage(DayOfWeek.Sunday, (Button)sender));
        }
        
		public static void ChooseClosingTimeMethod(DayOfWeek day, Button sender) {
			Application.Current.MainPage.Navigation.PushAsync(new ModifyClosingTImePage(day, sender));
		}
    }
}
