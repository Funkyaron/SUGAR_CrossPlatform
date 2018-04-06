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

        public void FetchContacts(object sender, EventArgs args) {
            IContactsFetcher fetcher = DependencyService.Get<IContactsFetcher>();
            SUGARContact[] allContacts = fetcher.GetAllContacts();

            ResultLabel.Text = "";
            foreach(var contact in allContacts) {
                ResultLabel.Text += $"{contact}\n";
            }
        }

        public void RequestAccess(object sender, EventArgs args) {
            try {
                //new CNContactStore().RequestAccess(CNEntityType.Contacts, (granted, error) => { });
            } catch(Exception e) {
                ResultLabel.Text = e.ToString();
            }
        }

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
