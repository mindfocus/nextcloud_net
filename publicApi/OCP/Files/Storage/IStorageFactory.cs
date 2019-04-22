using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Storage
{
    /**
     * Creates storage instances and manages and applies storage wrappers
     * @since 8.0.0
     */
    public interface IStorageFactory
    {
        /**
         * allow modifier storage behaviour by adding wrappers around storages
         *
         * callback should be a function of type (string mountPoint, Storage storage) => Storage
         *
         * @param string wrapperName
         * @param callable callback
         * @return bool true if the wrapper was added, false if there was already a wrapper with this
         * name registered
         * @since 8.0.0
         */
        bool addStorageWrapper(string wrapperName, Action callback);

        /**
         * @param \OCP\Files\Mount\IMountPoint mountPoint
         * @param string class
         * @param array arguments
         * @return \OCP\Files\Storage
         * @since 8.0.0
         */
        IStorage getInstance(Mount.IMountPoint mountPoint, string classp, IList<string> arguments);
}

}
