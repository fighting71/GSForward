using Common.MySqlProvide;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

namespace GS.AppContext.Impl
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/21/2020 3:09:14 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public partial class MySqlDbContext 
    {
        #region members&ctro

        private readonly IFrommGnerate formGnerate;
        private readonly ISelectGnerate selectGnerate;
        private readonly IWhereParameterGenerate whereGnerate;
        private readonly IEditGenerate editGenerate;
        private readonly DbConnection conn;
        private readonly ILogger<MySqlDbContext> logger;

        public MySqlDbContext(IFrommGnerate formGnerate, ISelectGnerate selectGnerate, IWhereParameterGenerate whereGnerate, IEditGenerate editGenerate
            , DbConnection conn, ILogger<MySqlDbContext> logger)
        {
            this.formGnerate = formGnerate;
            this.selectGnerate = selectGnerate;
            this.whereGnerate = whereGnerate;
            this.editGenerate = editGenerate;
            this.conn = conn;
            this.logger = logger;
        }

        #endregion

        #region help method

        private StringBuilder QueryString<TSource, TResult>(
        ISelectGnerate selectGnerate, IFrommGnerate formGnerate, IWhereParameterGenerate whereGnerate,
        Expression<Func<TSource, TResult>> selectExpression, Expression formExpression, Expression<Func<TSource, bool>> whereExpression,
        IDictionary<string,object> dic)
        {

            StringBuilder build = new StringBuilder();

            build.AppendLine(selectGnerate.Explain(selectExpression));
            build.AppendLine(formGnerate.Explain(formExpression));
            build.AppendLine(whereGnerate.Explain(whereExpression, dic));

            return build;
        }

        private void Log(string content)
        {
            logger.LogInformation(content);
        }

        #endregion

    }
}
