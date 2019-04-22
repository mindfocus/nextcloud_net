using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.OCS
{
    /**
     * Interface IDiscoveryService
     *
     * Allows you to discover OCS end-points on a remote server
     *
     * @package OCP\OCS
     * @since 12.0.0
     */
    public interface IDiscoveryService
    {

        /**
         * Discover OCS end-points
         *
         * If no valid discovery data is found the defaults are returned
         *
         * @since 12.0.0
         *
         * @param string $remote
         * @param string $service the service you want to discover
         * @param bool $skipCache We won't check if the data is in the cache. This is useful if a background job is updating the status - Added in 14.0.0
         * @return array
         */
        PhpArray discover(string remote, string service, bool skipCache = false);

}

}
