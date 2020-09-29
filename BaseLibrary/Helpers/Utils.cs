using System;

namespace BaseLibrary.Helpers
{
    public class Utils
    {
        public static long TimeStamp(DateTime? dateTime = null)
        {
            if (!dateTime.HasValue) dateTime = DateTime.UtcNow;
            return (long)dateTime.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public static long TimeStampTicks(DateTime? dateTime = null)
        {
            if (!dateTime.HasValue) dateTime = DateTime.UtcNow;
            return dateTime.Value.Subtract(new DateTime(1970, 1, 1)).Ticks;
        }

        //public static Guid TimeStampGuid(DateTime? dateTime = null)
        //{
        //    if (!dateTime.HasValue) dateTime = DateTime.UtcNow;
        //    return FluentCassandra.GuidGenerator.GenerateTimeBasedGuid(dateTime.Value);
        //}

        //public static long GuidToTimeStamp(Guid guid) =>
        //    TimeStamp(FluentCassandra.GuidGenerator.GetDateTime(guid));

        //public static long GuidToTimeStampTicks(Guid guid) =>
        //    TimeStampTicks(FluentCassandra.GuidGenerator.GetDateTime(guid));
    }
}