using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.AccountApi.Domain.Config
{
    public class AuthAESConfig
    {

        public string Key { get; set; }

        public byte[] SaltBytes { get; set; }

    }
}
