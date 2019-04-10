using System;
using System.Collections.Generic;

namespace DotNetHelper_Contracts.Extension
{
	public static class ExtObject
    {

        public static void IsNullThrow(this object obj, string name, Exception error = null)
        {
            if (obj != null) return;
            if (error == null) error = new ArgumentNullException(name);
            throw error;
        }



        /// <summary>
        /// The value passed as the first parameter is converted to a boolean
        /// value. If value is 0, null, false, NaN, DBNull, char '\0', char '0',
        /// the empty string (""), the string "false"(ignore case) or the string
        /// "0" will return false. All other values, including any object, will
        /// return true.
        /// </summary>
        /// <param name="o">input value</param>
        /// <returns>bool</returns>
        public static bool ToBoolEx(this object o)
	    {
		    if (o == null)
		    {
			    return false;
		    }

		    if (o is bool b)
		    {
			    return b;
		    }

		    if (o is DBNull)
		    {
			    return false;
		    }

		    if (o is DateTime)
		    {
			    return true;
		    }

		    if (o is char c)
		    {
		        if (char.MinValue.Equals(c) || '0'.Equals(c))
			    {
				    return false;
			    }

			    return true;
		    }

            if (o is string s)
            {
                if (string.Empty.Equals(s, StringComparison.OrdinalIgnoreCase) || "0".Equals(s, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                return !string.Equals(s, bool.FalseString, StringComparison.OrdinalIgnoreCase);
            }

            if (o is IConvertible i)
            {
                switch (i)
                {
                    case double d:
                        if (double.IsNaN(d))
                        {
                            return false;
                        }

                        return i.ToDouble(null) != 0D;
                    case float f:
                        if (float.IsNaN(f))
                        {
                            return false;
                        }

                        return i.ToSingle(null) != 0F;
                }

                return i.ToDecimal(null) != decimal.Zero;
            }

            return true;
	    }

	    public static byte ToByte(this object o)
	    {
		    return o.ToByte(true);
	    }

	    public static byte ToByte(this object o, bool throwEx, byte returnValueOnError = default)
	    {
		    if (throwEx)
		    {
			    return Convert.ToByte(o);
		    }
		    else
		    {
                if (o is string s)
                {
                    if (string.IsNullOrEmpty(s)) return returnValueOnError;
                    byte.TryParse(s, out var b);
                    return b;
                }
                else
                {
                    byte b;
                    try
                    {
                        b = Convert.ToByte(o);
                    }
                    catch
                    {
                        b = returnValueOnError;
                    }

                    return b;
                }
            }
	    }

	    public static char ToChar(this object o)
	    {
		    return o.ToChar(true);
	    }

	    public static char ToChar(this object o, bool throwEx, char returnValueOnError = default)
	    {
		    if (throwEx)
		    {
			    return Convert.ToChar(o);
		    }
		    else
		    {
                if (o is string s)
                {
                    if (string.IsNullOrEmpty(s)) return returnValueOnError;
                    char.TryParse(s, out var c);
                    return c;
                }
                else
                {
                    char c;
                    try
                    {
                        c = Convert.ToChar(o);
                    }
                    catch
                    {
                        c = returnValueOnError;
                    }

                    return c;
                }
            }
	    }

	    public static DateTime ToDateTime(this object o)
	    {
		    return o.ToDateTime(true);
	    }

	    public static DateTime ToDateTime(this object o, bool throwEx, DateTime returnValueOnError = default)
	    {
		    if (throwEx)
		    {
			    return Convert.ToDateTime(o);
		    }
		    else
		    {
		        if (o == null) return returnValueOnError;
                if (o is string s)
                {
                    if (string.IsNullOrEmpty(s)) return returnValueOnError;
                    DateTime.TryParse(s, out var dt);
                    return dt;
                }
                else
                {
                    DateTime dt;
                    try
                    {
                        dt = Convert.ToDateTime(o);
                    }
                    catch
                    {
                        dt = returnValueOnError;
                    }

                    return dt;
                }
            }
	    }

	    public static decimal ToDecimal(this object o)
	    {
		    return o.ToDecimal(true);
	    }

	    public static decimal ToDecimal(this object o, bool throwEx, int returnValueOnError = default)
	    {
		    if (throwEx)
		    {
			    return Convert.ToDecimal(o);
		    }
		    else
		    {
		        if (o == null) return returnValueOnError;
                if (o is string s)
                {
                    if (string.IsNullOrEmpty(s)) return returnValueOnError;
                    decimal.TryParse(s, out var d);
                    return d;
                }
                else
                {
                    decimal d;
                    try
                    {
                        d = Convert.ToDecimal(o);
                    }
                    catch
                    {
                        d = returnValueOnError;
                    }

                    return d;
                }
            }
	    }

	    public static double ToDouble(this object o)
	    {
		    return o.ToDouble(true);
	    }

	    public static double ToDouble(this object o, bool throwEx, double returnValueOnError = default)
	    {
		    if (throwEx)
		    {
			    return Convert.ToDouble(o);
		    }
		    else
		    {
		        if (o == null) return returnValueOnError;
                if (o is string s)
                {
                    if (string.IsNullOrEmpty(s)) return returnValueOnError;
                    double.TryParse(s, out var d);
                    return d;
                }
                else
                {
                    double d;
                    try
                    {
                        d = Convert.ToDouble(o);
                    }
                    catch
                    {
                        d = returnValueOnError;
                    }

                    return d;
                }
            }
	    }

	    // C# does not support enum as generic type parameter constraints
	    // you can use UnconstrainedMelody to implement enum constraints
	    public static T ToEnum<T>(this object o)
		    where T : struct/*, IEnumConstraint*/
	    {
		    return o.ToEnum<T>(true);
	    }

	    public static T ToEnum<T>(this object o, bool throwEx, T returnValueOnError = default)  where T : struct/*, IEnumConstraint*/
	    {
		    if (!typeof(T).IsEnum)
		    {
			    throw new NotSupportedException("type parameter must be a enum");
		    }

	        if (o is int)
	        {
	            return (T) o;
	        }
	      
		    var s = o as string;

		    if (throwEx)
		    {
			    if (s != null)
			    {
				    return (T)System.Enum.Parse(typeof(T), s);
			    }
			    else
			    {
				    return (T)o;
			    }
		    }
		    else
		    {
                if (o == null) return returnValueOnError;
                if (s != null)
			    {
                    System.Enum.TryParse<T>(s, out var e);
                    return e;
			    }
			    else
			    {
				    T e;
				    try
				    {
					    e = (T)o;
				    }
				    catch
				    {
                        return returnValueOnError;
                     //s   e = (T)(object)0;
				    }

				    return e;
			    }
		    }
	    }

	    public static float ToFloat(this object o)
	    {
		    return o.ToFloat(true);
	    }

	    public static float ToFloat(this object o, bool throwEx)
	    {
		    if (throwEx)
		    {
			    return Convert.ToSingle(o);
		    }
		    else
		    {
                if (o is string s)
                {
                    float.TryParse(s, out var f);
                    return f;
                }
                else
                {
                    float f;
                    try
                    {
                        f = Convert.ToSingle(o);
                    }
                    catch
                    {
                        f = default;
                    }

                    return f;
                }
            }
	    }

	    public static int ToInt(this object o)
	    {
		    return o.ToInt(true);
	    }

	    public static int ToInt(this object o, bool throwEx, int returnValueOnError = default)
	    {
		    if (throwEx)
		    {
			    return Convert.ToInt32(o);
		    }
		    else
		    {
		        if (o == null) return returnValueOnError;
                if (o is string s)
			    {
			        if (string.IsNullOrEmpty(s)) return returnValueOnError;
                    int.TryParse(s, out var i);
				    return i;
			    }
			    else
			    {
				    int i;
				    try
				    {
					    i = Convert.ToInt32(o);
				    }
				    catch
				    {
					    i = returnValueOnError;
				    }

				    return i;
			    }
		    }
	    }

	    public static long ToLong(this object o)
	    {
		    return o.ToLong(true);
	    }

	    public static long ToLong(this object o, bool throwEx)
	    {
		    if (throwEx)
		    {
			    return Convert.ToInt64(o);
		    }
		    else
		    {
                if (o is string s)
                {
                    long.TryParse(s, out var l);
                    return l;
                }
                else
                {
                    long l;
                    try
                    {
                        l = Convert.ToInt64(o);
                    }
                    catch
                    {
                        l = default;
                    }

                    return l;
                }
            }
	    }

	    public static sbyte ToSByte(this object o)
	    {
		    return o.ToSByte(true);
	    }

	    public static sbyte ToSByte(this object o, bool throwEx)
	    {
		    if (throwEx)
		    {
			    return Convert.ToSByte(o);
		    }
		    else
		    {
                if (o is string s)
                {
                    sbyte.TryParse(s, out var sb);
                    return sb;
                }
                else
                {
                    sbyte sb;
                    try
                    {
                        sb = Convert.ToSByte(o);
                    }
                    catch
                    {
                        sb = default;
                    }

                    return sb;
                }
            }
	    }

	    public static short ToShort(this object o)
	    {
		    return o.ToShort(true);
	    }

	    public static short ToShort(this object o, bool throwEx)
	    {
		    if (throwEx)
		    {
			    return Convert.ToInt16(o);
		    }
		    else
		    {
                if (o is string s)
                {
                    short.TryParse(s, out var sh);
                    return sh;
                }
                else
                {
                    short sh;
                    try
                    {
                        sh = Convert.ToInt16(o);
                    }
                    catch
                    {
                        sh = default;
                    }

                    return sh;
                }
            }
	    }

	    public static uint ToUInt(this object o)
	    {
		    return o.ToUInt(true);
	    }

	    public static uint ToUInt(this object o, bool throwEx, uint returnValueOnError = default)
	    {
		    if (throwEx)
		    {
			    return Convert.ToUInt32(o);
		    }
		    else
		    {
                if (o is string s)
                {
                    if (string.IsNullOrEmpty(s)) return returnValueOnError;
                    uint.TryParse(s, out var ui);
                    return ui;
                }
                else
                {
                    uint ui;
                    try
                    {
                        ui = Convert.ToUInt32(o);
                    }
                    catch
                    {
                        ui = returnValueOnError;
                    }

                    return ui;
                }
            }
	    }

	    public static ulong ToULong(this object o)
	    {
		    return o.ToULong(true);
	    }

	    public static ulong ToULong(this object o, bool throwEx)
	    {
		    if (throwEx)
		    {
			    return Convert.ToUInt64(o);
		    }
		    else
		    {
                if (o is string s)
                {
                    ulong.TryParse(s, out var ul);
                    return ul;
                }
                else
                {
                    ulong ul;
                    try
                    {
                        ul = Convert.ToUInt64(o);
                    }
                    catch
                    {
                        ul = default;
                    }

                    return ul;
                }
            }
	    }

	    public static ushort ToUShort(this object o)
	    {
		    return o.ToUShort(true);
	    }

	    public static ushort ToUShort(this object o, bool throwEx)
	    {
		    if (throwEx)
		    {
			    return Convert.ToUInt16(o);
		    }
		    else
		    {
                if (o is string s)
                {
                    ushort.TryParse(s, out var us);
                    return us;
                }
                else
                {
                    ushort us;
                    try
                    {
                        us = Convert.ToUInt16(o);
                    }
                    catch
                    {
                        us = default;
                    }

                    return us;
                }
            }
	    }


	 

	    #region Comparison

	    public static IComparer<T> ToComparer<T>(this Comparison<T> comparison)
	    {
		    return new FunctorComparer<T>(comparison);
	    }

	    #endregion Comparison

	    private sealed class FunctorComparer<T> : IComparer<T>
	    {
		    private readonly Comparison<T> comparison;

		    public FunctorComparer(Comparison<T> comparison)
		    {
			    this.comparison = comparison;
		    }

		    public int Compare(T x, T y)
		    {
			    return comparison(x, y);
		    }
	    }
    }
}

