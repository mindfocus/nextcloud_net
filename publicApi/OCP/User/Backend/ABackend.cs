//using System;
//using OCP;

namespace OCP.User.Backend
{
    /**
     * @since 14.0.0
     */
    abstract class ABackend : IUserBackend, UserInterface
    {

        /**
         * @deprecated 14.0.0
         *
         * @param int actions The action to check for
         * @return bool
         */
        public bool implementsActions(string actions) {
            var implements = 0;

        if (this is ICreateUserBackend) {
            implements |= Backend::CREATE_USER;
        }
        if (this is ISetPasswordBackend) {
            implements |= Backend::SET_PASSWORD;
        }
        if (this is ICheckPasswordBackend) {
            implements |= Backend::CHECK_PASSWORD;
        }
        if (this is IGetHomeBackend) {
            implements |= Backend::GET_HOME;
        }
        if (this is IGetDisplayNameBackend) {
            implements |= Backend::GET_DISPLAYNAME;
        }
        if (this is ISetDisplayNameBackend) {
            implements |= Backend::SET_DISPLAYNAME;
        }
        if (this is IProvideAvatarBackend) {
            implements |= Backend::PROVIDE_AVATAR;
        }
        if (this is ICountUsersBackend) {
            implements |= Backend::COUNT_USERS;
        }

        return (bool) (actions & implements);
    }
}

}
