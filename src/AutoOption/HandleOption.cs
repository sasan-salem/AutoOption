using AutoOption.Database;
using AutoOption.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoOption
{
    internal class HandleOption
    {
        private readonly Type type;
        private readonly IDBQueries dbQueries;
        private readonly string TABLE_NAME = "Options";

        internal HandleOption(Type type, IDatabaseWrapper dbWrapper, IDBQueries dbQueries = null)
        {
            this.type = type;
            this.dbQueries = dbQueries ?? new DBQueries(dbWrapper, TABLE_NAME);
        }

        // Internal Functions //

        /// <summary>
        /// Creates Option table if dose not exist and creates records
        /// </summary>
        internal void Register()
        {
            if (!dbQueries.IsTableExist())
                dbQueries.CreateTable();
            RefreshOption();
        }

        /// <summary>
        /// Represents all record of Option table
        /// </summary>
        /// <returns>List of OptionRecord</returns>
        internal List<OptionEntity> ReadEntities()
        {
            return dbQueries.GetAll<OptionEntity>();
        }

        /// <summary>
        /// Updates all record of Option table
        /// </summary>
        /// <param name="options"></param>
        internal void UpdateEntities(List<OptionEntity> options)
        {
            dbQueries.Update(options);
        }

        /// <summary>
        /// Represents an instanse of Option class with value
        /// </summary>
        /// <typeparam name="T">Option class</typeparam>
        /// <returns>Option class</returns>
        internal T GetOption<T>() where T : new()
        {
            object obj = new T();
            var options = ReadEntities();
            foreach (var item in type.GetProperties())
            {
                var value = options.Single(s => s.Key == item.Name).Value;
                item.SetValue(obj, item.Convert(value));
            }
            return (T)obj;
        }

        // Private Functions //

        /// <summary>
        /// Updates Option table with Option class values
        /// </summary>
        private void RefreshOption()
        {
            List<string> InTableKeys = dbQueries.GetOneColumn("[Key]");
            var optionRows = type.ExtractKeysFromType();
            RemoveIfNotExist(InTableKeys, optionRows);
            AddNewItems(InTableKeys, optionRows);
        }

        /// <summary>
        /// Adds new records to the Option table if there is new properties in the Option class
        /// </summary>
        /// <param name="InTableKeys"></param>
        /// <param name="optionRows"></param>
        private void AddNewItems(List<string> InTableKeys, Dictionary<string, string> optionRows)
        {
            // Remove options that have already been added
            InTableKeys.ForEach(key =>
            {
                if(optionRows.ContainsKey(key))
                    optionRows.Remove(key);
            });

            if (optionRows.Count != 0)
                dbQueries.GroupAdd(optionRows);
        }

        /// <summary>
        /// Deletes extra records from the Option table if not exist in the Option class
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="optionRows"></param>
        private void RemoveIfNotExist(List<string> keys, Dictionary<string, string> optionRows)
        {
            // Get all keys in Table if not exist in Option class
            List<string> deletedKeys = keys.FindAll(key => !optionRows.ContainsKey(key));

            if (deletedKeys.Count != 0)
                dbQueries.GroupDeleteByKeys(deletedKeys, "[Key]");
        }
    }

}
