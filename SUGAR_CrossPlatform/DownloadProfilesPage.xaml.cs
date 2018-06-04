using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class DownloadProfilesPage : ContentPage
    {
		private FTPDownloader GetFTP;
    
        public DownloadProfilesPage()
        {
            InitializeComponent();
			GetFTP = new FTPDownloader();
            
            
        }
        
        public async void ConfirmDownload(object sender, EventArgs args) {
            string userName = UserNameInput.Text;
                string userPassword = UserPasswordInput.Text;
                Task<bool> downloadFilesTask = GetFTP.RetrieveFilesAsync(userName, userPassword);
                // irgendwasLabel.Text = "Lädt...";
                bool success = await downloadFilesTask;
                if(success)
                {
                    Navigation.PopAsync();
                } else if(!success) {
                    DisplayAlert("Fehler:", "Während des Downloads ist ein Fehler aufgetreten", "OK");
                }
        }
    }
}
