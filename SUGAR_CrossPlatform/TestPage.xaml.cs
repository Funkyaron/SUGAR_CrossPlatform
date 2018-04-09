using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class TestPage : ContentPage
    {
        private const string privateNumber = "0163 9018985";

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
          
        public void FetchContacts(object sender, EventArgs args) {
            IContactsFetcher fetcher = DependencyService.Get<IContactsFetcher>();
            SUGARContact[] allContacts = fetcher.GetAllContacts();
            ResultLabel.Text = "";
            foreach(var contact in allContacts) {
                ResultLabel.Text += $"{contact}\n";
            }
        }

        public void ToggleProfile(object sender, EventArgs e) {
            ProfileManager mgr = new ProfileManager();
            Profile prof = mgr.GetProfile("Kurz");
            bool newValue = !prof.Allowed;
            prof.Allowed = newValue;
            mgr.SaveProfile(prof);

            ResultLabel.Text = "Profile toggled";

        public void ShowNumber(object sender, EventArgs args) {
            ResultLabel.Text = privateNumber;
        }

        public void ShowNormalized(object sender, EventArgs args) {
            IContactsFetcher con = DependencyService.Get<IContactsFetcher>();
            //ResultLabel.Text = con.NormalizeNumber(privateNumber);
        }

        public void DisplayCountryCode(object sender, EventArgs args) {
            IContactsFetcher con = DependencyService.Get<IContactsFetcher>();
            //ResultLabel.Text = con.GetCountryCode();
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

        public void ShowLong(object sender, EventArgs args) {
            IContactsFetcher contacts = DependencyService.Get<IContactsFetcher>();
            /*string normalizedNumber = contacts.NormalizeNumber(privateNumber);
            long longNumber = 0;
            try {
                longNumber = ParseNumberAsLong(normalizedNumber);
                ResultLabel.Text = longNumber.ToString();
            } catch(Exception e) {
                string bla = e.StackTrace;
                longNumber = 123;
                ResultLabel.Text = normalizedNumber;
            }*/
        }

        public long ParseNumberAsLong(string originalNumber) {
            StringBuilder builder = new StringBuilder();
            foreach (char c in originalNumber) {
                if (char.IsDigit(c)) {
                    builder.Append(c);
                }
            }
            string resultString = builder.ToString();
            return Int64.Parse(resultString);
        }
    }
}
