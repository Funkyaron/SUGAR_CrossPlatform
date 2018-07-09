using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class DownloadProfilesPage : ContentPage
    {
		private FTPDownloader GetFTP;
		private OverviewPage parent;
    
		public DownloadProfilesPage(OverviewPage parentPage)
        {
            InitializeComponent();
			GetFTP = new FTPDownloader();

			parent = parentPage;
        }
        
        public async void ConfirmDownload(object sender, EventArgs args) {
			string serverAdress = ServerAdressInput.Text;
            string userName = UserNameInput.Text;
            string userPassword = UserPasswordInput.Text;
            Task<bool> downloadFilesTask = GetFTP.RetrieveFilesAsync(serverAdress,userName, userPassword);
            // irgendwasLabel.Text = "Lädt...";
            bool success = await downloadFilesTask;
			parent.UpdateList();
            if(success)
            {
                await Navigation.PopAsync();
            } else if(!success) {
                await DisplayAlert("Fehler:", "Während des Downloads ist ein Fehler aufgetreten", "OK");
				await Navigation.PopAsync();
            }
        }

		public void AbortDownload(object sender, EventArgs args) {
			Navigation.PopAsync();
		}
    }
}
