using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Remote.Api
{
    /**
     * Provides access to the various apis of a remote instance
     *
     * @since 13.0.0
     */
    interface IApiCollection
    {
        /**
         * @return IUserApi
         *
         * @since 13.0.0
         */
        IUserApi getUserApi();

        /**
         * @return ICapabilitiesApi
         *
         * @since 13.0.0
         */
        ICapabilitiesApi getCapabilitiesApi();
    }

}
