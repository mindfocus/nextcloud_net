using System;
namespace OCP.Authentication.TwoFactorAuth
{
    /**
     * Marks a 2FA provider as activatable by the administrator. This means that an
     * admin can activate this provider without user interaction. The provider,
     * therefore, must not require any user-provided configuration.
     *
     * @since 15.0.0
     */
    interface IActivatableByAdmin : IProvider
    {

    /**
     * Enable this provider for the given user.
     *
     * @param IUser user the user to activate this provider for
     *
     * @return void
     *
     * @since 15.0.0
     */
    void enableFor(IUser user);

}

}
