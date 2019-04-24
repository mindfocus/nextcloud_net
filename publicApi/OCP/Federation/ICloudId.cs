using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Federation
{
/**
 * Parsed federated cloud id
 *
 * @since 12.0.0
 */
    public interface ICloudId {
        /**
         * The remote cloud id
         *
         * @return string
         * @since 12.0.0
         */
        string getId();

        /**
         * Get a clean representation of the cloud id for display
         *
         * @return string
         * @since 12.0.0
         */
        string getDisplayId();

        /**
         * The username on the remote server
         *
         * @return string
         * @since 12.0.0
         */
        string getUser();

        /**
         * The base address of the remote server
         *
         * @return string
         * @since 12.0.0
         */
        string getRemote();
    }

}
