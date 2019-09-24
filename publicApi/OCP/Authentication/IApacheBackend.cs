using System;
namespace OCP.Authentication
{
    /**
     * Interface IApacheBackend
     *
     * @package OCP\Authentication
     * @since 6.0.0
     */
    public interface IApacheBackend
    {

        /**
         * In case the user has been authenticated by a module true is returned.
         *
         * @return boolean whether the module reports a user as currently logged in.
         * @since 6.0.0
         */
        bool isSessionActive();

        /**
         * Gets the current logout URL
         *
         * @return string
         * @since 12.0.3
         */
        string getLogoutUrl();

        /**
         * Return the id of the current user
         * @return string
         * @since 6.0.0
         */
        string getCurrentUserId();

    }

}
