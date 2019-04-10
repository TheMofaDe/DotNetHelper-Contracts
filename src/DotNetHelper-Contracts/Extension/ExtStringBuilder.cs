using System;
using System.IO;
using System.Text;

namespace DotNetHelper_Contracts.Extension
{
    public static class ExtStringBuilder
    {

        /// <summary>
        /// Copies the contents of the StringBuilder to the MemoryStream using the specified encoding 
        /// </summary>
        /// <param name="builder">StringBuilder source</param>
        /// <param name="ms">MemoryStream destination</param>
        /// <param name="encoding">Encoding used for converter string into byte-stream</param>
        public static void CopyToStream(this StringBuilder builder, MemoryStream ms, Encoding encoding)
        {
                // Faster than MemoryStream, but generates garbage
                var str = builder.ToString();
                byte[] bytes = encoding.GetBytes(str);
                ms.Write(bytes, 0, bytes.Length);
     
        }

        public static StringBuilder ReplaceFirstOccurrence(this StringBuilder source, string find, string replace, StringComparison comparison)
        {
            if (source == null || source.Length <= 0 || string.IsNullOrEmpty(find))
                return source;
            var place = source.ToString().IndexOf(find, comparison);
            if (place == -1)
                return source;
            return source.Remove(place, find.Length).Insert(place, replace);
        }

        public static StringBuilder ReplaceLastOccurrence(this StringBuilder source, string find, string replace, StringComparison comparison)
        {
            if (source == null || source.Length <= 0 || string.IsNullOrEmpty(find))
                return source;
            var place = source.ToString().LastIndexOf(find, comparison);
            if (place == -1)
                return source;
            source = source.Remove(place, find.Length).Insert(place, replace);
            return source;
        }
    }
}
