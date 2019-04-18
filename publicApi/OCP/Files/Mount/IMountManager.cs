using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Mount
{
    /**
     * Interface IMountManager
     *
     * Manages all mounted storages in the system
     * @since 8.2.0
     */
    interface IMountManager
    {

        /**
         * Add a new mount
         *
         * @param \OCP\Files\Mount\IMountPoint $mount
         * @since 8.2.0
         */
        void addMount(IMountPoint mount);

        /**
         * Remove a mount
         *
         * @param string $mountPoint
         * @since 8.2.0
         */
        void removeMount(string mountPoint);

        /**
         * Change the location of a mount
         *
         * @param string $mountPoint
         * @param string $target
         * @since 8.2.0
         */
        void moveMount(string mountPoint, string target);

        /**
         * Find the mount for $path
         *
         * @param string $path
         * @return \OCP\Files\Mount\IMountPoint|null
         * @since 8.2.0
         */
        IMountPoint? find(string path);

        /**
         * Find all mounts in $path
         *
         * @param string $path
         * @return \OCP\Files\Mount\IMountPoint[]
         * @since 8.2.0
         */
        IList<IMountPoint> findIn(string path);

	/**
	 * Remove all registered mounts
	 *
	 * @since 8.2.0
	 */
	void clear();

        /**
         * Find mounts by storage id
         *
         * @param string $id
         * @return \OCP\Files\Mount\IMountPoint[]
         * @since 8.2.0
         */
        IList<IMountPoint> findByStorageId(string id);

	/**
	 * @return \OCP\Files\Mount\IMountPoint[]
	 * @since 8.2.0
	 */
	IList<IMountPoint> getAll();

	/**
	 * Find mounts by numeric storage id
	 *
	 * @param int $id
	 * @return \OCP\Files\Mount\IMountPoint[]
	 * @since 8.2.0
	 */
	IList<IMountPoint> findByNumericId(int id);
}

}
