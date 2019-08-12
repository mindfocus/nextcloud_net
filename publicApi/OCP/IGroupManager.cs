using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Class Manager
     *
     * Hooks available in scope \OC\Group:
     * - preAddUser(\OC\Group\Group group, \OC\User\User user)
     * - postAddUser(\OC\Group\Group group, \OC\User\User user)
     * - preRemoveUser(\OC\Group\Group group, \OC\User\User user)
     * - postRemoveUser(\OC\Group\Group group, \OC\User\User user)
     * - preDelete(\OC\Group\Group group)
     * - postDelete(\OC\Group\Group group)
     * - preCreate(string groupId)
     * - postCreate(\OC\Group\Group group)
     *
     * @package OC\Group
     * @since 8.0.0
     */
    public interface IGroupManager
    {
        /**
         * Checks whether a given backend is used
         *
         * @param string backendClass Full classname including complete namespace
         * @return bool
         * @since 8.1.0
         */
        bool isBackendUsed(string backendClass);

        /**
         * @param \OCP\GroupInterface backend
         * @since 8.0.0
         */
        void addBackend(GroupInterface backend);

        /**
         * @since 8.0.0
         */
        void clearBackends();

        /**
         * Get the active backends
         * @return \OCP\GroupInterface[]
         * @since 13.0.0
         */
        IList<GroupInterface> getBackends();

        /**
         * @param string gid
         * @return \OCP\IGroup
         * @since 8.0.0
         */
        IGroup get(string gid);

        /**
         * @param string gid
         * @return bool
         * @since 8.0.0
         */
        bool groupExists(string gid);

        /**
         * @param string gid
         * @return \OCP\IGroup
         * @since 8.0.0
         */
        IGroup createGroup(string gid);

        /**
         * @param string search
         * @param int limit
         * @param int offset
         * @return \OCP\IGroup[]
         * @since 8.0.0
         */
        IList<IGroup> search(string search, int limit = -1, int offset = 0);

        /**
         * @param \OCP\IUser|null user
         * @return \OCP\IGroup[]
         * @since 8.0.0
         */
        IList<IGroup> getUserGroups(IUser user = null);

        /**
         * @param \OCP\IUser user
         * @return array with group names
         * @since 8.0.0
         */
        IList<string> getUserGroupIds(IUser user);

        /**
         * get a list of all display names in a group
         *
         * @param string gid
         * @param string search
         * @param int limit
         * @param int offset
         * @return array an array of display names (value) and user ids (key)
         * @since 8.0.0
         */
        IDictionary<string,IList<string>>  displayNamesInGroup(string gid, string search = "", int limit = -1,int offset = 0);

        /**
         * Checks if a userId is in the admin group
         * @param string userId
         * @return bool if admin
         * @since 8.0.0
         */
        bool isAdmin(string userId);

        /**
         * Checks if a userId is in a group
         * @param string userId
         * @param string group
         * @return bool if in group
         * @since 8.0.0
         */
        bool isInGroup(string userId, string group);
    }

}
