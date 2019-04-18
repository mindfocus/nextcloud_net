using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Config
{
    /**
     * Provides
     * @since 8.0.0
     */
    public interface IMountProvider
    {
        /**
         * Get all mountpoints applicable for the user
         *
         * @param \OCP\IUser $user
         * @param \OCP\Files\Storage\IStorageFactory $loader
         * @return \OCP\Files\Mount\IMountPoint[]
         * @since 8.0.0
         */
        IList<Mount.IMountPoint> getMountsForUser(IUser user, Storage.IStorageFactory loader);
    }

}
