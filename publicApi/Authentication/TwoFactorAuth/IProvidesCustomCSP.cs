﻿using System;
namespace publicApi.Authentication.TwoFactorAuth
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
        public ContentSecurityPolicy getCSP();
}

}
