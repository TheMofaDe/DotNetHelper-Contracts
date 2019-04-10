// ***********************************************************************
// Assembly         : TheMoFaDe:
// Author           : Joseph McNeal Jr 
// Created          : 02-10-2017
//
// Last Modified By : Joseph McNeal Jr
// Last Modified On : 09-04-2017
// ***********************************************************************
// <copyright>
//             Copyright (C) 2017 Joseph McNeal Jr - All Rights Reserved
//             You may use, distribute and modify this code under the
//             terms of the TheMoFaDe: license,
//             You should have received a copy of the TheMoFaDe: license with
//             this file. If not, please write to: josephmcnealjr@mofade.com
//</copyright>
// <summary>   </summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;


//using Microsoft.Data.Sqlite;

namespace DotNetHelper_Contracts.Extension
{
    public static class ExtList
    {
        /// <summary>
        /// Method Name Pretty Much Says It All
        /// </summary> 
        /// <param name="source"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this List<T> source, Func<T,bool> whereClause = null)
        {
            if(whereClause == null) return source == null || !source.Any();
            return source == null || !source.Any(whereClause);
        }


        public static List<T> GetRandomItems<T>(this List<T> list, int numberToReturn)
        {
            var rand = new Random();
            return list.OrderBy(c => rand.Next()).Select(c => c).Take(numberToReturn).ToList();

        }

  


        public static List<List<T>> BatchIntoGroups<T>(this List<T> source, int numberOfGroup)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / numberOfGroup)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        /// <summary>
        /// return a list of the source in groups of specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="maxNumberOfItemsInPerGroup"></param>
        /// <returns></returns>
        public static List<IGrouping<int, T>> BatchIntoGroupsWithMax<T>(this List<T> source, int maxNumberOfItemsInPerGroup)
        {
            if(maxNumberOfItemsInPerGroup <= 0) return new List<IGrouping<int, T>>(){};
            return source.Select((x, i) => new { x, i }).GroupBy(p => (p.i / maxNumberOfItemsInPerGroup), (p => p.x)).ToList();
        }

        
    }













    public static class ExtIEnumerable
    {

        /// <summary>
        /// Method Name Pretty Much Says It All
        /// </summary> 
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this IEnumerable source)
        {
            return source == null || !source.GetEnumerator().MoveNext();
        }


        // are there any common values between a and b?
        public static bool ContainAnySameItem(this IEnumerable<string> a, IEnumerable<string> b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            return a.Intersect(b).Any();
        }


        internal static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ReadOnlyCollection<T>(enumerable.ToList());
        }

        internal static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T item)
        {
            return enumerable.Concat(item.AsEnumerable());
        }

        internal static IEnumerable<T> AsEnumerable<T>(this T item)
        {
            yield return item;
        }

    }





    public static class ExtCollection
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this ICollection source)
        {
            return source == null || source.Count <= 0;
        }

        public static ICollection<T> ForEach<T>(this ICollection<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
            return collection;
        }

        public static void AddRange<T>(this ICollection<T> collection, ICollection<T> collectionToAdd)
        {
            collectionToAdd.ForEach(collection.Add);
        }

        public static void AddRange<T>(this IDataParameterCollection collection, IEnumerable<T> collectionToAdd)
        {
            var toAdd = collectionToAdd as IList<T> ?? collectionToAdd.ToList();
            for (var i = 0; i < toAdd.ToList().Count(); i++)
            {
                collection.Add(toAdd[i]);
            }
        }

        public static void RemoveRange<T>(this ICollection<T> collection, ICollection<T> collectionToRemove)
        {
            collectionToRemove.ForEach((item) => { collection.Remove(item); });
        }


        public static string ParamToSql(this IDataParameterCollection list, string query)
        {
            var sql = query.Clone().ToString();
            // Convert Query To Human Readable  
            var temp = new List<DbParameter>() { };
            for (var i = 0; i < list.Count; i++)
            {
                temp.Add(list[i] as DbParameter);
            }
            temp = temp.OrderByDescending(x => x.ParameterName).ToList();
            temp.ForEach(delegate (DbParameter parameter)
            {
                var name = parameter.ParameterName;
                sql = sql.Replace(name.StartsWith("@") ? $"{name}" : $"@{name}", CommandToSQl(parameter.Value, Encoding.UTF8));
            });

            return sql;

        }



        private static string CommandToSQl(object obj,Encoding encoding)
        {

            if (obj == null || obj == DBNull.Value)
            {
                return "NULL";
            }
            else if (obj is byte[] value1)
            {
                return value1.Length <= 0 ? $@"NULL" : $@"'{encoding.GetString(value1)}'";
            }
            else if (obj is string)
            {
                var value = obj.ToString();
                return $@"'{value.Replace("'", "''")}'"; // escape single quotes
            }
            else if (obj is int?)
            {
                var value = obj as int?;
                return $@"{value}";
            }
            else if (obj is float?)
            {
                var value = obj as float?;
                return $@"{value}";
            }
            else if (obj is DateTime?)
            {
                var value = obj as DateTime?;
                return value == DateTime.MinValue ? $"NULL" : $@"'{value:s}'";
            }

            else if (obj is bool?)
            {
                var value = obj as bool?;
                return (bool)value ? $"1" : $"0"; //$@"{value}";

            }
            else if (obj is Guid?)
            {
                var value = obj as Guid?;
                return $@"CAST('{value}' AS UNIQUEIDENTIFIER)";

            }

            else
            {
                // We Convert Non System Types To Json
                return $"NULL";

            }

        }


    }
}
