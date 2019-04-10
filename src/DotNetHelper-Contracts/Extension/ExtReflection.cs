﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DotNetHelper_Contracts.Extension
{
    /// <summary>
    /// Extensions to help with reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the type from the member.
        /// </summary>
        /// <param name="member">The member to get the type from.</param>
        /// <returns>The type.</returns>
        public static Type MemberType(this MemberInfo member)
        {
            var property = member as PropertyInfo;
            if (property != null)
            {
                return property.PropertyType;
            }

            var field = member as FieldInfo;
            if (field != null)
            {
                return field.FieldType;
            }

            throw new InvalidOperationException("Member is not a property or a field.");
        }

        /// <summary>
        /// Gets a member expression for the member.
        /// </summary>
        /// <param name="member">The member to get the expression for.</param>
        /// <param name="expression">The member expression.</param>
        /// <returns>The member expression.</returns>
        public static MemberExpression GetMemberExpression(this MemberInfo member, Expression expression)
        {
            var property = member as PropertyInfo;
            if (property != null)
            {
                return Expression.Property(expression, property);
            }

            var field = member as FieldInfo;
            if (field != null)
            {
                return Expression.Field(expression, field);
            }

            throw new InvalidOperationException("Member is not a property or a field.");
        }

        /// <summary>
        /// Gets a value indicating if the given type is anonymous.
        /// True for anonymous, otherwise false.
        /// </summary>
        /// <param name="type">The type.</param>
        public static bool IsAnonymous(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            // https://stackoverflow.com/a/2483054/68499
            var isAnonymous = System.Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType
                && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.Public) != TypeAttributes.Public;

            return isAnonymous;
        }

        /// <summary>
        /// Gets a value indicating if the given type has a parameterless constructor.
        /// True if it has a parameterless constructor, otherwise false.
        /// </summary>
        /// <param name="type">The type.</param>
        public static bool HasParameterlessConstructor(this Type type)
        {
            return type.GetConstructors().Any(t => t.GetParameters().Length == 0);
        }

        /// <summary>
        /// Gets the constructor that contains the most parameters.
        /// </summary>
        /// <param name="type">The type.</param>
        public static ConstructorInfo GetConstructorWithMostParameters(this Type type)
        {
            return type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First();
        }

        /// <summary>
        /// Gets a value indicating if the type is a user defined struct.
        /// True if it is a user defined struct, otherwise false.
        /// </summary>
        /// <param name="type">The type.</param>
        public static bool IsUserDefinedStruct(this Type type)
        {
            return type.IsValueType && !type.IsPrimitive && !type.IsEnum;
        }
    }
}
