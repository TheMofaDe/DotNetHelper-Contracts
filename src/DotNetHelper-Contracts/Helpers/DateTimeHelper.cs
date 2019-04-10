using System;
using System.Text;

namespace DotNetHelper_Contracts.Helpers
{
    public static class DateTimeHelper
    {

        public enum CalendarMonth : byte
        {
            April = 4,
            August = 8,
            December = 12,
            February = 2,
            January = 1,
            July = 7,
            June = 6,
            March = 3,
            May = 5,
            November = 11,
            October = 10,
            September = 9
        }

        public static DateTime CalculateMonthEnd(DateTime dtDate)
        {
            var builder = new StringBuilder();
            var time3 = dtDate.AddMonths(1);
            builder.AppendFormat("{0}/1/{1}", time3.Month, time3.Year);
            return Convert.ToDateTime(builder.ToString()).AddDays(-1.0);
        }






        /// <summary>
        /// Time Elapse Enum Value is basically the # of milliseconds in the selected enum 
        /// </summary>
        public enum TimeElapse
        {
            Second = 1000,
            Minute = 60 * 1000,
            Hour = 60 * 60 * 1000,
            Day = 24 * 60 * 60 * 1000,
            Week = 7 * 24 * 60 * 60 * 1000,
            // Month  Not supporting month options due to all months not containing the same amount of day and trying to keep this api simple
            //  Year  Not supporting month options due to all months not containing the same amount of day and trying to keep this api simple
        }

        

        //  String.Format("{0:t}", dt);  // "4:05 PM"                         ShortTime
        //  String.Format("{0:d}", dt);  // "3/9/2008"                        ShortDate
        //  String.Format("{0:T}", dt);  // "4:05:07 PM"                      LongTime
        //  String.Format("{0:D}", dt);  // "Sunday, March 09, 2008"          LongDate
        //  String.Format("{0:f}", dt);  // "Sunday, March 09, 2008 4:05 PM"  LongDate+ShortTime
        //  String.Format("{0:F}", dt);  // "Sunday, March 09, 2008 4:05:07 PM" FullDateTime
        //  String.Format("{0:g}", dt);  // "3/9/2008 4:05 PM"                ShortDate+ShortTime
        //  String.Format("{0:G}", dt);  // "3/9/2008 4:05:07 PM"             ShortDate+LongTime
        //  String.Format("{0:m}", dt);  // "March 09"                        MonthDay
        //  String.Format("{0:y}", dt);  // "March, 2008"                     YearMonth
        //  String.Format("{0:r}", dt);  // "Sun, 09 Mar 2008 16:05:07 GMT"   RFC1123
        //  String.Format("{0:s}", dt);  // "2008-03-09T16:05:07"             SortableDateTime
        //  String.Format("{0:u}", dt);  // "2008-03-09 16:05:07Z"            UniversalSortableDateTime


      
          
           
#pragma warning disable 618
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long TimeStamp(this DateTime time)
            {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

                return (int)(time - startTime).TotalSeconds;
            }

            /// <summary>
            /// 通過时间戳獲取日期
            /// </summary>
            /// <param name="timeStamp"></param>
            /// <returns></returns>
            public static DateTime FromTimeStamp(long timeStamp)
            {

            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
#pragma warning restore 618

                return startTime.AddSeconds(timeStamp);

            }



        private static DateTime GetPreviousSpecifiedDayOfWeek(this DateTime dt, DayOfWeek day)
        {
            if (dt.DayOfWeek == day)
            {
                return dt;
            }

            while (dt.DayOfWeek != day)
            {
                dt = dt.AddDays(-1);
            }

            return dt;
        }
        private static DateTime GetNextSpecifiedDayOfWeek(this DateTime dt, DayOfWeek day)
        {
            if (dt.DayOfWeek == day)
            {
                return dt;
            }

            while (dt.DayOfWeek != day)
            {
                dt = dt.AddDays(1);
            }

            return dt;
        }

    }
}
