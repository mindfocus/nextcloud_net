using System;
namespace OCP.User.Backend
{
    /**
     * @since 15.0.0
     */
    interface IPasswordConfirmationBackend
    {

        /**
         * @since 15.0.0
         */
        bool canConfirmPassword(string uid);
}

}
