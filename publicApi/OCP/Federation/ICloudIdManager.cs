using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Federation
{
/**
 * Interface for resolving federated cloud ids
 *
 * @since 12.0.0
 */
    public interface ICloudIdManager {
        /**
         * @param string cloudId
         * @return ICloudId
         * @throws \InvalidArgumentException
         *
         * @since 12.0.0
         */
        ICloudId resolveCloudId(string cloudId) ;

        /**
         * Get the cloud id for a remote user
         *
         * @param string user
         * @param string remote
         * @return ICloudId
         *
         * @since 12.0.0
         */
        ICloudId getCloudId(string user, string remote);

        /**
         * Check if the input is a correctly formatted cloud id
         *
         * @param string cloudId
         * @return bool
         *
         * @since 12.0.0
         */
        bool isValidCloudId(string cloudId);
    }

}
