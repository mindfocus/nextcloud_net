using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Cache
{
    /**
     * Scan files from the storage and save to the cache
     *
     * @since 9.0.0
     */
    public interface IScanner
    {
        //const SCAN_RECURSIVE_INCOMPLETE = 2; // only recursive into not fully scanned folders
        //const SCAN_RECURSIVE = true;
        //const SCAN_SHALLOW = false;

        //const REUSE_NONE = 0;
        //const REUSE_ETAG = 1;
        //const REUSE_SIZE = 2;

        /**
         * scan a single file and store it in the cache
         *
         * @param string $file
         * @param int $reuseExisting
         * @param int $parentId
         * @param array | null $cacheData existing data in the cache for the file to be scanned
         * @param bool $lock set to false to disable getting an additional read lock during scanning
         * @return array an array of metadata of the scanned file
         * @throws \OC\ServerNotAvailableException
         * @throws \OCP\Lock\LockedException
         * @since 9.0.0
         */
        IList<string> scanFile(string file, int reuseExisting = 0, int parentId = -1, IList<string> cacheData = null, bool lockp = true);

        /**
         * scan a folder and all its children
         *
         * @param string $path
         * @param bool $recursive
         * @param int $reuse
         * @param bool $lock set to false to disable getting an additional read lock during scanning
         * @return array an array of the meta data of the scanned file or folder
         * @since 9.0.0
         */
        IList<string> scan(string path, bool  recursive , int reuse = -1, bool lockp = true);

        /**
         * check if the file should be ignored when scanning
         * NOTE: files with a '.part' extension are ignored as well!
         *       prevents unfinished put requests to be scanned
         *
         * @param string $file
         * @return boolean
         * @since 9.0.0
         */
        bool isPartialFile(string file);

        /**
         * walk over any folders that are not fully scanned yet and scan them
         *
         * @since 9.0.0
         */
        void backgroundScan();
    }


}
