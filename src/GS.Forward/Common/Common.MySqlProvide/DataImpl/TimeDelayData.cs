using Common.MySqlProvide.CusStruct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Common.MySqlProvide.DataImpl
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/20/2020 5:56:16 PM
    /// @source : 
    /// @des : 延时加载
    /// TODO: impl
    /// </summary>
    public class TimeDelayData<T> : IData<T>
    {
        private readonly IGenerateSql generate;

        public TimeDelayData(IGenerateSql generate) {
            this.generate = generate;
        }

        public void Add(T t)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
