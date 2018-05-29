using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class ChangeTimePage : ContentPage
    {
		private String[] weekDays = { "Montag","Dienstag","Mittwoch","Donnerstag","Freitag","Samstag","Sonntag" };
		private ProfileManager ProfManager;
		private bool InvalidTime;
		private bool LastWeekDay;
		private TimeUnit[] currentDayTimeSpan;
		private TimeUnit[] previousDayTimeSpan;

        public ChangeTimePage(int weekDay,TimeUnit oldTime,bool isStartTime,bool existingProfile,Profile passedProfile)
        {
            InitializeComponent();

			InvalidTime = false;

            if(isStartTime)
			{
				ChooseWeekDay.Text = "Wann soll das Profil \n am " + weekDays[weekDay] + " starten?";
			} else if (!isStartTime)
			{
				ChooseWeekDay.Text = "Wann soll das Profil am " + weekDays[weekDay] + " enden?";
			}

			ChooseWeekDayTime.Time = new TimeSpan(oldTime.Hour,oldTime.Minute,0);

			Confirm.Clicked += (sender, e) =>
			{
				if (LastWeekDay)
				{
				} else if(!LastWeekDay)
				{
				}
				Application.Current.MainPage.Navigation.PopAsync();
				Application.Current.MainPage.Navigation.PopAsync();
				if (existingProfile)
					Application.Current.MainPage.Navigation.PushAsync(new EditProfilePage(passedProfile.Name));
				else
					Application.Current.MainPage.Navigation.PushAsync(new CreateProfilePage(passedProfile));
			};
           

        }

        private bool IsTimeInputValid()
		{
			return true;
		}
    }
}
