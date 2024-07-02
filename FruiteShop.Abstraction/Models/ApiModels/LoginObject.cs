using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models.ApiModels
{
    public class LoginObject
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }

    public class AuthenticateResponse
    {
        public bool Status { get; set; }

        public dynamic Data { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }

    public class JwtRequest
    {
        public int Id { get; set; }

        public string Token { set; get; }

        public string RefreshToken { get; set; }
    }
}
