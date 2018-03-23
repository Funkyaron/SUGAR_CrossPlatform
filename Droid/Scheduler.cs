using System;
using Android.App;
using Android.Content;
using SUGAR_CrossPlatform.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Scheduler))]
namespace SUGAR_CrossPlatform.Droid
{
    public class Scheduler : IScheduler
    {
        public Scheduler()
        {
        }

        public void ScheduleNextEnable(Profile prof) {
            Context context = Application.Context;
        }

        public void ScheduleNextDisable(Profile prof) {
            
        }
    }
}
