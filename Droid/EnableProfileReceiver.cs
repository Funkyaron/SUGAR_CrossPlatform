
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SUGAR_CrossPlatform.Droid
{
    [BroadcastReceiver]
    public class EnableProfileReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string profileName = intent.Categories.ToArray()[0];

            ProfileManager profileMgr = new ProfileManager();
            Profile prof = profileMgr.GetProfile(profileName);

            if(prof == null || !prof.Active) {
                return;
            }

            prof.Allowed = false;
            Scheduler scheduler = new Scheduler();
            scheduler.ScheduleNextEnable(prof);

            profileMgr.SaveProfile(prof);

            Notification.Builder builder = new Notification.Builder(Application.Context);
            builder/*.SetSmallIcon(???)*/
                   .SetContentTitle(profileName)
                   .SetContentText("Anufe sind jetzt verboten.");
            Notification noti = builder.Build();
            NotificationManager notiMgr = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            notiMgr?.Notify(profileName.GetHashCode(), noti);
        }
    }
}
