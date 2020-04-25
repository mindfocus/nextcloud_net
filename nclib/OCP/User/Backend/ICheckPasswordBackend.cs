using System;
namespace OCP.User.Backend
{
    /**
     * @since 14.0.0
     */
    public interface ICheckPasswordBackend
    {
        /**
         * @since 14.0.0
         *
         * @param string uid The username
         * @param string password The password
         * @return string|bool The uid on success false on failure
         */
        string checkPassword(string loginName, string password);
    }

}
