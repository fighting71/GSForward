using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MySqlProvide.CusAttr
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/20/2020 4:34:15 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public class AliasAttribute : Attribute
    {
        public AliasAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
