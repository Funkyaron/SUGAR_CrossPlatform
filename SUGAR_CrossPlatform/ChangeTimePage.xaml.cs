using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class ChangeTimePage : ContentPage
    {
		private String[] weekDays = { "Montag","Dienstag","Mittwoch","Donnerstag","Freitag","Samstag","Sonntag" };

        public ChangeTimePage(int weekDay,bool isStartTime)
        {
            InitializeComponent();
            if(isStartTime)
			{
				ChooseWeekDay.Text = "Wann soll das Profil \n am " + weekDays[weekDay] + " starten?";
			} else if (!isStartTime)
			{
				ChooseWeekDay.Text = "Wann soll das Profil am " + weekDays[weekDay] + " enden?";
			}
        }
    }
}
