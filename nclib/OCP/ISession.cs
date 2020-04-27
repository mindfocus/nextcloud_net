using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Interface ISession
     *
     * wrap PHP's internal session handling into the ISession interface
     * @since 6.0.0
     */
    public interface ISession
    {

        /**
         * Set a value in the session
         *
         * @param string key
         * @param mixed value
         * @since 6.0.0
         */
        void set(string key, object value);

        /**
         * Get a value from the session
         *
         * @param string key
         * @return mixed should return null if key does not exist
         * @since 6.0.0
         */
        object get(string key);

        /**
         * Check if a named key exists in the session
         *
         * @param string key
         * @return bool
         * @since 6.0.0
         */
        bool exists(string key);

	/**
	 * Remove a key/value pair from the session
	 *
	 * @param string key
	 * @since 6.0.0
	 */
	void remove(string key);

        /**
         * Reset and recreate the session
         * @since 6.0.0
         */
        void clear();

        /**
         * Close the session and release the lock
         * @since 7.0.0
         */
        void close();

        /**
         * Wrapper around session_regenerate_id
         *
         * @param bool deleteOldSession Whether to delete the old associated session file or not.
         * @param bool updateToken Wheater to update the associated auth token
         * @return void
         * @since 9.0.0, updateToken added in 14.0.0
         */
        void regenerateId(bool deleteOldSession = true, bool updateToken = false);

        /**
         * Wrapper around session_id
         *
         * @return string
         * @throws SessionNotAvailableException
         * @since 9.1.0
         */
        string getId();
}

}
