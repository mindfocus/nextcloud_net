using System;
namespace OCP.Group.Backend
{
    /**
     * @since 14.0.0
     */
    public interface ICountUsersBackend
    {

        /**
         * @since 14.0.0
         */
        int countUsersInGroup(string gid, string search = "");
}

}
