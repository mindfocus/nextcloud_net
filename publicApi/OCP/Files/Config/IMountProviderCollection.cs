using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Config
{

    /**
     * Manages the different mount providers
     * @since 8.0.0
     */
    public interface IMountProviderCollection
    {
        /**
         * Get all configured mount points for the user
         *
         * @param \OCP\IUser user
         * @return \OCP\Files\Mount\IMountPoint[]
         * @since 8.0.0
         */
        IList<Mount.IMountPoint> getMountsForUser(IUser user);

        /**
         * Get the configured home mount for this user
         *
         * @param \OCP\IUser user
         * @return \OCP\Files\Mount\IMountPoint
         * @since 9.1.0
         */
        Mount.IMountPoint getHomeMountForUser(IUser user);

        /**
         * Add a provider for mount points
         *
         * @param \OCP\Files\Config\IMountProvider provider
         * @since 8.0.0
         */
        void registerProvider(IMountProvider provider);

        /**
         * Add a filter for mounts
         *
         * @param callable filter (IMountPoint mountPoint, IUser user) => boolean
         * @since 14.0.0
         */
        void registerMountFilter(Action filter);

        /**
         * Add a provider for home mount points
         *
         * @param \OCP\Files\Config\IHomeMountProvider provider
         * @since 9.1.0
         */
        void registerHomeProvider(IHomeMountProvider provider);

        /**
         * Get the mount cache which can be used to search for mounts without setting up the filesystem
         *
         * @return IUserMountCache
         * @since 9.0.0
         */
        IUserMountCache getMountCache();
    }

}
