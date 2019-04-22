using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Config
{

    /**
     * Cache mounts points per user in the cache so we can easily look them up
     *
     * @since 9.0.0
     */
    public interface IUserMountCache
    {
        /**
         * Register mounts for a user to the cache
         *
         * @param IUser user
         * @param IMountPoint[] mounts
         * @since 9.0.0
         */
        void registerMounts(IUser user, IList<Mount.IMountPoint> mounts);

        /**
         * Get all cached mounts for a user
         *
         * @param IUser user
         * @return ICachedMountInfo[]
         * @since 9.0.0
         */
        IList<ICachedMountFileInfo> getMountsForUser(IUser user);

        /**
         * Get all cached mounts by storage
         *
         * @param int numericStorageId
         * @param string|null user limit the results to a single user @since 12.0.0
         * @return ICachedMountInfo[]
         * @since 9.0.0
         */
        IList<ICachedMountInfo> getMountsForStorageId(int numericStorageId, string? user = null);

        /**
         * Get all cached mounts by root
         *
         * @param int rootFileId
         * @return ICachedMountInfo[]
         * @since 9.0.0
         */
        IList<ICachedMountInfo> getMountsForRootId(int rootFileId);

        /**
         * Get all cached mounts that contain a file
         *
         * @param int fileId
         * @param string|null user optionally restrict the results to a single user @since 12.0.0
         * @return ICachedMountFileInfo[]
         * @since 9.0.0
         */
        IList<ICachedMountFileInfo> getMountsForFileId(int fileId, string? user = null);

        /**
         * Remove all cached mounts for a user
         *
         * @param IUser user
         * @since 9.0.0
         */
        void removeUserMounts(IUser user);

        /**
         * Remove all mounts for a user and storage
         *
         * @param storageId
         * @param string userId
         * @return mixed
         * @since 9.0.0
         */
        object removeUserStorageMount(string storageId, string userId);

        /**
         * Remove all cached mounts for a storage
         *
         * @param storageId
         * @return mixed
         * @since 9.0.0
         */
        object remoteStorageMounts(string storageId);

        /**
         * Get the used space for users
         *
         * Note that this only includes the space in their home directory,
         * not any incoming shares or external storages.
         *
         * @param IUser[] users
         * @return int[] [userId => userSpace]
         * @since 13.0.0
         */
        IList<int> getUsedSpaceForUsers(IList<IUser> users);
    }

}
