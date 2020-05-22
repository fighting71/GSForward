using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.AccountApi.Domain.Req;
using AutoMapper;
using Common.GrpcLibrary;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.QuestionApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly IMapper mapper;

        public AccountController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<object> Login([FromServices] AccountLib.AccountLibClient client, [FromBody]LoginDto dto)
        {

            LoginRes res = await client.LoginAsync(mapper.Map<LoginReq>(dto));

            if (res.AccountID > 0) return "login success member-" + res.AccountID;
            else return "userName or Pwd error!";
        }

        [HttpPost("Register")]
        public async Task<object> Register([FromServices] AccountLib.AccountLibClient client,[FromBody]RegisterDto dto)
        {

            Common.GrpcLibrary.Single.Types.BoolData res = await client.IsExistsAsync(new Common.GrpcLibrary.Single.Types.StringData() { Data = dto.Name});

            if (res.Data)
            {
                return $"username : '{dto.Name}' has been created!!!";
            }

            RegisterRes registerRes = await client.RegisterAsync(mapper.Map<RegisterReq>(dto));

            return registerRes;
        }

    }
}
