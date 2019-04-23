using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.ShareNS
{
    /**
     * Interface IShareProvider
     *
     * @package OCP\Share
     * @since 9.0.0
     */
    public interface IShareProvider
    {

        /**
         * Return the identifier of this provider.
         *
         * @return string Containing only [a-zA-Z0-9]
         * @since 9.0.0
         */
        string identifier();

        /**
         * Create a share
         *
         * @param IShare share
         * @return IShare The share object
         * @since 9.0.0
         */
        IShare create(IShare share);

        /**
         * Update a share
         *
         * @param IShare share
         * @return IShare The share object
         * @since 9.0.0
         */
        IShare update(IShare share);

        /**
         * Delete a share
         *
         * @param IShare share
         * @since 9.0.0
         */
        void delete(IShare share);

        /**
         * Unshare a file from self as recipient.
         * This may require special handling. If a user unshares a group
         * share from their self then the original group share should still exist.
         *
         * @param IShare share
         * @param string recipient UserId of the recipient
         * @since 9.0.0
         */
        void deleteFromSelf(IShare share, string recipient);

        /**
         * Restore a share for a given recipient. The implementation could be provider independant.
         *
         * @param IShare share
         * @param string recipient
         * @return IShare The restored share object
         *
         * @since 14.0.0
         * @throws GenericShareException In case the share could not be restored
         */
        IShare restore(IShare share, string recipient);

	/**
	 * Move a share as a recipient.
	 * This is updating the share target. Thus the mount point of the recipient.
	 * This may require special handling. If a user moves a group share
	 * the target should only be changed for them.
	 *
	 * @param IShare share
	 * @param string recipient userId of recipient
	 * @return IShare
	 * @since 9.0.0
	 */
	IShare move(IShare share, string recipient);

        /**
         * Get all shares by the given user in a folder
         *
         * @param string userId
         * @param Folder node
         * @param bool reshares Also get the shares where user is the owner instead of just the shares where user is the initiator
         * @return IShare[]
         * @since 11.0.0
         */
        IList<IShare > getSharesInFolder(string userId, Files.Folder node, bool reshares);

        /**
         * Get all shares by the given user
         *
         * @param string userId
         * @param int shareType
         * @param Node|null node
         * @param bool reshares Also get the shares where user is the owner instead of just the shares where user is the initiator
         * @param int limit The maximum number of shares to be returned, -1 for all shares
         * @param int offset
         * @return IShare[]
         * @since 9.0.0
         */
        IList<IShare> getSharesBy(string userId, int shareType, Files.Node node, bool reshares, int limit, int offset);

        /**
         * Get share by id
         *
         * @param int id
         * @param string|null recipientId
         * @return IShare
         * @throws ShareNotFound
         * @since 9.0.0
         */
        IShare getShareById(string id, string recipientId = null);

        /**
         * Get shares for a given path
         *
         * @param Node path
         * @return IShare[]
         * @since 9.0.0
         */
        IList<IShare > getSharesByPath(Files.Node path);

        /**
         * Get shared with the given user
         *
         * @param string userId get shares where this user is the recipient
         * @param int shareType
         * @param Node|null node
         * @param int limit The max number of entries returned, -1 for all
         * @param int offset
         * @return IShare[]
         * @since 9.0.0
         */
        IList<IShare> getSharedWith(string userId, int shareType, Files.Node node, int limit, int offset);

        /**
         * Get a share by token
         *
         * @param string token
         * @return IShare
         * @throws ShareNotFound
         * @since 9.0.0
         */
        IShare getShareByToken(string token);

        /**
         * A user is deleted from the system
         * So clean up the relevant shares.
         *
         * @param string uid
         * @param int shareType
         * @since 9.1.0
         */
        void userDeleted(string uid, int shareType);

        /**
         * A group is deleted from the system.
         * We have to clean up all shares to this group.
         * Providers not handling group shares should just return
         *
         * @param string gid
         * @since 9.1.0
         */
        void groupDeleted(string gid);

        /**
         * A user is deleted from a group
         * We have to clean up all the related user specific group shares
         * Providers not handling group shares should just return
         *
         * @param string uid
         * @param string gid
         * @since 9.1.0
         */
        void userDeletedFromGroup(string uid, string gid);

        /**
         * Get the access list to the array of provided nodes.
         *
         * @see IManager::getAccessList() for sample docs
         *
         * @param Node[] nodes The list of nodes to get access for
         * @param bool currentAccess If current access is required (like for removed shares that might get revived later)
         * @return array
         * @since 12
         */
        PhpArray getAccessList(IList<Files.Node> nodes, bool currentAccess);
    }

}
