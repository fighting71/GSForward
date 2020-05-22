using System.Threading.Tasks;
using Application.AccountApi.Domain.Req;
using AutoMapper;
using Common.GrpcLibrary;
using Microsoft.AspNetCore.Mvc;

namespace Application.QuestionApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {

        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet("/health")]
        public IActionResult Heathle()
        {
            return Ok();
        }

    }
}
