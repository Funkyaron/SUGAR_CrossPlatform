using System;
using Foundation;
using UserNotifications;

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
            bool shouldApply = false;
            bool[] days = prof.Days;
            foreach(bool day in days) {
                shouldApply |= day;
            }
            if (!shouldApply) return;

            var content = new UNMutableNotificationContent();
            content.Title = "Profil: " + prof.Name;
            content.Subtitle = "Anrufe sind jetzt verboten.";
            content.Body = "";
            content.UserInfo = new NSDictionary("ProfileName", prof.Name);
            content.Sound = UNNotificationSound.Default;

            var targetTime = GetTargetTime(prof, true);
            var trigger = UNCalendarNotificationTrigger.CreateTrigger(targetTime, false);
            var requestID = prof.Name + "Enable";
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => {
                if (err != null) {
                    // Do something with error..
                }
            });
        }

        public void ScheduleNextDisable(Profile prof) {
            bool shouldApply = false;
            bool[] days = prof.Days;
            foreach (bool day in days) {
                shouldApply |= day;
            }
            if (!shouldApply) return;

            var content = new UNMutableNotificationContent();
            content.Title = "Profil: " + prof.Name;
            content.Subtitle = "Anrufe sind jetzt erlaubt.";
            content.Body = "";
            content.UserInfo = new NSDictionary("ProfileName", prof.Name);
            content.Sound = UNNotificationSound.Default;

            var targetTime = GetTargetTime(prof, false);
            var trigger = UNCalendarNotificationTrigger.CreateTrigger(targetTime, false);
            var requestID = prof.Name + "Disable";
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => {
                if (err != null)
                {
                    // Do something with error..
                }
            });
        }

        private NSDateComponents GetTargetTime(Profile prof, bool enable) {
            // We need: Day, Hour and minute.

            var targetTime = new NSDateComponents();
            var now = DateTime.Now;

            var days = prof.Days;
            TimeUnit[] times = null;

            var currentDayIndex = ToIndex(now.DayOfWeek);
            var currentTimeOfDay = new TimeUnit(now.Hour, now.Minute);

            int targetHour;
            int targetMinute;

            if(enable) {
                times = prof.StartTimes;
                // Push the current day index forward until we reach the target day of week
                if (!days[currentDayIndex] || currentTimeOfDay >= times[currentDayIndex]) {
                    for (int i = 0; i < 7; i++) {
                        currentDayIndex = (currentDayIndex + 1) % 7;
                        if (days[currentDayIndex]) {
                            break;
                        }
                    }
                }
                targetHour = times[currentDayIndex].Hour;
                targetMinute = times[currentDayIndex].Minute;
            } else {
                times = prof.EndTimes;
                int previousDayIndex = ((currentDayIndex - 1) % 7 + 7) % 7;
                if (days[previousDayIndex] && prof.EndTimes[previousDayIndex] < prof.StartTimes[previousDayIndex] && currentTimeOfDay < prof.EndTimes[previousDayIndex]) {
                    // Now the end time from the previous day reaches today AND the time is later than now
                    // -> Trigger today. But take the end time from "yesterday".
                    targetHour = times[previousDayIndex].Hour;
                    targetMinute = times[previousDayIndex].Minute;
                    currentDayIndex += 0;
                } else if (days[currentDayIndex] && prof.StartTimes[currentDayIndex] < prof.EndTimes[currentDayIndex] && currentTimeOfDay < prof.EndTimes[currentDayIndex]) {
                    // The first case is excluded, but the end time from today is later than now AND
                    // the end time from today does not reach tomorrow.
                    // -> Trigger today.
                    targetHour = times[currentDayIndex].Hour;
                    targetMinute = times[currentDayIndex].Minute;
                    currentDayIndex += 0;
                } else if(days[currentDayIndex] && prof.EndTimes[currentDayIndex] < prof.StartTimes[currentDayIndex]) {
                    // Now we have to trigger tomorrow with the end time from today.
                    targetHour = times[currentDayIndex].Hour;
                    targetMinute = times[currentDayIndex].Minute;
                    currentDayIndex += 1;
                } else {
                    // Now we don't have to trigger some other day.
                    // -> Push day index like above.
                    for (int i = 0; i < 7; i++) {
                        currentDayIndex = (currentDayIndex + 1) % 7;
                        if(days[currentDayIndex]) {
                            break;
                        }
                    }
                    targetHour = times[currentDayIndex].Hour;
                    targetMinute = times[currentDayIndex].Minute;
                    if(prof.EndTimes[currentDayIndex] < prof.StartTimes[currentDayIndex]) {
                        // The end time from the target day of week reaches the next day.
                        currentDayIndex = (currentDayIndex + 1) % 7;
                    }
                }
            }

            targetTime.SetValueForComponent((int) ToNSDay(currentDayIndex), NSCalendarUnit.Weekday);
            targetTime.SetValueForComponent(targetHour, NSCalendarUnit.Hour);
            targetTime.SetValueForComponent(targetMinute, NSCalendarUnit.Minute);
            return targetTime;
        }

        private NSWeekDay ToNSDay(int index) {
            switch(index) {
                case 0:
                    return NSWeekDay.Monday;
                case 1:
                    return NSWeekDay.Tuesday;
                case 2:
                    return NSWeekDay.Wednesday;
                case 3:
                    return NSWeekDay.Thursday;
                case 4:
                    return NSWeekDay.Friday;
                case 5:
                    return NSWeekDay.Saturday;
                case 6:
                    return NSWeekDay.Sunday;
                default:
                    return NSWeekDay.Monday;
            }
        }

        private int ToIndex(DayOfWeek cSharpDay) {
            switch(cSharpDay) {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
                default:
                    return 0;
            }
        }
    }
}
