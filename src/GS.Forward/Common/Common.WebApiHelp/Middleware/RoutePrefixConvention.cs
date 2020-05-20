using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace Common.WebApiHelp.Middleware
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/18/2020 10:56:12 AM
    /// @source : https://blog.csdn.net/sD7O95O/article/details/82837031
    /// @des : 配置路由前缀
    /// </summary>

    public class RoutePrefixConvention : IApplicationModelConvention
    {

        /// <summary>
        /// 定义一个路由前缀变量
        /// </summary>

        private readonly AttributeRouteModel _centralPrefix;

        /// <summary>
        /// 调用时传入指定的路由前缀
        /// </summary>
        /// <param name="routeTemplateProvider"></param>
        public RoutePrefixConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        public void Apply(ApplicationModel application)
        {
            //遍历所有的 Controller
            foreach (var controller in application.Controllers)
            {

                controller.Selectors.Where(x => x.AttributeRouteModel != null).Select(u =>
                {
                    u.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix, u.AttributeRouteModel);
                    return u;
                });

                // 1、已经标记了 RouteAttribute 的 Controller
                //这一块需要注意，如果在控制器中已经标注有路由了，则会在路由的前面再添加指定的路由内容。
                //var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();

                //if (matchedSelectors.Any())
                //{

                //    foreach (var selectorModel in matchedSelectors)
                //    {
                //        // 在 当前路由上 再 添加一个 路由前缀
                //        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
                //            selectorModel.AttributeRouteModel);
                //    }

                //}

                //2、 没有标记 RouteAttribute 的 Controller
                //var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();

                //if (unmatchedSelectors.Any())
                //{
                //    foreach (var selectorModel in unmatchedSelectors)
                //    {
                //        // 添加一个 路由前缀
                //        selectorModel.AttributeRouteModel = _centralPrefix;
                //    }

                //}

            }
        }
    }
}
