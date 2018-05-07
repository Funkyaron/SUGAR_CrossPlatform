using System;
using System.Text;
using System.IO;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public class ClosingTimeManager
    {
        private const string fileName = "closingtimes.txt";
        public ClosingTimeManager()
        {
        }

        public void SaveAndScheduleClosingTime(DayOfWeek day, TimeUnit newTime) {
            TimeUnit[] allClosingTimes = GetAllClosingTimes();
            int dayIndex = ToIndex(day);
            allClosingTimes[dayIndex] = newTime;

            SaveAllClosingTimes(allClosingTimes);

            IScheduler scheduler = DependencyService.Get<IScheduler>();
            scheduler.ScheduleNextClosingTime(day, newTime);
        }

        public void RemoveAndCancelClosingTime(DayOfWeek day) {
            TimeUnit[] allClosingTimes = GetAllClosingTimes();
            int dayIndex = ToIndex(day);
            allClosingTimes[dayIndex] = null;

            SaveAllClosingTimes(allClosingTimes);

            IScheduler scheduler = DependencyService.Get<IScheduler>();
            scheduler.CancelClosingTime(day);
        }

        // Returns an array with one element for each weekday. The proper TimeUnit if there is
        // a closing time set. Null if not.
        public TimeUnit[] GetAllClosingTimes() {
            string filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
            if(!File.Exists(filePath)) {
                File.WriteAllText(filePath, "-,-,-,-,-,-,-,");
            }
            string fileText = File.ReadAllText(filePath);
            string[] timeStrings = fileText.Split(',');
            TimeUnit[] closingTimes = new TimeUnit[7];
            for (int i = 0; i < 7; i++) {
                if(timeStrings[i] == "-") {
                    closingTimes[i] = null;
                } else {
                    closingTimes[i] = TimeUnit.Parse(timeStrings[i]);
                }
            }

            return closingTimes;
        }

        private void SaveAllClosingTimes(TimeUnit[] times) {
            StringBuilder fileStringBuilder = new StringBuilder();
            foreach (TimeUnit time in times) {
                if (time != null) {
                    fileStringBuilder.Append(time.ToString());
                } else {
                    fileStringBuilder.Append("-");
                }
                fileStringBuilder.Append(",");
            }

            string filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
            File.WriteAllText(filePath, fileStringBuilder.ToString());
        }

        private int ToIndex(DayOfWeek cSharpDay) {
            switch(cSharpDay) {
                case DayOfWeek.Monday: return 0;
                case DayOfWeek.Tuesday: return 1;
                case DayOfWeek.Wednesday: return 2;
                case DayOfWeek.Thursday: return 3;
                case DayOfWeek.Friday: return 4;
                case DayOfWeek.Saturday: return 5;
                case DayOfWeek.Sunday: return 6;
                default: return 0;
            }
        }
    }
}
