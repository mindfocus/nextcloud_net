using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Interface for memcache backends that support setting ttl after the value is set
     *
     * @since 8.2.2
     */
    public interface IMemcacheTTL : IMemcache
    {
    /**
	 * Set the ttl for an existing value
	 *
	 * @param string $key
	 * @param int $ttl time to live in seconds
	 * @since 8.2.2
	 */
    void setTTL(string key, int ttl);
}

}
