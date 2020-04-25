using System;
namespace OCP.User.Backend
{
    /**
     * @since 14.0.0
     */
    public interface IGetHomeBackend
    {

        /**
         * @since 14.0.0
         *
         * @param string uid the username
         * @return string|bool Datadir on success false on failure
         */
        string? getHome(string uid);
    }

}
