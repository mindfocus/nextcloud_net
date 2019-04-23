using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.ShareNS
{
    /**
     * Interface IManager
     *
     * @package OCP\Share
     * @since 9.0.0
     */
    public interface IManager
    {

        /**
         * Create a Share
         *
         * @param IShare share
         * @return IShare The share object
         * @throws \Exception
         * @since 9.0.0
         */
        IShare createShare(IShare share);

        /**
         * Update a share.
         * The target of the share can't be changed this way: use moveShare
         * The share can't be removed this way (permission 0): use deleteShare
         *
         * @param IShare share
         * @return IShare The share object
         * @throws \InvalidArgumentException
         * @since 9.0.0
         */
        IShare updateShare(IShare share);

        /**
         * Delete a share
         *
         * @param IShare share
         * @throws ShareNotFound
         * @throws \InvalidArgumentException
         * @since 9.0.0
         */
        void deleteShare(IShare share);

        /**
         * Unshare a file as the recipient.
         * This can be different from a regular delete for example when one of
         * the users in a groups deletes that share. But the provider should
         * handle this.
         *
         * @param IShare share
         * @param string recipientId
         * @since 9.0.0
         */
        void deleteFromSelf(IShare share, string recipientId);

        /**
         * Restore the share when it has been deleted
         * Certain share types can be restored when they have been deleted
         * but the provider should properly handle this\
         *
         * @param IShare share The share to restore
         * @param string recipientId The user to restore the share for
         * @return IShare The restored share object
         * @throws GenericShareException In case restoring the share failed
         *
         * @since 14.0.0
         */
        IShare restoreShare(IShare share, string recipientId);

	/**
	 * Move the share as a recipient of the share.
	 * This is updating the share target. So where the recipient has the share mounted.
	 *
	 * @param IShare share
	 * @param string recipientId
	 * @return IShare
	 * @throws \InvalidArgumentException If share is a link share or the recipient does not match
	 * @since 9.0.0
	 */
	IShare moveShare(IShare share, string recipientId);

        /**
         * Get all shares shared by (initiated) by the provided user in a folder.
         *
         * @param string userId
         * @param Folder node
         * @param bool reshares
         * @return IShare[][] [fileId => IShare[], ...]
         * @since 11.0.0
         */
        IDictionary<string, IList<IShare>> getSharesInFolder(string userId, Files.Folder node, bool reshares = false);

        /**
         * Get shares shared by (initiated) by the provided user.
         *
         * @param string userId
         * @param int shareType
         * @param Node|null path
         * @param bool reshares
         * @param int limit The maximum number of returned results, -1 for all results
         * @param int offset
         * @return IShare[]
         * @since 9.0.0
         */
        IList<IShare> getSharesBy(string userId,int shareType, Files.Node? path = null, bool reshares = false, int limit = 50, int offset = 0);

        /**
         * Get shares shared with user.
         * Filter by node if provided
         *
         * @param string userId
         * @param int shareType
         * @param Node|null node
         * @param int limit The maximum number of shares returned, -1 for all
         * @param int offset
         * @return IShare[]
         * @since 9.0.0
         */
        IList<IShare> getSharedWith(string userId, int shareType, Files.Node? node = null, int limit = 50, int offset = 0);

        /**
         * Get deleted shares shared with user.
         * Filter by node if provided
         *
         * @param string userId
         * @param int shareType
         * @param Node|null node
         * @param int limit The maximum number of shares returned, -1 for all
         * @param int offset
         * @return IShare[]
         * @since 14.0.0
         */
        IList<IShare> getDeletedSharedWith(string userId, int shareType, Files.Node? node = null,int limit = 50,int offset = 0);

        /**
         * Retrieve a share by the share id.
         * If the recipient is set make sure to retrieve the file for that user.
         * This makes sure that if a user has moved/deleted a group share this
         * is reflected.
         *
         * @param string id
         * @param string|null recipient userID of the recipient
         * @return IShare
         * @throws ShareNotFound
         * @since 9.0.0
         */
        IShare getShareById(string id, string recipient = null);

        /**
         * Get the share by token possible with password
         *
         * @param string token
         * @return IShare
         * @throws ShareNotFound
         * @since 9.0.0
         */
        IShare getShareByToken(string token);

        /**
         * Verify the password of a public share
         *
         * @param IShare share
         * @param string password
         * @return bool
         * @since 9.0.0
         */
        bool checkPassword(IShare share, string password);

        /**
         * The user with UID is deleted.
         * All share providers have to cleanup the shares with this user as well
         * as shares owned by this user.
         * Shares only initiated by this user are fine.
         *
         * @param string uid
         * @since 9.1.0
         */
        void userDeleted(string uid);

        /**
         * The group with gid is deleted
         * We need to clear up all shares to this group
         *
         * @param string gid
         * @since 9.1.0
         */
        void groupDeleted(string gid);

        /**
         * The user uid is deleted from the group gid
         * All user specific group shares have to be removed
         *
         * @param string uid
         * @param string gid
         * @since 9.1.0
         */
        void userDeletedFromGroup(string uid, string gid);

        /**
         * Get access list to a path. This means
         * all the users that can access a given path.
         *
         * Consider:
         * -root
         * |-folder1 (23)
         *  |-folder2 (32)
         *   |-fileA (42)
         *
         * fileA is shared with user1 and user1@server1
         * folder2 is shared with group2 (user4 is a member of group2)
         * folder1 is shared with user2 (renamed to "folder (1)") and user2@server2
         *
         * Then the access list to '/folder1/folder2/fileA' with currentAccess is:
         * [
         *  users  => [
         *      'user1' => ['node_id' => 42, 'node_path' => '/fileA'],
         *      'user4' => ['node_id' => 32, 'node_path' => '/folder2'],
         *      'user2' => ['node_id' => 23, 'node_path' => '/folder (1)'],
         *  ],
         *  remote => [
         *      'user1@server1' => ['node_id' => 42, 'token' => 'SeCr3t'],
         *      'user2@server2' => ['node_id' => 23, 'token' => 'FooBaR'],
         *  ],
         *  public => bool
         *  mail => bool
         * ]
         *
         * The access list to '/folder1/folder2/fileA' **without** currentAccess is:
         * [
         *  users  => ['user1', 'user2', 'user4'],
         *  remote => bool,
         *  public => bool
         *  mail => bool
         * ]
         *
         * This is required for encryption/activity
         *
         * @param \OCP\Files\Node path
         * @param bool recursive Should we check all parent folders as well
         * @param bool currentAccess Should the user have currently access to the file
         * @return array
         * @since 12
         */
        PhpArray getAccessList(Files.Node path, bool recursive = true, bool currentAccess = false);

        /**
         * Instantiates a new share object. This is to be passed to
         * createShare.
         *
         * @return IShare
         * @since 9.0.0
         */
        IShare newShare();

        /**
         * Is the share API enabled
         *
         * @return bool
         * @since 9.0.0
         */
        bool shareApiEnabled();

        /**
         * Is public link sharing enabled
         *
         * @return bool
         * @since 9.0.0
         */
        bool shareApiAllowLinks();

        /**
         * Is password on public link requires
         *
         * @return bool
         * @since 9.0.0
         */
        bool shareApiLinkEnforcePassword();

        /**
         * Is default expire date enabled
         *
         * @return bool
         * @since 9.0.0
         */
        bool shareApiLinkDefaultExpireDate();

        /**
         * Is default expire date enforced
         *`
         * @return bool
         * @since 9.0.0
         */
        bool shareApiLinkDefaultExpireDateEnforced();

        /**
         * Number of default expire days
         *
         * @return int
         * @since 9.0.0
         */
        int shareApiLinkDefaultExpireDays();

        /**
         * Allow public upload on link shares
         *
         * @return bool
         * @since 9.0.0
         */
        bool shareApiLinkAllowPublicUpload();

        /**
         * check if user can only share with group members
         * @return bool
         * @since 9.0.0
         */
        bool shareWithGroupMembersOnly();

        /**
         * Check if users can share with groups
         * @return bool
         * @since 9.0.1
         */
        bool allowGroupSharing();

        /**
         * Check if sharing is disabled for the given user
         *
         * @param string userId
         * @return bool
         * @since 9.0.0
         */
        bool sharingDisabledForUser(string userId);

        /**
         * Check if outgoing server2server shares are allowed
         * @return bool
         * @since 9.0.0
         */
        bool outgoingServer2ServerSharesAllowed();

        /**
         * Check if outgoing server2server shares are allowed
         * @return bool
         * @since 14.0.0
         */
        bool outgoingServer2ServerGroupSharesAllowed();


        /**
         * Check if a given share provider exists
         * @param int shareType
         * @return bool
         * @since 11.0.0
         */
        bool shareProviderExists(int shareType);

    }

}
