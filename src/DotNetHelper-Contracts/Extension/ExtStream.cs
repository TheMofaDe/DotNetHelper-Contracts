
using System.IO;
using System.Text;

namespace DotNetHelper_Contracts.Extension
{
    public static class ExtStream
    {
        public static Stream Clear(this Stream stream)
        {
            stream.SetLength(0);
            return stream;
        }

        public static Stream ResetPosition(this Stream stream)
        {
            stream.Position = 0;
            return stream;
        }

        public static byte[] ToArray(this Stream stream)
        {
            var data = new byte[stream.Length];

            stream.Read(data, 0, data.Length);

            return data;
        }
        public static string ReadToString(this Stream stream, Encoding encoding, int startPosition = 0)
        {
            stream.IsNullThrow(nameof(stream));
            stream.Position = startPosition;
            var reader = new StreamReader(stream);
            var text = reader.ReadToEnd();
            return text;
        }
    }
}
