using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Mount
{
    /**
     * A storage mounted to folder on the filesystem
     * @since 8.0.0
     */
    public interface IMountPoint
    {

        /**
         * get complete path to the mount point
         *
         * @return string
         * @since 8.0.0
         */
        string getMountPoint();

        /**
         * Set the mountpoint
         *
         * @param string $mountPoint new mount point
         * @since 8.0.0
         */
        void setMountPoint(string mountPoint);

        /**
         * Get the storage that is mounted
         *
         * @return \OC\Files\Storage\Storage
         * @since 8.0.0
         */
        Storage.IStorage getStorage();

        /**
         * Get the id of the storages
         *
         * @return string
         * @since 8.0.0
         */
        string getStorageId();

        /**
         * Get the id of the storages
         *
         * @return int
         * @since 9.1.0
         */
        int getNumericStorageId();

        /**
         * Get the path relative to the mountpoint
         *
         * @param string $path absolute path to a file or folder
         * @return string
         * @since 8.0.0
         */
        string getInternalPath(string path);

        /**
         * Apply a storage wrapper to the mounted storage
         *
         * @param callable $wrapper
         * @since 8.0.0
         */
        void wrapStorage(Action wrapper);

        /**
         * Get a mount option
         *
         * @param string $name Name of the mount option to get
         * @param mixed $default Default value for the mount option
         * @return mixed
         * @since 8.0.0
         */
        object getOption(string name, object @default);

        /**
         * Get all options for the mount
         *
         * @return array
         * @since 8.1.0
         */
        IDictionary<string,object> getOptions();

        /**
         * Get the file id of the root of the storage
         *
         * @return int
         * @since 9.1.0
         */
        int getStorageRootId();

        /**
         * Get the id of the configured mount
         *
         * @return int|null mount id or null if not applicable
         * @since 9.1.0
         */
        int? getMountId();

        /**
         * Get the type of mount point, used to distinguish things like shares and external storages
         * in the web interface
         *
         * @return string
         * @since 12.0.0
         */
        string getMountType();
    }

}
