using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * This class provides an easy way for apps to store config values in the
     * database.
     * @since 7.0.0
     */
    public interface IAppConfig
    {
        /**
         * check if a key is set in the appconfig
         * @param string $app
         * @param string $key
         * @return bool
         * @since 7.0.0
         */
        bool hasKey(string app, string key);

        /**
         * get multiply values, either the app or key can be used as wildcard by setting it to false
         *
         * @param string|false $key
         * @param string|false $app
         * @return array|false
         * @since 7.0.0
         */
        IList<string> getValues(string app, string key);

        /**
         * get all values of the app or and filters out sensitive data
         *
         * @param string $app
         * @return array
         * @since 12.0.0
         */
        IList<string> getFilteredValues(string app);

        /**
         * Get all apps using the config
         * @return array an array of app ids
         *
         * This function returns a list of all apps that have at least one
         * entry in the appconfig table.
         * @since 7.0.0
         */
        IList<string> getApps();
    }

}
