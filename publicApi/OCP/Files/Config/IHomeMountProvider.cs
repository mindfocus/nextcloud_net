using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Config
{
    /**
     * Provides
     *
     * @since 9.1.0
     */
    public interface IHomeMountProvider
    {
        /**
         * Get all mountpoints applicable for the user
         *
         * @param \OCP\IUser $user
         * @param \OCP\Files\Storage\IStorageFactory $loader
         * @return \OCP\Files\Mount\IMountPoint|null
         * @since 9.1.0
         */
        Mount.IMountPoint getHomeMountForUser(IUser user, Storage.IStorageFactory loader);
    }

}
