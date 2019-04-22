using Pchp.Library.DateTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Interface IDateTimeZone
     *
     * @package OCP
     * @since 8.0.0
     */
    public interface IDateTimeZone
    {
        /**
         * @param bool|int timestamp
         * @return \DateTimeZone
         * @since 8.0.0 - parameter timestamp was added in 8.1.0
         */
        DateTimeZone getTimeZone(int timestamp = 0);
    }

}
