using System;
namespace OCP.Authentication.TwoFactorAuth
{
    /**
     * Two Factor Authentication failed
     *
     * It defines an Exception a 2FA app can
     * throw in case of an error. The 2FA Controller will catch this exception and
     * display this error.
     *
     * @since 12
     */
    class TwoFactorException : System.Exception
    {

    }

}
