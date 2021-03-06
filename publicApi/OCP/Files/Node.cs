﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files
{
    /**
     * Interface Node
     *
     * @package OCP\Files
     * @since 6.0.0 - extends FileInfo was added in 8.0.0
     */
    public interface Node : FileInfo
    {
    /**
	 * Move the file or folder to a new location
	 *
	 * @param string targetPath the absolute target path
	 * @throws \OCP\Files\NotPermittedException
	 * @return \OCP\Files\Node
	 * @since 6.0.0
	 */
    Node move(string targetPath);

    /**
	 * Delete the file or folder
	 * @return void
	 * @since 6.0.0
	 */
    void delete();

    /**
	 * Cope the file or folder to a new location
	 *
	 * @param string targetPath the absolute target path
	 * @return \OCP\Files\Node
	 * @since 6.0.0
	 */
    Node copy(string targetPath);

    /**
	 * Change the modified date of the file or folder
	 * If mtime is omitted the current time will be used
	 *
	 * @param int mtime (optional) modified date as unix timestamp
	 * @throws \OCP\Files\NotPermittedException
	 * @return void
	 * @since 6.0.0
	 */
    void touch(int? mtime = null);

    /**
	 * Get the storage backend the file or folder is stored on
	 *
	 * @return \OCP\Files\Storage
	 * @throws \OCP\Files\NotFoundException
	 * @since 6.0.0
	 */
    Storage.IStorage getStorage();

    /**
	 * Get the full path of the file or folder
	 *
	 * @return string
	 * @since 6.0.0
	 */
    string getPath();

    /**
	 * Get the path of the file or folder relative to the mountpoint of it's storage
	 *
	 * @return string
	 * @since 6.0.0
	 */
    string getInternalPath();

    /**
	 * Get the internal file id for the file or folder
	 *
	 * @return int
	 * @throws InvalidPathException
	 * @throws NotFoundException
	 * @since 6.0.0
	 */
    int getId();

    /**
	 * Get metadata of the file or folder
	 * The returned array contains the following values:
	 *  - mtime
	 *  - size
	 *
	 * @return array
	 * @since 6.0.0
	 */
    IList<string> stat();

    /**
	 * Get the modified date of the file or folder as unix timestamp
	 *
	 * @return int
	 * @throws InvalidPathException
	 * @throws NotFoundException
	 * @since 6.0.0
	 */
    int getMTime();

    /**
	 * Get the size of the file or folder in bytes
	 *
	 * @param bool includeMounts
	 * @return int
	 * @throws InvalidPathException
	 * @throws NotFoundException
	 * @since 6.0.0
	 */
    int getSize(bool includeMounts = true);

    /**
	 * Get the Etag of the file or folder
	 * The Etag is an string id used to detect changes to a file or folder,
	 * every time the file or folder is changed the Etag will change to
	 *
	 * @return string
	 * @throws InvalidPathException
	 * @throws NotFoundException
	 * @since 6.0.0
	 */
    string getEtag();


    /**
	 * Get the permissions of the file or folder as a combination of one or more of the following constants:
	 *  - \OCP\Constants::PERMISSION_READ
	 *  - \OCP\Constants::PERMISSION_UPDATE
	 *  - \OCP\Constants::PERMISSION_CREATE
	 *  - \OCP\Constants::PERMISSION_DELETE
	 *  - \OCP\Constants::PERMISSION_SHARE
	 *
	 * @return int
	 * @throws InvalidPathException
	 * @throws NotFoundException
	 * @since 6.0.0 - namespace of constants has changed in 8.0.0
	 */
    int getPermissions();

    /**
	 * Check if the file or folder is readable
	 *
	 * @return bool
	 * @throws InvalidPathException
	 * @throws NotFoundException
	 * @since 6.0.0
	 */
    bool isReadable();

    /**
	 * Check if the file or folder is writable
	 *
	 * @return bool
	 * @throws InvalidPathException
	 * @throws NotFoundException
	 * @since 6.0.0
	 */
    bool isUpdateable();

    /**
	 * Check if the file or folder is deletable
	 *
	 * @return bool
	 * @throws InvalidPathException
	 * @throws NotFoundException
	 * @since 6.0.0
	 */
    bool isDeletable();

    /**
	 * Check if the file or folder is shareable
	 *
	 * @return bool
	 * @throws InvalidPathException
	 * @throws NotFoundException
	 * @since 6.0.0
	 */
    bool isShareable();

    /**
	 * Get the parent folder of the file or folder
	 *
	 * @return Folder
	 * @since 6.0.0
	 */
    Folder getParent();

    /**
	 * Get the filename of the file or folder
	 *
	 * @return string
	 * @since 6.0.0
	 */
    string getName();

    /**
	 * Acquire a lock on this file or folder.
	 *
	 * A shared (read) lock will prevent any exclusive (write) locks from being created but any number of shared locks
	 * can be active at the same time.
	 * An exclusive lock will prevent any other lock from being created (both shared and exclusive).
	 *
	 * A locked exception will be thrown if any conflicting lock already exists
	 *
	 * Note that this uses mandatory locking, if you acquire an exclusive lock on a file it will block *all*
	 * other operations for that file, even within the same php process.
	 *
	 * Acquiring any lock on a file will also create a shared lock on all parent folders of that file.
	 *
	 * Note that in most cases you won't need to manually manage the locks for any files you're working with,
	 * any filesystem operation will automatically acquire the relevant locks for that operation.
	 *
	 * @param int type \OCP\Lock\ILockingProvider::LOCK_SHARED or \OCP\Lock\ILockingProvider::LOCK_EXCLUSIVE
	 * @throws \OCP\Lock\LockedException
	 * @since 9.1.0
	 */
    void lockC(int type);
    //void lock(int type);

	/**
	 * Check the type of an existing lock.
	 *
	 * A shared lock can be changed to an exclusive lock is there is exactly one shared lock on the file,
	 * an exclusive lock can always be changed to a shared lock since there can only be one exclusive lock int he first place.
	 *
	 * A locked exception will be thrown when these preconditions are not met.
	 * Note that this is also the case if no existing lock exists for the file.
	 *
	 * @param int targetType \OCP\Lock\ILockingProvider::LOCK_SHARED or \OCP\Lock\ILockingProvider::LOCK_EXCLUSIVE
	 * @throws \OCP\Lock\LockedException
	 * @since 9.1.0
	 */
	void changeLock(int targetType);

    /**
	 * Release an existing lock.
	 *
	 * This will also free up the shared locks on any parent folder that were automatically acquired when locking the file.
	 *
	 * Note that this method will not give any sort of error when trying to free a lock that doesn't exist.
	 *
	 * @param int type \OCP\Lock\ILockingProvider::LOCK_SHARED or \OCP\Lock\ILockingProvider::LOCK_EXCLUSIVE
	 * @throws \OCP\Lock\LockedException
	 * @since 9.1.0
	 */
    void unlock(int type);
}

}
