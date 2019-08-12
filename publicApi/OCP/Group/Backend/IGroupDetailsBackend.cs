using System;
namespace OCP.Group.Backend
{
    /**
     * @since 14.0.0
     */
    public interface IGroupDetailsBackend:IBackend
    {

        /**
         * @since 14.0.0
         */
        Pchp.Core.PhpArray getGroupDetails(string gid);
}

}
