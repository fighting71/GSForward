using AccountDomain;
using AutoMapper;
using Common.Core;
using Common.GrpcLibrary;
using Grpc.Core;
using GS.AppContext;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Services.Account.Services
{
    public class AccountService : AccountLib.AccountLibBase
    {
        private readonly IDbContext queryContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AccountService(IDbContext queryContext, IMapper mapper, IConfiguration configuration)
        {
            this.queryContext = queryContext;
            this.mapper = mapper;
            this.configuration = configuration;
        }


        public async override Task<Common.GrpcLibrary.Single.Types.BoolData> IsExists(Common.GrpcLibrary.Single.Types.StringData request, ServerCallContext context)
        {
            var res = await queryContext.AnyAsync<GSUser>(u => u.Name == request.Data);

            return new Common.GrpcLibrary.Single.Types.BoolData() { Data = res };

        }

        public async override Task<LoginRes> Login(LoginReq request, ServerCallContext context)
        {

            request.LoginPwd = Md5Help.Md5Hash(request.LoginPwd + configuration["PwdHashSuffix"]);

            int id = await queryContext.QueryFirstOrDefaultAsync<GSUser, int>(u => u.Id, u => u.LoginPwd == request.LoginPwd && u.Name == request.LoginUser);

            if(id == default)
            {
                return new LoginRes() { Message = "failure" };
            }

            // refresh loginTime
            await queryContext.UpdateAsync<GSUser>(new GSUser { LoginTime = DateTime.Now }, u => u.Id == id);

            return new LoginRes() { AccountID = id, Message = "success" };

        }

        public async override Task<RegisterRes> Register(RegisterReq request, ServerCallContext context)
        {
            GSUser user = mapper.Map<GSUser>(request);

            user.NickName = user.Name;
            user.LoginTime = user.CreateTime = DateTime.Now;
            user.LoginPwd = Md5Help.Md5Hash(user.LoginPwd + configuration["PwdHashSuffix"]);

            var key = await queryContext.AddAndGetKeyAsync(user);

            return new RegisterRes() { ID = key, IsSuccess = true };

        }

    }
}
