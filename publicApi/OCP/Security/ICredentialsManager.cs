using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Security
{
    /**
     * Store and retrieve credentials for external services
     *
     * @package OCP\Security
     * @since 8.2.0
     */
    interface ICredentialsManager
    {

        /**
         * Store a set of credentials
         *
         * @param string|null $userId Null for system-wide credentials
         * @param string $identifier
         * @param mixed $credentials
         * @since 8.2.0
         */
        void store(string? userId, string identifier, object credentials);

        /**
         * Retrieve a set of credentials
         *
         * @param string|null $userId Null for system-wide credentials
         * @param string $identifier
         * @return mixed
         * @since 8.2.0
         */
        object retrieve(string? userId, string identifier);

        /**
         * Delete a set of credentials
         *
         * @param string|null $userId Null for system-wide credentials
         * @param string $identifier
         * @return int rows removed
         * @since 8.2.0
         */
        int delete(string? userId, string identifier);

        /**
         * Erase all credentials stored for a user
         *
         * @param string $userId
         * @return int rows removed
         * @since 8.2.0
         */
        int erase(string userId);

    }
}
