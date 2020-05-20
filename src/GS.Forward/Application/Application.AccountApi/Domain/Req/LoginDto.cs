using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.AccountApi.Domain.Req
{
    public class LoginDto
    {

        public string Name { get; set; }
        public string Pwd { get; set; }

    }
}
