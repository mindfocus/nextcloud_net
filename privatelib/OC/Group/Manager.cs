using System;
using System.Collections;
using System.Collections.Generic;
using ext;
using OC.Hooks;
using OCP;
using OCP.Sym;

namespace OC.Group
{

/**
 * Class Manager
 *
 * Hooks available in scope \OC\Group:
 * - preAddUser(\OC\Group\Group group, \OC\User\User user)
 * - postAddUser(\OC\Group\Group group, \OC\User\User user)
 * - preRemoveUser(\OC\Group\Group group, \OC\User\User user)
 * - postRemoveUser(\OC\Group\Group group, \OC\User\User user)
 * - preDelete(\OC\Group\Group group)
 * - postDelete(\OC\Group\Group group)
 * - preCreate(string groupId)
 * - postCreate(\OC\Group\Group group)
 *
 * @package OC\Group
 */
public class Manager : PublicEmitter , IGroupManager {
	/** @var GroupInterface[] */
	private IList<GroupInterface> backends = new List<GroupInterface>();

	/** @var \OC\User\Manager */
	private OC.User.Manager userManager;
	/** @var EventDispatcherInterface */
	private EventDispatcherInterface dispatcher;
	/** @var ILogger */
	private ILogger logger;

	/** @var \OC\Group\Group[] */
	private IList<OC.Group.Group > cachedGroups = new List<OC.Group.Group>();

	/** @var \OC\Group\Group[] */
	private IList<OC.Group.Group> cachedUserGroups =  new List<Group>();

	/** @var \OC\SubAdmin */
	private OC.SubAdmin subAdmin = null;

	/**
	 * @param \OC\User\Manager userManager
	 * @param EventDispatcherInterface dispatcher
	 * @param ILogger logger
	 */
	public Manager(OC.User.Manager userManager,
								EventDispatcherInterface dispatcher,
								ILogger logger) {
		this.userManager = userManager;
		this.dispatcher = dispatcher;
		this.logger = logger;

//		cachedGroups = & this.cachedGroups;
//		cachedUserGroups = & this.cachedUserGroups;
//		this.listen("OC.Group", "postDelete", function (group) use (&cachedGroups, &cachedUserGroups) {
//			/**
//			 * @var \OC\Group\Group group
//			 */
//			unset(cachedGroups[group.getGID()]);
//			cachedUserGroups = [];
//		});
//		this.listen('\OC\Group', 'postAddUser', function (group) use (&cachedUserGroups) {
//			/**
//			 * @var \OC\Group\Group group
//			 */
//			cachedUserGroups = [];
//		});
//		this.listen('\OC\Group', 'postRemoveUser', function (group) use (&cachedUserGroups) {
//			/**
//			 * @var \OC\Group\Group group
//			 */
//			cachedUserGroups = [];
//		});
	}

	/**
	 * Checks whether a given backend is used
	 *
	 * @param string backendClass Full classname including complete namespace
	 * @return bool
	 */
	public bool isBackendUsed(string backendClass) {
		backendClass = strtolower(ltrim(backendClass, '\\'));

		foreach (this.backends as backend) {
			if (strtolower(get_class(backend)) === backendClass) {
				return true;
			}
		}

		return false;
	}

	/**
	 * @param \OCP\GroupInterface backend
	 */
	public void addBackend(OCP.GroupInterface backend) {
		this.backends.Add(backend);;
		this.clearCaches();
	}

	public void clearBackends() {
		this.backends.Clear();
		this.clearCaches();
	}

	/**
	 * Get the active backends
	 * @return \OCP\GroupInterface[]
	 */
	public IList<OCP.GroupInterface> getBackends() {
		return this.backends;
	}


	protected void clearCaches() {
		this.cachedGroups .Clear();
		this.cachedUserGroups.Clear();
	}

	/**
	 * @param string gid
	 * @return \OC\Group\Group
	 */
	public OCP.IGroup get(string gid) {
		if (isset(this.cachedGroups[gid])) {
			return this.cachedGroups[gid];
		}
		return this.getGroupObject(gid);
	}

	/**
	 * @param string gid
	 * @param string displayName
	 * @return \OCP\IGroup
	 */
	protected IGroup getGroupObject(string gid, string displayName = null) {
		backends = [];
		foreach (this.backends as backend) {
			if (backend.implementsActions(\OC\Group\Backend::GROUP_DETAILS)) {
				groupData = backend.getGroupDetails(gid);
				if (is_array(groupData) && !empty(groupData)) {
					// take the display name from the first backend that has a non-null one
					if (is_null(displayName) && isset(groupData['displayName'])) {
						displayName = groupData['displayName'];
					}
					backends[] = backend;
				}
			} else if (backend.groupExists(gid)) {
				backends[] = backend;
			}
		}
		if (count(backends) === 0) {
			return null;
		}
		this.cachedGroups[gid] = new Group(gid, backends, this.dispatcher, this.userManager, this, displayName);
		return this.cachedGroups[gid];
	}

	/**
	 * @param string gid
	 * @return bool
	 */
	public bool groupExists(string gid) {
		return this.get(gid) is IGroup;
	}

