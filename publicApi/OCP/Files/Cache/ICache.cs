﻿using OCP.Files.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Cache
{
    /**
     * Metadata cache for a storage
     *
     * The cache stores the metadata for all files and folders in a storage and is kept up to date trough the following mechanisms:
     *
     * - Scanner: scans the storage and updates the cache where needed
     * - Watcher: checks for changes made to the filesystem outside of the ownCloud instance and rescans files and folder when a change is detected
     * - Updater: listens to changes made to the filesystem inside of the ownCloud instance and updates the cache where needed
     * - ChangePropagator: updates the mtime and etags of parent folders whenever a change to the cache is made to the cache by the updater
     *
     * @since 9.0.0
     */
    public interface ICache
    {
        //const NOT_FOUND = 0;
        //const PARTIAL = 1; //only partial data available, file not cached in the database
        //const SHALLOW = 2; //folder in cache, but not all child files are completely scanned
        //const COMPLETE = 3;

        /**
         * Get the numeric storage id for this cache's storage
         *
         * @return int
         * @since 9.0.0
         */
        int getNumericStorageId();

        /**
         * get the stored metadata of a file or folder
         *
         * @param string | int file either the path of a file or folder or the file id for a file or folder
         * @return ICacheEntry|false the cache entry or false if the file is not found in the cache
         * @since 9.0.0
         */
        ICacheEntry get(string file);

        /**
         * get the metadata of all files stored in folder
         *
         * Only returns files one level deep, no recursion
         *
         * @param string folder
         * @return ICacheEntry[]
         * @since 9.0.0
         */
        IList<ICacheEntry> getFolderContents(string folder);

        /**
         * get the metadata of all files stored in folder
         *
         * Only returns files one level deep, no recursion
         *
         * @param int fileId the file id of the folder
         * @return ICacheEntry[]
         * @since 9.0.0
         */
        IList<ICacheEntry> getFolderContentsById(int fileId);

        /**
         * store meta data for a file or folder
         * This will automatically call either insert or update depending on if the file exists
         *
         * @param string file
         * @param array data
         *
         * @return int file id
         * @throws \RuntimeException
         * @since 9.0.0
         */
        int put(string file, IDictionary<string,object> data);

        /**
         * insert meta data for a new file or folder
         *
         * @param string file
         * @param array data
         *
         * @return int file id
         * @throws \RuntimeException
         * @since 9.0.0
         */
        int insert(string file, IDictionary<string,object> data);

        /**
         * update the metadata of an existing file or folder in the cache
         *
         * @param int id the fileid of the existing file or folder
         * @param array data [key => value] the metadata to update, only the fields provided in the array will be updated, non-provided values will remain unchanged
         * @since 9.0.0
         */
        void update(int id, IDictionary<string,object> data);

        /**
         * get the file id for a file
         *
         * A file id is a numeric id for a file or folder that's unique within an owncloud instance which stays the same for the lifetime of a file
         *
         * File ids are easiest way for apps to store references to a file since unlike paths they are not affected by renames or sharing
         *
         * @param string file
         * @return int
         * @since 9.0.0
         */
        int getId(string file);

        /**
         * get the id of the parent folder of a file
         *
         * @param string file
         * @return int
         * @since 9.0.0
         */
        int getParentId(string file);

        /**
         * check if a file is available in the cache
         *
         * @param string file
         * @return bool
         * @since 9.0.0
         */
        bool inCache(string file);

        /**
         * remove a file or folder from the cache
         *
         * when removing a folder from the cache all files and folders inside the folder will be removed as well
         *
         * @param string file
         * @since 9.0.0
         */
        void remove(string file);

        /**
         * Move a file or folder in the cache
         *
         * @param string source
         * @param string target
         * @since 9.0.0
         */
        void move(string source, string target);

        /**
         * Move a file or folder in the cache
         *
         * Note that this should make sure the entries are removed from the source cache
         *
         * @param \OCP\Files\Cache\ICache sourceCache
         * @param string sourcePath
         * @param string targetPath
         * @throws \OC\DatabaseException
         * @since 9.0.0
         */
        void moveFromCache(ICache sourceCache, string sourcePath, string targetPath);

        /**
         * Get the scan status of a file
         *
         * - ICache::NOT_FOUND: File is not in the cache
         * - ICache::PARTIAL: File is not stored in the cache but some incomplete data is known
         * - ICache::SHALLOW: The folder and it's direct children are in the cache but not all sub folders are fully scanned
         * - ICache::COMPLETE: The file or folder, with all it's children) are fully scanned
         *
         * @param string file
         *
         * @return int ICache::NOT_FOUND, ICache::PARTIAL, ICache::SHALLOW or ICache::COMPLETE
         * @since 9.0.0
         */
        int getStatus(string file);

        /**
         * search for files matching pattern, files are matched if their filename matches the search pattern
         *
         * @param string pattern the search pattern using SQL search syntax (e.g. '%searchstring%')
         * @return ICacheEntry[] an array of cache entries where the name matches the search pattern
         * @since 9.0.0
         * @deprecated 9.0.0 due to lack of pagination, not all backends might implement this
         */
        IList<ICacheEntry> search(string pattern);

        /**
         * search for files by mimetype
         *
         * @param string mimetype either a full mimetype to search ('text/plain') or only the first part of a mimetype ('image')
         *        where it will search for all mimetypes in the group ('image/*')
         * @return ICacheEntry[] an array of cache entries where the mimetype matches the search
         * @since 9.0.0
         * @deprecated 9.0.0 due to lack of pagination, not all backends might implement this
         */
        IList<ICacheEntry> searchByMime(string mimetype);

        /**
         * Search for files with a flexible query
         *
         * @param ISearchQuery query
         * @return ICacheEntry[]
         * @throw \InvalidArgumentException if the cache is unable to perform the query
         * @since 12.0.0
         */
        IList<ICacheEntry> searchQuery(Search.ISearchQuery query);

        /**
         * Search for files by tag of a given users.
         *
         * Note that every user can tag files differently.
         *
         * @param string|int tag name or tag id
         * @param string userId owner of the tags
         * @return ICacheEntry[] file data
         * @since 9.0.0
         * @deprecated 9.0.0 due to lack of pagination, not all backends might implement this
         */
        IList<ICacheEntry> searchByTag(string tag, string userId);

        /**
         * find a folder in the cache which has not been fully scanned
         *
         * If multiple incomplete folders are in the cache, the one with the highest id will be returned,
         * use the one with the highest id gives the best result with the background scanner, since that is most
         * likely the folder where we stopped scanning previously
         *
         * @return string|bool the path of the folder or false when no folder matched
         * @since 9.0.0
         */
        string? getIncomplete();

        /**
         * get the path of a file on this storage by it's file id
         *
         * @param int id the file id of the file or folder to search
         * @return string|null the path of the file (relative to the storage) or null if a file with the given id does not exists within this cache
         * @since 9.0.0
         */
        string? getPathById(int id);

        /**
         * normalize the given path for usage in the cache
         *
         * @param string path
         * @return string
         * @since 9.0.0
         */
        string normalize(string path);
    }

}
