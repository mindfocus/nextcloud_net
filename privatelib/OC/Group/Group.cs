using System.Collections.Generic;
using System.Linq;
using ext;
using OC.Hooks;
using OC.User;
using OCP;
using OCP.Group;
using OCP.Group.Backend;
using OCP.Sym;

namespace OC.Group
{

public class Group : IGroup {
	/** @var null|string  */
	protected string displayName;

	/** @var string */
	private string gid;

	/** @var \OC\User\User[] */
	private IDictionary<string, OC.User.User> users = new Dictionary<string, User.User>();

	/** @var bool */
	private bool usersLoaded;

	/** @var Backend[] */
	private IList<GroupInterface> backends;
	/** @var EventDispatcherInterface */
	private EventDispatcherInterface dispatcher;
	/** @var \OC\User\Manager|IUserManager  */
	private IUserManager userManager;
	/** @var PublicEmitter */
	private PublicEmitter emitter;


	/**
	 * @param string gid
	 * @param Backend[] backends
	 * @param EventDispatcherInterface dispatcher
	 * @param IUserManager userManager
	 * @param PublicEmitter emitter
	 * @param string displayName
	 */
	public Group(string gid, IList<GroupInterface> backends, EventDispatcherInterface dispatcher, IUserManager userManager, PublicEmitter emitter = null, string displayName = null) {
		this.gid = gid;
		this.backends = backends;
		this.dispatcher = dispatcher;
		this.userManager = userManager;
		this.emitter = emitter;
		this.displayName = displayName;
	}

	public string getGID() {
		return this.gid;
	}

	public string getDisplayName() {
		if (this.displayName == null) {
			foreach (var backend in this.backends) {
				if (backend is IGetDisplayNameBackend displayNameBackend) {
					var displayNameTemp = displayNameBackend.getDisplayName(this.gid);
					if (displayNameTemp.Trim().IsNotEmpty()) {
						this.displayName = displayNameTemp;
						return this.displayName;
					}
				}
			}
			return this.gid;
		}
		return this.displayName;
	}

	/**
	 * get all users in the group
	 *
	 * @return \OC\User\User[]
	 */
	public IList<IUser> getUsers() {
		if (this.usersLoaded) {
			return this.users.Values.ToList();
		}

		var userIds = new List<string>();
		foreach (var backend in this.backends) {
			diff = array_diff(
				backend.usersInGroup(this.gid),
				userIds
			);
			if (diff) {
				userIds = array_merge(userIds, diff);
			}
		}

		this.users = this.getVerifiedUsers(userIds);
		this.usersLoaded = true;
		return this.users;
	}

	/**
	 * check if a user is in the group
	 *
	 * @param IUser user
	 * @return bool
	 */
	public bool inGroup(IUser user) {
		if (this.users.ContainsKey(user.getUID())) {
			return true;
		}
		foreach (var backend in this.backends) {
			if (backend.inGroup(user.getUID(), this.gid)) {
				this.users[user.getUID()] = (User.User)user;
				return true;
			}
		}
		return false;
	}

	/**
	 * add a user to the group
	 *
	 * @param IUser user
	 */
	public bool addUser(IUser user) {
		if (this.inGroup(user)) {
			return true;
		}

		this.dispatcher.dispatch(IGroup::class . '::preAddUser', new GenericEvent(this, [
			'user' => user,
		]));

		if (this.emitter) {
			this.emitter.emit('\OC\Group', 'preAddUser', array(this, user));
		}
		foreach (this.backends as backend) {
			if (backend.implementsActions(\OC\Group\Backend::ADD_TO_GROUP)) {
				backend.addToGroup(user.getUID(), this.gid);
				if (this.users) {
					this.users[user.getUID()] = user;
				}

				this.dispatcher.dispatch(IGroup::class . '::postAddUser', new GenericEvent(this, [
					'user' => user,
				]));

				if (this.emitter) {
					this.emitter.emit('\OC\Group', 'postAddUser', array(this, user));
				}
				return;
			}
		}
	}

	/**
	 * remove a user from the group
	 *
	 * @param \OC\User\User user
	 */
	public bool removeUser(IUser user) {
		var result = false;
		this.dispatcher.dispatch(IGroup::class . '::preRemoveUser', new GenericEvent(this, [
			'user' => user,
		]));
		if (this.emitter) {
			this.emitter.emit('\OC\Group', 'preRemoveUser', array(this, user));
		}
		foreach (this.backends as backend) {
			if (backend.implementsActions(\OC\Group\Backend::REMOVE_FROM_GOUP) and backend.inGroup(user.getUID(), this.gid)) {
				backend.removeFromGroup(user.getUID(), this.gid);
				result = true;
			}
		}
		if (result) {
			this.dispatcher.dispatch(IGroup::class . '::postRemoveUser', new GenericEvent(this, [
				'user' => user,
			]));
			if (this.emitter) {
				this.emitter.emit('\OC\Group', 'postRemoveUser', array(this, user));
			}
			if (this.users) {
				foreach (this.users as index => groupUser) {
					if (groupUser.getUID() === user.getUID()) {
						unset(this.users[index]);
						return;
					}
				}
			}
		}
	}

