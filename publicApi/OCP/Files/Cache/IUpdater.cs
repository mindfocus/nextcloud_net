using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Cache
{
    /**
     * Update the cache and propagate changes
     *
     * @since 9.0.0
     */
    public interface IUpdater
    {
        /**
         * Get the propagator for etags and mtime for the view the updater works on
         *
         * @return IPropagator
         * @since 9.0.0
         */
        IPropagator getPropagator();

        /**
         * Propagate etag and mtime changes for the parent folders of path up to the root of the filesystem
         *
         * @param string path the path of the file to propagate the changes for
         * @param int|null time the timestamp to set as mtime for the parent folders, if left out the current time is used
         * @since 9.0.0
         */
        void propagate(string path, long? time = null);

        /**
         * Update the cache for path and update the size, etag and mtime of the parent folders
         *
         * @param string path
         * @param int time
         * @since 9.0.0
         */
        void update(string path, long?  time = null);

        /**
         * Remove path from the cache and update the size, etag and mtime of the parent folders
         *
         * @param string path
         * @since 9.0.0
         */
        void remove(string path);

        /**
         * Rename a file or folder in the cache and update the size, etag and mtime of the parent folders
         *
         * @param IStorage sourceStorage
         * @param string source
         * @param string target
         * @since 9.0.0
         */
        void renameFromStorage(Storage.IStorage sourceStorage, string source, string target);
    }

}
