﻿using System;
namespace OCP.Authentication.TwoFactorAuth
{
    /**
     * Interface for two-factor providers that provide dark and light provider
     * icons
     *
     * @since 15.0.0
     */
    public interface IProvidesIcons : IProvider
    {

    /**
     * Get the path to the light (white) icon of this provider
     *
     * @return String
     *
     * @since 15.0.0
     */
    string getLightIcon();

    /**
     * Get the path to the dark (black) icon of this provider
     *
     * @return String
     *
     * @since 15.0.0
     */
    string getDarkIcon();

}

}
