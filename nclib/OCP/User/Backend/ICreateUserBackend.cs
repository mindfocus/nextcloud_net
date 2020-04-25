using System;
namespace OCP.User.Backend
{
    /**
     * @since 14.0.0
     */
    public interface ICreateUserBackend
    {

        /**
         * @since 14.0.0
         *
         * @param string uid The username of the user to create
         * @param string password The password of the new user
         * @return bool
         */
        bool createUser(string uid, string password);
}
}
