using System;

using Java.Util;

using Android.App;
using Android.Content;

using SUGAR_CrossPlatform.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Scheduler))]
namespace SUGAR_CrossPlatform.Droid
{
    public class Scheduler : IScheduler
    {
        private AlarmManager mAlarmManager = null;
        public Scheduler()
        {
        }

        public void ScheduleNextEnable(Profile prof) {
            Context context = Application.Context;

            string name = prof.Name;
            bool[] days = prof.Days;
            TimeUnit[] startTimes = prof.StartTimes;

            // First check if any day of week should apply
            bool shouldApply = false;
            foreach(bool day in days) {
                shouldApply |= day;
            }
            if (!shouldApply) return;

            Intent intent = new Intent(context, typeof(EnableProfileReceiver));
            intent.AddCategory(name);
            PendingIntent pending = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.CancelCurrent);

            long targetTime = GetTargetTime(prof, true);

            if(mAlarmManager == null) {
                mAlarmManager = (AlarmManager) context.GetSystemService(Context.AlarmService);
            }
            mAlarmManager.SetExact(AlarmType.RtcWakeup, targetTime, pending);
        }

        public void ScheduleNextDisable(Profile prof) {
            Context context = Application.Context;

            string name = prof.Name;
            bool[] days = prof.Days;
            TimeUnit[] endTimes = prof.EndTimes;

            // First check if any day of week should apply
            bool shouldApply = false;
            foreach (bool day in days) {
                shouldApply |= day;
            }
            if (!shouldApply) return;

            Intent intent = new Intent(context, typeof(DisableProfileReceiver));
            intent.AddCategory(name);
            PendingIntent pending = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.CancelCurrent);

            long targetTime = GetTargetTime(prof, false);

            if (mAlarmManager == null) {
                mAlarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            }
            mAlarmManager.SetExact(AlarmType.RtcWakeup, targetTime, pending);
        }


        private long GetTargetTime(Profile prof, bool enable) {
            Calendar cal = Calendar.Instance;
            int currentDayIndex = ToIndex(cal.Get(CalendarField.DayOfWeek));
            int previousDayIndex = ((currentDayIndex - 1) % 7 + 7) % 7;
            TimeUnit currentTimeOfDay = new TimeUnit(cal.Get(CalendarField.HourOfDay), cal.Get(CalendarField.Minute));

            bool[] days = prof.Days;
            TimeUnit[] startTimes = prof.StartTimes;
            TimeUnit[] endTimes = prof.EndTimes;

            int daysToAdd;
            int targetHour;
            int targetMinute;

            if(enable) {
                if(days[currentDayIndex] && currentTimeOfDay < startTimes[currentDayIndex]) {
                    daysToAdd = 0;
                } else {
                    // Push the current day index forward until we reach the target day of week
                    daysToAdd = 0;
                    do {
                        daysToAdd++;
                        currentDayIndex = (currentDayIndex + 1) % 7;
                        if (daysToAdd > 6) break;
                    } while (!days[currentDayIndex]);
                }
                targetHour = startTimes[currentDayIndex].Hour;
                targetMinute = startTimes[currentDayIndex].Minute;
            } else {
                if(days[previousDayIndex] && endTimes[previousDayIndex] < startTimes[previousDayIndex] && currentTimeOfDay < endTimes[previousDayIndex]) {
                    // Now the end time from the previous day reaches today AND the time is later than now
                    // -> Trigger today. But take the end time from "yesterday".
                    daysToAdd = 0;
                    targetHour = endTimes[previousDayIndex].Hour;
                    targetMinute = endTimes[previousDayIndex].Minute;
                } else if(days[currentDayIndex] && startTimes[currentDayIndex] < endTimes[currentDayIndex] && currentTimeOfDay < endTimes[currentDayIndex]) {
                    // The first case is excluded, but the end time from today is later than now AND
                    // the end time from today does not reach tomorrow.
                    // -> Trigger today.
                    daysToAdd = 0;
                    targetHour = endTimes[currentDayIndex].Hour;
                    targetMinute = endTimes[currentDayIndex].Minute;
                } else if(days[currentDayIndex] && endTimes[currentDayIndex] < startTimes[currentDayIndex]) {
                    // Now we have to trigger tomorrow with the end time from today.
                    daysToAdd = 1;
                    targetHour = endTimes[currentDayIndex].Hour;
                    targetMinute = endTimes[currentDayIndex].Minute;
                } else {
                    // Now we don't have to trigger some other day.
                    // -> Push day index like above.
                    daysToAdd = 0;
                    do {
                        daysToAdd++;
                        currentDayIndex = (currentDayIndex + 1) % 7;
                        if (daysToAdd > 6) break;
                    } while (!days[currentDayIndex]);
                    targetHour = endTimes[currentDayIndex].Hour;
                    targetMinute = endTimes[currentDayIndex].Minute;
                    if(endTimes[currentDayIndex] < startTimes[currentDayIndex]) {
                        // The end time from the target day of week reaches the next day.
                        daysToAdd += 1;
                    }
                }
            }

            cal.Add(CalendarField.DayOfMonth, daysToAdd);
            cal.Set(CalendarField.HourOfDay, targetHour);
            cal.Set(CalendarField.Minute, targetMinute);
            cal.Set(CalendarField.Second, 0);

            return cal.TimeInMillis;
        }

        private int ToIndex(int calendarDay) {
            switch(calendarDay) {
                case Calendar.Monday:
                    return 0;
                case Calendar.Tuesday:
                    return 1;
                case Calendar.Wednesday:
                    return 2;
                case Calendar.Thursday:
                    return 3;
                case Calendar.Friday:
                    return 4;
                case Calendar.Saturday:
                    return 5;
                case Calendar.Sunday:
                    return 6;
                default:
                    return 0;
            }
        }
    }
}
