using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeProject.ObjectPool;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.ConnectionPool;

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

        [HttpGet(nameof(TestRedis))]
        public string TestRedis([FromServices] ObjectPool<PooledConnectionMultiplexer> pool)
        {
            var key = "test.key";

            Parallel.For(0, 1_000_000, (num) =>
            {
                pool.GetObject().GetDatabase().StringGet(key);
            });

            return "success";
        }

        [HttpGet]
        public string Get()
        {
            return "this is question api";
        }

        [HttpGet("/health")]
        public IActionResult Heathle()
        {
            return Ok();
        }

    }
}
