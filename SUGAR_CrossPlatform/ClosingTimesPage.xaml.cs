using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class ClosingTimesPage : ContentPage
    {

		private TimeUnit[] CurrentClosingTimes = new TimeUnit[7];
    
        public ClosingTimesPage()
        {
            InitializeComponent();
			int selectedWeekDay = 0;
			CurrentClosingTimes = new ClosingTimeManager().GetAllClosingTimes();
            if(CurrentClosingTimes == null)
            {
				for (int currWeekDay = 0; currWeekDay < 7;currWeekDay++ )
                {
					CurrentClosingTimes[currWeekDay] = new TimeUnit(0, 0);
                }
            }
            
            foreach(Button currButton in WeekDayGrid.Children)
            {
                if(currButton.Text != "")
                {
					currButton.Text = CurrentClosingTimes[selectedWeekDay].ToString();
					currButton.Clicked += (sender, args) =>
					{
						int currHour = CurrentClosingTimes[selectedWeekDay].Hour;
						int currMinute = CurrentClosingTimes[selectedWeekDay].Minute;
						Navigation.PushAsync(new ChangeClosingTimesPage(currHour,currMinute,selectedWeekDay));
						selectedWeekDay++;
					};
                } else if ( currButton.Text == "") 
                {
					currButton.Text = "";
					currButton.Clicked += (sender, args) =>
					{
						Navigation.PushAsync(new ChangeClosingTimesPage(12, 00, selectedWeekDay));
						selectedWeekDay++;
					};
                }
            }
        }
    }
}
