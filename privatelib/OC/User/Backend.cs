using ext;
using System;
using System.Collections.Generic;
using System.Text;

namespace OC.User
{
    /**
     * Abstract base class for user management. Provides methods for querying backend
     * capabilities.
     */
    public abstract class Backend : OCP.UserInterface
    {
        /**
         * error code for functions not provided by the user backend
         */
        public static int NOT_IMPLEMENTED = -501;

        /**
         * actions that user backends can define
         */
        public static int CREATE_USER = 1; // 1 << 0
        public static int SET_PASSWORD = 16; // 1 << 4
        public static int CHECK_PASSWORD = 256; // 1 << 8
        public static int GET_HOME = 4096; // 1 << 12
        public static int GET_DISPLAYNAME = 65536; // 1 << 16
        public static int SET_DISPLAYNAME = 1048576; // 1 << 20
        public static int PROVIDE_AVATAR = 16777216; // 1 << 24
        public static int COUNT_USERS = 268435456; // 1 << 28


        protected IDictionary<int, string> possibleActions = new Dictionary<int, string>
        {
            {CREATE_USER, "createUser"}, {SET_PASSWORD, "setPassword"}, {CHECK_PASSWORD, "checkPassword"},
            {GET_HOME, "getHome"},
            {GET_DISPLAYNAME, "getDisplayName"}, {SET_DISPLAYNAME, "setDisplayName"},
            {PROVIDE_AVATAR, "canChangeAvatar"},
            {COUNT_USERS, "countUsers"}
        };

        /**
        * Get all supported actions
        * @return int bitwise-or'ed actions
        *
        * Returns the supported actions as int to be
        * compared with self::CREATE_USER etc.
        */
        public int getSupportedActions()
        {
            var actions = 0;
            foreach (var action in this.possibleActions)
            {
                if (ClassUtility.HasMethod(this, action.Value))
                {
                    actions |= action.Key;
                }
            }

            return actions;
        }

        /**
        * Check if backend implements actions
        * @param int $actions bitwise-or'ed actions
        * @return boolean
        *
        * Returns the supported actions as int to be
        * compared with self::CREATE_USER etc.
        */
        public bool implementsActions(int actions)
        {
            return (getSupportedActions() & actions) != 0;
        }

        /**
         * delete a user
         * @param string $uid The username of the user to delete
         * @return bool
         *
         * Deletes a user
         */
        public bool deleteUser(string uid)
        {
            return false;
        }

        /**
         * Get a list of all users
         *
         * @param string $search
         * @param null|int $limit
         * @param null|int $offset
         * @return string[] an array of all uids
         */
        public IList<string> getUsers(string search = "", int? limit = null, int? offset = null)
        {
            return new List<string>();
        }

        /**
        * check if a user exists
        * @param string $uid the username
        * @return boolean
        */
        public bool userExists(string uid)
        {
            return false;
        }

        /**
        * get the user's home directory
        * @param string $uid the username
        * @return boolean
        */
        public bool getHome(string uid)
        {
            return false;
        }

        /**
         * get display name of the user
         * @param string $uid user ID of the user
         * @return string display name
         */
        public string getDisplayName(string uid)
        {
            return uid;
        }

        /**
         * Get a list of all display names and user ids.
         *
         * @param string $search
         * @param string|null $limit
         * @param string|null $offset
         * @return array an array of all displayNames (value) and the corresponding uids (key)
         */
        public IDictionary<string, string> getDisplayNames(string search = "", int? limit = null, int? offset = null)
        {
            var displayNames = new Dictionary<string, string>();
            var users = this.getUsers(search, limit, offset);
            foreach (var user in users)
            {
                displayNames[user] = user;
            }

            return displayNames;
        }

        /**
         * Check if a user list is available or not
         * @return boolean if users can be listed or not
         */
        public bool hasUserListings()
        {
            return false;
        }
    }
}