using System;
using SUGAR_CrossPlatform.iOS;
using Foundation;
using UserNotifications;

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
            content.Subtitle = "Anrufe sind jetzt erlaubt.";
            content.Body = "";
            content.UserInfo = new NSDictionary("ProfileName", prof.Name);

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
            content.Subtitle = "Anrufe sind jetzt verboten.";
            content.Body = "";
            content.UserInfo = new NSDictionary("ProfileName", prof.Name);

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
            if(enable) {
                times = prof.StartTimes;
            } else {
                times = prof.EndTimes;
            }

            var currentDayIndex = ToIndex(now.DayOfWeek);
            var currentTimeOfDay = new TimeUnit(now.Hour, now.Minute);

            if (!days[currentDayIndex] || currentTimeOfDay >= times[currentDayIndex])
            {
                for (int i = 0; i < 7; i++)
                {
                    currentDayIndex = (currentDayIndex + 1) % 7;
                    if (days[currentDayIndex])
                    {
                        break;
                    }
                }
            }
            targetTime.SetValueForComponent((int) ToNSDay(currentDayIndex), NSCalendarUnit.Weekday);
            targetTime.SetValueForComponent(times[currentDayIndex].Hour, NSCalendarUnit.Hour);
            targetTime.SetValueForComponent(times[currentDayIndex].Minute, NSCalendarUnit.Minute);
            return targetTime;
        }

        private NSWeekDay ToNSDay(DayOfWeek cSharpDay) {
            switch(cSharpDay) {
                case DayOfWeek.Monday:
                    return NSWeekDay.Monday;
                case DayOfWeek.Tuesday:
                    return NSWeekDay.Tuesday;
                case DayOfWeek.Wednesday:
                    return NSWeekDay.Wednesday;
                case DayOfWeek.Thursday:
                    return NSWeekDay.Thursday;
                case DayOfWeek.Friday:
                    return NSWeekDay.Friday;
                case DayOfWeek.Saturday:
                    return NSWeekDay.Saturday;
                case DayOfWeek.Sunday:
                    return NSWeekDay.Sunday;
                default:
                    return NSWeekDay.Monday;
            }
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
