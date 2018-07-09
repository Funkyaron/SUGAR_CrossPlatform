using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

#if __IOS__
using Foundation;
#endif

namespace SUGAR_CrossPlatform
{
    public class FTPDownloader
    {
        public FTPDownloader()
        {
        }

		// Use this method like this:
        //
		// Task<bool> downloadFilesTask = ftpDownloader.RetrieveFilesAsync(username, password);
		// DoSomeOtherStuff(); like telling the user that the app is downloading
		// bool success = await downloadFilesTask;
        //
		// if(success)...
        //
        public async Task<bool> RetrieveFilesAsync(string serverAdress, string userName, string password) {
            bool isSuccessful = false;
			FtpWebResponse response = null;
			Stream responseStream = null;
			StreamReader directoryReader = null;
			try {
				string requestUrl = "ftp://" + serverAdress + "/%2F/SUGAR-CrossPlatform/SUGAR-CrossPlatform";
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(requestUrl);
				request.Credentials = new NetworkCredential(userName, password);
				request.Method = WebRequestMethods.Ftp.ListDirectory;
				request.KeepAlive = false;
				request.UseBinary = true;
				request.UsePassive = true;
                
				response = (FtpWebResponse)request.GetResponse();
				responseStream = response.GetResponseStream();
				directoryReader = new StreamReader(responseStream);
                
				List<string> fileNames = new List<string>();
				directoryReader.ReadLine(); // skip ".." directory
				string line = directoryReader.ReadLine();
				while (!string.IsNullOrEmpty(line)) {
					fileNames.Add(line);
					line = directoryReader.ReadLine();
				}

				foreach (string fileName in fileNames) {
					WebClient ftpClient = new WebClient();
					ftpClient.Credentials = new NetworkCredential(userName, password);
					string remotePath = "ftp://ftp.strato.com/%2F/SUGAR-CrossPlatform/" + fileName;
				    string localPath = Path.Combine(GetFolderPath(), fileName);
					if (!File.Exists(localPath)) {
						await ftpClient.DownloadFileTaskAsync(remotePath, localPath);
					}
				}
				isSuccessful = true;
			} catch(Exception e) {
				e.ToString();
				isSuccessful = false;
			} finally {
				directoryReader?.Close();
                responseStream?.Close();
                response?.Close();
			}

            return isSuccessful;
        }

		private string GetFolderPath() {
			string folderPath = "";
#if __Android__
            folderPath += Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#endif
#if __IOS__
            NSFileManager fileMgr = NSFileManager.DefaultManager;
            NSUrl url = fileMgr.GetContainerUrl("group.de.unisiegen.SUGAR-CrossPlatform");
            folderPath += url.Path;
#endif
            return folderPath;
		}
    }
}
