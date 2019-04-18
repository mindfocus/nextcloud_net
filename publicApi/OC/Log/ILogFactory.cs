using publicApi.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi.OC.Log
{
    /**
     * Interface ILogFactory
     *
     * @package OCP\Log
     * @since 14.0.0
     */
    interface ILogFactory
    {
        /**
         * @param string $type - one of: file, errorlog, syslog, systemd
         * @return IWriter
         * @since 14.0.0
         */
        IWriter get(string type);

        /**
         * @param string $path
         * @return ILogger
         * @since 14.0.0
         */
        ILogger getCustomLogger(string path) ;
}
}
