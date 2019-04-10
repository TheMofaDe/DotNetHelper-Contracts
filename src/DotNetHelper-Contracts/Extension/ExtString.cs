

using System;
using System.Text;


namespace DotNetHelper_Contracts.Extension
{
   public static class ExtString
    {
       

        public static string CloneString(this string value, int count)
        {
            if (value == null)
            {
                return null;
            }
            var result = new StringBuilder();
            for (var i = 0; i < count; i++)
            {
                result.Append(value);
            }
            return result.ToString();
        }



        public static byte[] GetBytes(this string value, Encoding encoding)
        {
            return encoding.GetBytes(value);
        }

      

        

        public static string ReplaceFirstOccurrence(this string source, string find, string replace, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(find))
                return source;
            var place = source.IndexOf(find, comparison);
            if (place == -1)
                return source;
            return source.Remove(place, find.Length).Insert(place, replace);
        }

        public static string ReplaceLastOccurrence(this string source, string find, string replace, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(find))
                return source;
            var place = source.LastIndexOf(find, comparison);
            if (place == -1)
                return source;
            source = source.Remove(place, find.Length).Insert(place, replace);
            return source;
        }

    }
}
