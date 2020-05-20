using AccountDomain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GS.AppContext
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/19/2020 5:48:48 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public class GSDbContext
    {
        // TODO: customer orm
        public IList<GSUser> Users { get; set; }

    }
}
