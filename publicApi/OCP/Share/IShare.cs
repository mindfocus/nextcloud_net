using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.ShareNS
{
    /**
     * Interface IShare
     *
     * @package OCP\Share
     * @since 9.0.0
     */
    public interface IShare
    {

        /**
         * Set the internal id of the share
         * It is only allowed to set the internal id of a share once.
         * Attempts to override the internal id will result in an IllegalIDChangeException
         *
         * @param string id
         * @return \OCP\Share\IShare
         * @throws IllegalIDChangeException
         * @throws \InvalidArgumentException
         * @since 9.1.0
         */
        IShare setId(string id);

        /**
         * Get the internal id of the share.
         *
         * @return string
         * @since 9.0.0
         */
        string getId();

        /**
         * Get the full share id. This is the <providerid>:<internalid>.
         * The full id is unique in the system.
         *
         * @return string
         * @since 9.0.0
         * @throws \UnexpectedValueException If the fullId could not be constructed
         */
        string getFullId();

        /**
         * Set the provider id of the share
         * It is only allowed to set the provider id of a share once.
         * Attempts to override the provider id will result in an IllegalIDChangeException
         *
         * @param string id
         * @return \OCP\Share\IShare
         * @throws IllegalIDChangeException
         * @throws \InvalidArgumentException
         * @since 9.1.0
         */
        IShare setProviderId(string id);

        /**
         * Set the node of the file/folder that is shared
         *
         * @param Node node
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setNode(Files.Node node);

        /**
         * Get the node of the file/folder that is shared
         *
         * @return File|Folder
         * @since 9.0.0
         * @throws NotFoundException
         */
        Files.Node getNode();

        /**
         * Set file id for lazy evaluation of the node
         * @param int fileId
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setNodeId(int fileId);

        /**
         * Get the fileid of the node of this share
         * @return int
         * @since 9.0.0
         * @throws NotFoundException
         */
        int getNodeId();

        /**
         * Set the type of node (file/folder)
         *
         * @param string type
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setNodeType(string type);

        /**
         * Get the type of node (file/folder)
         *
         * @return string
         * @since 9.0.0
         * @throws NotFoundException
         */
        string getNodeType();

        /**
         * Set the shareType
         *
         * @param int shareType
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setShareType(int shareType);

        /**
         * Get the shareType
         *
         * @return int
         * @since 9.0.0
         */
        int getShareType();

        /**
         * Set the receiver of this share.
         *
         * @param string sharedWith
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setSharedWith(string sharedWith);

        /**
         * Get the receiver of this share.
         *
         * @return string
         * @since 9.0.0
         */
        string getSharedWith();

        /**
         * Set the display name of the receiver of this share.
         *
         * @param string displayName
         * @return \OCP\Share\IShare The modified object
         * @since 14.0.0
         */
        IShare setSharedWithDisplayName(string displayName);

        /**
         * Get the display name of the receiver of this share.
         *
         * @return string
         * @since 14.0.0
         */
        string getSharedWithDisplayName();

        /**
         * Set the avatar of the receiver of this share.
         *
         * @param string src
         * @return \OCP\Share\IShare The modified object
         * @since 14.0.0
         */
        IShare setSharedWithAvatar(string src);

        /**
         * Get the avatar of the receiver of this share.
         *
         * @return string
         * @since 14.0.0
         */
        string getSharedWithAvatar();

        /**
         * Set the permissions.
         * See \OCP\Constants::PERMISSION_*
         *
         * @param int permissions
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setPermissions(int permissions);

        /**
         * Get the share permissions
         * See \OCP\Constants::PERMISSION_*
         *
         * @return int
         * @since 9.0.0
         */
        int getPermissions();

        /**
         * Attach a note to a share
         *
         * @param string note
         * @return \OCP\Share\IShare The modified object
         * @since 14.0.0
         */
        IShare setNote(string note);

        /**
         * Get note attached to a share
         *
         * @return string
         * @since 14.0.0
         */
        string getNote();


        /**
         * Set the expiration date
         *
         * @param null|\DateTime expireDate
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setExpirationDate(DateTime expireDate);

        /**
         * Get the expiration date
         *
         * @return \DateTime
         * @since 9.0.0
         */
        DateTime getExpirationDate();

        /**
         * set a label for a share, some shares, e.g. public links can have a label
         *
         * @param string label
         * @return \OCP\Share\IShare The modified object
         * @since 15.0.0
         */
        IShare setLabel(string label);

        /**
         * get label for the share, some shares, e.g. public links can have a label
         *
         * @return string
         * @since 15.0.0
         */
        string getLabel();

        /**
         * Set the sharer of the path.
         *
         * @param string sharedBy
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setSharedBy(string sharedBy);

        /**
         * Get share sharer
         *
         * @return string
         * @since 9.0.0
         */
        string getSharedBy();

        /**
         * Set the original share owner (who owns the path that is shared)
         *
         * @param string shareOwner
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setShareOwner(string shareOwner);

        /**
         * Get the original share owner (who owns the path that is shared)
         *
         * @return string
         * @since 9.0.0
         */
        string getShareOwner();

        /**
         * Set the password for this share.
         * When the share is passed to the share manager to be created
         * or updated the password will be hashed.
         *
         * @param string password
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setPassword(string password);

        /**
         * Get the password of this share.
         * If this share is obtained via a shareprovider the password is
         * hashed.
         *
         * @return string
         * @since 9.0.0
         */
        string getPassword();


        /**
         * Set if the recipient can start a conversation with the owner to get the
         * password using Nextcloud Talk.
         *
         * @param bool sendPasswordByTalk
         * @return \OCP\Share\IShare The modified object
         * @since 14.0.0
         */
        IShare setSendPasswordByTalk(bool sendPasswordByTalk);

        /**
         * Get if the recipient can start a conversation with the owner to get the
         * password using Nextcloud Talk.
         * The returned value does not take into account other factors, like Talk
         * being enabled for the owner of the share or not; it just cover whether
         * the option is enabled for the share itself or not.
         *
         * @return bool
         * @since 14.0.0
         */
        bool getSendPasswordByTalk();

	/**
	 * Set the public link token.
	 *
	 * @param string token
	 * @return \OCP\Share\IShare The modified object
	 * @since 9.0.0
	 */
	IShare setToken(string token);

        /**
         * Get the public link token.
         *
         * @return string
         * @since 9.0.0
         */
        string getToken();

        /**
         * Set the target path of this share relative to the recipients user folder.
         *
         * @param string target
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setTarget(string target);

        /**
         * Get the target path of this share relative to the recipients user folder.
         *
         * @return string
         * @since 9.0.0
         */
        string getTarget();

        /**
         * Set the time this share was created
         *
         * @param \DateTime shareTime
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setShareTime(DateTime shareTime);

        /**
         * Get the timestamp this share was created
         *
         * @return \DateTime
         * @since 9.0.0
         */
        DateTime getShareTime();

        /**
         * Set if the recipient is informed by mail about the share.
         *
         * @param bool mailSend
         * @return \OCP\Share\IShare The modified object
         * @since 9.0.0
         */
        IShare setMailSend(bool mailSend);

        /**
         * Get if the recipient informed by mail about the share.
         *
         * @return bool
         * @since 9.0.0
         */
        bool getMailSend();

        /**
         * Set the cache entry for the shared node
         *
         * @param ICacheEntry entry
         * @since 11.0.0
         */
        void setNodeCacheEntry(Files.Cache.ICacheEntry entry);

        /**
         * Get the cache entry for the shared node
         *
         * @return null|ICacheEntry
         * @since 11.0.0
         */
        Files.Cache.ICacheEntry getNodeCacheEntry();

        /**
         * Sets a shares hide download state
         * This is mainly for public shares. It will signal that the share page should
         * hide download buttons etc.
         *
         * @param bool ro
         * @return IShare
         * @since 15.0.0
         */
        IShare setHideDownload(bool hide);

        /**
         * Gets a shares hide download state
         * This is mainly for public shares. It will signal that the share page should
         * hide download buttons etc.
         *
         * @return bool
         * @since 15.0.0
         */
        bool getHideDownload();
}

}
