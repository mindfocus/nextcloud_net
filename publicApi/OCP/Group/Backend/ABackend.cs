namespace OCP.Group.Backend
{
/**
 * @since 14.0.0
 */
    public abstract class ABackend : GroupInterface {

    /**
     * @deprecated 14.0.0
     *
     * @param int actions The action to check for
     * @return bool
     */
    public bool implementsActions(int actions) {
        implements = 0;

        if (this instanceof IAddToGroupBackend) {
            implements |= GroupInterface::ADD_TO_GROUP;
        }
        if (this instanceof ICountUsersBackend) {
            implements |= GroupInterface::COUNT_USERS;
        }
        if (this instanceof ICreateGroupBackend) {
            implements |= GroupInterface::CREATE_GROUP;
        }
        if (this instanceof IDeleteGroupBackend) {
            implements |= GroupInterface::DELETE_GROUP;
        }
        if (this instanceof IGroupDetailsBackend) {
            implements |= GroupInterface::GROUP_DETAILS;
        }
        if (this instanceof IIsAdminBackend) {
            implements |= GroupInterface::IS_ADMIN;
        }
        if (this instanceof IRemoveFromGroupBackend) {
            implements |= GroupInterface::REMOVE_FROM_GOUP;
        }

        return (bool)(actions & implements);
    }
    }

}