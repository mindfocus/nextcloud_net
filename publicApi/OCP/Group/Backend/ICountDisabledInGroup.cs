using System;
namespace OCP.Group.Backend
{
    /**
     * @since 14.0.0
     */
    public interface ICountDisabledInGroup : IBackend
    {

        /**
         * @since 14.0.0
         */
        int countDisabledInGroup(string gid);
}

}
