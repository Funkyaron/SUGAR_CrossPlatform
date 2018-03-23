using System;
using SUGAR_CrossPlatform.iOS;
using Foundation;
using UserNotifications;

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

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            ProfileManager mgr = new ProfileManager();
            Scheduler scheduler = new Scheduler();

            var info = notification.Request.Content.UserInfo;
            NSString profileName = (NSString) info.ValueForKey(new NSString("ProfileName"));
            Profile prof = mgr.GetProfile(profileName);

            if (!prof.Active) return;

            string id = notification.Request.Identifier;
            if(id.Contains("Enable")) {
                prof.Allowed = true;
                // Do something with the numbers...?
                scheduler.ScheduleNextEnable(prof);
            } else if(id.Contains("Disable")) {
                prof.Allowed = false;
                // Same here...
                scheduler.ScheduleNextDisable(prof);
            } else {
                return;
            }

            mgr.SaveProfile(prof);
 
            base.WillPresentNotification(center, notification, completionHandler);
        }
    }
}
