using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;

#if __IOS__
using Foundation;
using CallKit;
#endif

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
            prof.PhoneNumbersAsStrings.Add("1234");
            prof.PhoneNumbersAsStrings.Add("5432");
            prof.PhoneNumbersAsLongs.Add(8765);
            prof.PhoneNumbersAsLongs.Add(4321);
            prof.PhoneNumbersAsLongs.Add(491639018985);
            prof.ContactNames.Add("Marius Müller");
            prof.ContactNames.Add("Max Mustermann");
            prof.ContactNames.Add("Martin Mikus");

            TimeUnit[] startTimes = prof.StartTimes;
            startTimes[2] = new TimeUnit(21, 7);
            prof.StartTimes = startTimes;
            TimeUnit[] endTimes = prof.EndTimes;
            endTimes[2] = new TimeUnit(21, 59);
            prof.EndTimes = endTimes;
            bool[] days = prof.Days;
            days[2] = true;
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

        public void ToggleProfile(object sender, EventArgs e) {
            ProfileManager mgr = new ProfileManager();
            Profile prof = mgr.GetProfile("Kurz");
            bool newValue = !prof.Allowed;
            prof.Allowed = newValue;
            mgr.SaveProfile(prof);

            ResultLabel.Text = "Profile toggled";
        }

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

        public void ScheduleEnable(object sender, EventArgs e) {
            ProfileManager mgr = new ProfileManager();
            Profile prof = mgr.GetProfile("Kurz");
            IScheduler scheduler = DependencyService.Get<IScheduler>();
            scheduler.ScheduleNextEnable(prof);

            ResultLabel.Text = "Enable was scheduled";
        }

        public void ScheduleDisable(object sender, EventArgs e) {
            ProfileManager mgr = new ProfileManager();
            Profile prof = mgr.GetProfile("Kurz");
            IScheduler scheduler = DependencyService.Get<IScheduler>();
            scheduler.ScheduleNextDisable(prof);

            ResultLabel.Text = "Disable was scheduled";
        }

        public void Compare(object sender, EventArgs e) {
            DateTime now = DateTime.Now;
            TimeUnit currentTime = new TimeUnit(now.Hour, now.Minute);
            TimeUnit compareTime = new TimeUnit(21, 7);

            ResultLabel.Text = "Current Time: " + currentTime.ToString() + "\n";

            ResultLabel.Text += $"{compareTime} < {currentTime}? {compareTime < currentTime}\n";
            ResultLabel.Text += $"{currentTime} < {compareTime}? {currentTime < compareTime}\n";
            ResultLabel.Text += $"{compareTime} <= {currentTime}? {compareTime <= currentTime}\n";
            ResultLabel.Text += $"{currentTime} <= {compareTime}? {currentTime <= compareTime}\n";
            ResultLabel.Text += $"{compareTime} > {currentTime}? {compareTime > currentTime}\n";
            ResultLabel.Text += $"{currentTime} > {compareTime}? {currentTime > compareTime}\n";
            ResultLabel.Text += $"{compareTime} >= {currentTime}? {compareTime >= currentTime}\n";
            ResultLabel.Text += $"{currentTime} >= {compareTime}? {currentTime >= compareTime}\n";

            TimeUnit startTime = new TimeUnit(21, 7);
            TimeUnit endTime = new TimeUnit(21, 59);

            ResultLabel.Text += $"Current Time: {currentTime}\n";
            ResultLabel.Text += $"Start Time: {startTime}\n";
            ResultLabel.Text += $"End Time: {endTime}\n";

            ResultLabel.Text += $"Start Time <= Current Time? {startTime <= currentTime}\n";
            ResultLabel.Text += $"Current Time < End Time? {currentTime < endTime}\n";

            if(startTime <= currentTime && currentTime < endTime) {
                ResultLabel.Text += "The current time lies inside the time span";
            } else {
                ResultLabel.Text += "The current time is not inside the time span";
            }
        }
    }
}
