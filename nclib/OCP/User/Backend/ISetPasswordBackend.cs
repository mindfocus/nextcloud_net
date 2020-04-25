using System;
namespace OCP.User.Backend
{
    /**
     * @since 14.0.0
     */
    public interface ISetPasswordBackend
    {

        /**
         * @since 14.0.0
         *
         * @param string uid The username
         * @param string password The new password
         * @return bool
         */
        bool setPassword(string uid, string password);
}

}
