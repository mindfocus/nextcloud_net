using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace OCP.Files.Cache
{
    /**
     * meta data for a file or folder
     *
     * @since 9.0.0
     */
    public interface ICacheEntry
    {
        //const DIRECTORY_MIMETYPE = 'httpd/unix-directory';
        string DIRECTORY_MIMETYPE { get; }

        /**
         * Get the numeric id of a file
         *
         * @return int
         * @since 9.0.0
         */
        int getId();

        /**
         * Get the numeric id for the storage
         *
         * @return int
         * @since 9.0.0
         */
        int getStorageId();

        /**
         * Get the path of the file relative to the storage root
         *
         * @return string
         * @since 9.0.0
         */
        string getPath();

        /**
         * Get the file name
         *
         * @return string
         * @since 9.0.0
         */
        string getName();

        /**
         * Get the full mimetype
         *
         * @return string
         * @since 9.0.0
         */
        string getMimeType();

        /**
         * Get the first part of the mimetype
         *
         * @return string
         * @since 9.0.0
         */
        string getMimePart();

        /**
         * Get the file size in bytes
         *
         * @return int
         * @since 9.0.0
         */
        int getSize();

        /**
         * Get the last modified date as unix timestamp
         *
         * @return int
         * @since 9.0.0
         */
        long getMTime();

        /**
         * Get the last modified date on the storage as unix timestamp
         *
         * Note that when a file is updated we also update the mtime of all parent folders to make it visible to the user which folder has had updates most recently
         * This can differ from the mtime on the underlying storage which usually only changes when a direct child is added, removed or renamed
         *
         * @return int
         * @since 9.0.0
         */
        long getStorageMTime();

        /**
         * Get the etag for the file
         *
         * An etag is used for change detection of files and folders, an etag of a file changes whenever the content of the file changes
         * Etag for folders change whenever a file in the folder has changed
         *
         * @return string
         * @since 9.0.0
         */
        string getEtag();

        /**
         * Get the permissions for the file stored as bitwise combination of \OCP\PERMISSION_READ, \OCP\PERMISSION_CREATE
         * \OCP\PERMISSION_UPDATE, \OCP\PERMISSION_DELETE and \OCP\PERMISSION_SHARE
         *
         * @return int
         * @since 9.0.0
         */
        int getPermissions();

        /**
         * Check if the file is encrypted
         *
         * @return bool
         * @since 9.0.0
         */
        bool isEncrypted();
    }
}