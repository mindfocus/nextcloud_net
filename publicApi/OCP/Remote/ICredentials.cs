using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Remote
{
    /**
     * The credentials for a remote user
     *
     * @since 13.0.0
     */
    public interface ICredentials
    {
        /**
         * @return string
         *
         * @since 13.0.0
         */
        string getUsername();

        /**
         * @return string
         *
         * @since 13.0.0
         */
        string getPassword();
    }

}
