using System;
using System.Collections.Generic;
using System.Text;

namespace OC.Files.Cache
{
    public class AbstractCacheEvent : OCP.Files.Cache.ICacheEvent, ext.Event
    {
        protected OCP.Files.Storage.IStorage storage;
	protected string path;
	protected int fileId;

        /**
         * @param IStorage $storage
         * @param string $path
         * @param int $fileId
         * @since 16.0.0
         */
        public AbstractCacheEvent(OCP.Files.Storage.IStorage storage, string path, int fileId)
        {
            this.path = path;
            this.storage = storage;
            this.fileId = fileId;
        }

        /**
         * @return IStorage
         * @since 16.0.0
         */
        public OCP.Files.Storage.IStorage getStorage() {
            return this.storage;
	}

    /**
	 * @return string
	 * @since 16.0.0
	 */
    public string getPath() {
            return this.path;

    }

    /**
	 * @return int
	 * @since 16.0.0
	 */
    public int getFileId(){
            return this.fileId;

    }
}
}
