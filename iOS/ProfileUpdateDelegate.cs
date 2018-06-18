using System;

using Foundation;
using UserNotifications;
using CallKit;

namespace SUGAR_CrossPlatform.iOS
{
    public class ProfileUpdateDelegate : UNUserNotificationCenterDelegate
    {
        public ProfileUpdateDelegate()
        {
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            base.DidReceiveNotificationResponse(center, response, completionHandler);
        }
        
        public override async void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
			Console.WriteLine("ProfileUpdateDelegate: WillPresentNotification()");
            if(notification.Request.Identifier.Contains("ClosingTime") == false) {
                ProfileManager mgr = new ProfileManager();
                Scheduler scheduler = new Scheduler();

                var info = notification.Request.Content.UserInfo;
                NSString profileName = (NSString) info.ValueForKey(new NSString("ProfileName"));
                Profile prof = mgr.GetProfile(profileName);

                if (prof == null || !prof.Active) return;

                string id = notification.Request.Identifier;
                if(id.Contains("Enable")) {
                    prof.Allowed = false;
                    scheduler.ScheduleNextEnable(prof);
                } else if(id.Contains("Disable")) {
                    prof.Allowed = true;
                    scheduler.ScheduleNextDisable(prof);
                } else {
                    return;
                }

                mgr.SaveProfile(prof);

                var callDirManager = CXCallDirectoryManager.SharedInstance;
				await callDirManager.ReloadExtensionAsync("de.unisiegen.SUGAR-CrossPlatform.PhoneBlockExtension");
                /*callDirManager.ReloadExtension(
                    "de.unisiegen.SUGAR-CrossPlatform.PhoneBlockExtension",
                   error =>
                   {
                       if (error == null)
                       {
                           // Everything's fine.
                       }
                       else
                       {
                           // An error occured. But what to do with it?
                       }
                   });*/
            }

            completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}
