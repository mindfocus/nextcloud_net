using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * TODO actually this is a IGroupBackend
     *
     * @package OCP
     * @since 4.5.0
     */
    public interface GroupInterface
    {

        /**
         * actions that user backends can define
         */
        //const CREATE_GROUP      = 0x00000001;
        //const DELETE_GROUP      = 0x00000010;
        //const ADD_TO_GROUP      = 0x00000100;
        //const REMOVE_FROM_GOUP  = 0x00001000; // oops
        //const REMOVE_FROM_GROUP = 0x00001000;
        //OBSOLETE const GET_DISPLAYNAME	= 0x00010000;
        //const COUNT_USERS       = 0x00100000;
        //const GROUP_DETAILS     = 0x01000000;
        /**
         * @since 13.0.0
         */
        //const IS_ADMIN  = 0x10000000;

        /**
         * Check if backend implements actions
         * @param int $actions bitwise-or'ed actions
         * @return boolean
         * @since 4.5.0
         *
         * Returns the supported actions as int to be
         * compared with \OC_Group_Backend::CREATE_GROUP etc.
         */
        //public function implementsActions($actions);

        /**
         * is user in group?
         * @param string $uid uid of the user
         * @param string $gid gid of the group
         * @return bool
         * @since 4.5.0
         *
         * Checks whether the user is member of a group or not.
         */
        bool inGroup(string uid, string gid);

        /**
         * Get all groups a user belongs to
         * @param string $uid Name of the user
         * @return array an array of group names
         * @since 4.5.0
         *
         * This function fetches all groups a user belongs to. It does not check
         * if the user exists at all.
         */
        IList<string> getUserGroups(string uid);

        /**
         * get a list of all groups
         * @param string $search
         * @param int $limit
         * @param int $offset
         * @return array an array of group names
         * @since 4.5.0
         *
         * Returns a list with all groups
         */
        IList<IGroup> getGroups(string search = "", int limit = -1, int offset = 0);

        /**
         * check if a group exists
         * @param string $gid
         * @return bool
         * @since 4.5.0
         */
        bool groupExists(string gid);

        /**
         * get a list of all users in a group
         * @param string $gid
         * @param string $search
         * @param int $limit
         * @param int $offset
         * @return array an array of user ids
         * @since 4.5.0
         */
        IList<string> usersInGroup(string gid, string search = "", int limit = -1, int offset = 0);

    }

}
