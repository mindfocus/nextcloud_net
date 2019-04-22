//using System;
//using OCP;

//namespace OCP.User.Backend
//{
//    /**
//     * @since 14.0.0
//     */
//    abstract class ABackend : IUserBackend, UserInterface {

//    /**
//     * @deprecated 14.0.0
//     *
//     * @param int actions The action to check for
//     * @return bool
//     */
//    public function implementsActions(actions): bool {
//        implements = 0;

//        if (this instanceof ICreateUserBackend) {
//            implements |= Backend::CREATE_USER;
//    }
//        if (this instanceof ISetPasswordBackend) {
//            implements |= Backend::SET_PASSWORD;
//    }
//        if (this instanceof ICheckPasswordBackend) {
//            implements |= Backend::CHECK_PASSWORD;
//    }
//        if (this instanceof IGetHomeBackend) {
//            implements |= Backend::GET_HOME;
//    }
//        if (this instanceof IGetDisplayNameBackend) {
//            implements |= Backend::GET_DISPLAYNAME;
//    }
//        if (this instanceof ISetDisplayNameBackend) {
//            implements |= Backend::SET_DISPLAYNAME;
//    }
//        if (this instanceof IProvideAvatarBackend) {
//            implements |= Backend::PROVIDE_AVATAR;
//    }
//        if (this instanceof ICountUsersBackend) {
//            implements |= Backend::COUNT_USERS;
//    }

//        return (bool)(actions & implements);
//    }
//}

//}
