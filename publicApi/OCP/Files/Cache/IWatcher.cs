using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Cache
{
    /**
     * check the storage backends for updates and change the cache accordingly
     *
     * @since 9.0.0
     */
    public interface IWatcher
    {
        //const CHECK_NEVER = 0; // never check the underlying filesystem for updates
        //const CHECK_ONCE = 1; // check the underlying filesystem for updates once every request for each file
        //const CHECK_ALWAYS = 2; // always check the underlying filesystem for updates

        /**
         * @param int $policy either IWatcher::CHECK_NEVER, IWatcher::CHECK_ONCE, IWatcher::CHECK_ALWAYS
         * @since 9.0.0
         */
        void setPolicy(int policy);

        /**
         * @return int either IWatcher::CHECK_NEVER, IWatcher::CHECK_ONCE, IWatcher::CHECK_ALWAYS
         * @since 9.0.0
         */
        int getPolicy();

        /**
         * check $path for updates and update if needed
         *
         * @param string $path
         * @param ICacheEntry|null $cachedEntry
         * @return boolean true if path was updated
         * @since 9.0.0
         */
        bool checkUpdate(string path, ICacheEntry? cachedEntry = null);

        /**
         * Update the cache for changes to $path
         *
         * @param string $path
         * @param ICacheEntry $cachedData
         * @since 9.0.0
         */
        void update(string path, ICacheEntry cachedData);

        /**
         * Check if the cache for $path needs to be updated
         *
         * @param string $path
         * @param ICacheEntry $cachedData
         * @return bool
         * @since 9.0.0
         */
        bool needsUpdate(string path, ICacheEntry cachedData);

        /**
         * remove deleted files in $path from the cache
         *
         * @param string $path
         * @since 9.0.0
         */
        void cleanFolder(string path);
    }

}
