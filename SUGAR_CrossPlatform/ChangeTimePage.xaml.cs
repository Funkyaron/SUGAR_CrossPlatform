using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class ChangeTimePage : ContentPage
    {
		private String[] weekDays = { "Montag","Dienstag","Mittwoch","Donnerstag","Freitag","Samstag","Sonntag" };
		private ProfileManager ProfManager;

		private bool OverflowingDayActive;
		private bool LastWeekDay;
		private TimeUnit[] currentDayTimeSpan;
		private TimeUnit[] previousDayTimeSpan;
        
		public ChangeTimePage(int weekDay,TimeUnit oldTime,bool isStartTime,Profile passedProfile, IOnTimeChangedListener listener)
        {
            InitializeComponent();

			ChooseWeekDayTimePicker.Format = "HH:mm";
            
			LastWeekDay = false;
            
            if(isStartTime)
			{
				ChooseWeekDayLabel.Text = "Wann soll das Profil \n am " + weekDays[weekDay] + " starten?";
			} else if (!isStartTime)
			{
				ChooseWeekDayLabel.Text = "Wann soll das Profil am " + weekDays[weekDay] + " enden?";
			}

			ChooseWeekDayTimePicker.Time = new TimeSpan(oldTime.Hour,oldTime.Minute,0);

			ConfirmButton.Clicked += (sender, e) =>
			{
				TimeUnit modifiedTime = new TimeUnit(ChooseWeekDayTimePicker.Time.Hours, ChooseWeekDayTimePicker.Time.Minutes);
				bool isValid = true;
				TimeUnit startTime;
				TimeUnit endTime;
                if(isStartTime)
				{
					startTime = new TimeUnit(modifiedTime);
					endTime = passedProfile.EndTimes[weekDay];
				} else {
					startTime = passedProfile.StartTimes[weekDay];
					endTime = new TimeUnit(modifiedTime);
				}

				TimeUnit nextDayStartTime = new TimeUnit(0, 0);
				TimeUnit previousDayEndTime = new TimeUnit(0, 0);
				TimeUnit newNextDayStartTime = new TimeUnit(0, 0);
                TimeUnit newPreviousDayEndTime = new TimeUnit(0, 0);            
                
                if(endTime < startTime)
				{
					nextDayStartTime = passedProfile.StartTimes[(weekDay + 1) % 7];
					isValid = endTime < nextDayStartTime;
                    if(!isValid)
					{
						if(passedProfile.Days[(weekDay+1)%7])
						{
							DisplayAlert("Ungültige Eingabe","Zeiten überschneiden sich!" ,"OK");
							return;
						}
						else 
						{
							newNextDayStartTime = endTime;
							passedProfile.StartTimes[(weekDay + 1) % 7] = newNextDayStartTime;
						}
					}
				}

				int prevDayIndex = ((weekDay - 1) % 7 + 7) % 7;
                
                if(passedProfile.EndTimes[prevDayIndex] < passedProfile.StartTimes[prevDayIndex])
				{
					previousDayEndTime = passedProfile.EndTimes[prevDayIndex];
					isValid = previousDayEndTime < startTime;
                    if(!isValid)
					{
						if(passedProfile.Days[prevDayIndex])
						{
							DisplayAlert("Ungültige Eingabe", "Zeiten überschneiden sich!", "OK");
							return;
						} else {
							newPreviousDayEndTime = startTime;
							passedProfile.EndTimes[prevDayIndex] = newPreviousDayEndTime;
						}
					}
				}

				Console.WriteLine("modifiedTime: " + modifiedTime);
				Console.WriteLine("startTime: " + startTime);
				Console.WriteLine("endTime: " + endTime);
				Console.WriteLine("nextDayStartTime: " + nextDayStartTime);
				Console.WriteLine("previousDayEndTime: " + previousDayEndTime);
				Console.WriteLine("newNextDayStartTime: " + newNextDayStartTime);
				Console.WriteLine("newPreviousDayEndTime: " + newPreviousDayEndTime);

                if(isStartTime)
				{
					passedProfile.StartTimes[weekDay] = modifiedTime;
				} else {
					passedProfile.EndTimes[weekDay] = modifiedTime;
				}

				listener.OnTimeChanged();

				Navigation.PopAsync();
			};

			AbortButton.Clicked += (sender, args) =>
			{
				Navigation.PopAsync();
			};

        }
        
    }
}
