using AutoOption.Database.Interface;
using System;
using System.Collections.Generic;

namespace AutoOption
{
    /// <summary>
    /// Helper class for everithing end-user needs.
    /// </summary>
    public static class OptionHelper
    {
        private static object option;
        private static HandleOption handleOption;

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
            handleOption = new HandleOption(type, dbWrapper);
            handleOption.Register();
        }

        /// <summary>
        /// Represent Option class with cached value.
        /// </summary>
        /// <typeparam name="T">Option class. the class that stores your options</typeparam>
        /// <returns>Option class. the class that stores your options</returns>
        public static T GetOption<T>() where T : new()
        {
            if (option == null)
                option = handleOption.GetOption<T>();
            return (T)option;
        }

        /// <summary>
        /// Cleans the cache
        /// </summary>
        public static void ResetOption() => option = null;

        /// <summary>
        /// Gives access to stored data in option table
        /// </summary>
        /// <remarks>
        /// You can get list of OptionRecord for display and set to store data in table
        /// </remarks>
        public static List<OptionEntity> OptionEntities
        {
            get => handleOption.ReadEntities();
            set => handleOption.UpdateEntities(value);
        }
    }
}
