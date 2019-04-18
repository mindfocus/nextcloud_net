using System;
using System.Collections.Generic;

namespace OCP
{
    /**
     * TODO actually this is a IUserBackend
     *
     * @package OCP
     * @since 4.5.0
     */
    public interface UserInterface
    {

        /**
         * Check if backend implements actions
         * @param int $actions bitwise-or'ed actions
         * @return boolean
         *
         * Returns the supported actions as int to be
         * compared with \OC\User\Backend::CREATE_USER etc.
         * @since 4.5.0
         * @deprecated 14.0.0 Switch to the interfaces from OCP\User\Backend
         */
        bool implementsActions(Action actions);

        /**
         * delete a user
         * @param string $uid The username of the user to delete
         * @return bool
         * @since 4.5.0
         */
        bool deleteUser(string uid);

        /**
         * Get a list of all users
         *
         * @param string $search
         * @param null|int $limit
         * @param null|int $offset
         * @return string[] an array of all uids
         * @since 4.5.0
         */
        IList<string> getUsers(string search = "", string limit = null, string offset = null);

        /**
         * check if a user exists
         * @param string $uid the username
         * @return boolean
         * @since 4.5.0
         */
        bool userExists(string uid);

        /**
         * get display name of the user
         * @param string $uid user ID of the user
         * @return string display name
         * @since 4.5.0
         */
        string getDisplayName(string uid);

        /**
         * Get a list of all display names and user ids.
         *
         * @param string $search
         * @param string|null $limit
         * @param string|null $offset
         * @return array an array of all displayNames (value) and the corresponding uids (key)
         * @since 4.5.0
         */
        IList<string> getDisplayNames(string search = "", int? limit = null, int? offset = null);

        /**
         * Check if a user list is available or not
         * @return boolean if users can be listed or not
         * @since 4.5.0
         */
        bool hasUserListings();

    }

}