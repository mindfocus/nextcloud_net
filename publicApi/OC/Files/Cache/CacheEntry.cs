using System.Collections;
using System.Collections.Generic;
using OCP.Files.Cache;

namespace OC.Files.Cache
{
/**
 * meta data for a file or folder
 */
    class CacheEntry : ICacheEntry
    {
//    class CacheEntry : ICacheEntry, ArrayAccess {
        /**
         * @var array
         */
        private IDictionary<string, object> data;

        public CacheEntry(IDictionary<string, object> data)
        {
            this.data = data;
        }

        public void offsetSet(string offset, object value)
        {
            this.data[offset] = value;
        }

        public bool offsetExists(string offset)
        {
            return this.data.ContainsKey(offset);
        }

        public void offsetUnset(string offset)
        {
            this.data.Remove(offset);
        }

        public object offsetGet(string offset)
        {
            if (this.data.ContainsKey(offset))
            {
                return this.data[offset];
            }

            return null;
        }

        public string DIRECTORY_MIMETYPE => "httpd/unix-directory";

        public int getId()
        {
            return (int) this.data["fileid"];
        }

        public int getStorageId()
        {
            return (int) this.data["storage"];
        }


        public string getPath()
        {
            return (string) this.data["path"];
        }


        public string getName()
        {
            return (string) this.data["name"];
        }


        public string getMimeType()
        {
            return (string) this.data["mimetype"];
        }


        public string getMimePart()
        {
            return (string) this.data["mimepart"];
        }

        public int getSize()
        {
            return (int) this.data["size"];
        }

        public long getMTime()
        {
            return (long) this.data["mtime"];
        }

        public long getStorageMTime()
        {
            return (long) this.data["storage_mtime"];
        }

        public string getEtag()
        {
            return (string) this.data["etag"];
        }

        public int getPermissions()
        {
            return (int) this.data["permissions"];
        }

        public bool isEncrypted()
        {
            return this.data.ContainsKey("encrypted") && (bool) this.data["encrypted"];
        }

        public IDictionary<string, object> getData()
        {
            return this.data;
        }
    }
}