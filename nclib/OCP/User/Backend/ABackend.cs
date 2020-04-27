//using System;
//using OCP;

using System.Collections.Generic;

namespace OCP.User.Backend
{
    /**
     * @since 14.0.0
     */
    public abstract class ABackend : IUserBackend, UserInterface
    {

        /**
         * @deprecated 14.0.0
         *
         * @param int actions The action to check for
         * @return bool
         */
        public bool implementsActions(int actions) {
            int implements = 0;

        if (this is ICreateUserBackend) {
            implements |= BackendConst.CREATE_USER;
        }
        if (this is ISetPasswordBackend) {
            implements |= BackendConst.SET_PASSWORD;
        }
        if (this is ICheckPasswordBackend) {
            implements |= BackendConst.CHECK_PASSWORD;
        }
        if (this is IGetHomeBackend) {
            implements |= BackendConst.GET_HOME;
        }
        if (this is IGetDisplayNameBackend) {
            implements |= BackendConst.GET_DISPLAYNAME;
        }
        if (this is ISetDisplayNameBackend) {
            implements |= BackendConst.SET_DISPLAYNAME;
        }
        if (this is IProvideAvatarBackend) {
            implements |= BackendConst.PROVIDE_AVATAR;
        }
        if (this is ICountUsersBackend) {
            implements |= BackendConst.COUNT_USERS;
        }

        return (actions & implements) == 0;
    }

        public abstract bool deleteUser(string uid);
        public abstract IList<string> getUsers(string search = "", int? limit = null, int? offset = null);
        public abstract bool userExists(string uid);
        public abstract string getDisplayName(string uid);
        public abstract IDictionary<string, string> getDisplayNames(string search = "", int? limit = null, int? offset = null);
        public abstract bool hasUserListings();

        public abstract string getBackendName();
    }

}
