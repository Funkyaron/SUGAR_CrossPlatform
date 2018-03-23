using System;
namespace SUGAR_CrossPlatform
{
    public interface IScheduler
    {
        void ScheduleNextEnable(Profile prof);
        void ScheduleNextDisable(Profile prof);
    }
}
