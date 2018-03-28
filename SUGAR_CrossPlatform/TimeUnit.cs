using System;
namespace SUGAR_CrossPlatform
{
    public struct TimeUnit
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

        public static bool operator < (TimeUnit left, TimeUnit right) {
            if(left.Hour < right.Hour) {
                return true;
            } else if(left.Hour == right.Hour && left.Minute < right.Minute) {
                return true;
            } else {
                return false;
            }
        }

        public static bool operator > (TimeUnit left, TimeUnit right) {
            if(left.Hour > right.Hour) {
                return true;
            } else if(left.Hour == right.Hour && left.Minute > right.Minute) {
                return true;
            } else {
                return false;
            }
        }

        public static bool operator == (TimeUnit left, TimeUnit right) {
            return left.Hour == right.Hour && left.Minute == right.Minute;
        }

        public static bool operator != (TimeUnit left, TimeUnit right) {
            return left.Hour != right.Hour || left.Minute != right.Minute;
        }

        public static bool operator <= (TimeUnit left, TimeUnit right) {
            return left < right || left == right;
        }

        public static bool operator >= (TimeUnit left, TimeUnit right) {
            return left > right || left == right;
        }

        public override string ToString()
        {
            string minuteString = Minute.ToString("00");
            return $"{Hour}:{minuteString}";
        }
    }
}
