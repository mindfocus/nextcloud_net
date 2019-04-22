using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Http.Client
{
    /**
     * Interface IResponse
     *
     * @package OCP\Http
     * @since 8.1.0
     */
    public interface IResponse
    {
        /**
         * @return string|resource
         * @since 8.1.0
         */
        string getBody();

        /**
         * @return int
         * @since 8.1.0
         */
        int getStatusCode();

        /**
         * @param string $key
         * @return string
         * @since 8.1.0
         */
        string getHeader(string key);

        /**
         * @return array
         * @since 8.1.0
         */
        PhpArray getHeaders();
}

}
