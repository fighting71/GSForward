using System.Threading.Tasks;
using Application.AccountApi.Domain.Config;
using Application.AccountApi.Domain.Req;
using Application.AccountApi.Domain.Res;
using AutoMapper;
using Common.Core;
using Common.GrpcLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Application.QuestionApi.Controllers
{
    /// <summary>
    /// 账号中心
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper"></param>
        public AccountController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<AuthResDto> Login([FromServices] AccountLib.AccountLibClient client, [FromServices] IOptionsMonitor<AuthAESConfig> options, [FromBody]LoginDto dto)
        {

            LoginRes res = await client.LoginAsync(mapper.Map<LoginReq>(dto));

            if (res.AccountID > 0)
            {
                return new AuthResDto()
                {
                    Success = true,
                    Token = EncryptionHelp.AESEncrypt(res.AccountID.ToString(), options.CurrentValue.Key, options.CurrentValue.SaltBytes)
                };
            }
            else
            {
                return new AuthResDto()
                {
                    Success = false,
                    Message = res.Message
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<AuthResDto> Register([FromServices] AccountLib.AccountLibClient client, [FromServices] IOptionsMonitor<AuthAESConfig> options, [FromBody]RegisterDto dto)
        {

            Common.GrpcLibrary.Single.Types.BoolData res = await client.IsExistsAsync(new Common.GrpcLibrary.Single.Types.StringData() { Data = dto.Name });

            if (res.Data)
            {
                return new AuthResDto()
                {
                    Success = false,
                    Message = $"username : '{dto.Name}' has been created!!!",
                };
            }

            RegisterRes registerRes = await client.RegisterAsync(mapper.Map<RegisterReq>(dto));

            var authRes = new AuthResDto()
            {
                Success = registerRes.IsSuccess,
                Message = registerRes.Error
            };

            if (registerRes.ID > 0)
            {
                authRes.Token = EncryptionHelp.AESEncrypt(registerRes.ID.ToString(), options.CurrentValue.Key, options.CurrentValue.SaltBytes);
            }
            return authRes;
        }

    }
}
