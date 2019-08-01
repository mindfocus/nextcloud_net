namespace OCP.Authentication.TwoFactorAuth
{
    public interface ILoginSetupProvider
    {
        /**
         * @return Template
         *
         * @since 17.0.0
         */
        Template getBody();
    }
}