using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contacts.Repositories
{
    public interface IAuthTokenRepository
    {
        int Add(DTO.AuthToken token);
        DTO.AuthToken Get(string phone, string token);
    }
}
