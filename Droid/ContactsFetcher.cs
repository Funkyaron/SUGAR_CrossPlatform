using System;
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
            return new SUGARContact[0];
        }
    }
}
