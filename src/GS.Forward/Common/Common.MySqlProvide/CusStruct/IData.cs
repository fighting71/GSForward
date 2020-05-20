using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Common.MySqlProvide.CusStruct
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/20/2020 5:54:08 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public interface IData<T> : IEnumerable<T>
    {

        void Add(T t);

    }
}
