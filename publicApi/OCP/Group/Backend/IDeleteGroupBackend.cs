using System;
namespace OCP.Group.Backend
{
    /**
     * @since 14.0.0
     */
    public interface IDeleteGroupBackend
    {

        /**
         * @since 14.0.0
         */
        bool deleteGroup(string gid);
}
}
