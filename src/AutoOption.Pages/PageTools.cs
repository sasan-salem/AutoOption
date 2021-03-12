using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace AutoOption.Pages
{
    public static class PageTools
    {
        private static Dictionary<string, List<SelectListItem>> _selectItemsHolder;

        /// <summary>
        /// Extracts List of OptionEntity from IFormCollection
        /// </summary>
        public static List<OptionEntity> ExtractOption(IFormCollection Inputs)
        {
            var Options = OptionHelper.ReadEntities();

            foreach (var item in Options)
            {
                item.Value = Inputs[item.Key];
            }

            return Options;
        }

        /// <summary>
        /// Converts all enums into a List<SelectListItem> holder and cache it. 
        /// after converting, returns the current OptionEntity as List<SelectListItem>
        /// </summary>
        /// <param name="entity">Current OptionEntity</param>
        /// <param name="entities">All OptionEntity</param>
        public static List<SelectListItem> CreateEnum(OptionEntity entity, List<OptionEntity> entities)
        {
            if (_selectItemsHolder != null)
                return RefreshHolder(entity);

            _selectItemsHolder = new Dictionary<string, List<SelectListItem>>();

            var type = OptionHelper.Type;

            var optionEnums = type.GetProperties().Where(t => t.PropertyType.IsEnum);

            foreach (var entityEnum in entities.Where(e => e.Type == "Enum").ToList())
            {
                foreach (var optionEnum in optionEnums)
                {
                    if (optionEnum.Name == entityEnum.Key)
                    {
                        _selectItemsHolder.Add(entityEnum.Key, EnumToSelectedListItem(optionEnum.PropertyType, entity));
                        break;
                    }
                }
            }

            return _selectItemsHolder[entity.Key];
        }

        // Privates

        /// <summary>
        /// When _selectItemsHolder has data, we use it instead of recreating SelectListItems.
        /// but maybe the selected item has been changed in every enum. so we should refresh the holder
        /// </summary>
        private static List<SelectListItem> RefreshHolder(OptionEntity entity)
        {
            _selectItemsHolder[entity.Key].ForEach(a =>
            {
                if (a.Value == entity.Value)
                    a.Selected = true;
                else
                    a.Selected = false;
            });
            return _selectItemsHolder[entity.Key];
        }

        private static List<SelectListItem> EnumToSelectedListItem(Type enumType, OptionEntity entity)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var value = item.GetValue();
                selectListItems.Add(new SelectListItem()
                {
                    Text = item.GetDisplay(),
                    Value = value,
                    Selected = entity.Value == value
                });
            }
            return selectListItems;
        }

        private static string GetValue(this FieldInfo propertyInfo) =>
            ((int)propertyInfo.GetValue(null)).ToString();

        private static string GetDisplay(this FieldInfo propertyInfo)
        {
            var customAttributes = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (customAttributes.Length == 0)
                return propertyInfo.Name;
            else
                return customAttributes.Cast<DisplayAttribute>().Single().Name;
        }
    }
}
