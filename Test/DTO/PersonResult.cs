using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PersonResult
    {
        public PersonResult()
        {
            Errors = new Error();
        }
        public string Result { get; set; }
        public Error Errors { get; set; }
    }
}
