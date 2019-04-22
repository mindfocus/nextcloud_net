using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Http.Client
{
    /**
     * Interface IClientService
     *
     * @package OCP\Http
     * @since 8.1.0
     */
    public interface IClientService
    {
        /**
         * @return IClient
         * @since 8.1.0
         */
        IClient newClient();
}

}
