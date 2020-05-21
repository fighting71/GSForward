using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.AccountApi.Domain.Req
{
    public class RegisterDto
    {

        /// <summary>
        /// 登录名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPwd { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Contact { get; set; }
    }
}
