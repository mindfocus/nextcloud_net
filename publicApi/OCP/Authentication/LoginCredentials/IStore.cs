using System;
namespace OCP.Authentication.LoginCredentials
{
    /**
     * @since 12
     */
    public interface IStore
    {

        /**
         * Get login credentials of the currently logged in user
         *
         * @since 12
         *
         * @throws CredentialsUnavailableException
         * @return ICredentials the login credentials of the current user
         */
        public ICredentials getLoginCredentials();

    }
}
