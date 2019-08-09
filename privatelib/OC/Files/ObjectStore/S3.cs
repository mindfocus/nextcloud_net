using System.Collections.Generic;
using OCP.Files.ObjectStore;

namespace OC.Files.ObjectStore
{
    class S3 : S3ObjectTrait, IObjectStore
    {
        public S3(IDictionary<string, object> parameters)
        {
            this.parseParams(parameters);
        }

        /**
         * @return string the container or bucket name where objects are stored
         * @since 7.0.0
         */
        public string getStorageId()
        {
            return this.id;
        }
    }
}