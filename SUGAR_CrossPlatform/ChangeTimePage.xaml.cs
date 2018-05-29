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
		private bool OverflowingDayActive;
		private bool LastWeekDay;
		private TimeUnit[] currentDayTimeSpan;
		private TimeUnit[] previousDayTimeSpan;
        
		public ChangeTimePage(int weekDay,TimeUnit oldTime,bool isStartTime,bool existingProfile,Profile passedProfile, IOnTimeChangedListener listener)
        {
            InitializeComponent();

			ChooseWeekDayTime.Format = "HH:mm";
            
			InvalidTime = false;
			LastWeekDay = false;
            
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
				//...
				listener.OnTimeChanged();
			};
           

        }

        private bool IsTimeInputValid()
		{
			return true;
		}
    }
}
