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
            
            ConfirmDownloadButton.Clicked += (sender,e) =>
            {
				string userName = UserNameInput.Text;
				string userPassword = UserPasswordInput.Text;
				Task<bool> downloadFilesTask = GetFTP.RetrieveFilesAsync(userName, userPassword);
				bool success = false;
                if(success)
                {
					Navigation.PopAsync();
                } else if(!success) {
					DisplayAlert("Fehler:", "Während des Downloads ist ein Fehler aufgetreten", "OK");
                }
            };
        }
    }
}
