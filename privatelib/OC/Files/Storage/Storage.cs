using System.Collections.Generic;

namespace OC.Files.Storage
{

/**
 * Provide a common interface to all different storage options
 *
 * All paths passed to the storage are relative to the storage and should NOT have a leading slash.
 */
interface Storage : OCP.Files.Storage.IStorage {

	/**
	 * get a cache instance for the storage
	 *
	 * @param string path
	 * @param .OC.Files.Storage.Storage (optional) the storage to pass to the cache
	 * @return .OC.Files.Cache.Cache
	 */
	Cache.Cache getCache(string path = "", Storage storage = null);

	/**
	 * get a scanner instance for the storage
	 *
	 * @param string path
	 * @param .OC.Files.Storage.Storage (optional) the storage to pass to the scanner
	 * @return .OC.Files.Cache.Scanner
	 */
	OC.Files.Cache.Scanner getScanner(string path = "", OC.Files.Storage.Storage storage = null);


	/**
	 * get the user id of the owner of a file or folder
	 *
	 * @param string path
	 * @return string
	 */
	string getOwner(string path);

	/**
	 * get a watcher instance for the cache
	 *
	 * @param string path
	 * @param .OC.Files.Storage.Storage (optional) the storage to pass to the watcher
	 * @return .OC.Files.Cache.Watcher
	 */
	OC.Files.Cache.Watcher getWatcher(string path = "", OC.Files.Storage.Storage storage = null);

	/**
	 * get a propagator instance for the cache
	 *
	 * @param .OC.Files.Storage.Storage (optional) the storage to pass to the watcher
	 * @return .OC.Files.Cache.Propagator
	 */
	OC.Files.Cache.Propagator getPropagator(Storage storage = null);

	/**
	 * get a updater instance for the cache
	 *
	 * @param .OC.Files.Storage.Storage (optional) the storage to pass to the watcher
	 * @return .OC.Files.Cache.Updater
	 */
	OC.Files.Cache.Updater getUpdater(Storage storage = null);

	/**
	 * @return .OC.Files.Cache.Storage
	 */
	OC.Files.Cache.Storage getStorageCache();

	/**
	 * @param string path
	 * @return array
	 */
	IDictionary<string,object> getMetaData(string path);

	/**
	 * @param string path The path of the file to acquire the lock for
	 * @param int type .OCP.Lock.ILockingProvider::LOCK_SHARED or .OCP.Lock.ILockingProvider::LOCK_EXCLUSIVE
	 * @param .OCP.Lock.ILockingProvider provider
	 * @throws .OCP.Lock.LockedException
	 */
	void acquireLock(string path,int type, OCP.Lock.ILockingProvider provider);

	/**
	 * @param string path The path of the file to release the lock for
	 * @param int type .OCP.Lock.ILockingProvider::LOCK_SHARED or .OCP.Lock.ILockingProvider::LOCK_EXCLUSIVE
	 * @param .OCP.Lock.ILockingProvider provider
	 * @throws .OCP.Lock.LockedException
	 */
	void releaseLock(string path,int type, OCP.Lock.ILockingProvider provider);

	/**
	 * @param string path The path of the file to change the lock for
	 * @param int type .OCP.Lock.ILockingProvider::LOCK_SHARED or .OCP.Lock.ILockingProvider::LOCK_EXCLUSIVE
	 * @param .OCP.Lock.ILockingProvider provider
	 * @throws .OCP.Lock.LockedException
	 */
	void changeLock(string path,int type, OCP.Lock.ILockingProvider provider);
}

}