using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TimeConverterDroid
{
    public static class TimeConverter
    {

        public static TimeConverterOutput ConvertTime(DateTime inDate, string TimeZoneName)
        {
            TimeConverterOutput result = new TimeConverterOutput();

            try
            {
                DateTime utc = inDate.ToUniversalTime();
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneName);
                DateTime convertedTime = TimeZoneInfo.ConvertTimeFromUtc(utc, tzi);

                result = new TimeConverterOutput
                {
                    TimeOutput = convertedTime,
                    Success = true,
                    Exception = null,
                    IsDST = tzi.IsDaylightSavingTime(convertedTime),
                    TimeZoneFound = true
                };
            }catch(TimeZoneNotFoundException exc)
            {
                result = new TimeConverterOutput
                {
                    TimeOutput = null,
                    Success = false,
                    Exception = exc,
                    IsDST = false,
                    TimeZoneFound = false
                    
                };
            }
            catch (Exception exc)
            {
                result = new TimeConverterOutput
                {
                    TimeOutput = null,
                    Success = false,
                    Exception = exc,
                    IsDST = false,
                    TimeZoneFound = true

                };
            }
            return result;
        }
    }

    public class TimeConverterOutput
    {
        public DateTime? TimeOutput { get; set; }
        public bool Success { get; set; }
        public Exception Exception { get; set; }
        public bool IsDST { get; set; }
        public bool TimeZoneFound { get; set; }
    }
}