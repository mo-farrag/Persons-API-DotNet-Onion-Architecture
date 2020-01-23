using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contacts.Repositories
{
    public interface ICountryRepository
    {
         bool IsCountryCodeExists(string countrCode);
    }
}
