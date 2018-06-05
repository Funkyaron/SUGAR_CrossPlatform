using System;
using System.Collections.Generic;

namespace SUGAR_CrossPlatform
{
    public class SUGARContact
    {
        public string Name { get; }
        public List<string> PhoneNumbersAsStrings { get; }
        public List<long> PhoneNumbersAsLongs { get; }

        public SUGARContact()
        {
        }

        public SUGARContact(string name, List<string> phoneNumbersAsStrings, List<long> phoneNumbersAsLongs) {
            Name = name;
            PhoneNumbersAsStrings = phoneNumbersAsStrings;
            PhoneNumbersAsLongs = phoneNumbersAsLongs;
        }

		public override string ToString()
		{
            string result = $"Name: {Name}\n";
            result += "Strings: ";
            foreach(string number in PhoneNumbersAsStrings) {
                result += $"{number}, ";
            }
            result += "\nLongs: ";
            foreach(long number in PhoneNumbersAsLongs) {
                result += $"{number}, ";
            }
            return result;
		}
	}
}
