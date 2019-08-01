using System;
using System.Collections.Generic;
using System.Text;

namespace ext
{
    public static class TimeUtility
    {
        public static long GetTimestampFormNow()
        {
            DateTimeOffset dto = new DateTimeOffset(DateTime.Now);
            var unixTime = dto.ToUnixTimeSeconds();
            return unixTime;
        }
    }
}
