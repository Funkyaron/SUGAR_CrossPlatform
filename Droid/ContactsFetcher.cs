using System;
using System.Collections.Generic;

using Android.Database;
using Android.Provider;
using Android.App;

using SUGAR_CrossPlatform.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ContactsFetcher))]
namespace SUGAR_CrossPlatform.Droid
{
    public class ContactsFetcher : IContactsFetcher
    {
        public ContactsFetcher()
        {
        }

        public SUGARContact[] GetAllContacts() {
            var dataCursor = GetDataCursor();

            List<SUGARContact> allContacts = new List<SUGARContact>();

            // The idea is to go through all rows of the data cursor and remember the
            // latest contact.

            string latestContactName = null;
            SUGARContact latestContact = null;
            dataCursor.MoveToPosition(-1);
            while(dataCursor.MoveToNext()) {
                string currentContactName = dataCursor.GetString(dataCursor.GetColumnIndex("display_name"));
                if(latestContactName != null && currentContactName == latestContactName) {
                    // Now we have at least two rows with the same name -> get numbers
                    // and add them to the latest contact if not present.
                    string nextNumber = dataCursor.GetString(dataCursor.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                    string nextNormNumber = dataCursor.GetString(dataCursor.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.NormalizedNumber));
                    if(nextNumber != null) {
                        if(!latestContact.PhoneNumbersAsStrings.Contains(nextNumber)) {
                            latestContact.PhoneNumbersAsStrings.Add(nextNumber);
                        }
                    }
                    if(nextNormNumber != null) {
                        if(!latestContact.PhoneNumbersAsStrings.Contains(nextNormNumber)) {
                            latestContact.PhoneNumbersAsStrings.Add(nextNormNumber);
                        }
                    }
                } else {
                    // Now it is either the first contact or the name has changed. In the first case,
                    // there has been no contact created yet, the latestContact is null. Otherwise
                    // the latestContact is created and contains all numbers.
                    if(latestContact != null) {
                        allContacts.Add(latestContact);
                    }
                    string number = dataCursor.GetString(dataCursor.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                    string normNumber = dataCursor.GetString(dataCursor.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.NormalizedNumber));
                    List<string> numbers = new List<string>(2);
                    if(number != null) {
                        numbers.Add(number);
                    }
                    if(normNumber != null) {
                        numbers.Add(normNumber);
                    }
                    // Update latest contact
                    latestContact = new SUGARContact(currentContactName, numbers, new List<long>(0)/*We don't need longs in Android*/);
                    latestContactName = currentContactName;
                }
            }

            dataCursor.Close();
            return allContacts.ToArray();
        }




        private ICursor GetDataCursor() {
            string[] dataProjection = {
                "display_name" /*Android documentation says this column exists*/,
                ContactsContract.CommonDataKinds.Phone.Number,
                ContactsContract.CommonDataKinds.Phone.NormalizedNumber,
                ContactsContract.DataColumns.Mimetype,
            };
            string dataSelection = "(" + ContactsContract.DataColumns.Mimetype + " =?)";
            string[] dataSelectionArgs = {
                ContactsContract.CommonDataKinds.Phone.ContentItemType
            };
            string dataSortOrder = "display_name";
            return Application.Context.ContentResolver.Query(
                ContactsContract.Data.ContentUri,
                dataProjection,
                dataSelection,
                dataSelectionArgs,
                dataSortOrder);
        }
    }
}
