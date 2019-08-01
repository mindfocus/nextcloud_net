using System;
namespace OCP.Authentication.TwoFactorAuth
{
    /**
     * @since 9.1.0
     */
    public interface IProvider
    {

        /**
         * @since 14.0.0
         */
        //const EVENT_SUCCESS = self::class . '::success';
    //const EVENT_FAILED  = self::class . '::failed';

    /**
     * Get unique identifier of this 2FA provider
     *
     * @since 9.1.0
     *
     * @return string
     */
    string getId() ;

    /**
     * Get the display name for selecting the 2FA provider
     *
     * Example: "Email"
     *
     * @since 9.1.0
     *
     * @return string
     */
    string getDisplayName();

    /**
     * Get the description for selecting the 2FA provider
     *
     * Example: "Get a token via e-mail"
     *
     * @since 9.1.0
     *
     * @return string
     */
    string getDescription();

    /**
     * Get the template for rending the 2FA provider view
     *
     * @since 9.1.0
     *
     * @param IUser user
     * @return Template
     */
    Template getTemplate(IUser user);

    /**
     * Verify the given challenge
     *
     * @since 9.1.0
     *
     * @param IUser user
     * @param string challenge
     * @return bool
     */
    bool verifyChallenge(IUser user, string challenge);

    /**
     * Decides whether 2FA is enabled for the given user
     *
     * @since 9.1.0
     *
     * @param IUser user
     * @return bool
     */
    bool isTwoFactorAuthEnabledForUser(IUser user);
}

}
