using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public enum AuthTokenResult
    {
        NotFound = 0,
        Expired = 1,
        Valid = 2
    }
}
