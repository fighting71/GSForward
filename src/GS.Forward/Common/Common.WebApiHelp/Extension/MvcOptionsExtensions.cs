using Common.WebApiHelp.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Common.WebApiHelp.Extension
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/18/2020 11:22:39 AM
    /// @source : 
    /// @des : 
    /// </summary>
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// 添加统一前缀
        /// </summary>
        /// <param name="options"></param>
        /// <param name="provider"></param>
        public static void UseCentralRoutePrefix(this MvcOptions options, IRouteTemplateProvider provider)
        {
            options.Conventions.Insert(0, new RoutePrefixConvention(provider));
        }

    }
}
