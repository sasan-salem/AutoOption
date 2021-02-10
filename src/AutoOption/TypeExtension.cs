using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace AutoOption
{
    internal static class TypeExtension
    {
        internal static List<OptionEntity> ExtractKeysFromType(this Type type)
        {
            var optionRows = new List<OptionEntity>();

            foreach (var item in type.GetProperties())
            {
                optionRows.Add(new OptionEntity()
                {
                    Key = item.Name,
                    Display = item.GetDisplay(),
                    Type = item.PropertyType.Name
                });
            }
            return optionRows;
        }

        internal static string GetDisplay(this PropertyInfo propertyInfo) =>
            propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>().Single().Name;

        internal static object Convert(this PropertyInfo propertyInfo, string value) =>
            Type.GetTypeCode(propertyInfo.PropertyType) switch
            {
                TypeCode.Int32 => string.IsNullOrEmpty(value) ? 0 : int.Parse(value),
                TypeCode.Boolean => string.IsNullOrEmpty(value) ? false : bool.Parse(value),
                _ => value
            };
    }
}