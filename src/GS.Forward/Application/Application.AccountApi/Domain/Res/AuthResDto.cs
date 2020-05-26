using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.AccountApi.Domain.Res
{
    public class AuthResDto
    {

        public string Message { get; set; }

        public bool Success { get; set; }

        public string Token { get; set; }

    }
}
