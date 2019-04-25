using OCP;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace OC.User
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
     * - change(\OC\User\User $user)
     * - assignedUserId(string $uid)
     * - preUnassignedUserId(string $uid)
     * - postUnassignedUserId(string $uid)
     *
     * @package OC\User
     */
    class Manager : Hooks.PublicEmitter , IUserManager
    {
    /**
	 * @var \OCP\UserInterface[] $backends
	 */
    private IList<UserInterface> backends = new List<UserInterface>();

        /**
         * @var \OC\User\User[] $cachedUsers
         */
        private IDictionary<string,User> cachedUsers = new Dictionary<string,User>();

    /** @var IConfig */
    private IConfig config;
	/** @var EventDispatcherInterface */
	private ext.Event dispatcher;

	public Manager(IConfig config, ext.Event dispatcher)
    {
		this.config = config;
		this.dispatcher = dispatcher;
		// $this->listen('\OC\User', 'postDelete', function($user) use(&$cachedUsers) {
        //     /** @var \OC\User\User $user */
        //     unset($cachedUsers[$user->getUID()]);
        // });
    }

    /**
	 * Get the active backends
	 * @return \OCP\UserInterface[]
	 */
    public IList<OCP.UserInterface> getBackends()
    {
        return this.backends;
    }

    /**
	 * register a user backend
	 *
	 * @param \OCP\UserInterface $backend
	 */
    public void registerBackend(OCP.UserInterface backend)
    {
		this.backends.Add(backend);
    }

    /**
	 * remove a user backend
	 *
	 * @param \OCP\UserInterface $backend
	 */
    public void removeBackend(OCP.UserInterface backend)
    {
		this.cachedUsers.Clear();
		this.backends.Remove(backend);
    }

    /**
	 * remove all user backends
	 */
    public void clearBackends()
    {
		this.cachedUsers.Clear();
		this.backends.Clear();
    }

    /**
	 * get a user by user id
	 *
	 * @param string $uid
	 * @return \OC\User\User|null Either the user or null if the specified user does not exist
	 */
    public IUser get(string uid)
    {
		if(string.IsNullOrEmpty(uid)) {
			return null;
		}
		if(this.cachedUsers.ContainsKey(uid)) {
			return this.cachedUsers[uid];
		}
		foreach(var backend in this.backends) {
			if(backend.userExists(uid)){
				return getUserObject(uid, backend);
			}
		}
		return null;
    }

    /**
	 * get or construct the user object
	 *
	 * @param string $uid
	 * @param \OCP\UserInterface $backend
	 * @param bool $cacheUser If false the newly created user object will not be cached
	 * @return \OC\User\User
	 */
    protected User getUserObject(string uid, UserInterface backend, bool cacheUser = true)
    {
		if(this.cachedUsers.ContainsKey(uid)){
			return this.cachedUsers[uid];
		}
		var user = new User(uid,backend, this.dispatcher, this, this.config);
		this.cachedUsers[uid] = user;
        return user;
    }

    /**
	 * check if a user exists
	 *
	 * @param string $uid
	 * @return bool
	 */
    public bool userExists(string uid)
    {
		var user = this.get(uid);
		return user != null;
    }

    /**
	 * Check if the password is valid for the user
	 *
	 * @param string $loginName
	 * @param string $password
	 * @return mixed the User object on success, false otherwise
	 */
    public User checkPassword(string loginName, string password)
    {
		var result = this.checkPasswordNoLogging(loginName, password);
        if (result == false) {
			\OC::$server->getLogger()->warning('Login failed: \''. $loginName.'\' (Remote IP: \''. \OC::$server->getRequest()->getRemoteAddress(). '\')', ['app' => 'core']);
        }

        return result;
    }

    /**
	 * Check if the password is valid for the user
	 *
	 * @internal
	 * @param string $loginName
	 * @param string $password
	 * @return mixed the User object on success, false otherwise
	 */
    public User checkPasswordNoLogging(string loginName, string password)
    {
        loginName = loginName.Trim();
            //.Replace('\0','');
		password = password.Trim();
		    //.Replace('\0','');
		foreach(var backend in this.backends) {
			if(backend.implementsActions(Backend.CHECK_PASSWORD)) {
				var uid = ((OCP.User.Backend.ICheckPasswordBackend)backend).checkPassword(loginName, password);
				if(uid != null) {
					return this.getUserObject(uid, backend);
				}
			}
		}
        return null;
    }

    /**
	 * search by user id
	 *
	 * @param string $pattern
	 * @param int $limit
	 * @param int $offset
	 * @return \OC\User\User[]
	 */
    public IDictionary<string,User> search(string pattern, int limit = -1, int offset = -1)
    {
		var users = new Dictionary<string,User>();
		foreach (var backend in this.backends)
		{
			var backendUserIds = backend.getUsers(pattern,limit,offset);
			if(backendUserIds != null) {
				foreach (var backenduserId in backendUserIds)
				{
				users[backenduserId] = this.getUserObject(backenduserId, backend);					
				}
			}
		}
        // uasort($users, function($a, $b) {
        //     /**
		// 	 * @var \OC\User\User $a
		// 	 * @var \OC\User\User $b
		// 	 */
        //     return strcasecmp($a->getUID(), $b->getUID());
        // });
        return users;
    }

    /**
	 * search by displayName
	 *
	 * @param string $pattern
	 * @param int $limit
	 * @param int $offset
	 * @return \OC\User\User[]
	 */
    public IDictionary<string,User> searchDisplayName(string pattern, int limit = -1, int offset = -1)
    {
		var users = new Dictionary<string,User>();
		foreach (var backend in this.backends)
		{
			var backendUsers = backend.getDisplayNames(pattern, limit, offset);
			if(backendUsers != null) {
				foreach (var backendUser in backendUsers)
				{
					users[backendUser.Key] = this.getUserObject(backendUser.Key, backend);
				}
			}
		}
        // usort($users, function($a, $b) {
        //     /**
		// 	 * @var \OC\User\User $a
		// 	 * @var \OC\User\User $b
		// 	 */
        //     return strcasecmp($a->getDisplayName(), $b->getDisplayName());
        // });
        return users;
    }

    /**
	 * @param string $uid
	 * @param string $password
	 * @throws \InvalidArgumentException
	 * @return bool|IUser the created user or false
	 */
    public IUser createUser(string uid, string password)
    {
		var localBackends = new List<OCP.UserInterface>();
		foreach (var backend in this.backends)
		{
			if(backend is Database) {
				localBackends.Add(backend);
				continue;
			}
			if(backend.implementsActions(Backend.CREATE_USER)) {
				return this.createUserFromBackend(uid, password, backend);
			}
		}
		foreach (var backend in localBackends)
		{
			if(backend.implementsActions(Backend.CREATE_USER)) {
				return this.createUserFromBackend(uid, password, backend);
			}
		}
		return null;
	}

	/**
	 * @param string $uid
	 * @param string $password
	 * @param UserInterface $backend
	 * @return IUser|null
	 * @throws \InvalidArgumentException
	 */
	public function createUserFromBackend($uid, $password, UserInterface $backend) {
		$l = \OC::$server->getL10N('lib');

		// Check the name for bad characters
		// Allowed are: "a-z", "A-Z", "0-9" and "_.@-'"
		if (preg_match('/[^a-zA-Z0-9 _\.@\-\']/', $uid)) {
			throw new \InvalidArgumentException($l->t('Only the following characters are allowed in a username:'
				. ' "a-z", "A-Z", "0-9", and "_.@-\'"'));
		}
		// No empty username
		if (trim($uid) === '') {
			throw new \InvalidArgumentException($l->t('A valid username must be provided'));
		}
		// No whitespace at the beginning or at the end
		if (trim($uid) !== $uid) {
			throw new \InvalidArgumentException($l->t('Username contains whitespace at the beginning or at the end'));
		}
		// Username only consists of 1 or 2 dots (directory traversal)
		if ($uid === '.' || $uid === '..') {
			throw new \InvalidArgumentException($l->t('Username must not consist of dots only'));
		}
		// No empty password
		if (trim($password) === '') {
			throw new \InvalidArgumentException($l->t('A valid password must be provided'));
		}

		// Check if user already exists
		if ($this->userExists($uid)) {
			throw new \InvalidArgumentException($l->t('The username is already being used'));
		}

		$this->emit('\OC\User', 'preCreateUser', [$uid, $password]);
		$state = $backend->createUser($uid, $password);
		if($state === false) {
			throw new \InvalidArgumentException($l->t('Could not create user'));
		}
		$user = $this->getUserObject($uid, $backend);
		if ($user instanceof IUser) {
			$this->emit('\OC\User', 'postCreateUser', [$user, $password]);
		}
		return $user;
	}

	/**
	 * returns how many users per backend exist (if supported by backend)
	 *
	 * @param boolean $hasLoggedIn when true only users that have a lastLogin
	 *                entry in the preferences table will be affected
	 * @return array|int an array of backend class as key and count number as value
	 *                if $hasLoggedIn is true only an int is returned
	 */
	public function countUsers($hasLoggedIn = false) {
		if ($hasLoggedIn) {
			return $this->countSeenUsers();
		}
		$userCountStatistics = [];
		foreach ($this->backends as $backend) {
			if ($backend->implementsActions(Backend::COUNT_USERS)) {
				$backendUsers = $backend->countUsers();
				if($backendUsers !== false) {
					if($backend instanceof IUserBackend) {
						$name = $backend->getBackendName();
					} else {
						$name = get_class($backend);
					}
					if(isset($userCountStatistics[$name])) {
						$userCountStatistics[$name] += $backendUsers;
					} else {
						$userCountStatistics[$name] = $backendUsers;
					}
				}
			}
		}
		return $userCountStatistics;
	}

	/**
	 * returns how many users per backend exist in the requested groups (if supported by backend)
	 *
	 * @param IGroup[] $groups an array of gid to search in
	 * @return array|int an array of backend class as key and count number as value
	 *                if $hasLoggedIn is true only an int is returned
	 */
	public function countUsersOfGroups(array $groups) {
		$users = [];
		foreach($groups as $group) {
			$usersIds = array_map(function($user) {
				return $user->getUID();
			}, $group->getUsers());
			$users = array_merge($users, $usersIds);
		}
		return count(array_unique($users));
	}

	/**
	 * The callback is executed for each user on each backend.
	 * If the callback returns false no further users will be retrieved.
	 *
	 * @param \Closure $callback
	 * @param string $search
	 * @param boolean $onlySeen when true only users that have a lastLogin entry
	 *                in the preferences table will be affected
	 * @since 9.0.0
	 */
	public function callForAllUsers(\Closure $callback, $search = '', $onlySeen = false) {
		if ($onlySeen) {
			$this->callForSeenUsers($callback);
		} else {
			foreach ($this->getBackends() as $backend) {
				$limit = 500;
				$offset = 0;
				do {
					$users = $backend->getUsers($search, $limit, $offset);
					foreach ($users as $uid) {
						if (!$backend->userExists($uid)) {
							continue;
						}
						$user = $this->getUserObject($uid, $backend, false);
						$return = $callback($user);
						if ($return === false) {
							break;
						}
					}
					$offset += $limit;
				} while (count($users) >= $limit);
			}
		}
	}

	/**
	 * returns how many users are disabled
	 *
	 * @return int
	 * @since 12.0.0
	 */
	public function countDisabledUsers(): int {
		$queryBuilder = \OC::$server->getDatabaseConnection()->getQueryBuilder();
		$queryBuilder->select($queryBuilder->func()->count('*'))
			->from('preferences')
			->where($queryBuilder->expr()->eq('appid', $queryBuilder->createNamedParameter('core')))
			->andWhere($queryBuilder->expr()->eq('configkey', $queryBuilder->createNamedParameter('enabled')))
			->andWhere($queryBuilder->expr()->eq('configvalue', $queryBuilder->createNamedParameter('false'), IQueryBuilder::PARAM_STR));

		
		$result = $queryBuilder->execute();
		$count = $result->fetchColumn();
		$result->closeCursor();
		
		if ($count !== false) {
			$count = (int)$count;
		} else {
			$count = 0;
		}

		return $count;
	}

	/**
	 * returns how many users are disabled in the requested groups
	 *
	 * @param array $groups groupids to search
	 * @return int
	 * @since 14.0.0
	 */
	public function countDisabledUsersOfGroups(array $groups): int {
		$queryBuilder = \OC::$server->getDatabaseConnection()->getQueryBuilder();
		$queryBuilder->select($queryBuilder->createFunction('COUNT(DISTINCT ' . $queryBuilder->getColumnName('uid') . ')'))
			->from('preferences', 'p')
			->innerJoin('p', 'group_user', 'g', $queryBuilder->expr()->eq('p.userid', 'g.uid'))
			->where($queryBuilder->expr()->eq('appid', $queryBuilder->createNamedParameter('core')))
			->andWhere($queryBuilder->expr()->eq('configkey', $queryBuilder->createNamedParameter('enabled')))
			->andWhere($queryBuilder->expr()->eq('configvalue', $queryBuilder->createNamedParameter('false'), IQueryBuilder::PARAM_STR))
			->andWhere($queryBuilder->expr()->in('gid', $queryBuilder->createNamedParameter($groups, IQueryBuilder::PARAM_STR_ARRAY)));

		$result = $queryBuilder->execute();
		$count = $result->fetchColumn();
		$result->closeCursor();
		
		if ($count !== false) {
			$count = (int)$count;
		} else {
			$count = 0;
		}

		return $count;
	}

	/**
	 * returns how many users have logged in once
	 *
	 * @return int
	 * @since 11.0.0
	 */
	public function countSeenUsers() {
		$queryBuilder = \OC::$server->getDatabaseConnection()->getQueryBuilder();
		$queryBuilder->select($queryBuilder->func()->count('*'))
			->from('preferences')
			->where($queryBuilder->expr()->eq('appid', $queryBuilder->createNamedParameter('login')))
			->andWhere($queryBuilder->expr()->eq('configkey', $queryBuilder->createNamedParameter('lastLogin')))
			->andWhere($queryBuilder->expr()->isNotNull('configvalue'));

		$query = $queryBuilder->execute();

		$result = (int)$query->fetchColumn();
		$query->closeCursor();

		return $result;
	}

	/**
	 * @param \Closure $callback
	 * @since 11.0.0
	 */
	public function callForSeenUsers(\Closure $callback) {
		$limit = 1000;
		$offset = 0;
		do {
			$userIds = $this->getSeenUserIds($limit, $offset);
			$offset += $limit;
			foreach ($userIds as $userId) {
				foreach ($this->backends as $backend) {
					if ($backend->userExists($userId)) {
						$user = $this->getUserObject($userId, $backend, false);
						$return = $callback($user);
						if ($return === false) {
							return;
						}
						break;
					}
				}
			}
		} while (count($userIds) >= $limit);
	}

	/**
	 * Getting all userIds that have a listLogin value requires checking the
	 * value in php because on oracle you cannot use a clob in a where clause,
	 * preventing us from doing a not null or length(value) > 0 check.
	 *
	 * @param int $limit
	 * @param int $offset
	 * @return string[] with user ids
	 */
	private function getSeenUserIds($limit = null, $offset = null) {
		$queryBuilder = \OC::$server->getDatabaseConnection()->getQueryBuilder();
		$queryBuilder->select(['userid'])
			->from('preferences')
			->where($queryBuilder->expr()->eq(
				'appid', $queryBuilder->createNamedParameter('login'))
			)
			->andWhere($queryBuilder->expr()->eq(
				'configkey', $queryBuilder->createNamedParameter('lastLogin'))
			)
			->andWhere($queryBuilder->expr()->isNotNull('configvalue')
			);

		if ($limit !== null) {
			$queryBuilder->setMaxResults($limit);
		}
		if ($offset !== null) {
			$queryBuilder->setFirstResult($offset);
		}
		$query = $queryBuilder->execute();
		$result = [];

		while ($row = $query->fetch()) {
			$result[] = $row['userid'];
		}

		$query->closeCursor();

		return $result;
	}

	/**
	 * @param string $email
	 * @return IUser[]
	 * @since 9.1.0
	 */
	public function getByEmail($email) {
		$userIds = $this->config->getUsersForUserValueCaseInsensitive('settings', 'email', $email);

		$users = array_map(function($uid) {
			return $this->get($uid);
		}, $userIds);

		return array_values(array_filter($users, function($u) {
			return ($u instanceof IUser);
		}));
	}
}

}
