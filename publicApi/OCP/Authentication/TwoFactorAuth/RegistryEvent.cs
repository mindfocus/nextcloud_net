using System;
using ext;

namespace OCP.Authentication.TwoFactorAuth
{
    /*
     * @since 15.0.0
     */
    public class RegistryEvent : Event
    {

    /** @var IProvider */
    private IProvider provider;

    /** @IUser */
    private IUser user;

    /*
     * @since 15.0.0
     */
    public RegistryEvent(IProvider provider, IUser user)
    {
        this.provider = provider;
        this.user = user;
    }

    /*
     * @since 15.0.0
     */
    public IProvider getProvider() {
        return this.provider;
    }

    /*
     * @since 15.0.0
     */
    public IUser getUser() {
        return this.user;
    }
}

}
