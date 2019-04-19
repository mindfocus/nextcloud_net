using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Interface ICacheFactory
     *
     * @package OCP
     * @since 7.0.0
     */
    public interface ICacheFactory
    {
        /**
         * Get a distributed memory cache instance
         *
         * All entries added trough the cache instance will be namespaced by $prefix to prevent collisions between apps
         *
         * @param string $prefix
         * @return ICache
         * @since 7.0.0
         * @deprecated 13.0.0 Use either createLocking, createDistributed or createLocal
         */
        ICache create(string prefix = "");

	/**
	 * Check if any memory cache backend is available
	 *
	 * @return bool
	 * @since 7.0.0
	 */
	bool isAvailable();

	/**
	 * Check if a local memory cache backend is available
	 *
	 * @return bool
	 * @since 14.0.0
	 */
	bool isLocalCacheAvailable();

	/**
	 * create a cache instance for storing locks
	 *
	 * @param string $prefix
	 * @return IMemcache
	 * @since 13.0.0
	 */
	IMemcache createLocking(string prefix= "");

	/**
	 * create a distributed cache instance
	 *
	 * @param string $prefix
	 * @return ICache
	 * @since 13.0.0
	 */
	ICache createDistributed(string prefix = "");

	/**
	 * create a local cache instance
	 *
	 * @param string $prefix
	 * @return ICache
	 * @since 13.0.0
	 */
	ICache createLocal(string prefix = "");
}

}
