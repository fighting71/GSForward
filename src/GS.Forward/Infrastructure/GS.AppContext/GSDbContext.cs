using AccountDomain;
using Common.MySqlProvide.CusStruct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GS.AppContext
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/20/2020 5:51:56 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public class GSDbContext
    {

        public IData<GSUser> Users { get; set; }

    }
}
