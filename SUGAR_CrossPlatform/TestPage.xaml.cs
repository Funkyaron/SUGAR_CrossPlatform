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
            ProfileManager mgr = new ProfileManager();
            bool success = mgr.SaveProfile(prof);
            if(success) {
                ResultLabel.Text = "Profile saved.";
            } else {
                ResultLabel.Text = "Error while saving.";
            }
        }

        public void ReadAsText(object sender, EventArgs e) {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, "Kurz.xml");
            try {
                ResultLabel.Text = File.ReadAllText(filePath);
            } catch(Exception ex) {
                ResultLabel.Text = ex.StackTrace;
            }
        }

        public void ReadAsProfile(object sender, EventArgs e) {
            ProfileManager mgr = new ProfileManager();
            Profile prof = mgr.ReadProfile("Kurz");
            if (prof != null) {
                ResultLabel.Text = prof.ToString();
            } else {
                ResultLabel.Text = "Error while reading.";
            }
        }
    }
}
