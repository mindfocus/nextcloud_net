using System;
namespace OCP.Authentication.LoginCredentials
{
    /**
     * @since 12
     */
    public interface ICredentials
    {

        /**
         * Get the user UID
         *
         * @since 12
         *
         * @return string
         */
        string getUID();

        /**
         * Get the login name the users used to login
         *
         * @since 12
         *
         * @return string
         */
        string getLoginName();

        /**
         * Get the password
         *
         * @since 12
         *
         * @return string
         * @throws PasswordUnavailableException
         */
        string getPassword();
    }

}
