using System;
namespace OCP.User.Backend
{
    /**
     * @since 14.0.0
     */
    public interface ICountUsersBackend
    {

        /**
         * @since 14.0.0
         *
         * @return int|bool The number of users on success false on failure
         */
        int? countUsers();
    }

}
