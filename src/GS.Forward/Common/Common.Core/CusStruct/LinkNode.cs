using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core.CusStruct
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/22/2020 11:25:06 AM
    /// @source : 
    /// @des : 单向链表
    /// </summary>
    public class LinkNode<T, TFlag>
    {
        public T Val { get; set; }

        /// <summary>
        /// 标记值
        /// </summary>
        public TFlag Flag { get; set; }
        public LinkNode<T, TFlag> Next { get; set; }

        public LinkNode() { }

        public LinkNode(T val)
        {
            Val = val;
        }

        public LinkNode(TFlag flag)
        {
            Flag = flag;
        }
    }
}
