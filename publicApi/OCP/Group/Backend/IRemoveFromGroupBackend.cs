using System;
namespace OCP.Group.Backend
{
    /**
     * @since 14.0.0
     */
    interface IRemoveFromGroupBackend : IBackend
    {

        /**
         * @since 14.0.0
         */
        void removeFromGroup(string uid, string gid);
    }

}
