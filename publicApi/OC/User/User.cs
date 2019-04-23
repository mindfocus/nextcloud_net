using ext;
using System;
using System.Collections.Generic;
using System.Text;

namespace OC.User
{
    class User : OCP.IUser
    {
    /** @var string */
    private string uid;

	/** @var string */
	private string displayName;

	/** @var UserInterface|null */
	private OCP.UserInterface? backend;
	/** @var EventDispatcherInterface */
	private ext.Event dispatcher;

	/** @var bool */
	private bool enabled;

	/** @var Emitter|Manager */
	private Hooks.Emitter emitter;

	/** @var string */
	private string home;

	/** @var int */
	private long lastLogin;

	/** @var \OCP\IConfig */
	private OCP.IConfig config;

	/** @var IAvatarManager */
	private OCP.IAvatarManager avatarManager;

	/** @var IURLGenerator */
	private OCP.IURLGenerator urlGenerator;

	public User(string uid, OCP.UserInterface backend, ext.Event dispatcher, Hooks.Emitter emitter = null, OCP.IConfig config = null, OCP.IURLGenerator urlGenerator = null)
    {

            this.uid = uid;
            this.backend = backend;
            this.dispatcher = dispatcher;
            this.emitter = emitter;
            if(config == null)
            {
                config = \OC::server.getConfig();
            }
            this.config = config;
            this.urlGenerator = urlGenerator;
            this.enabled = this.config.getUserValue(uid, "core", "enabled", true);
            this.lastLogin = this.config.getUserValue(uid, "login", "lastlogin", 0);
            if(this.urlGenerator == null)
            {
                this.urlGenerator = \OC::server.getURLGenerator();

            }
    }

    /**
	 * get the user id
	 *
	 * @return string
	 */
    public string getUID()
    {
        return this.uid;
    }

    /**
	 * get the display name for the user, if no specific display name is set it will fallback to the user id
	 *
	 * @return string
	 */
    public string getDisplayName()
    {
        if (this.displayName == null)
        {
			    var pDisplayName = "";
                if (this.backend != null && this.backend.implementsActions(Backend.GET_DISPLAYNAME))
                {
                    // get display name and strip whitespace from the beginning and end of it
                    var backendDisplayName = this.backend.getDisplayName(this.uid);
                    pDisplayName = backendDisplayName.Trim();
                }

                if (pDisplayName != "")
            {
				this.displayName = pDisplayName;
            }
            else
            {
				this.displayName = this.uid;
            }
        }
        return this.displayName;
    }

    /**
	 * set the displayname for the user
	 *
	 * @param string displayName
	 * @return bool
	 */
    public bool setDisplayName(string pDisplayName)
    {
            pDisplayName = pDisplayName.Trim();
        if (this.backend.implementsActions(Backend.SET_DISPLAYNAME) && pDisplayName != "") {
			var result = ((OCP.User.Backend.ISetDisplayNameBackend)this.backend).setDisplayName(this.uid, pDisplayName);
            if (result) {
				this.displayName = pDisplayName;
				this.triggerChange("displayName", pDisplayName);
            }
            return result != false;
        } else
        {
            return false;
        }
    }

    /**
	 * set the email address of the user
	 *
	 * @param string|null mailAddress
	 * @return void
	 * @since 9.0.0
	 */
    public void setEMailAddress(string mailAddress)
    {
		var oldMailAddress = this.getEMailAddress();
        if (mailAddress == "") {
			this.config.deleteUserValue(this.uid, "settings", "email");
        } else
        {
			this.config.setUserValue(this.uid, "settings", "email", mailAddress);
        }
        if (oldMailAddress != mailAddress) {
			this.triggerChange("eMailAddress", mailAddress, oldMailAddress);
        }
    }

    /**
	 * returns the timestamp of the user"s last login or 0 if the user did never
	 * login
	 *
	 * @return int
	 */
    public long getLastLogin()
    {
        return this.lastLogin;
    }

    /**
	 * updates the timestamp of the most recent login of this user
	 */
    public bool updateLastLoginTimestamp()
    {
		var firstTimeLogin = (this.lastLogin == 0);
            this.lastLogin =TimeUtility.GetTimestampFormNow();
		this.config.setUserValue(
			this.uid, "login", "lastLogin", this.lastLogin.ToString());

        return firstTimeLogin;
    }

