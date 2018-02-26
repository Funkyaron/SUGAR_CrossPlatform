using System;
namespace SUGAR_CrossPlatform
{
    public class TimeUnit
    {
        public TimeUnit(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
        }

        public int Hour { get; set; }
        public int Minute { get; set; }

        public static TimeUnit Parse(string str) {
            string[] numberStrings = str.Split(new char[] { ':' });
            var hour = Int32.Parse(numberStrings[0]);
            var minute = Int32.Parse(numberStrings[1]);
            return new TimeUnit(hour, minute);
        }

        public override string ToString()
        {
            return $"{Hour}:{Minute}";
        }
    }
}
