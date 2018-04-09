using System;
using System.Collections.Generic;
using System.Text;

using Contacts;
using CoreTelephony;
using Foundation;

using PhoneNumbers;

using SUGAR_CrossPlatform.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(ContactsFetcher))]
namespace SUGAR_CrossPlatform.iOS
{
    public class ContactsFetcher : IContactsFetcher
    {
        public ContactsFetcher()
        {
        }

        public SUGARContact[] GetAllContacts() {
            List<SUGARContact> resultList = new List<SUGARContact>();

            CNContactStore store = new CNContactStore();
            CNContactFetchRequest request = new CNContactFetchRequest(
                CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.PhoneNumbers);
            store.EnumerateContacts(request, out NSError err, (CNContact contact, ref Boolean stop) =>
            {
                CNLabeledValue<CNPhoneNumber>[] phoneNumberValues = contact.PhoneNumbers;
                List<string> phoneNumbersAsStrings = new List<string>();
                List<long> phoneNumbersAsLongs = new List<long>();
                foreach(var labeledValue in phoneNumberValues) {
                    string stringValue = labeledValue.Value.StringValue;
                    phoneNumbersAsStrings.Add(stringValue);
                    phoneNumbersAsLongs.Add(ParseNumberAsLong(stringValue));
                }
                SUGARContact nextContact = new SUGARContact($"{contact.GivenName} {contact.FamilyName}", 
                                                            phoneNumbersAsStrings, phoneNumbersAsLongs);
                resultList.Add(nextContact);
            });

            return resultList.ToArray();
        }


        private long ParseNumberAsLong(string originalNumber) {
            string countryCode;

            CTTelephonyNetworkInfo info = new CTTelephonyNetworkInfo();
            CTCarrier carrier = info.SubscriberCellularProvider;
            if (carrier != null) {
                countryCode = carrier.IsoCountryCode.ToUpper();
            } else {
                countryCode = "US";
            }

            PhoneNumberUtil util = PhoneNumberUtil.GetInstance();
            PhoneNumber number = util.Parse(originalNumber, countryCode);
            string normalizedNumber = util.Format(number, PhoneNumberFormat.E164);

            StringBuilder builder = new StringBuilder();
            foreach(char c in normalizedNumber) {
                if(char.IsDigit(c)) {
                    builder.Append(c);
                }
            }
            return long.Parse(builder.ToString());
        }
    }
}
