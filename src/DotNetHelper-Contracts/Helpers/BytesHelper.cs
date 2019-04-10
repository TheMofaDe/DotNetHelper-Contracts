using System.Linq;

namespace DotNetHelper_Contracts.Helpers
{
   public static class BytesHelper
    {

        public static bool DeepEqual(byte[] one, byte[] two)
        {
            
                if (one != null && two != null)
                {
                    var byteCount1 = one.Count();
                    var byteCount2 = two.Count();
                    if (byteCount1 == byteCount2)
                    {
                        var i = 0;
                        foreach (var value in one.Select(b => b == two[i]))
                        {

                            if (!value)
                            {


                                return false;
                            }
                            i++;
                        }

                        return true;
                    }
                }
          
            return false;
        }

    }
}
