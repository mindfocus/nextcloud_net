using System;
namespace OCP.User.Backend
{
    /**
     * @since 14.0.0
     */
    public interface IGetDisplayNameBackend
    {

        /**
         * @since 14.0.0
         *
         * @param string uid user ID of the user
         * @return string display name
         */
        string getDisplayName(string uid);
}

}
