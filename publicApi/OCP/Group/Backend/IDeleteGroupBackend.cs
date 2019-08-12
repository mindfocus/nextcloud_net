using System;
namespace OCP.Group.Backend
{
    /**
     * @since 14.0.0
     */
    public interface IDeleteGroupBackend:IBackend
    {

        /**
         * @since 14.0.0
         */
        bool deleteGroup(string gid);
}
}
