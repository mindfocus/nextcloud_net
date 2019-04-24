using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Federation
{
/**
 * Interface ICloudFederationProvider
 *
 * Enable apps to create their own cloud federation provider
 *
 * @since 14.0.0
 *
 * @package OCP\Federation
 */

    public interface ICloudFederationProvider {

        /**
         * get the name of the share type, handled by this provider
         *
         * @return string
         *
         * @since 14.0.0
         */
        string getShareType();

        /**
         * share received from another server
         *
         * @param ICloudFederationShare share
         * @return string provider specific unique ID of the share
         *
         * @throws ProviderCouldNotAddShareException
         *
         * @since 14.0.0
         */
        string shareReceived(ICloudFederationShare share);

        /**
         * notification received from another server
         *
         * @param string notificationType (e.g SHARE_ACCEPTED)
         * @param string providerId share ID
         * @param array notification provider specific notification
         * @return array data send back to sender
         *
         * @throws ShareNotFound
         * @throws ActionNotSupportedException
         * @throws BadRequestException
         * @throws AuthenticationFailedException
         *
         * @since 14.0.0
         */
        void notificationReceived(string notificationType, string providerId, IList<string> notification);

        /**
         * get the supported share types, e.g. "user", "group", etc.
         *
         * @return array
         *
         * @since 14.0.0
         */
        IList<string> getSupportedShareTypes();

    }

}
