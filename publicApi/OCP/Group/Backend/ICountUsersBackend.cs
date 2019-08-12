using System;
namespace OCP.Group.Backend
{
    /**
     * @since 14.0.0
     */
    public interface ICountUsersBackend : IBackend
    {

        /**
         * @since 14.0.0
         */
        int countUsersInGroup(string gid, string search = "");
}

}
