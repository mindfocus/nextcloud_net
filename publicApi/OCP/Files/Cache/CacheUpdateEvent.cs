using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Cache
{
    class CacheUpdateEvent : OC.Files.Cache.AbstractCacheEvent
    {
        public CacheUpdateEvent(Storage.IStorage storage, string path, int fileId) : base(storage, path, fileId)
        {
        }
    }
}
