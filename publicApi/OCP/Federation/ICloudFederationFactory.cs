using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Federation
{
/**
 * Interface ICloudFederationFactory
 *
 * @package OCP\Federation
 *
 * @since 14.0.0
 */
    public interface ICloudFederationFactory {

        /**
         * get a CloudFederationShare Object to prepare a share you want to send
         *
         * @param string shareWith
         * @param string name resource name (e.g. document.odt)
         * @param string description share description (optional)
         * @param string providerId resource UID on the provider side
         * @param string owner provider specific UID of the user who owns the resource
         * @param string ownerDisplayName display name of the user who shared the item
         * @param string sharedBy provider specific UID of the user who shared the resource
         * @param string sharedByDisplayName display name of the user who shared the resource
         * @param string sharedSecret used to authenticate requests across servers
         * @param string shareType ('group' or 'user' share)
         * @param resourceType ('file', 'calendar',...)
         * @return ICloudFederationShare
         *
         * @since 14.0.0
         */
        ICloudFederationShare getCloudFederationShare(string shareWith, string name, string description, string providerId, string owner, string ownerDisplayName, string sharedBy, string sharedByDisplayName, string sharedSecret, string shareType, string resourceType);

        /**
         * get a Cloud FederationNotification object to prepare a notification you
         * want to send
         *
         * @return ICloudFederationNotification
         *
         * @since 14.0.0
         */
        ICloudFederationNotification getCloudFederationNotification();
    }

}
