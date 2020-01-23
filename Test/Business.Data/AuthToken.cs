using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Data
{
   public class AuthToken
    {
        public int Id { get; set; }
        public string  Phone { get; set; }
        public string  Password { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
