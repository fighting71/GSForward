using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.AccountApi.Domain.Req;
using Common.GrpcLibrary;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.QuestionApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {

        private readonly ILogger<HelloController> _logger;

        public HelloController(ILogger<HelloController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "this is account api";
        }

        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet("/health")]
        public IActionResult Heathle()
        {
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<object> Login([FromServices] AccountLib.AccountLibClient client,[FromBody]LoginDto dto)
        {

            LoginRes res = await client.LoginAsync(new LoginReq() { LoginUser = dto.Name, LoginPwd = dto.Pwd });

            return res;
        }

    }
}
