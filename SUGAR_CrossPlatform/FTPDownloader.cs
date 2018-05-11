using System;
using System.Net;
using System.IO;
using System.Collections.Generic;

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

        public bool RetrieveFiles(string userName, string password) {
            bool isSuccessful = false;

			try {
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.strato.com/%2F/SUGAR-CrossPlatform/SUGAR-CrossPlatform");
				request.Credentials = new NetworkCredential(userName, password);
				request.Method = WebRequestMethods.Ftp.ListDirectory;
				request.KeepAlive = false;
				request.UseBinary = true;
				request.UsePassive = true;

				FtpWebResponse response = (FtpWebResponse)request.GetResponse();
				Stream responseStream = response.GetResponseStream();
				StreamReader directoryReader = new StreamReader(responseStream);

				List<string> fileNames = new List<string>();
				directoryReader.ReadLine(); // skip ".." directory
				string line = directoryReader.ReadLine();
				while (!string.IsNullOrEmpty(line)) {
					fileNames.Add(line);
					line = directoryReader.ReadLine();
				}

				directoryReader.Close();
				responseStream.Close();
				response.Close();

				foreach (string fileName in fileNames) {
					WebClient ftpClient = new WebClient();
					ftpClient.Credentials = new NetworkCredential(userName, password);
					string remotePath = "ftp://ftp.strato.com/%2F/SUGAR-CrossPlatform/" + fileName;
				    string localPath = Path.Combine(GetFolderPath(), fileName);
					if (!File.Exists(localPath)) {
						ftpClient.DownloadFile(remotePath, localPath);
					}
				}
				isSuccessful = true;
			} catch(Exception e) {
				e.ToString();
				isSuccessful = false;
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
