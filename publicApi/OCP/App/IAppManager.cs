using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.App
{
    /**
     * Interface IAppManager
     *
     * @package OCP\App
     * @since 8.0.0
     */
    interface IAppManager
    {

        /**
         * Returns the app information from "appinfo/info.xml".
         *
         * @param string appId
         * @return mixed
         * @since 14.0.0
         */
        string getAppInfo(string appId, bool path = false, string lang = null);

        /**
         * Returns the app information from "appinfo/info.xml".
         *
         * @param string appId
         * @param bool useCache
         * @return string
         * @since 14.0.0
         */
        string getAppVersion(string appId, bool useCache = true);

	/**
	 * Check if an app is enabled for user
	 *
	 * @param string appId
	 * @param \OCP\IUser user (optional) if not defined, the currently loggedin user will be used
	 * @return bool
	 * @since 8.0.0
	 */
	bool isEnabledForUser(string appId, IUser user = null);

        /**
         * Check if an app is enabled in the instance
         *
         * Notice: This actually checks if the app is enabled and not only if it is installed.
         *
         * @param string appId
         * @return bool
         * @since 8.0.0
         */
        bool isInstalled(string appId);

        /**
         * Enable an app for every user
         *
         * @param string appId
         * @throws AppPathNotFoundException
         * @since 8.0.0
         */
        void enableApp(string appId);

        /**
         * Whether a list of types contains a protected app type
         *
         * @param string[] types
         * @return bool
         * @since 12.0.0
         */
        bool hasProtectedAppType(string[] types);

        /**
         * Enable an app only for specific groups
         *
         * @param string appId
         * @param \OCP\IGroup[] groups
         * @throws \Exception
         * @since 8.0.0
         */
        void enableAppForGroups(string appId, IGroup[] groups);

        /**
         * Disable an app for every user
         *
         * @param string appId
         * @since 8.0.0
         */
        bool disableApp(string appId);

        /**
         * Get the directory for the given app.
         *
         * @param string appId
         * @return string
         * @since 11.0.0
         * @throws AppPathNotFoundException
         */
        string getAppPath(string appId);

        /**
         * List all apps enabled for a user
         *
         * @param \OCP\IUser user
         * @return string[]
         * @since 8.1.0
         */
        string[] getEnabledAppsForUser(IUser user);

        /**
         * List all installed apps
         *
         * @return string[]
         * @since 8.1.0
         */
        string[] getInstalledApps();

        /**
         * Clear the cached list of apps when enabling/disabling an app
         * @since 8.1.0
         */
        void clearAppsCache();

        /**
         * @param string appId
         * @return boolean
         * @since 9.0.0
         */
        bool isShipped(string appId);

        /**
         * @return string[]
         * @since 9.0.0
         */
        string[] getAlwaysEnabledApps();
    }

}
