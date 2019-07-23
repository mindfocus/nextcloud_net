using System;
using System.Collections;
using System.Collections.Generic;
namespace OCP
{
    /**
     * Access to all the configuration options ownCloud offers
     * @since 6.0.0
     */
    public interface IConfig
    {
        /**
         * @since 8.2.0
         */
        //const SENSITIVE_VALUE = '***REMOVED SENSITIVE VALUE***';

        /**
         * Sets and deletes system wide values
         *
         * @param array configs Associative array with `key => value` pairs
         *                       If value is null, the config key will be deleted
         * @since 8.0.0
         */
         void setSystemValues(IDictionary<string,object> configs);

        /**
         * Sets a new system wide value
         *
         * @param string key the key of the value, under which will be saved
         * @param mixed value the value that should be stored
         * @since 8.0.0
         */
         void setSystemValue(string key, object value);

        /**
         * Looks up a system wide defined value
         *
         * @param string key the key of the value, under which it was saved
         * @param mixed default the default value to be returned if the value isn't set
         * @return mixed the value or default
         * @since 6.0.0 - parameter default was added in 7.0.0
         */
         object getSystemValue(string key, object @default);

        /**
         * Looks up a boolean system wide defined value
         *
         * @param string key the key of the value, under which it was saved
         * @param bool default the default value to be returned if the value isn't set
         * @return bool the value or default
         * @since 16.0.0
         */
         bool getSystemValueBool(string key, bool @default = false);

    /**
     * Looks up an integer system wide defined value
     *
     * @param string key the key of the value, under which it was saved
     * @param int default the default value to be returned if the value isn't set
     * @return int the value or default
     * @since 16.0.0
     */
     int getSystemValueInt(string key, int @default = 0);

    /**
     * Looks up a string system wide defined value
     *
     * @param string key the key of the value, under which it was saved
     * @param string default the default value to be returned if the value isn't set
     * @return string the value or default
     * @since 16.0.0
     */
     string getSystemValueString(string key, string @default = "");

    /**
     * Looks up a system wide defined value and filters out sensitive data
     *
     * @param string key the key of the value, under which it was saved
     * @param mixed default the default value to be returned if the value isn't set
     * @return mixed the value or default
     * @since 8.2.0
     */
     object getFilteredSystemValue(string key, object @default);

        /**
         * Delete a system wide defined value
         *
         * @param string key the key of the value, under which it was saved
         * @since 8.0.0
         */
         void deleteSystemValue(string key);

        /**
         * Get all keys stored for an app
         *
         * @param string appName the appName that we stored the value under
         * @return string[] the keys stored for the app
         * @since 8.0.0
         */
         IList<string> getAppKeys(string appName);

        /**
         * Writes a new app wide value
         *
         * @param string appName the appName that we want to store the value under
         * @param string|float|int key the key of the value, under which will be saved
         * @param string value the value that should be stored
         * @return void
         * @since 6.0.0
         */
         void setAppValue(string appName, string key, string value);

        /**
         * Looks up an app wide defined value
         *
         * @param string appName the appName that we stored the value under
         * @param string key the key of the value, under which it was saved
         * @param string default the default value to be returned if the value isn't set
         * @return string the saved value
         * @since 6.0.0 - parameter default was added in 7.0.0
         */
         T getAppValue<T>(string appName, string key, T @default);

        /**
         * Delete an app wide defined value
         *
         * @param string appName the appName that we stored the value under
         * @param string key the key of the value, under which it was saved
         * @since 8.0.0
         */
         void deleteAppValue(string appName, string key);

        /**
         * Removes all keys in appconfig belonging to the app
         *
         * @param string appName the appName the configs are stored under
         * @since 8.0.0
         */
         void deleteAppValues(string appName);


        /**
         * Set a user defined value
         *
         * @param string userId the userId of the user that we want to store the value under
         * @param string appName the appName that we want to store the value under
         * @param string key the key under which the value is being stored
         * @param string value the value that you want to store
         * @param string preCondition only update if the config value was previously the value passed as preCondition
         * @throws \OCP\PreConditionNotMetException if a precondition is specified and is not met
         * @throws \UnexpectedValueException when trying to store an unexpected value
         * @since 6.0.0 - parameter precondition was added in 8.0.0
         */
         void setUserValue(string userId, string appName, string key, string value, string? preCondition = null);

        /**
         * Shortcut for getting a user defined value
         *
         * @param string userId the userId of the user that we want to store the value under
         * @param string appName the appName that we stored the value under
         * @param string key the key under which the value is being stored
         * @param mixed default the default value to be returned if the value isn't set
         * @return string
         * @since 6.0.0 - parameter default was added in 7.0.0
         */
         T getUserValue<T>(string userId, string appName, string key, T @default);

        /**
         * Fetches a mapped list of userId . value, for a specified app and key and a list of user IDs.
         *
         * @param string appName app to get the value for
         * @param string key the key to get the value for
         * @param array userIds the user IDs to fetch the values for
         * @return array Mapped values: userId => value
         * @since 8.0.0
         */
         IDictionary getUserValueForUsers(string appName, string key, IList<string> userIds);

        /**
         * Get the keys of all stored by an app for the user
         *
         * @param string userId the userId of the user that we want to store the value under
         * @param string appName the appName that we stored the value under
         * @return string[]
         * @since 8.0.0
         */
         IList<string> getUserKeys(string userId, string appName);

        /**
         * Delete a user value
         *
         * @param string userId the userId of the user that we want to store the value under
         * @param string appName the appName that we stored the value under
         * @param string key the key under which the value is being stored
         * @since 8.0.0
         */
         void deleteUserValue(string userId, string appName, string key);

        /**
         * Delete all user values
         *
         * @param string userId the userId of the user that we want to remove all values from
         * @since 8.0.0
         */
         void deleteAllUserValues(string userId);

        /**
         * Delete all user related values of one app
         *
         * @param string appName the appName of the app that we want to remove all values from
         * @since 8.0.0
         */
         void deleteAppFromAllUsers(string appName);

        /**
         * Determines the users that have the given value set for a specific app-key-pair
         *
         * @param string appName the app to get the user for
         * @param string key the key to get the user for
         * @param string value the value to get the user for
         * @return array of user IDs
         * @since 8.0.0
         */
         IList<string> getUsersForUserValue(string appName, string key, string value);
    }

}
