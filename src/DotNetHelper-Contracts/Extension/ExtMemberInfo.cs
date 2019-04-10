using System;
using System.Reflection;

namespace DotNetHelper_Contracts.Extension
{
    public static class ExtMemberInfo
    {
        public static Type GetMemberType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.All:
                    break;
                case MemberTypes.Constructor:
                    break;
                case MemberTypes.Custom:
                    break;
                case MemberTypes.Event:
                    break;
                case MemberTypes.Method:
                    break;
                case MemberTypes.NestedType:
                    break;
                case MemberTypes.TypeInfo:
                    break;
                default:
                    throw new InvalidOperationException();
            }
            throw new InvalidOperationException();
        }

        public static object GetValue(this MemberInfo member, object instance)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    return ((PropertyInfo)member).GetValue(instance, null);
                case MemberTypes.Field:
                    return ((FieldInfo)member).GetValue(instance);
                case MemberTypes.All:
                    break;
                case MemberTypes.Constructor:
                    break;
                case MemberTypes.Custom:
                    break;
                case MemberTypes.Event:
                    break;
                case MemberTypes.Method:
                    break;
                case MemberTypes.NestedType:
                    break;
                case MemberTypes.TypeInfo:
                    break;
                default:
                    throw new InvalidOperationException();
            }
            throw new InvalidOperationException();
        }

        public static void SetValue(this MemberInfo member, object instance, object value)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    var pi = (PropertyInfo)member;
                    pi.SetValue(instance, value, null);
                    break;
                case MemberTypes.Field:
                    var fi = (FieldInfo)member;
                    fi.SetValue(instance, value);
                    break;
                case MemberTypes.All:
                    break;
                case MemberTypes.Constructor:
                    break;
                case MemberTypes.Custom:
                    break;
                case MemberTypes.Event:
                    break;
                case MemberTypes.Method:
                    break;
                case MemberTypes.NestedType:
                    break;
                case MemberTypes.TypeInfo:
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