	/**
	 * search for users in the group by userid
	 *
	 * @param string search
	 * @param int limit
	 * @param int offset
	 * @return \OC\User\User[]
	 */
	public IList<User.User> searchUsers(string search, int limit = -1, int offset = -1) {
		var users = new List<User.User>();
		foreach (var backend in this.backends) {
			var userIds = backend.usersInGroup(this.gid, search, limit, offset);
			users.AddRange(this.getVerifiedUsers(userIds));
		}
		return users;
	}

	/**
	 * returns the number of users matching the search string
	 *
	 * @param string search
	 * @return int|bool
	 */
	public int count(string search = "") {
		var usersCount = 0;
		var countAlready = false;
		foreach (var backend in this.backends) {
			if(backend is ICountUsersBackend countUsersBackend) {
				if(countAlready == false) {
					//we could directly add to a bool variable, but this would
					//be ugly
					usersCount = 0;
					countAlready = true;
				}
				usersCount += countUsersBackend.countUsersInGroup(this.gid);
			}
		}
		return usersCount;
	}

	/**
	 * returns the number of disabled users
	 *
	 * @return int|bool
	 */
	public int countDisabled() {
		var usersCount = 0;
		var countAlready = false;
		foreach (var backend in this.backends) {
			if(backend is ICountDisabledInGroup countDisabledInGroup) {
				if(countAlready == false) {
					//we could directly add to a bool variable, but this would
					//be ugly
					usersCount = 0;
					countAlready = true;
				}
				usersCount += countDisabledInGroup.countDisabledInGroup(this.gid);
			}
		}
		return usersCount;
	}

	/**
	 * search for users in the group by displayname
	 *
	 * @param string search
	 * @param int limit
	 * @param int offset
	 * @return \OC\User\User[]
	 */
	public IList<User.User> searchDisplayName(string search, int limit = -1, int offset = -1) {
		var users = new List<User.User>();
		foreach (var backend in this.backends) {
			var userIds = backend.usersInGroup(this.gid, search, limit, offset);
			users.AddRange(this.getVerifiedUsers(userIds));
		}
		return users;
	}

	/**
	 * delete the group
	 *
	 * @return bool
	 */
	public bool delete() {
		// Prevent users from deleting group admin
		if (this.getGID() == "admin") {
			return false;
		}

		var result = false;
		this.dispatcher.dispatch(IGroup::class . '::preDelete', new GenericEvent(this));
		if (this.emitter) {
			this.emitter.emit('\OC\Group', 'preDelete', array(this));
		}
		foreach (var backend in this.backends) {
			if (backend.implementsActions(\OC\Group\Backend::DELETE_GROUP)) {
				result = true;
				((IDeleteGroupBackend)backend).deleteGroup(this.gid);
			}
		}
		if (result) {
			this.dispatcher.dispatch(IGroup::class . '::postDelete', new GenericEvent(this));
			if (this.emitter) {
				this.emitter.emit('\OC\Group', 'postDelete', array(this));
			}
		}
		return result;
	}

	/**
	 * returns all the Users from an array that really exists
	 * @param string[] userIds an array containing user IDs
	 * @return \OC\User\User[] an Array with the userId as Key and \OC\User\User as value
	 */
	private IList<OC.User.User> getVerifiedUsers(IList<string> userIds) {
		var users = new List<User.User>();
		foreach (var userId in userIds)
		{
			var user = this.userManager.get(userId);
			if (user!= null)
			{
				users.Add((User.User)user);
			}
		}
		return users;
	}

	/**
	 * @return bool
	 * @since 14.0.0
	 */
	public bool canRemoveUser() {
		foreach (var backend in this.backends) {
			if (backend.implementsActions(GroupInterface::REMOVE_FROM_GOUP)) {
				return true;
			}
		}
		return false;
	}

	/**
	 * @return bool
	 * @since 14.0.0
	 */
	public bool canAddUser() {
		foreach (var backend in this.backends) {
			if (backend.implementsActions(GroupInterface::ADD_TO_GROUP)) {
				return true;
			}
		}
		return false;
	}

	/**
	 * @return bool
	 * @since 16.0.0
	 */
	public bool hideFromCollaboration(){
		foreach (var groupInterface in this.backends)
		{
			if (groupInterface is IHideFromCollaborationBackend hideFromCollaborationBackend && hideFromCollaborationBackend.hideGroup(this.gid))
			{
				return true;
			}
		}

		return false;
	}
}

}