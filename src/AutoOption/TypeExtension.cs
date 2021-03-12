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
                    Type = item.PropertyType.IsEnum ? "Enum" : item.PropertyType.Name
                });
            }
            return optionRows;
        }

        internal static string GetDisplay(this PropertyInfo propertyInfo)
        {
            var customAttributes = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (customAttributes.Length == 0)
                return propertyInfo.Name;
            else
                return customAttributes.Cast<DisplayAttribute>().Single().Name;
        }

        internal static object Convert(this PropertyInfo propertyInfo, string value) =>
            Type.GetTypeCode(propertyInfo.PropertyType) switch
            {
                TypeCode.Int16 => string.IsNullOrEmpty(value) ? 0 : short.Parse(value),
                TypeCode.UInt16 => string.IsNullOrEmpty(value) ? 0 : ushort.Parse(value),
                TypeCode.Int32 => string.IsNullOrEmpty(value) ? 0 : int.Parse(value),
                TypeCode.UInt32 => string.IsNullOrEmpty(value) ? 0 : uint.Parse(value),
                TypeCode.Int64 => string.IsNullOrEmpty(value) ? 0 : long.Parse(value),
                TypeCode.UInt64 => string.IsNullOrEmpty(value) ? 0 : ulong.Parse(value),
                TypeCode.Single => string.IsNullOrEmpty(value) ? 0 : float.Parse(value),
                TypeCode.Double => string.IsNullOrEmpty(value) ? 0 : double.Parse(value),
                TypeCode.Decimal => string.IsNullOrEmpty(value) ? 0 : decimal.Parse(value),
                TypeCode.Boolean => string.IsNullOrEmpty(value) ? false : bool.Parse(value),
                _ => value
            };
    }
}