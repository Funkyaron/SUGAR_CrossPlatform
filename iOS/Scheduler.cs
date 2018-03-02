using System;
using SUGAR_CrossPlatform.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(Scheduler))]
namespace SUGAR_CrossPlatform.iOS
{
    public class Scheduler : IScheduler
    {
        public Scheduler()
        {
        }

        public void ScheduleNextEnable(Profile prof) {
            
        }

        public void ScheduleNextDisable(Profile prof) {
            
        }
    }
}
