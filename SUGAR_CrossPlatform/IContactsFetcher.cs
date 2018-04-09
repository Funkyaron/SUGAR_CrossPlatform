using System;
namespace SUGAR_CrossPlatform
{
    public interface IContactsFetcher
    {
        SUGARContact[] GetAllContacts();
    }
}
