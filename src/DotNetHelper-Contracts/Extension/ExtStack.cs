using System.Collections.Generic;

namespace DotNetHelper_Contracts.Extension
{
    public static class ExtStack
    {

            public static IEnumerable<T> Pop<T>(this Stack<T> stack, int number)
            {
                for (var i = 0; i < number; i++)
                    yield return stack.Pop();
            }
        
    }
}
