using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class SelectContactsPage : ContentPage
	{
		private static Profile SelectedProfile;

        public SelectContactsPage(Profile passedProfile)
        {
            InitializeComponent();
			SelectedProfile = passedProfile;
        }
        
        public static List<string> GetProfileContactNames()
		{
			return SelectedProfile.ContactNames;
		}
    }
}
