using System.Collections;
using System.Collections.Generic;
using OCP.Files.Cache;

namespace OC.Files.Cache
{
/**
 * Fallback implementation for moveFromCache
 */
    public abstract class MoveFromCacheTrait
    {

        /// <summary>
        /// store meta data for a file or folder
        /// </summary>
        /// <param name="file"></param>
        /// <param name="data"></param>
        /// <exception cref="RuntimeExcetion"></exception>
        /// <returns>file id</returns>
        abstract public int put(string file, IDictionary<string, object> data);

        /// <summary>
        /// Move a file or folder in the cache
        /// </summary>
        /// <param name="sourceCache"></param>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        public void moveFromCache(ICache sourceCache, string sourcePath, string targetPath)
        {
            var sourceEntry = sourceCache.get(sourcePath);

            this.copyFromCache(sourceCache, sourceEntry, targetPath);

            sourceCache.remove(sourcePath);
        }


        /// <summary>
        /// Copy a file or folder in the cache
        /// </summary>
        /// <param name="sourceCache"></param>
        /// <param name="sourceEntry"></param>
        /// <param name="targetPath"></param>
        public void copyFromCache(ICache sourceCache, ICacheEntry sourceEntry, string targetPath)
        {
            this.put(targetPath, this.cacheEntryToArray(sourceEntry));
            if (sourceEntry.getMimeType() == sourceEntry.DIRECTORY_MIMETYPE)
            {
                var folderContent = sourceCache.getFolderContentsById(sourceEntry.getId());
                foreach (var subEntry in folderContent)
                {
                    var subTargetPath = targetPath + "/" + subEntry.getName();
                    this.copyFromCache(sourceCache, subEntry, subTargetPath);
                }
            }
        }

        private IDictionary<string, object> cacheEntryToArray(ICacheEntry entry)
        {
            return new Dictionary<string, object>
            {
                {"size", entry.getSize()},
                {"mtime", entry.getMTime()},
                {"storage_mtime", entry.getStorageMTime()},
                {"mimetype", entry.getMimeType()},
                {"mimepart", entry.getMimePart()},
                {"etag", entry.getEtag()},
                {"permissions", entry.getPermissions()},
                {"encrypted", entry.isEncrypted()}
            };
        }
    }
}