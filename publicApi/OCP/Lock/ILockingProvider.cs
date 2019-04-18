using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Lock
{
    /**
     * Interface ILockingProvider
     *
     * @package OCP\Lock
     * @since 8.1.0
     */
    public interface ILockingProvider
    {
        /**
         * @since 8.1.0
         */
        //const LOCK_SHARED = 1;
        /**
         * @since 8.1.0
         */
        //const LOCK_EXCLUSIVE = 2;

        /**
         * @param string $path
         * @param int $type self::LOCK_SHARED or self::LOCK_EXCLUSIVE
         * @return bool
         * @since 8.1.0
         */
        bool isLocked(string path, int type);

	/**
	 * @param string $path
	 * @param int $type self::LOCK_SHARED or self::LOCK_EXCLUSIVE
	 * @throws \OCP\Lock\LockedException
	 * @since 8.1.0
	 */
	void acquireLock(string path, int type);

        /**
         * @param string $path
         * @param int $type self::LOCK_SHARED or self::LOCK_EXCLUSIVE
         * @since 8.1.0
         */
        void releaseLock(string path, int type);

        /**
         * Change the type of an existing lock
         *
         * @param string $path
         * @param int $targetType self::LOCK_SHARED or self::LOCK_EXCLUSIVE
         * @throws \OCP\Lock\LockedException
         * @since 8.1.0
         */
        void changeLock(string path, int targetType);

        /**
         * release all lock acquired by this instance
         * @since 8.1.0
         */
        void releaseAll();
    }

}
