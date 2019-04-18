using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Storage
{
    /**
     * Storage backends that require explicit locking
     *
     * Storage backends implementing this interface do not need to implement their own locking implementation but should use the provided lockingprovider instead
     * The implementation of the locking methods only need to map internal storage paths to "lock keys"
     *
     * @since 9.0.0
     */
    public interface ILockingStorage
    {
        /**
         * @param string $path The path of the file to acquire the lock for
         * @param int $type \OCP\Lock\ILockingProvider::LOCK_SHARED or \OCP\Lock\ILockingProvider::LOCK_EXCLUSIVE
         * @param \OCP\Lock\ILockingProvider $provider
         * @throws \OCP\Lock\LockedException
         * @since 9.0.0
         */
        void acquireLock(string path, string type, Lock.ILockingProvider provider);

        /**
         * @param string $path The path of the file to acquire the lock for
         * @param int $type \OCP\Lock\ILockingProvider::LOCK_SHARED or \OCP\Lock\ILockingProvider::LOCK_EXCLUSIVE
         * @param \OCP\Lock\ILockingProvider $provider
         * @throws \OCP\Lock\LockedException
         * @since 9.0.0
         */
        void releaseLock(string path, string type, Lock.ILockingProvider provider);

        /**
         * @param string $path The path of the file to change the lock for
         * @param int $type \OCP\Lock\ILockingProvider::LOCK_SHARED or \OCP\Lock\ILockingProvider::LOCK_EXCLUSIVE
         * @param \OCP\Lock\ILockingProvider $provider
         * @throws \OCP\Lock\LockedException
         * @since 9.0.0
         */
        void changeLock(string path, int type, Lock.ILockingProvider provider);
    }

}
