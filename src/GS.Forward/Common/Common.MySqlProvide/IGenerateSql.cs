using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Common.MySqlProvide
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/20/2020 4:30:54 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public interface IGenerateSql
    {

        string Explain(Expression expression);

    }

    public interface IFrommGnerate : IGenerateSql { }
    public interface ISelectGnerate : IGenerateSql { }
    public interface IWhereGnerate : IGenerateSql { }
    public interface IWhereParameterGenerate 
    {
        string Explain(Expression expression,IDictionary<string,object> param);
    }
    public interface IEditGenerate 
    {
        StringBuilder CreateCommandString<T>(T data);

        StringBuilder UpdateCommandString<T>(T data, IDictionary<string, object> dic);

    }

}
