using System;
using System.Collections.Generic;

namespace DotNetHelper_Contracts.Comparer
{
    public class ExceptionEqualityComparer : IEqualityComparer<Exception>
    {
        public bool Equals(Exception e1, Exception e2)
        {
            if (e2 == null && e1 == null)
                return true;
            else if (e1 == null | e2 == null)
                return false;
            else
            {
                var fullName = e1.GetType().FullName;
                if (fullName != null && (fullName.Equals(e2.GetType().FullName) && e1.Message.Equals(e2.Message)))
                    return true;
                else
                    return false;
            }
        }

        public int GetHashCode(Exception e)
        {
            return (e.GetType().FullName + e.Message).GetHashCode();
        }
    }
}
