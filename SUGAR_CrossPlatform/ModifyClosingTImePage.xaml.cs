using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class ModifyClosingTImePage : ContentPage
    {
		private TimeUnit selectedClosingTime;
		private DayOfWeek selectedDay;
		private Button parentButton;

		public ModifyClosingTImePage(DayOfWeek day, Button sender)
        {
            InitializeComponent();

			ClosingTimeDescription.Text = GetDayString(day);
			selectedDay = day;
			parentButton = sender;
			ClosingTimeManager mgr = new ClosingTimeManager();
			TimeUnit oldTime = mgr.GetClosingTime(day);
			if(oldTime != null) {
				ClosingTimePicker.Time = new TimeSpan(oldTime.Hour, oldTime.Minute, 0);
			}
        }

		public void SaveClosingTime(object sender, EventArgs args) {
			selectedClosingTime = new TimeUnit(ClosingTimePicker.Time.Hours, ClosingTimePicker.Time.Minutes);
			ClosingTimeManager mgr = new ClosingTimeManager();
			mgr.SaveAndScheduleClosingTime(selectedDay, selectedClosingTime);
			parentButton.Text = selectedClosingTime.ToString();
			Navigation.PopAsync();
		}

		public void RemoveClosingTime(object sender, EventArgs args) {
			ClosingTimeManager mgr = new ClosingTimeManager();
			mgr.RemoveAndCancelClosingTime(selectedDay);
			parentButton.Text = "";
			Navigation.PopAsync();
		}

		public void Abort(object sender, EventArgs args) {
			Navigation.PopAsync();
		}

		private string GetDayString(DayOfWeek day) {
			switch(day) {
				case DayOfWeek.Monday: return "Montag";
				case DayOfWeek.Tuesday: return "Dienstag";
				case DayOfWeek.Wednesday: return "Mittwoch";
				case DayOfWeek.Thursday: return "Donnerstag";
				case DayOfWeek.Friday: return "Freitag";
				case DayOfWeek.Saturday: return "Samstag";
				case DayOfWeek.Sunday: return "Sonntag";
				default: return "";
			}
		}
    }
}
