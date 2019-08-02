using System.Collections;

namespace HSPD_API.Models
{
    public interface IContactRepository
    {
        IEnumerable GetAllContacts();
        Contact GetContact(string id);
        Contact AddContact(Contact item);
        bool RemoveContact(string id);
        bool UpdateContact(string id, Contact item);
    }
}
