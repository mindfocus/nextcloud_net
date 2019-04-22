using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Remote.Api
{
    /**
     * @since 13.0.0
     */
    public interface IApiFactory
    {
        /**
         * @param IInstance instance
         * @param ICredentials credentials
         * @return IApiCollection
         *
         * @since 13.0.0
         */
        IApiCollection getApiCollection(IInstance instance, ICredentials credentials);
    }

}
