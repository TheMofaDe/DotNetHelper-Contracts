using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;


namespace DotNetHelper_Contracts.Extension
{
    public static class ExtDictionary
    {
  



        /// <summary>
        /// create key with value if not exist otherwise update value for key
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>return  new value </returns>
        public static V AddOrUpdate<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
            return value;
        }

#if NETFRAMEWORK
        public static V GetValueOrDefault<K, V>(this IDictionary<K, V> dictionary, K key, V defaultValue = default)
        {
            if (dictionary.ContainsKey(key))
            {
                if (dictionary.TryGetValue(key, out var a))
                    return a;
            }
            return defaultValue;
        }
#else

        public static V GetValueOrDefaultValue<K, V>(this IDictionary<K, V> dictionary, K key, V defaultValue = default)
        {
            if (dictionary.ContainsKey(key))
            {
                if (dictionary.TryGetValue(key, out var a))
                    return a;
            }
            return defaultValue;
        }

#endif
        public static void AddOrUpdateHighestValue<K>(this IDictionary<K, int> dictionary, K key, int value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.TryGetValue(key, out var currentValue);
                if (value > currentValue)
                    dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
        public static void AddOrUpdateLowestValue<K>(this IDictionary<K, int> dictionary, K key, int value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.TryGetValue(key, out var currentValue);
                if (value < currentValue)
                    dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }


        public static Dictionary<K, V> Clone<K, V>(this IDictionary<K, V> dictionary)
        {
            return dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }


        public static IDataReader ToDataReader(this IDictionary<string, object> dictionary)
        {
            return dictionary.MapToDataTable().CreateDataReader();
            //  TODO :: This should work for non-dynamic types so check if type is dynamic if false use this return new DictionaryDataReader(dictionary);
        }
        public static DataTable MapToDataTable(this IDictionary<string, object> dictionary, string tableName = null)
        {
            var dataTable = new DataTable(tableName);
            for (var i = 0; i < dictionary.Keys.Count; i++)
            {
                dataTable.Columns.Add(dictionary.Keys.ToList()[i], dictionary.Values.ToList()[i].GetType());
            }
            var dataRow = dataTable.NewRow();
            dataRow.ItemArray = dictionary.Values.Select(a => a ?? DBNull.Value).ToArray();
            dataTable.Rows.Add(dataRow);
            return dataTable;
        }



        /// IOrderedDictionary
        public static T GetKey<T>(this IOrderedDictionary dictionary, int index)
        {
            if (dictionary == null)
            {
                return default;
            }

            try
            {
                return (T)dictionary.Cast<DictionaryEntry>().ElementAt(index).Key;
            }
            catch (ArgumentOutOfRangeException)
            {
                return default;
            }
        }

        public static U GetValue<T, U>(this IOrderedDictionary dictionary, T key)
        {
            if (dictionary == null)
            {
                return default;
            }

            try
            {
                return (U)dictionary.Cast<DictionaryEntry>().AsQueryable().Single(kvp => ((T)kvp.Key).Equals(key))
                    .Value;
            }
            catch (InvalidCastException)
            {
                return default;
            }

        }


        public static V AddIfNotExist<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.TryGetValue(key, out var v);
                return v;
            }
            else
            {
                dictionary.Add(key, value);
                return value;
            }

        }






    }
}
