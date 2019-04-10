
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using DotNetHelper_Contracts.Extension;

namespace DotNetHelper_Contracts.Helpers
{
    public static class DynamicObjectHelper
    {

        public static void AddProperty(ExpandoObject expandoObject, string propertyName, object value)
        {
            var x = expandoObject as IDictionary<string, object>;
            if (x.Keys.Contains(propertyName))
            {
                x[propertyName] = value;   
            }
            x.Add(propertyName, value);
        }

        public static void RemoveProperty(ExpandoObject expandoObject, string propertyName)
        {
            if (!(expandoObject is IDictionary<string, object> x)) return;
            if (x.Keys.Contains(propertyName))
            {
                x.Remove(propertyName);
            }
        }

        public static void AddPropertyChangedHandler(ExpandoObject expandoObject, PropertyChangedEventHandler action)
        {
            ((INotifyPropertyChanged)expandoObject).PropertyChanged += action;
        }

        public static IDataReader ToDataReader(ExpandoObject expandoObject)
        {
            var x = expandoObject as IDictionary<string, object>;
            return x.MapToDataTable().CreateDataReader();
        }


        public static DataTable ToDataTable(ExpandoObject expandoObject)
        {
            var x = expandoObject as IDictionary<string, object>;
            return x.MapToDataTable();
        }

        /// <summary>
        /// return a dictionary of properties and values
        /// </summary>
        /// <param name="expandoObject"></param>
        /// <returns></returns>
        public static IDictionary<string, object> GetProperties(ExpandoObject expandoObject)
        {
            var x = expandoObject as IDictionary<string, object>;
            return x;
        }


        public static object GetPropertyValue(ExpandoObject expandoObject, string propertyName, bool searchAllChildrens = true)
        {
            propertyName.IsNullThrow(nameof(propertyName));

            var props = GetProperties(expandoObject);
            if (props != null)
            {
                if (props.ContainsKey(propertyName))
                {
#if NETFRAMEWORK
                    return props.GetValueOrDefault(propertyName);
#else
                    return props.GetValueOrDefaultValue(propertyName);
#endif
                }
                foreach (var x in props)
                {
                    if (x.Value is ExpandoObject x1)
                    {
                        var z = GetPropertyValue(x1, propertyName);
                        if (z == null) continue;
                        return z;
                    }
                }
            }
            return null;
        }

       
    }
    /// <summary>
    /// Provides various reflection-related methods.
    /// </summary>
    public static class Reflect
    {
        /// <summary>
        /// Returns the name of the property referred to by the specified property expression.
        /// </summary>
        /// <typeparam name="TSource">
        /// Type containing the property.
        /// </typeparam>
        /// <typeparam name="TProperty">
        /// Type of the property.
        /// </typeparam>
        /// <param name="propertyExpression">
        /// An <see cref="Expression"/> representing a Func mapping an instance of type TSource
        /// to an instance of type TProperty.
        /// </param>
        /// <returns>
        /// The name of the property referred to by the specified property expression.
        /// </returns>
        public static string GetPropertyName<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression), "propertyExpression should not be null.");
            }

            return GetPropertyInfo<TSource, TProperty>(propertyExpression).Name;
        }

        /// <summary>
        /// Returns a <see cref="PropertyInfo"/> instance for the property referred to by the specified
        /// property expression.
        /// </summary>
        /// <typeparam name="TSource">
        /// Type containing the property.
        /// </typeparam>
        /// <typeparam name="TProperty">
        /// Type of the property.
        /// </typeparam>
        /// <param name="propertyExpression">
        /// An <see cref="Expression"/> representing a Func mapping an instance of type TSource to an instance
        /// of type TProperty.
        /// </param>
        /// <returns>
        /// A <see cref="PropertyInfo"/> instance for the property referred to by the specified
        /// property expression.
        /// </returns>
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression), "propertyExpression should not be null.");
            }

            var sourceType = typeof(TSource);

            if (!(propertyExpression.Body is MemberExpression member))
            {
                var message = $"Expression '{propertyExpression}' refers to a method, not a property.";
                throw new ArgumentException(message);
            }

            var propertyInfo = member.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                var message = $"Expression '{propertyExpression}' refers to a field, not a property.";
                throw new ArgumentException(message);
            }

            return propertyInfo;
        }
    }

}
