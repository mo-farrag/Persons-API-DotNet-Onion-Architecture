using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contacts
{
    public interface IPersonsRepository
    {
        Data.Person Add(DTO.PersonRequest item);
        Data.Person Get(string phone);
        bool IsEmailExists(string email);
        bool IsPhoneExists(string phone);
    }
}
