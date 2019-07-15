using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Remote
{
    /**
     * Provides some basic info about a remote Nextcloud instance
     *
     * @since 13.0.0
     */
    public interface IInstance
    {
        /**
         * @return string The url of the remote server without protocol
         *
         * @since 13.0.0
         */
        string getUrl();

        /**
         * @return string The of of the remote server with protocol
         *
         * @since 13.0.0
         */
        string getFullUrl();

        /**
         * @return string The full version string in '13.1.2.3' format
         *
         * @since 13.0.0
         */
        string getVersion();

        /**
         * @return string 'http' or 'https'
         *
         * @since 13.0.0
         */
        string getProtocol();

        /**
         * Check that the remote server is installed and not in maintenance mode
         *
         * @since 13.0.0
         *
         * @return bool
         */
        bool isActive();
    }

}
