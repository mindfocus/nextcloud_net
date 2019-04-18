using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Config
{
    /**
     * Holds information about a mount for a user
     *
     * @since 9.0.0
     */
    public interface ICachedMountInfo
    {
        /**
         * @return IUser
         * @since 9.0.0
         */
        IUser getUser();

        /**
         * @return int the numeric storage id of the mount
         * @since 9.0.0
         */
        int  getStorageId();

        /**
         * @return int the fileid of the root of the mount
         * @since 9.0.0
         */
        int  getRootId();

        /**
         * @return Node the root node of the mount
         * @since 9.0.0
         */
        Node getMountPointNode();

        /**
         * @return string the mount point of the mount for the user
         * @since 9.0.0
         */
        string  getMountPoint();

        /**
         * Get the id of the configured mount
         *
         * @return int|null mount id or null if not applicable
         * @since 9.1.0
         */
        int? getMountId();

        /**
         * Get the internal path (within the storage) of the root of the mount
         *
         * @return string
         * @since 11.0.0
         */
        string getRootInternalPath();
    }

}
