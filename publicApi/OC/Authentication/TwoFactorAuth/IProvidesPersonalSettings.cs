using System;
namespace publicApi.Authentication.TwoFactorAuth
{
    /**
     * Interface for admins that have personal settings. These settings will be shown in the
     * security sections. Some information like the display name of the provider is read
     * from the provider directly.
     *
     * @since 15.0.0
     */
    public interface IProvidesPersonalSettings : IProvider
    {

    /**
     * @param IUser $user
     *
     * @return IPersonalProviderSettings
     *
     * @since 15.0.0
     */
    public IPersonalProviderSettings getPersonalSettings(IUser user) ;

}

}
