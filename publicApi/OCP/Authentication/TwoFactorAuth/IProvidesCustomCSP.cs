using System;
using OCP.AppFramework.Http;

namespace OCP.Authentication.TwoFactorAuth
{
    /**
     * @since 13.0.0
     */
    public interface IProvidesCustomCSP
    {

        /**
         * @return ContentSecurityPolicy
         *
         * @since 13.0.0
         */
        ContentSecurityPolicy getCSP();
}

}
