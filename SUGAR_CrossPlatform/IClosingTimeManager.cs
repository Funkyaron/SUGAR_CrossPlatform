using System;
namespace SUGAR_CrossPlatform
{
    public interface IClosingTimeManager
    {
        void SaveAndScheduleClosingTime(DayOfWeek dayOfWeek, TimeUnit time);
        void RemoveAndCancelClosingTime(DayOfWeek dayOfWeek);
        TimeUnit[] GetAllClosingTimes();
    }
}
