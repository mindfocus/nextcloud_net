using System;
namespace OCP.Group.Backend
{
    /**
     * @since 14.0.0
     */
    interface IIsAdminBackend
    {

        /**
         * @since 14.0.0
         */
        bool isAdmin(string uid) ;
}
}
