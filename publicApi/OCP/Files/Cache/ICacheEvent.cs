using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Cache
{
    /**
     * @since 16.0.0
     */
    public interface ICacheEvent
    {
        /**
         * @return IStorage
         * @since 16.0.0
         */
        Storage.IStorage getStorage();

        /**
         * @return string
         * @since 16.0.0
         */
        string getPath();

        /**
         * @return int
         * @since 16.0.0
         */
        int getFileId();
}

}
