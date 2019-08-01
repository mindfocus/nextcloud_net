namespace OCP.Authentication.TwoFactorAuth
{
    public interface IActivatableAtLogin : IProvider
    {
        
        /**
         * @param IUser $user
         *
         * @return ILoginSetupProvider
         *
         * @since 17.0.0
         */
        ILoginSetupProvider getLoginSetup(IUser user) ;
    }
}