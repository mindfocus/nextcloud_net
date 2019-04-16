using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi.AppFramework.Utility
{
    /**
     * Needed to mock calls to time()
     * @since 8.0.0
     */
    interface ITimeFactory
    {

        /**
         * @return int the result of a call to time()
         * @since 8.0.0
         */
        int getTime();

	/**
	 * @param string $time
	 * @param \DateTimeZone $timezone
	 * @return \DateTime
	 * @since 15.0.0
	 */
	DateTime getDateTime(string time = "now", TimeZoneInfo timezone = null);

}

}
