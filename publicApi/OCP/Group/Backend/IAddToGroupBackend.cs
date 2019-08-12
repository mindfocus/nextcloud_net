using System;

namespace OCP.Group.Backend
{
    /**
     * @since 14.0.0
     */
    public interface IAddToGroupBackend : IBackend
    {
        /**
         * @since 14.0.0
         */
        bool addToGroup(string uid, string gid);
    }
}