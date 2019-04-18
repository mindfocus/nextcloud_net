using System;
using System.Collections.Generic;
using System.Text;
using OCP.Files.Storage;

namespace OCP.Files.Cache
{
    class CacheInsertEvent : OC.Files.Cache.AbstractCacheEvent
    {
        public CacheInsertEvent(Storage.IStorage storage, string path, int fileId) : base(storage, path, fileId)
        {
        }
    }
}
