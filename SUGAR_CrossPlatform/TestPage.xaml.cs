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
        private static Profile prof;

        public TestPage()
        {
            InitializeComponent();
        }

        public void SaveTestProfile(object sender, EventArgs e) {
            prof = new Profile();
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

        public void ToggleProfile(object sender, EventArgs e) {
            bool newValue = !prof.Allowed;
            prof.Allowed = newValue;
            ProfileManager mgr = new ProfileManager();
            mgr.SaveProfile(prof);
        }

        public void WriteText(object sender, EventArgs e) {
            /*try {
                NSFileManager fileMgr = NSFileManager.DefaultManager;
                NSUrl url = fileMgr.GetContainerUrl("group.de.unisiegen.SUGAR-CrossPlatform");
                string filePath = Path.Combine(url.Path, "Test.txt");
                File.WriteAllText(filePath, "HelloWorld");
                ResultLabel.Text = "Mission accomplished";
            } catch(Exception ex) {
                ResultLabel.Text = ex.ToString();
            }*/
        }

        public void ReadAsText(object sender, EventArgs e) {
            /*try {
                //var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                NSFileManager filemMgr = NSFileManager.DefaultManager;
                NSUrl url = filemMgr.GetContainerUrl("group.de.unisiegen.SUGAR-CrossPlatform");
                var filePath = Path.Combine(url.Path, "Test.txt");
                ResultLabel.Text = File.ReadAllText(filePath);
            } catch(Exception ex) {
                ResultLabel.Text = ex.ToString();
            }*/
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

        public void ReloadExtension(object sender, EventArgs e) {
            var callDirManager = CXCallDirectoryManager.SharedInstance;

            callDirManager.ReloadExtension(
                "de.unisiegen.SUGAR-CrossPlatform.PhoneBlockExtension",
               error =>
               {
                   if (error == null)
                   {
                       //ResultLabel.Text = "Reloaded Extension successfully";
                   }
                   else
                   {
                       //ResultLabel.Text = "Error while reloading";
                   }
               });
            ResultLabel.Text += "\nThe Button has been clicked";
        }

        public void GetURL(object sender, EventArgs e) {
            /*NSFileManager mgr = NSFileManager.DefaultManager;
            NSUrl url = mgr.GetContainerUrl("group.de.unisiegen.SUGAR-CrossPlatform");
            NSUrl filePathUrl = url.FilePathUrl;
            NSUrl newUrl = filePathUrl.FileReferenceUrl;
            ResultLabel.Text = url.Path;
            ResultLabel.Text += "\n";
            ResultLabel.Text += url.ToString();*/
        }

        public void GetFilePath(object sender, EventArgs e) {
            ResultLabel.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }
    }
}