    /**
	 * Delete the user
	 *
	 * @return bool
	 */
    public bool delete()
    {
            //this.dispatcher.dispatch(IUser::class . "::preDelete", new GenericEvent(this));
            //if (this.emitter) {
            //	this.emitter.emit("\OC\User", "preDelete", array(this));
            //  }
            //// get the home now because it won"t return it after user deletion
            //homePath = this.getHome();
            //result = this.backend.deleteUser(this.uid);
            //if (result) {

            //	// FIXME: Feels like an hack - suggestions?

            //	groupManager = \OC::server.getGroupManager();
            //      // We have to delete the user from all groups
            //      foreach (groupManager.getUserGroupIds(this) as groupId) {
            //		group = groupManager.get(groupId);
            //          if (group) {
            //			\OC_Hook::emit("OC_Group", "pre_removeFromGroup", ["run" => true, "uid" => this.uid, "gid" => groupId]);
            //			group.removeUser(this);
            //			\OC_Hook::emit("OC_User", "post_removeFromGroup", ["uid" => this.uid, "gid" => groupId]);
            //          }
            //      }
            //	// Delete the user"s keys in preferences
            //	\OC::server.getConfig().deleteAllUserValues(this.uid);

            //      // Delete user files in /data/
            //      if (homePath !== false) {
            //		// FIXME: this operates directly on FS, should use View instead...
            //		// also this is not testable/mockable...
            //		\OC_Helper::rmdirr(homePath);
            //      }

            //      // Delete the users entry in the storage table
            //      Storage::remove("home::". this.uid);

            //	\OC::server.getCommentsManager().deleteReferencesOfActor("users", this.uid);
            //	\OC::server.getCommentsManager().deleteReadMarksFromUser(this);

            //	notification = \OC::server.getNotificationManager().createNotification();
            //	notification.setUser(this.uid);
            //	\OC::server.getNotificationManager().markProcessed(notification);

            //	/** @var AccountManager accountManager */
            //	accountManager = \OC::server.query(AccountManager::class);
            //	accountManager.deleteUser(this);

            //	this.dispatcher.dispatch(IUser::class . "::postDelete", new GenericEvent(this));
            //	if (this.emitter) {
            //		this.emitter.emit("\OC\User", "postDelete", array(this));
            //  }
            //  }
            //return !(result === false);
            return true;
	}

/**
 * Set the password of the user
 *
 * @param string password
 * @param string recoveryPassword for the encryption app to reset encryption keys
 * @return bool
 */
public bool setPassword(string password, string recoveryPassword = null)
{
            //this.dispatcher.dispatch(IUser::class . "::preSetPassword", new GenericEvent(this, [
            //	"password" => password,
            //	"recoveryPassword" => recoveryPassword,
            //]));
            //if (this.emitter) {
            //	this.emitter.emit("\OC\User", "preSetPassword", array(this, password, recoveryPassword));
            //}
            //if (this.backend.implementsActions(Backend::SET_PASSWORD)) {
            //	result = this.backend.setPassword(this.uid, password);
            //	this.dispatcher.dispatch(IUser::class . "::postSetPassword", new GenericEvent(this, [
            //		"password" => password,
            //		"recoveryPassword" => recoveryPassword,
            //	]));
            //	if (this.emitter) {
            //		this.emitter.emit("\OC\User", "postSetPassword", array(this, password, recoveryPassword));
            //	}
            //	return !(result === false);
            //} else {
            //	return false;
            //}
            return true;
	}

	/**
	 * get the users home folder to mount
	 *
	 * @return string
	 */
	public string getHome() {
		if (this.home != null) {
			if (this.backend.implementsActions(Backend.GET_HOME) && ((OCP.User.Backend.IGetHomeBackend)this.backend).getHome(this.uid) != null) {
                    this.home = ((OCP.User.Backend.IGetHomeBackend)backend).getHome(uid);
            } else if (this.config != null) {
				this.home = this.config.getSystemValue("datadirectory", \OC::SERVERROOT + "/data") + "/"+ this.uid;
			} else {
				this.home = \OC::SERVERROOT+ "/data/"+ this.uid;
			}
		}
		return this.home;
	}

	/**
	 * Get the name of the backend class the user is connected with
	 *
	 * @return string
	 */
	public string getBackendClassName() {
		if(this.backend is OCP.IUserBackend) {
			return ((OCP.IUserBackend)this.backend).getBackendName();
		}
            return this.GetType().Name;
	}

