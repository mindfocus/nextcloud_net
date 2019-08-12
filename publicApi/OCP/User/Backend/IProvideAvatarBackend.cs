using System;
namespace OCP.User.Backend
{
    /**
     * @since 14.0.0
     */
    public interface IProvideAvatarBackend
    {

        /**
         * @since 14.0.0
         *
         * @param string uid
         * @return bool
         */
        bool canChangeAvatar(string uid);
}

}
