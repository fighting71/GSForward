using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.AccountApi.Middleware
{
    /// <summary>
    /// 生成枚举文档说明
    /// </summary>
    public class EnumDocumentFilter : IDocumentFilter
    {

        private Dictionary<string, string> _cache = new Dictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {

            // 枚举所在程序集
            Assembly assembly = typeof(HttpStatusCode).Assembly;

            IDictionary<string, OpenApiSchema> schemas = swaggerDoc.Components.Schemas;

            foreach (var item in schemas)
            {

                var property = item.Value;

                if (property.Enum != null && property.Enum.Count > 0)
                {

                    if (_cache.ContainsKey(item.Key))
                    {
                        property.Description = _cache[item.Key];
                    }
                    else
                    {

                        Type type = assembly.GetType(item.Key);

                        property.Description = DescribeEnum(property.Enum, type);

                        _cache[item.Key] = property.Description;

                    }

                    if (property.Description != null)
                        property.Enum.Clear();

                }


            }

        }

        private string DescribeEnum(IEnumerable<IOpenApiAny> enums, Type type)
        {
            if (type == null) return null;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(type.GetCustomAttribute<DescriptionAttribute>()?.Description);
            foreach (var enumOption in enums)
            {
                if (enumOption is OpenApiInteger integer)
                {
                    builder.AppendLine($"<br/>{integer.Value} - {GetDescription(type, Enum.GetName(type, integer.Value))};");
                }
            }

            return builder.ToString();
        }

        private string GetDescription(Type type, string name)
        {
            foreach (MemberInfo mInfo in type.GetMember(name))
            {
                MemberInfo[] memberInfos = type.GetMember(name);
                foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                {
                    if (attr.GetType() == typeof(DescriptionAttribute))
                    {
                        return ((DescriptionAttribute)attr).Description;
                    }
                }
            }
            return name;
        }

    }
}
