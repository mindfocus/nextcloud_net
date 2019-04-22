using System;
using OC.Authentication.Token;

namespace OCP.Lockdown
{
    /**
     * @since 9.2
     */
    public interface ILockdownManager
    {
        /**
         * Enable the lockdown restrictions
         *
         * @since 9.2
         */
        void enable();

        /**
         * Set the active token to get the restrictions from and enable the lockdown
         * use OC\Authentication\Token\IToken;
         * @param IToken token
         * @since 9.2
         */
        void setToken(IToken token);

        /**
         * Check whether or not filesystem access is allowed
         *
         * @return bool
         * @since 9.2
         */
        bool canAccessFilesystem();
    }

}
