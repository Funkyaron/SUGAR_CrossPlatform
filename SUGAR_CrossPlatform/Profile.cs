using System;
using System.Collections.Generic;

namespace SUGAR_CrossPlatform
{
    public class Profile
    {
        public Profile()
        {
            Name = "";

            Days = new bool[7];
            for (int i = 0; i < Days.Length; i++) Days[i] = false;

            StartTimes = new TimeUnit[7];
            for (int i = 0; i < StartTimes.Length; i++) StartTimes[i] = new TimeUnit(0, 0);

            EndTimes = new TimeUnit[7];
            for (int i = 0; i < EndTimes.Length; i++) EndTimes[i] = new TimeUnit(23, 59);

            Active = false;
            Allowed = true;
            Mode = BlockMode.WhiteList;
            PhoneNumbersAsStrings = new List<string>();
            PhoneNumbersAsLongs = new List<long>();
            ContactNames = new List<string>();
        }

        public Profile(string name, bool[] days, TimeUnit[] startTimes, TimeUnit[] endTimes, bool active, bool allowed, BlockMode mode, List<string> phoneNumbersAsStrings, List<long> phoneNumbersAsLongs, List<string> contactNames)
        {
            Name = name;
            Days = days;
            StartTimes = startTimes;
            EndTimes = endTimes;
            Active = active;
            Allowed = allowed;
            Mode = mode;
            PhoneNumbersAsStrings = phoneNumbersAsStrings;
            PhoneNumbersAsLongs = phoneNumbersAsLongs;
            ContactNames = contactNames;
        }

        public string Name { get; set; }
        public bool[] Days { get; set; }
        public TimeUnit[] StartTimes { get; set; }
        public TimeUnit[] EndTimes { get; set; }
        public bool Active { get; set; }
        public bool Allowed { get; set; }
        public BlockMode Mode { get; set; }
        public List<string> PhoneNumbersAsStrings { get; set; }
        public List<long> PhoneNumbersAsLongs { get; set; }
        public List<string> ContactNames { get; set; }

        public override string ToString()
        {
            return string.Format("[Profile: Name={0}, Days={1}, StartTimes={2}, EndTimes={3}, Active={4}, Allowed={5}, Mode={6}, PhoneNumbersAsStrings={7}, PhoneNumbersAsLongs={8}, ContactNames={9}]", Name, Days, StartTimes, EndTimes, Active, Allowed, Mode, PhoneNumbersAsStrings, PhoneNumbersAsLongs, ContactNames);
        }
    }
}
