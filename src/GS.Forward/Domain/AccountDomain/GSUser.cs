using System;
using System.Collections.Generic;
using System.Text;

namespace AccountDomain
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/19/2020 3:11:42 PM
    /// @source : 
    /// @des : 用户信息
    /// </summary>
    public class GSUser
    {
        public int Id { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string NickName { get; set; }
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

        public DateTime CreateTime { get; set; }

        public DateTime LoginTime { get; set; }

    }
}
