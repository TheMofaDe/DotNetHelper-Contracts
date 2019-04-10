using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetHelper_Contracts.Helpers
{
    public static class RegexHelper
    {

        public static int StringOccurrenceCount(string value, string lookFor)
        {

            if (string.IsNullOrEmpty(value)) return 0;
            if (string.IsNullOrEmpty(lookFor)) return 0;
            return Regex.Matches(value, lookFor).Count;
        }

        public static bool IsPasswordSecured(string inputVal)
            {
                var retVal = false;
                var regularExpression = @"^(?=.*?\d.*?\d)(?=.*?\w.*?\w)[\d\w]{6,8}$";
                if (Regex.IsMatch(inputVal, regularExpression))
                {
                    retVal = true;
                }
                return retVal;
            }

            public static bool HasSpecialCharactersRegEx(string inputString)
            {
                var retVal = true;
                var str = @"[^\w\.\,!""$%^&*\(\)-_+=::@']";
                var specialCharRegEx = new Regex(str);

                if ((specialCharRegEx.Matches(inputString.Trim()).Count) == 0)
                {
                    retVal = false;
                }

                return retVal;
            }

            public static bool HasUpperCase(string inputString)
            {
                var retVal = true;
                var upperCase = new Regex("[A-Z]");

                if (upperCase.Matches(inputString.Trim()).Count == 0)
                {
                    retVal = false;
                }

                return retVal;
            }

            public static bool HasLowerCase(string inputString)
            {
                var retVal = true;
                var lowerCase = new Regex("[a-z]");

                if (lowerCase.Matches(inputString.Trim()).Count == 0)
                {
                    retVal = false;
                }
                return retVal;
            }
		    /// <summary>
		    /// Check if string contains letters only supports international letters too.
		    /// </summary>
		    /// <param name="inputString"></param>
		    /// <returns></returns>
		    public static bool HasLettersOnly(string inputString)
		    {	    	  
		    	    return Regex.IsMatch(inputString, @"^[\p{L}]+$");;
		    }


	    public static bool HasLetter(string inputString)
	    {
		    return Regex.IsMatch(inputString, @"[a-zA-Z]"); 
	    }
	

		/// <summary>
		/// Check if string contains letters only supports international letters too.
		/// </summary>
		/// <param name="inputString"></param>
		/// <returns></returns>
		public static bool HasLettersAndNumbersOnly(string inputString)
	       {
		    	return Regex.IsMatch(inputString, @"^[a-zA-Z0-9]+$"); 
	       }

	    /// <summary>
	    /// Check if string contains letters only supports international letters too.
	    /// </summary>
	    /// <param name="inputString"></param>
	    /// <returns></returns>
	    public static bool HasLettersNumbersAndUnderscoreOnly(string inputString)
	    {
			return Regex.IsMatch(inputString, @"^[a-zA-Z0-9_]+$");
		}


	  


            public static bool HasNumber(string inputString)
            {
                var retVal = true;
                var numeric = new Regex("[0-9]");

                if (numeric.Matches(inputString.Trim()).Count == 0)
                {
                    retVal = false;
                }
                return retVal;
            }

            public static string RemoveSpecialCharacters(string inputString)
            {
                var retVal = string.Empty;
                var specialCharacters = new char[] {'`', '~', '!', '@', '#', '$', '%', '^', '&', '(',
                                                       ')', '_', '-', '=', '+', '|', '\\', '{', '}',
                                                       '[', ']', '"', '\'', ':', ';', '<', '>', '?',
                                                       '/', ',', '.'};
                inputString = specialCharacters.Aggregate(inputString, (current, chr) => current.Replace(chr.ToString(), ""));

                retVal = inputString.Trim();
                return retVal;
            }

            public static bool HasSpecialCharacters(string inputString)
            {
                var specialCharacters = new char[] {'`', '~', '!', '@', '#', '$', '%', '^', '&', '(',
                                                       ')', '_', '-', '=', '+', '|', '\\', '{', '}',
                                                       '[', ']', '"', '\'', ':', ';', '<', '>', '?',
                                                       '/', ','};
                return inputString.Where((t, i) => inputString.Contains(specialCharacters[i].ToString())).Any();
            }
        
    }
}