	public OCP.UserInterface getBackend() {
		return this.backend;
	}

	/**
	 * check if the backend allows the user to change his avatar on Personal page
	 *
	 * @return bool
	 */
	public bool canChangeAvatar() {
		if (this.backend.implementsActions(Backend.PROVIDE_AVATAR)) {
			return ((OCP.User.Backend.IProvideAvatarBackend)this.backend).canChangeAvatar(this.uid);
		}
		return true;
	}

	/**
	 * check if the backend supports changing passwords
	 *
	 * @return bool
	 */
	public bool canChangePassword() {
		return this.backend.implementsActions(Backend.SET_PASSWORD);
	}

	/**
	 * check if the backend supports changing display names
	 *
	 * @return bool
	 */
	public bool canChangeDisplayName() {
		if (this.config.getSystemValue("allow_user_to_change_display_name") == false) {
			return false;
		}
		return this.backend.implementsActions(Backend.SET_DISPLAYNAME);
	}

	/**
	 * check if the user is enabled
	 *
	 * @return bool
	 */
	public bool isEnabled() {
		return this.enabled;
	}

	/**
	 * set the enabled status for the user
	 *
	 * @param bool enabled
	 */
	public void setEnabled(bool enabled = true) {
		var oldStatus = this.isEnabled();
		this.enabled = enabled;
		if (oldStatus != this.enabled) {
			this.triggerChange("enabled", enabled);
			this.config.setUserValue(this.uid, "core", "enabled", enabled ? "true" : "false");
		}
	}

	/**
	 * get the users email address
	 *
	 * @return string|null
	 * @since 9.0.0
	 */
	public string getEMailAddress() {
		return this.config.getUserValue(this.uid, "settings", "email", "");
	}

	/**
	 * get the users" quota
	 *
	 * @return string
	 * @since 9.0.0
	 */
	public string getQuota() {
		var quota = this.config.getUserValue(this.uid, "files", "quota", "default");
		if(quota == "default") {
			quota = this.config.getAppValue("files", "default_quota", "none");
		}
		return quota;
	}

	/**
	 * set the users" quota
	 *
	 * @param string quota
	 * @return void
	 * @since 9.0.0
	 */
	public void setQuota(string quota) {
		var oldQuota = this.config.getUserValue(this.uid, "files", "quota", "");
		if(quota != "none" && quota != "default") {
			quota = OC_Helper::computerFileSize(quota);
			quota = OC_Helper::humanFileSize(quota);
		}
		this.config.setUserValue(this.uid, "files", "quota", quota);
		if(quota != oldQuota) {
			this.triggerChange("quota", quota);
		}
	}

	/**
	 * get the avatar image if it exists
	 *
	 * @param int size
	 * @return IImage|null
	 * @since 9.0.0
	 */
	public OCP.IImage getAvatarImage(int size) {
		// delay the initialization
		if (this.avatarManager == null) {
			this.avatarManager = \OC::server.getAvatarManager();
		}

		var avatar = this.avatarManager.getAvatar(this.uid);
		var image = avatar.get(-1);
            return image;
	}

	/**
	 * get the federation cloud id
	 *
	 * @return string
	 * @since 9.0.0
	 */
	public string getCloudId() {
		var uid = this.getUID();
		var server = this.urlGenerator.getAbsoluteURL("/");
		server =  rtrim( this.removeProtocolFromUrl(server), "/");
		return \OC::server.getCloudIdManager().getCloudId(uid, server).getId();
	}

	/**
	 * @param string url
	 * @return string
	 */
	private string removeProtocolFromUrl(string url) {
            if(url.IndexOf("https://") == 0)
            {
                return url.PadLeft("https://".Length);
            } else if(url.IndexOf("http://") == 0)
            {
                return url.PadLeft("http://".Length);

            }
		return url;
	}

	public void triggerChange(string feature, object value = null, object oldValue = null) {
		//this.dispatcher.dispatch(IUser::class . "::changeUser", new GenericEvent(this, [
		//	"feature" => feature,
		//	"value" => value,
		//	"oldValue" => oldValue,
		//]));
		//if (this.emitter) {
		//	this.emitter.emit("\OC\User", "changeUser", array(this, feature, value, oldValue));
		//}
	}
}

}
