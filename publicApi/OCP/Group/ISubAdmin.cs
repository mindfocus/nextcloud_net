using System;
using System.Collections.Generic;

namespace OCP.Group
{
    /**
     * @since 16.0.0
     */
    public interface ISubAdmin
    {

        /**
         * add a SubAdmin
         * @param IUser user user to be SubAdmin
         * @param IGroup group group user becomes subadmin of
         *
         * @since 16.0.0
         */
        void createSubAdmin(IUser user, IGroup group);

    /**
     * delete a SubAdmin
     * @param IUser user the user that is the SubAdmin
     * @param IGroup group the group
     *
     * @since 16.0.0
     */
    void deleteSubAdmin(IUser user, IGroup group);

    /**
     * get groups of a SubAdmin
     * @param IUser user the SubAdmin
     * @return IGroup[]
     *
     * @since 16.0.0
     */
    IList<IGroup> getSubAdminsGroups(IUser user);

        /**
         * get SubAdmins of a group
         * @param IGroup group the group
         * @return IUser[]
         *
         * @since 16.0.0
         */
        IList<IUser> getGroupsSubAdmins(IGroup group);

    /**
     * checks if a user is a SubAdmin of a group
     * @param IUser user
     * @param IGroup group
     * @return bool
     *
     * @since 16.0.0
     */
    bool isSubAdminOfGroup(IUser user, IGroup group);

    /**
     * checks if a user is a SubAdmin
     * @param IUser user
     * @return bool
     *
     * @since 16.0.0
     */
    bool isSubAdmin(IUser user);

    /**
     * checks if a user is a accessible by a subadmin
     * @param IUser subadmin
     * @param IUser user
     * @return bool
     *
     * @since 16.0.0
     */
    bool isUserAccessible(IUser subadmin, IUser user);
}

}
