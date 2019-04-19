using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Interface IGroup
     *
     * @package OCP
     * @since 8.0.0
     */
    public interface IGroup
    {
        /**
         * @return string
         * @since 8.0.0
         */
        string getGID();

        /**
         * Returns the group display name
         *
         * @return string
         * @since 12.0.0
         */
        string getDisplayName();

        /**
         * get all users in the group
         *
         * @return \OCP\IUser[]
         * @since 8.0.0
         */
        IUser[] getUsers();

        /**
         * check if a user is in the group
         *
         * @param \OCP\IUser $user
         * @return bool
         * @since 8.0.0
         */
        bool inGroup(IUser user);

        /**
         * add a user to the group
         *
         * @param \OCP\IUser $user
         * @since 8.0.0
         */
        bool addUser(IUser user);

        /**
         * remove a user from the group
         *
         * @param \OCP\IUser $user
         * @since 8.0.0
         */
        bool removeUser(IUser user);

        /**
         * search for users in the group by userid
         *
         * @param string $search
         * @param int $limit
         * @param int $offset
         * @return \OCP\IUser[]
         * @since 8.0.0
         */
        IUser[] searchUsers(string search, int? limit = null, int? offset = null);

        /**
         * returns the number of users matching the search string
         *
         * @param string $search
         * @return int|bool
         * @since 8.0.0
         */
        int count(string search = "");

        /**
         * returns the number of disabled users
         *
         * @return int|bool
         * @since 14.0.0
         */
        int countDisabled();

        /**
         * search for users in the group by displayname
         *
         * @param string $search
         * @param int $limit
         * @param int $offset
         * @return \OCP\IUser[]
         * @since 8.0.0
         */
        IUser[] searchDisplayName(string search, int? limit = null, int? offset = null);

        /**
         * delete the group
         *
         * @return bool
         * @since 8.0.0
         */
        bool delete();

        /**
         * @return bool
         * @since 14.0.0
         */
        bool canRemoveUser();

        /**
         * @return bool
         * @since 14.0.0
         */
        bool canAddUser();

        /**
         * @return bool
         * @since 16.0.0
         */
        bool hideFromCollaboration();
}

}
