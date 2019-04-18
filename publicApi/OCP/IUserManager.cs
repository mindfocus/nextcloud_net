using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Class Manager
     *
     * Hooks available in scope \OC\User:
     * - preSetPassword(\OC\User\User $user, string $password, string $recoverPassword)
     * - postSetPassword(\OC\User\User $user, string $password, string $recoverPassword)
     * - preDelete(\OC\User\User $user)
     * - postDelete(\OC\User\User $user)
     * - preCreateUser(string $uid, string $password)
     * - postCreateUser(\OC\User\User $user, string $password)
     * - assignedUserId(string $uid)
     * - preUnassignedUserId(string $uid)
     * - postUnassignedUserId(string $uid)
     *
     * @package OC\User
     * @since 8.0.0
     */
    public interface IUserManager
    {
        /**
	 * register a user backend
	 *
	 * @param \OCP\UserInterface $backend
	 * @since 8.0.0
	 */
        void registerBackend(UserInterface backend);

        /**
         * Get the active backends
         * @return \OCP\UserInterface[]
         * @since 8.0.0
         */
        IList<UserInterface> getBackends();

        /**
         * remove a user backend
         *
         * @param \OCP\UserInterface $backend
         * @since 8.0.0
         */
        void removeBackend(UserInterface backend);

        /**
         * remove all user backends
         * @since 8.0.0
         */
        void clearBackends();

        /**
         * get a user by user id
         *
         * @param string $uid
         * @return \OCP\IUser|null Either the user or null if the specified user does not exist
         * @since 8.0.0
         */
        IUser? get(string uid);

        /**
         * check if a user exists
         *
         * @param string $uid
         * @return bool
         * @since 8.0.0
         */
        bool userExists(string uid);

        /**
         * Check if the password is valid for the user
         *
         * @param string $loginName
         * @param string $password
         * @return mixed the User object on success, false otherwise
         * @since 8.0.0
         */
        object checkPassword(string loginName, string password);

        /**
         * search by user id
         *
         * @param string $pattern
         * @param int $limit
         * @param int $offset
         * @return \OCP\IUser[]
         * @since 8.0.0
         */
        IList<IUser> search(string pattern, int? limit = null, int? offset = null);

        /**
         * search by displayName
         *
         * @param string $pattern
         * @param int $limit
         * @param int $offset
         * @return \OCP\IUser[]
         * @since 8.0.0
         */
        IList<IUser> searchDisplayName(string pattern, int? limit = null, int? offset = null);

        /**
         * @param string $uid
         * @param string $password
         * @throws \InvalidArgumentException
         * @return bool|\OCP\IUser the created user or false
         * @since 8.0.0
         */
        IUser? createUser(string uid, string password);

        /**
         * @param string $uid
         * @param string $password
         * @param UserInterface $backend
         * @return IUser|null
         * @throws \InvalidArgumentException
         * @since 12.0.0
         */
        IUser? createUserFromBackend(string uid, string password, UserInterface backend);

        /**
         * returns how many users per backend exist (if supported by backend)
         *
         * @return array an array of backend class as key and count number as value
         * @since 8.0.0
         */
        IDictionary<string,int> countUsers();

        /**
         * @param \Closure $callback
         * @param string $search
         * @since 9.0.0
         */
        void callForAllUsers(Action callback, string search = "");

        /**
         * returns how many users have logged in once
         *
         * @return int
         * @since 11.0.0
         */
        int countDisabledUsers();

        /**
         * returns how many users have logged in once
         *
         * @return int
         * @since 11.0.0
         */
        int countSeenUsers();

        /**
         * @param \Closure $callback
         * @since 11.0.0
         */
        void callForSeenUsers(Action callback);

        /**
         * @param string $email
         * @return IUser[]
         * @since 9.1.0
         */
        IList<IUser> getByEmail(string email);
    }

}
