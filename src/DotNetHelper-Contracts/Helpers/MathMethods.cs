
using System;

namespace DotNetHelper_Contracts.Helpers
{
   public static class MathHelper
    {
        public static string GetPercentage(int value, int total, int decimalPlaces)
        {
                decimal percent = 0;
                var retval = string.Empty;
                var strplaces = new string('0', decimalPlaces);

                if (value == 0 || total == 0)
                {
                    percent = 0;
                }

                else
                {
                    percent = decimal.Divide(value, total) * 100;

                    if (decimalPlaces > 0)
                    {
                        strplaces = "." + strplaces;
                    }
                }

                retval = percent.ToString("#" + strplaces);

                return retval;

        }



        public static double FindPercentage(double value, double percent)
        {
           
                var d = percent / 100;
                var soome = d * value;
                return soome;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dividend">a number to be divided by another number.</param>
		/// <param name="divisor">a number that divides into another without a remainder.</param>
		/// <returns>Result,Remainder</returns>
		public static Tuple<int, int> Divide(int dividend, int divisor)
	    {
		    var result = dividend / divisor;
		    var remainder = dividend % divisor;
		    return Tuple.Create(result, remainder);
	    }


    }
}
