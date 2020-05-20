using AccountDomain;
using AutoMapper;
using Common.GrpcLibrary;
using Grpc.Core;
using GS.AppContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Services
{
    public class InfoService : AccountLib.AccountLibBase
    {
        private readonly GSDbContext dbContext;
        private readonly IMapper mapper;

        public InfoService(GSDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public async override Task<Common.GrpcLibrary.Single.Types.BoolData> IsExists(Common.GrpcLibrary.Single.Types.StringData request, ServerCallContext context)
        {

            var res = dbContext.Users.Any(u=>u.Name.Equals(request.Data,StringComparison.InvariantCultureIgnoreCase));

            return new Common.GrpcLibrary.Single.Types.BoolData() { Data = res};

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

            dbContext.Users.Add(user);

            return new RegisterRes() { ID = user.Id, IsSuccess = true };

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
