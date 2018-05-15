using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class EditProfilePage : ContentPage
    {
		private Profile ToEditProfile;

        public EditProfilePage(String name)
        {
            InitializeComponent();
			NameLabel.Text = name;
			Button[] ActivationButtons = { ActivateMonday, ActivateTuesday, ActivateWednesday, ActivateThursday, ActivateFriday, ActivateSaturday, ActivateSunday };
			for (int currWeekDay = 0; currWeekDay < ActivationButtons.Length;currWeekDay++ )
			{
				var activationTapGestureRecognizer = new TapGestureRecognizer();
				activationTapGestureRecognizer.Tapped += (s, e) =>
				{

				};               
			}
        }
    }
}
