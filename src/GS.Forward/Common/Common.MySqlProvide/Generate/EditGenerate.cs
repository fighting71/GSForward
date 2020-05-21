using Common.MySqlProvide.CusAttr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.MySqlProvide.Generate
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/21/2020 4:00:13 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public class EditGenerate : IEditGenerate
    {

        private static bool IsDefaultValue<T>(T val)
        {
            var res = val.Equals(default(T));

            return res;
        }

        private static readonly MethodInfo GetDefaultMethod = typeof(EditGenerate).GetMethod(nameof(IsDefaultValue), BindingFlags.NonPublic | BindingFlags.Static);

        public StringBuilder CreateCommandString<T>(T data)
        {
            StringBuilder builder = new StringBuilder("INSERT INTO ");

            Type type = typeof(T);

            AliasAttribute tableAlias = typeof(T).GetCustomAttribute<AliasAttribute>();

            if (tableAlias == null)
                builder.AppendLine(type.Name);
            else builder.AppendLine(tableAlias.Name);

            var fields = type.GetProperties().Where(u =>
            {
                var value = u.GetValue(data);

                return !(value == null || (bool)GetDefaultMethod.MakeGenericMethod(new[] { u.PropertyType }).Invoke(null, new object[] { value }));
            }).Select(u =>
            {

                var name = u.PropertyType.GetCustomAttribute<AliasAttribute>()?.Name ?? u.Name;

                return new { Key = name, Value = u.Name };
            }).ToArray();

            builder.Append("(");
            builder.Append(string.Join(",", fields.Select(u => u.Key)));
            builder.AppendLine(")");

            builder.Append(" VALUES(");
            builder.Append(string.Join(",", fields.Select(u => $"@{u.Value}")));
            builder.Append(" );");

            return builder;
        }

    }
}
