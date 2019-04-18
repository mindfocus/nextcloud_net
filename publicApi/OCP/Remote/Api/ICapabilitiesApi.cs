using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Remote.Api
{
    /**
     * @since 13.0.0
     */
    interface ICapabilitiesApi
    {
        /**
         * @return array The capabilities in the form of [$appId => [$capability => $value]]
         *
         * @since 13.0.0
         */
        IDictionary<string,object> getCapabilities();
    }

}
