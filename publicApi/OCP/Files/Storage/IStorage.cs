using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Storage
{
    /**
     * Provide a common interface to all different storage options
     *
     * All paths passed to the storage are relative to the storage and should NOT have a leading slash.
     *
     * @since 9.0.0
     */
    public interface IStorage
    {
        /**
         * $parameters is a free form array with the configuration options needed to construct the storage
         *
         * @param array $parameters
         * @since 9.0.0
         */
        //public function __construct($parameters);

        /**
         * Get the identifier for the storage,
         * the returned id should be the same for every storage object that is created with the same parameters
         * and two storage objects with the same id should refer to two storages that display the same files.
         *
         * @return string
         * @since 9.0.0
         */
        string getId();

        /**
         * see http://php.net/manual/en/function.mkdir.php
         * implementations need to implement a recursive mkdir
         *
         * @param string $path
         * @return bool
         * @since 9.0.0
         */
        bool mkdir(string path);

        /**
         * see http://php.net/manual/en/function.rmdir.php
         *
         * @param string $path
         * @return bool
         * @since 9.0.0
         */
        bool rmdir(string path);

        /**
         * see http://php.net/manual/en/function.opendir.php
         *
         * @param string $path
         * @return resource|false
         * @since 9.0.0
         */
        int opendir(string path);

        /**
         * see http://php.net/manual/en/function.is-dir.php
         *
         * @param string $path
         * @return bool
         * @since 9.0.0
         */
        bool is_dir(string path);

        /**
         * see http://php.net/manual/en/function.is-file.php
         *
         * @param string $path
         * @return bool
         * @since 9.0.0
         */
        bool is_file(string path);

        /**
         * see http://php.net/manual/en/function.stat.php
         * only the following keys are required in the result: size and mtime
         *
         * @param string $path
         * @return array|false
         * @since 9.0.0
         */
        IList<string> stat(string path);

        /**
         * see http://php.net/manual/en/function.filetype.php
         *
         * @param string $path
         * @return string|false
         * @since 9.0.0
         */
        string? filetype(string path);

        /**
         * see http://php.net/manual/en/function.filesize.php
         * The result for filesize when called on a folder is required to be 0
         *
         * @param string $path
         * @return int|false
         * @since 9.0.0
         */
        int filesize(string path);

        /**
         * check if a file can be created in $path
         *
         * @param string $path
         * @return bool
         * @since 9.0.0
         */
        bool isCreatable(string path);

        /**
         * check if a file can be read
         *
         * @param string $path
         * @return bool
         * @since 9.0.0
         */
        bool isReadable(string path);

        /**
         * check if a file can be written to
         *
         * @param string $path
         * @return bool
         * @since 9.0.0
         */
        bool isUpdatable(string path);

        /**
         * check if a file can be deleted
         *
         * @param string $path
         * @return bool
         * @since 9.0.0
         */
        bool isDeletable(string path);

        /**
         * check if a file can be shared
         *
         * @param string $path
         * @return bool
         * @since 9.0.0
         */
        bool isSharable(string path);

        /**
         * get the full permissions of a path.
         * Should return a combination of the PERMISSION_ constants defined in lib/public/constants.php
         *
         * @param string $path
         * @return int
         * @since 9.0.0
         */
        int getPermissions(string path);

        /**
         * see http://php.net/manual/en/function.file_exists.php
         *
         * @param string $path
         * @return bool
         * @since 9.0.0
         */
        bool file_exists(string path);

        /**
         * see http://php.net/manual/en/function.filemtime.php
         *
         * @param string $path
         * @return int|false
         * @since 9.0.0
         */
        int filemtime(string path);

        /**
         * see http://php.net/manual/en/function.file_get_contents.php
         *
         * @param string $path
         * @return string|false
         * @since 9.0.0
         */
        string? file_get_contents(string path);

        /**
         * see http://php.net/manual/en/function.file_put_contents.php
         *
         * @param string $path
         * @param string $data
         * @return bool
         * @since 9.0.0
         */
        bool file_put_contents(string path, string data);

        /**
         * see http://php.net/manual/en/function.unlink.php
         *
         * @param string $path
         * @return bool
         * @since 9.0.0
         */
        bool unlink(string path);

        /**
         * see http://php.net/manual/en/function.rename.php
         *
         * @param string $path1
         * @param string $path2
         * @return bool
         * @since 9.0.0
         */
        bool rename(string path1, string path2);

        /**
         * see http://php.net/manual/en/function.copy.php
         *
         * @param string $path1
         * @param string $path2
         * @return bool
         * @since 9.0.0
         */
        bool copy(string path1, string path2);

        /**
         * see http://php.net/manual/en/function.fopen.php
         *
         * @param string $path
         * @param string $mode
         * @return resource|false
         * @since 9.0.0
         */
        int fopen(string path, string mode);

        /**
         * get the mimetype for a file or folder
         * The mimetype for a folder is required to be "httpd/unix-directory"
         *
         * @param string $path
         * @return string|false
         * @since 9.0.0
         */
        string? getMimeType(string path);

        /**
         * see http://php.net/manual/en/function.hash-file.php
         *
         * @param string $type
         * @param string $path
         * @param bool $raw
         * @return string|false
         * @since 9.0.0
         */
        string? hash(string type, string path, bool raw = false);

        /**
         * see http://php.net/manual/en/function.free_space.php
         *
         * @param string $path
         * @return int|false
         * @since 9.0.0
         */
        int free_space(string path);

        /**
         * see http://php.net/manual/en/function.touch.php
         * If the backend does not support the operation, false should be returned
         *
         * @param string $path
         * @param int $mtime
         * @return bool
         * @since 9.0.0
         */
        bool touch(string path, int? mtime = null);

        /**
         * get the path to a local version of the file.
         * The local version of the file can be temporary and doesn't have to be persistent across requests
         *
         * @param string $path
         * @return string|false
         * @since 9.0.0
         */
        string? getLocalFile(string path);

        /**
         * check if a file or folder has been updated since $time
         *
         * @param string $path
         * @param int $time
         * @return bool
         * @since 9.0.0
         *
         * hasUpdated for folders should return at least true if a file inside the folder is add, removed or renamed.
         * returning true for other changes in the folder is optional
         */
        bool hasUpdated(string path, int time);

        /**
         * get the ETag for a file or folder
         *
         * @param string $path
         * @return string|false
         * @since 9.0.0
         */
        string? getETag(string path);

        /**
         * Returns whether the storage is local, which means that files
         * are stored on the local filesystem instead of remotely.
         * Calling getLocalFile() for local storages should always
         * return the local files, whereas for non-local storages
         * it might return a temporary file.
         *
         * @return bool true if the files are stored locally, false otherwise
         * @since 9.0.0
         */
        bool isLocal();

        /**
         * Check if the storage is an instance of $class or is a wrapper for a storage that is an instance of $class
         *
         * @param string $class
         * @return bool
         * @since 9.0.0
         */
        bool instanceOfStorage(string @class);

	/**
	 * A custom storage implementation can return an url for direct download of a give file.
	 *
	 * For now the returned array can hold the parameter url - in future more attributes might follow.
	 *
	 * @param string $path
	 * @return array|false
	 * @since 9.0.0
	 */
	IList<string> getDirectDownload(string path);

        /**
         * @param string $path the path of the target folder
         * @param string $fileName the name of the file itself
         * @return void
         * @throws InvalidPathException
         * @since 9.0.0
         */
        void verifyPath(string path, string fileName);

        /**
         * @param IStorage $sourceStorage
         * @param string $sourceInternalPath
         * @param string $targetInternalPath
         * @return bool
         * @since 9.0.0
         */
        bool copyFromStorage(IStorage sourceStorage, string sourceInternalPath, string targetInternalPath);

        /**
         * @param IStorage $sourceStorage
         * @param string $sourceInternalPath
         * @param string $targetInternalPath
         * @return bool
         * @since 9.0.0
         */
        bool moveFromStorage(IStorage sourceStorage, string sourceInternalPath, string targetInternalPath);

        /**
         * Test a storage for availability
         *
         * @since 9.0.0
         * @return bool
         */
        bool test();

        /**
         * @since 9.0.0
         * @return array [ available, last_checked ]
         */
        bool getAvailability();

        /**
         * @since 9.0.0
         * @param bool $isAvailable
         */
        void setAvailability(bool isAvailable);

        /**
         * @param string $path path for which to retrieve the owner
         * @since 9.0.0
         */
        string getOwner(string path);

        /**
         * @return ICache
         * @since 9.0.0
         */
        ICache getCache();

        /**
         * @return IPropagator
         * @since 9.0.0
         */
        Cache.IPropagator getPropagator();

        /**
         * @return IScanner
         * @since 9.0.0
         */
        Cache.IScanner getScanner();

        /**
         * @return IUpdater
         * @since 9.0.0
         */
        Cache.IUpdater getUpdater();

        /**
         * @return IWatcher
         * @since 9.0.0
         */
        Cache.IWatcher getWatcher();
    }

}
