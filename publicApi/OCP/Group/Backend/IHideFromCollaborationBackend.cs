using System;
namespace OCP.Group.Backend
{
    /**
     * @since 16.0.0
     *
     * Allow the backend to mark groups to be excluded from being shown in search dialogs
     */
    public interface IHideFromCollaborationBackend
    {
        /**
         * Check if a group should be hidden from search dialogs
         *
         * @param string groupId
         * @return bool
         * @since 16.0.0
         */
        bool hideGroup(string groupId) ;
}

}
