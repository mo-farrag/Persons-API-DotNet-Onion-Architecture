using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AuthToken
    {
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }

    public class AuthTokenRequest
    {

        [Required(ErrorMessage = "blank")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "blank")]
        public string Password { get; set; }
    }

    public class VerifyAuthTokenRequest
    {

        [Required(ErrorMessage = "blank")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "blank")]
        public string Token { get; set; }
    }

    public class AuthTokenResponse
    {
        public AuthTokenResponse()
        {
            Errors = new Error();
        }
        public string Token { get; set; }
        public Error Errors { get; set; }
    }

    public class VerifyAuthTokenResponse
    {
        public VerifyAuthTokenResponse()
        {
            Errors = new Error();
        }
        public AuthTokenResult Result { get; set; }
        public Error Errors { get; set; }
    }
}
