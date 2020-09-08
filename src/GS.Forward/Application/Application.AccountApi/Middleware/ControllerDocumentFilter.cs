using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Application.AccountApi.Middleware
{
    /// <summary>
    /// 生成控制器文档说明
    /// </summary>
    public class ControllerDocumentFilter : IDocumentFilter
    {
        private readonly IServiceProvider provider;

        /// <summary>
        /// 
        /// </summary>
        public ControllerDocumentFilter(IServiceProvider provider)
        {
            this.provider = provider;
        }

        private static List<OpenApiTag> _tags;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {

            if (_tags != null)
            {
                swaggerDoc.Tags = _tags;
                return;
            }

            XPathDocument xPathDocument = new XPathDocument(Path.Combine(AppContext.BaseDirectory, "Application.AccountApi.xml"));

            XPathNavigator xPathNavigator = xPathDocument.CreateNavigator();

            Assembly assembly = Assembly.GetExecutingAssembly();

            _tags = new List<OpenApiTag>();

            var baseType = typeof(ControllerBase);

            assembly.GetTypes().Where(u => u.IsClass && !u.IsAbstract && baseType.IsAssignableFrom(u)).Select(u =>
            {

                string value = xPathNavigator.SelectSingleNode($"doc/members/member[@name='T:{u.FullName}']/summary")?.Value;

                if (!string.IsNullOrEmpty(value))
                {
                    var name = u.Name;
                    var len = "Controller".Length;
                    ReadOnlySpan<char> readOnlySpan = name.AsSpan(0, name.Length - len);
                    _tags.Add(new OpenApiTag { Name = readOnlySpan.ToString(), Description = value });
                }

                return u;

            }).ToArray();

            swaggerDoc.Tags = _tags;
        }
    }
}
