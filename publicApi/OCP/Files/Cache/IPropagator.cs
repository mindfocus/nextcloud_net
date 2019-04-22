using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Cache
{
    /**
     * Propagate etags and mtimes within the storage
     *
     * @since 9.0.0
     */
    public interface IPropagator
    {
        /**
         * Mark the beginning of a propagation batch
         *
         * Note that not all cache setups support propagation in which case this will be a noop
         *
         * Batching for cache setups that do support it has to be explicit since the cache state is not fully consistent
         * before the batch is committed.
         *
         * @since 9.1.0
         */
        void beginBatch();

        /**
         * Commit the active propagation batch
         *
         * @since 9.1.0
         */
        void commitBatch();

        /**
         * @param string internalPath
         * @param int time
         * @since 9.0.0
         */
        void propagateChange(string internalPath, long time);
    }

}
