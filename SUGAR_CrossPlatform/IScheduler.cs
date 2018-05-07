using System;
namespace SUGAR_CrossPlatform
{
    public interface IScheduler
    {
        void ScheduleNextEnable(Profile prof);
        void ScheduleNextDisable(Profile prof);

        void ScheduleNextClosingTime(DayOfWeek day, TimeUnit time);
        void CancelClosingTime(DayOfWeek day);
    }
}
