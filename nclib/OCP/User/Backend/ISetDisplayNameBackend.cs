using System;
namespace OCP.User.Backend
{
    /**
     * @since 14.0.0
     */
    public interface ISetDisplayNameBackend
    {

        /**
         * @since 14.0.0
         *
         * @param string uid The username
         * @param string displayName The new display name
         * @return bool
         */
        bool setDisplayName(string uid, string displayName);
}

}
