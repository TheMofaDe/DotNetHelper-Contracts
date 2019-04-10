using System;
using System.Threading;

namespace DotNetHelper_Contracts.Extension
{
    public static class TimeSpanExtension
    {
        public static CancellationTokenSource ToCancellationTokenSource(this TimeSpan span)
        {
            return new CancellationTokenSource(span);

        }

    
    }
}
