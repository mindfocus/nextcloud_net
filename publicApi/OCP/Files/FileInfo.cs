using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files
{
    /**
     * Interface FileInfo
     *
     * @package OCP\Files
     * @since 7.0.0
     */
    public interface FileInfo
    {
        /**
         * @since 7.0.0
         */
        //const TYPE_FILE = 'file';
        /**
         * @since 7.0.0
         */
        //const TYPE_FOLDER = 'dir';

        /**
         * @const \OCP\Files\FileInfo::SPACE_NOT_COMPUTED Return value for a not computed space value
         * @since 8.0.0
         */
        //const SPACE_NOT_COMPUTED = -1;
        /**
         * @const \OCP\Files\FileInfo::SPACE_UNKNOWN Return value for unknown space value
         * @since 8.0.0
         */
        //const SPACE_UNKNOWN = -2;
        /**
         * @const \OCP\Files\FileInfo::SPACE_UNLIMITED Return value for unlimited space
         * @since 8.0.0
         */
        //const SPACE_UNLIMITED = -3;

        /**
         * @since 9.1.0
         */
        //const MIMETYPE_FOLDER = 'httpd/unix-directory';

        /**
         * @const \OCP\Files\FileInfo::BLACKLIST_FILES_REGEX Return regular expression to test filenames against (blacklisting)
         * @since 12.0.0
         */
        //const BLACKLIST_FILES_REGEX = '\.(part|filepart)$';

        /**
         * Get the Etag of the file or folder
         *
         * @return string
         * @since 7.0.0
         */
        string getEtag();

        /**
         * Get the size in bytes for the file or folder
         *
         * @param bool $includeMounts whether or not to include the size of any sub mounts, since 16.0.0
         * @return int
         * @since 7.0.0
         */
        int getSize(bool includeMounts = true);

        /**
         * Get the last modified date as timestamp for the file or folder
         *
         * @return int
         * @since 7.0.0
         */
        int getMtime();

        /**
         * Get the name of the file or folder
         *
         * @return string
         * @since 7.0.0
         */
        string getName();

        /**
         * Get the path relative to the storage
         *
         * @return string
         * @since 7.0.0
         */
        string getInternalPath();

        /**
         * Get the absolute path
         *
         * @return string
         * @since 7.0.0
         */
        string getPath();

        /**
         * Get the full mimetype of the file or folder i.e. 'image/png'
         *
         * @return string
         * @since 7.0.0
         */
        string getMimetype();

        /**
         * Get the first part of the mimetype of the file or folder i.e. 'image'
         *
         * @return string
         * @since 7.0.0
         */
        string getMimePart();

        /**
         * Get the storage the file or folder is storage on
         *
         * @return \OCP\Files\Storage
         * @since 7.0.0
         */
        Storage.IStorage getStorage();

        /**
         * Get the file id of the file or folder
         *
         * @return int|null
         * @since 7.0.0
         */
        int getId();

        /**
         * Check whether the file is encrypted
         *
         * @return bool
         * @since 7.0.0
         */
        bool isEncrypted();

        /**
         * Get the permissions of the file or folder as bitmasked combination of the following constants
         * \OCP\Constants::PERMISSION_CREATE
         * \OCP\Constants::PERMISSION_READ
         * \OCP\Constants::PERMISSION_UPDATE
         * \OCP\Constants::PERMISSION_DELETE
         * \OCP\Constants::PERMISSION_SHARE
         * \OCP\Constants::PERMISSION_ALL
         *
         * @return int
         * @since 7.0.0 - namespace of constants has changed in 8.0.0
         */
        int getPermissions();

        /**
         * Check whether this is a file or a folder
         *
         * @return string \OCP\Files\FileInfo::TYPE_FILE|\OCP\Files\FileInfo::TYPE_FOLDER
         * @since 7.0.0
         */
        string getType();

        /**
         * Check if the file or folder is readable
         *
         * @return bool
         * @since 7.0.0
         */
        bool isReadable();

        /**
         * Check if a file is writable
         *
         * @return bool
         * @since 7.0.0
         */
        bool isUpdateable();

        /**
         * Check whether new files or folders can be created inside this folder
         *
         * @return bool
         * @since 8.0.0
         */
        bool isCreatable();

        /**
         * Check if a file or folder can be deleted
         *
         * @return bool
         * @since 7.0.0
         */
        bool isDeletable();

        /**
         * Check if a file or folder can be shared
         *
         * @return bool
         * @since 7.0.0
         */
        bool isShareable();

        /**
         * Check if a file or folder is shared
         *
         * @return bool
         * @since 7.0.0
         */
        bool isShared();

        /**
         * Check if a file or folder is mounted
         *
         * @return bool
         * @since 7.0.0
         */
        bool isMounted();

        /**
         * Get the mountpoint the file belongs to
         *
         * @return \OCP\Files\Mount\IMountPoint
         * @since 8.0.0
         */
        Mount.IMountPoint getMountPoint();

        /**
         * Get the owner of the file
         *
         * @return \OCP\IUser
         * @since 9.0.0
         */
        IUser getOwner();

        /**
         * Get the stored checksum for this file
         *
         * @return string
         * @since 9.0.0
         */
        string getChecksum();

        /**
         * Get the extension of the file
         *
         * @return string
         * @since 15.0.0
         */
        string getExtension();
}

}
