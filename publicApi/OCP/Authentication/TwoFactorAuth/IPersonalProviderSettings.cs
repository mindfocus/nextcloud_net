using System;
namespace OCP.Authentication.TwoFactorAuth
{
    /**
     * Interface IPersonalProviderSettings
     *
     * @since 15.0.0
     */
    public interface IPersonalProviderSettings
    {

        /**
         * @return Template
         *
         * @since 15.0.0
         */
        public Template getBody();

}

}
