using System;
namespace OCP.Authentication.TwoFactorAuth
{
    /**
     * Marks a 2FA provider as activale by the administrator. This means that an
     * admin can activate this provider without user interaction. The provider,
     * therefore, must not require any user-provided configuration.
     *
     * @since 15.0.0
     */
    interface IDeactivatableByAdmin : IProvider
    {

    /**
     * Disable this provider for the given user.
     *
     * @param IUser user the user to deactivate this provider for
     *
     * @return void
     *
     * @since 15.0.0
     */
    public void disableFor(IUser user);

}

}
