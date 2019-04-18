using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi
{
    /**
     * This interface defines method for accessing the file based user cache.
     *
     * @since 8.1.0
     */
    public interface IMemcache : ICache
    {
    /**
	 * Set a value in the cache if it's not already stored
	 *
	 * @param string $key
	 * @param mixed $value
	 * @param int $ttl Time To Live in seconds. Defaults to 60*60*24
	 * @return bool
	 * @since 8.1.0
	 */
    bool add(string key, string value, int ttl = 0);

    /**
	 * Increase a stored number
	 *
	 * @param string $key
	 * @param int $step
	 * @return int | bool
	 * @since 8.1.0
	 */
    int inc(string key, int step = 1);

    /**
	 * Decrease a stored number
	 *
	 * @param string $key
	 * @param int $step
	 * @return int | bool
	 * @since 8.1.0
	 */
    int dec(string key, int step = 1);

    /**
	 * Compare and set
	 *
	 * @param string $key
	 * @param mixed $old
	 * @param mixed $new
	 * @return bool
	 * @since 8.1.0
	 */
    bool cas(string key, object old, object @new);

	/**
	 * Compare and delete
	 *
	 * @param string $key
	 * @param mixed $old
	 * @return bool
	 * @since 8.1.0
	 */
	bool cad(string key, object old);
}

}
