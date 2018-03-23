using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
        }

        public void SaveTestProfile(object sender, EventArgs e) {
            Profile prof = new Profile();
            prof.Name = "Kurz";
            prof.PhoneNumbers.Add("1234");
            prof.PhoneNumbers.Add("5432");
            prof.ContactNames.Add("Marius Müller");
            prof.ContactNames.Add("Max Mustermann");
            prof.ContactNames.Add("Martin Mikus");

            TimeUnit[] startTimes = prof.StartTimes;
            startTimes[4] = new TimeUnit(7, 0);
            prof.StartTimes = startTimes;
            TimeUnit[] endTimes = prof.EndTimes;
            endTimes[4] = new TimeUnit(13, 0);
            prof.EndTimes = endTimes;
            bool[] days = prof.Days;
            days[4] = true;
            prof.Days = days;
            prof.Active = true;
            prof.Allowed = false;

            ProfileManager mgr = new ProfileManager();
            bool success = mgr.SaveProfile(prof);
            if(success) {
                ResultLabel.Text = "Profile saved.";
            } else {
                ResultLabel.Text = "Error while saving.";
            }
        }

        /*public void ReadAsText(object sender, EventArgs e) {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, "Kurz.xml");
            try {
                ResultLabel.Text = File.ReadAllText(filePath);
            } catch(Exception ex) {
                ResultLabel.Text = ex.StackTrace;
            }
        }*/

        public void ReadAsProfile(object sender, EventArgs e) {
            ProfileManager mgr = new ProfileManager();
            Profile prof = mgr.GetProfile("Kurz");
            if (prof != null) {
                ResultLabel.Text = prof.ToString();
            } else {
                ResultLabel.Text = "Error while reading.";
            }
        }

        public void InitProfile(object sender, EventArgs args) {
            try {
                ProfileManager mgr = new ProfileManager();
                Profile prof = mgr.GetProfile("Kurz");
                if(prof != null) {
                    mgr.InitProfile(prof);
                    ResultLabel.Text = "Profile initialized";
                } else {
                    ResultLabel.Text = "Error while reading";
                }
            } catch(Exception e) {
                ResultLabel.Text = $"Error while initializing: {e}";
            }

        }

        public void Compare(object sender, EventArgs args) {
            TimeUnit tu1 = new TimeUnit(11, 0);
            TimeUnit tu2 = new TimeUnit(10, 0);
            bool earlier = tu1 < tu2;
            bool earlierOrEqual = tu1 <= tu2;

            string resultText = $"Time 1: {tu1}\nTime 2: {tu2}\n";
            if(earlier) {
                resultText += "Time 1 is earlier than time 2\n";
            } else {
                resultText += "Time 1 is later or equal time 2\n";
            }
            if(earlierOrEqual) {
                resultText += "Time 1 is earlier or equal time 2";
            } else {
                resultText += "Time 1 is later than time 2";
            }

            ResultLabel.Text = resultText;
        }
    }
}
