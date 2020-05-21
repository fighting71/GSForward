using AccountDomain;
using AutoMapper;
using Common.GrpcLibrary;
using Grpc.Core;
using GS.AppContext;
using System;
using System.Threading.Tasks;

namespace AccountService.Services
{
    public class InfoService : AccountLib.AccountLibBase
    {
        private readonly IQueryContext queryContext;
        private readonly IMapper mapper;

        public InfoService(IQueryContext queryContext,IMapper mapper)
        {
            this.queryContext = queryContext;
            this.mapper = mapper;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public async override Task<Common.GrpcLibrary.Single.Types.BoolData> IsExists(Common.GrpcLibrary.Single.Types.StringData request, ServerCallContext context)
        {
            var flag = request.Data;
            var res = await queryContext.AnyAsync<GSUser>(u => u.Name == flag);

            return new Common.GrpcLibrary.Single.Types.BoolData() { Data = res };

        }

        public async override Task<LoginRes> Login(LoginReq request, ServerCallContext context)
        {

            await Task.Delay(10);

            return new LoginRes() { AccountID = 1, Message = "success" };

        }

        public async override Task<RegisterRes> Register(RegisterReq request, ServerCallContext context)
        {
            GSUser user = mapper.Map<GSUser>(request);

            user.NickName = user.Name;
            user.LoginTime = user.CreateTime = DateTime.Now;

            var key = await queryContext.AddAndGetKeyAsync(user);

            return new RegisterRes() { ID = key, IsSuccess = true };

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
