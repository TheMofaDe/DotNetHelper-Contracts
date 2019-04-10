using System.Linq;

namespace DotNetHelper_Contracts.Helpers
{
    public static class RandomHelper
    {
        public static string GetRandomString(int size, bool allowAlphaCharacters = true, bool allowNumericCharacters = true)
        {
            var random = new System.Random();
            var input = "";
            if (allowAlphaCharacters)
                input += "abcdefghijklmnopqrstuvwxyz" + "abcdefghijklmnopqrstuvwxyz".ToUpper();
            if (allowNumericCharacters)
                input += "01234567890123456789";
            if (!allowNumericCharacters && !allowAlphaCharacters)
            {
                return "@$$";
            }
            //  var chars = Enumerable.Range(0, size).Select(x => input[random.Next(0, input.Length)]);
            var chars = Enumerable.Range(0, size).Select(x => input[random.Next(input.Length)]);
            return new string(chars.ToArray());
        }
    }
}
