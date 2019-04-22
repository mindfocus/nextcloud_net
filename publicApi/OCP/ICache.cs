using System;
using System.Collections;
using System.Collections.Generic;
namespace OCP
{
    /**
 * This interface defines method for accessing the file based user cache.
 * @since 6.0.0
 */
public interface ICache {

	/**
	 * Get a value from the user cache
	 * @param string key
	 * @return mixed
	 * @since 6.0.0
	 */
	object get(string key);

	/**
	 * Set a value in the user cache
	 * @param string key
	 * @param mixed value
	 * @param int ttl Time To Live in seconds. Defaults to 60*60*24
	 * @return bool
	 * @since 6.0.0
	 */
	bool set(string key, object value, int ttl = 0);

	/**
	 * Check if a value is set in the user cache
	 * @param string key
	 * @return bool
	 * @since 6.0.0
	 * @deprecated 9.1.0 Directly read from GET to prevent race conditions
	 */
	bool hasKey(string key);

	/**
	 * Remove an item from the user cache
	 * @param string key
	 * @return bool
	 * @since 6.0.0
	 */
	bool remove(string key);

	/**
	 * Clear the user cache of all entries starting with a prefix
	 * @param string prefix (optional)
	 * @return bool
	 * @since 6.0.0
	 */
	bool clear(string prefix = "");
}

}
