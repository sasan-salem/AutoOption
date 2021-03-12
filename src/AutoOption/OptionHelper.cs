using AutoOption.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AutoOption
{
    /// <summary>
    /// Helper class for everithing end-user needs.
    /// </summary>
    public static class OptionHelper
    {
        private static object option;
        private static HandleOption handleOption;
        public static Type Type { get; set; }

        /// <summary>
        /// Configure your AutoOption
        /// </summary>
        /// <param name="type">Option class. The class that stores your options</param>
        /// <param name="dbWrapper">The class which you specify what data source you are using with.</param>
        /// <remarks>
        /// Call once in startup your application
        /// </remarks>
        public static void Config(Type type, IDatabaseWrapper dbWrapper)
        {
            Type = type;
            handleOption = new HandleOption(type, dbWrapper);
            handleOption.Register();
        }

        /// <summary>
        /// Represent Option class with cached value.
        /// </summary>
        /// <typeparam name="T">Option class. the class that stores your options</typeparam>
        /// <returns>Option class. the class that stores your options</returns>
        public static T Get<T>() where T : new()
        {
            if (option == null)
                option = handleOption.GetOption<T>();
            return (T)option;
        }

        /// <summary>
        /// Changes the Options in the middle of code
        /// </summary>
        public static void Set<T>(Expression<Func<T>> expr, T value)
        {
            string key = ((MemberExpression)expr.Body).Member.Name;

            handleOption.UpdateEntities(new List<OptionEntity>()
            {
                new OptionEntity()
                {
                    Key = key,
                    Value = value.ToString()
                }
            });
        }

        /// <summary>
        /// Cleans the cache
        /// </summary>
        public static void ResetOption() => option = null;

        /// <summary>
        /// Gives access to stored data in option table
        /// </summary>
        public static List<OptionEntity> ReadEntities() => handleOption.ReadEntities();

        /// <summary>
        /// You can update entites in option table
        /// </summary>
        public static void UpdateEntities(List<OptionEntity> OptionEntities)
        {
            handleOption.UpdateEntities(OptionEntities);
            ResetOption();
        }
    }
}
