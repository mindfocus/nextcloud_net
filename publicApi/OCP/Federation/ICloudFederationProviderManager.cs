using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Federation
{
/**
 * Class ICloudFederationProviderManager
 *
 * Manage cloud federation providers
 *
 * @since 14.0.0
 *
 * @package OCP\Federation
 */
    public interface ICloudFederationProviderManager {

        /**
         * Registers an callback function which must return an cloud federation provider
         *
         * @param string resourceType which resource type does the provider handles
         * @param string displayName user facing name of the federated share provider
         * @param callable callback
         * @throws Exceptions\ProviderAlreadyExistsException
         *
         * @since 14.0.0
         */
        void addCloudFederationProvider(string resourceType, string displayName, Action callback);

        /**
         * remove cloud federation provider
         *
         * @param string resourceType
         *
         * @since 14.0.0
         */
        void removeCloudFederationProvider(string resourceType);

        /**
         * get a list of all cloudFederationProviders
         *
         * @return array [resourceType => ['resourceType' => resourceType, 'displayName' => displayName, 'callback' => callback]]
         *
         * @since 14.0.0
         */
        IList<string> getAllCloudFederationProviders();

        /**
         * get a specific cloud federation provider
         *
         * @param string resourceType
         * @return ICloudFederationProvider
         * @throws Exceptions\ProviderDoesNotExistsException
         *
         * @since 14.0.0
         */
        ICloudFederationProvider getCloudFederationProvider(string resourceType);

        /**
         * send federated share
         *
         * @param ICloudFederationShare share
         * @return mixed
         *
         * @since 14.0.0
         */
        object sendShare(ICloudFederationShare share);

        /**
         * send notification about existing share
         *
         * @param string url
         * @param ICloudFederationNotification notification
         * @return mixed
         *
         * @since 14.0.0
         */
        object sendNotification(string url, ICloudFederationNotification notification);

        /**
         * check if the new cloud federation API is ready to be used
         *
         * @return bool
         *
         * @since 14.0.0
         */
        bool isReady();


    }

}
