using Xamarin.Forms;
using System;

namespace SUGAR_CrossPlatform
{
    public partial class SUGAR_CrossPlatformPage : ContentPage
    {
        public SUGAR_CrossPlatformPage()
        {
            InitializeComponent();
			Logo.Source = ImageSource.FromResource("Sugar.png");
        }

        public  void OpenShowProfiles(object sender,EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new OverviewPage());
            System.Console.WriteLine("Opening the activity 'ProfileOverview' ...");
        }

        public void OpenDoNotDisturb(object sender,EventArgs e)
        {
            Console.WriteLine("I never asked for this.");
        }

        public void OpenClosingTimes(object sender,EventArgs e)
        {
            return;
        }
    }
}
