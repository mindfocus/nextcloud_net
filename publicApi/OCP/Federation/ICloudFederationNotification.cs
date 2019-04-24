using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Federation
{
/**
 * Interface ICloudFederationNotification
 *
 * @package OCP\Federation
 *
 * @since 14.0.0
 */
    public interface ICloudFederationNotification {

        /**
         * add a message to the notification
         *
         * @param string notificationType (e.g. SHARE_ACCEPTED)
         * @param string resourceType (e.g. file, calendar, contact,...)
         * @param providerId id of the share
         * @param array notification , payload of the notification
         *
         * @since 14.0.0
         */
        void setMessage(string notificationType, string resourceType, string providerId, IList<string> notification);

        /**
         * get message, ready to send out
         *
         * @return array
         *
         * @since 14.0.0
         */
        IList<string> getMessage();
    }

}