	/**
	 * @param string gid
	 * @return \OC\Group\Group
	 */
	public IGroup createGroup(string gid) {
		if (gid.IsEmpty()) {
			return null;
		} else if (this.get(gid) != null) {
			return this.get(gid) ;
		} else {
//			this.emit("\OC\Group", "preCreate", array(gid));
			foreach (var backend in this.backends)
			{
				if (backend.)
				{
					
				}
			}
			foreach (this.backends as backend) {
				if (backend.implementsActions(\OC\Group\Backend::CREATE_GROUP)) {
					backend.createGroup(gid);
					group = this.getGroupObject(gid);
					this.emit('\OC\Group', 'postCreate', array(group));
					return group;
				}
			}
			return null;
		}
	}

	/**
	 * @param string search
	 * @param int limit
	 * @param int offset
	 * @return \OC\Group\Group[]
	 */
	public IList<Group> search(string search, int limit = -1 , int offset = -1) {
		groups = [];
		foreach (this.backends as backend) {
			groupIds = backend.getGroups(search, limit, offset);
			foreach (groupIds as groupId) {
				aGroup = this.get(groupId);
				if (aGroup instanceof IGroup) {
					groups[groupId] = aGroup;
				} else {
					this.logger.debug('Group "' . groupId . '" was returned by search but not found through direct access', ['app' => 'core']);
				}
			}
			if (!is_null(limit) and limit <= 0) {
				return array_values(groups);
			}
		}
		return array_values(groups);
	}

	/**
	 * @param IUser|null user
	 * @return \OC\Group\Group[]
	 */
	public IList<IGroup> getUserGroups(IUser user= null) {
		if (!user instanceof IUser) {
			return [];
		}
		return this.getUserIdGroups(user.getUID());
	}

	/**
	 * @param string uid the user id
	 * @return \OC\Group\Group[]
	 */
	public IList<IGroup> getUserIdGroups(string uid) {
		if (isset(this.cachedUserGroups[uid])) {
			return this.cachedUserGroups[uid];
		}
		groups = [];
		foreach (this.backends as backend) {
			groupIds = backend.getUserGroups(uid);
			if (is_array(groupIds)) {
				foreach (groupIds as groupId) {
					aGroup = this.get(groupId);
					if (aGroup instanceof IGroup) {
						groups[groupId] = aGroup;
					} else {
						this.logger.debug('User "' . uid . '" belongs to deleted group: "' . groupId . '"', ['app' => 'core']);
					}
				}
			}
		}
		this.cachedUserGroups[uid] = groups;
		return this.cachedUserGroups[uid];
	}

	/**
	 * Checks if a userId is in the admin group
	 * @param string userId
	 * @return bool if admin
	 */
	public bool isAdmin(string userId) {
		foreach (var backend in this.backends) {
			if (backend.implementsActions(\OC\Group\Backend::IS_ADMIN) && backend.isAdmin(userId)) {
				return true;
			}
		}
		return this.isInGroup(userId, "admin");
	}

	/**
	 * Checks if a userId is in a group
	 * @param string userId
	 * @param string group
	 * @return bool if in group
	 */
	public bool isInGroup(string userId, string group) {
		return array_key_exists(group, this.getUserIdGroups(userId));
	}

	/**
	 * get a list of group ids for a user
	 * @param IUser user
	 * @return array with group ids
	 */
	public IList<string> getUserGroupIds(IUser user) {
		return array_map(function(value) {
			return (string) value;
		}, array_keys(this.getUserGroups(user)));
	}

	/**
	 * get an array of groupid and displayName for a user
	 * @param IUser user
	 * @return array ['displayName' => displayname]
	 */
	public IDictionary<string,string> getUserGroupNames(IUser user) {
		return array_map(function(group) {
			return array('displayName' => group.getDisplayName());
		}, this.getUserGroups(user));
	}

	/**
	 * get a list of all display names in a group
	 * @param string gid
	 * @param string search
	 * @param int limit
	 * @param int offset
	 * @return array an array of display names (value) and user ids (key)
	 */
	public IDictionary<string,string> displayNamesInGroup(string gid, string search = "", int limit = -1, int offset = 0) {
		group = this.get(gid);
		if(is_null(group)) {
			return [];
		}

		search = trim(search);
		groupUsers = [];

		if(!empty(search)) {
			// only user backends have the capability to do a complex search for users
			searchOffset = 0;
			searchLimit = limit * 100;
			if(limit === -1) {
				searchLimit = 500;
			}

			do {
				filteredUsers = this.userManager.searchDisplayName(search, searchLimit, searchOffset);
				foreach(filteredUsers as filteredUser) {
					if(group.inGroup(filteredUser)) {
						groupUsers[]= filteredUser;
					}
				}
				searchOffset += searchLimit;
			} while(count(groupUsers) < searchLimit+offset && count(filteredUsers) >= searchLimit);

			if(limit === -1) {
				groupUsers = array_slice(groupUsers, offset);
			} else {
				groupUsers = array_slice(groupUsers, offset, limit);
			}
		} else {
			groupUsers = group.searchUsers('', limit, offset);
		}

		matchingUsers = [];
		foreach(groupUsers as groupUser) {
			matchingUsers[groupUser.getUID()] = groupUser.getDisplayName();
		}
		return matchingUsers;
	}

	/**
	 * @return \OC\SubAdmin
	 */
	public OC.SubAdmin getSubAdmin() {
		if (this.subAdmin == null) {
			this.subAdmin = new OC.SubAdmin(
				this.userManager,
				this,
				OC.server.getDatabaseConnection()
			);
		}

		return this.subAdmin;
	}
}
}