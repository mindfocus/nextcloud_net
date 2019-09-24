using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonTypes;
using ext;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OC.legacy;
using OCP;
using OCP.App;
using OCP.Sym;

namespace OC.App
{
 class AppManager : IAppManager {

	/**
	 * Apps with these types can not be enabled for certain groups only
	 * @var string[]
	 */
	protected IList<string> protectedAppTypes = new List<string> {
		"filesystem",
		"prelogin",
		"authentication",
		"logging",
		"prevent_group_restriction",
	};

	/** @var IUserSession */
	private IUserSession userSession;

	/** @var AppConfig */
	private AppConfig appConfig;

	/** @var IGroupManager */
	private IGroupManager groupManager;

	/** @var ICacheFactory */
	private ICacheFactory memCacheFactory;

	/** @var EventDispatcherInterface */
	private EventDispatcherInterface dispatcher;

	/** @var string[] appId => enabled */
	private IDictionary<string,string> installedAppsCache = new Dictionary<string, string>();

	/** @var string[] */
	private IList<string> shippedApps;

	/** @var string[] */
	private IList<string> alwaysEnabled;

	/** @var array */
	private IDictionary<string, AppInfo> appInfos = new Dictionary<string, AppInfo>();

	/** @var array */
	private IDictionary<string,string> appVersions = new Dictionary<string,string>();

	/**
	 * @param IUserSession userSession
	 * @param AppConfig appConfig
	 * @param IGroupManager groupManager
	 * @param ICacheFactory memCacheFactory
	 * @param EventDispatcherInterface dispatcher
	 */
	public AppManager(IUserSession userSession,
								AppConfig appConfig,
								IGroupManager groupManager,
								ICacheFactory memCacheFactory,
								EventDispatcherInterface dispatcher) {
		this.userSession = userSession;
		this.appConfig = appConfig;
		this.groupManager = groupManager;
		this.memCacheFactory = memCacheFactory;
		this.dispatcher = dispatcher;
	}

	/**
	 * @return string[] appId => enabled
	 */
	private IDictionary<string,string> getInstalledAppsValues() {
		if (this.installedAppsCache.IsEmpty()) {
			var values = this.appConfig.getValues("", "enabled");

			var alwaysEnabledApps = this.getAlwaysEnabledApps();
			foreach(var appId in alwaysEnabledApps) {
				values[appId] = "yes";
			}

			this.installedAppsCache = values.Where(o => o != "no").ToDictionary(o=> o, p=> p);
//			ksort(this.installedAppsCache);
		}
		return this.installedAppsCache;
	}

	/**
	 * List all installed apps
	 *
	 * @return string[]
	 */
	public IList<string> getInstalledApps()
	{
		return this.getInstalledAppsValues().Keys.ToList();
	}

	/**
	 * List all apps enabled for a user
	 *
	 * @param \OCP\IUser user
	 * @return string[]
	 */
	public IList<string> getEnabledAppsForUser(IUser user) {
		var apps = this.getInstalledAppsValues();
		var appsForUser = apps.Where(o => this.checkAppForUser(o.Value, user)).ToDictionary( o => o.Key, p => p.Value);

		return appsForUser.Keys.ToList();
	}

	/**
	 * Check if an app is enabled for user
	 *
	 * @param string appId
	 * @param \OCP\IUser user (optional) if not defined, the currently logged in user will be used
	 * @return bool
	 */
	public bool isEnabledForUser(string appId, IUser user = null) {
		if (this.isAlwaysEnabled(appId)) {
			return true;
		}
		if (user == null) {
			user = this.userSession.getUser();
		}
		var installedApps = this.getInstalledAppsValues();
		if (installedApps.ContainsKey(appId)) {
			return this.checkAppForUser(installedApps[appId], user);
		} else {
			return false;
		}
	}

	/**
	 * @param string enabled
	 * @param IUser user
	 * @return bool
	 */
	private bool checkAppForUser(string enabled, IUser user) {
		if (enabled == "yes") {
			return true;
		} else if (user == null) {
			return false;
		} else {
			if(enabled.IsEmpty()){
				return false;
			}

			var groupIds = JToken.Parse(enabled);

			if (!(groupIds is JArray)) {
//				jsonError = json_last_error();
//				\OC::server.getLogger().warning("AppManger::checkAppForUser - can\"t decode group IDs: " . print_r(enabled, true) . " - json error code: " . jsonError, ["app" => "lib"]);
				return false;
			}

			var userGroups = this.groupManager.getUserGroupIds(user);
			foreach (var groupId in userGroups)
			{
				if (groupIds.Contains(groupId))
				{
					return true;
				}
			}
			return false;
		}
	}

	/**
	 * Check if an app is enabled in the instance
	 *
	 * Notice: This actually checks if the app is enabled and not only if it is installed.
	 *
	 * @param string appId
	 * @return bool
	 */
	public bool isInstalled(string appId) {
		var installedApps = this.getInstalledAppsValues();
		return installedApps.ContainsKey(appId);
	}

	/**
	 * Enable an app for every user
	 *
	 * @param string appId
	 * @throws AppPathNotFoundException
	 */
	public void enableApp(string appId) {
		// Check if app exists
		this.getAppPath(appId);

		this.installedAppsCache[appId] = "yes";
		this.appConfig.setValue(appId, "enabled", "yes");
//		this.dispatcher.dispatch(ManagerEvent::EVENT_APP_ENABLE, new ManagerEvent(
//			ManagerEvent::EVENT_APP_ENABLE, appId
//		));
		this.clearAppsCache();
	}

	/**
	 * Whether a list of types contains a protected app type
	 *
	 * @param string[] types
	 * @return bool
	 */
	public bool hasProtectedAppType(IList<string> types) {
		if (types.IsEmpty()) {
			return false;
		}

		var protectedTypes = this.protectedAppTypes.Intersect(types);
		return protectedTypes.Any();
	}

	/**
	 * Enable an app only for specific groups
	 *
	 * @param string appId
	 * @param \OCP\IGroup[] groups
	 * @throws \InvalidArgumentException if app can"t be enabled for groups
	 * @throws AppPathNotFoundException
	 */
	public void enableAppForGroups(string appId, IList<OCP.IGroup> groups) {
		// Check if app exists
		this.getAppPath(appId);

		var info = this.getAppInfo(appId);
		if ( info.Types.IsNotEmpty() && this.hasProtectedAppType(info.Types.Select(o => o.Filesystem).ToList())) {
			throw new ArgumentException (@"appId can't be enabled for groups.");
		}

		var groupIds = groups.Select(o => o.getGID());

		this.installedAppsCache[appId] = JsonConvert.SerializeObject(groupIds);
		this.appConfig.setValue(appId, "enabled", groupIds);
//		this.dispatcher.dispatch(ManagerEvent::EVENT_APP_ENABLE_FOR_GROUPS, new ManagerEvent(
//			ManagerEvent::EVENT_APP_ENABLE_FOR_GROUPS, appId, groups
//		));
		this.clearAppsCache();
	}

	/**
	 * Disable an app for every user
	 *
	 * @param string appId
	 * @throws \Exception if app can"t be disabled
	 */
	public void disableApp(string appId) {
		if (this.isAlwaysEnabled(appId)) {
			throw new Exception("appId can't be disabled.");
		}

		this.installedAppsCache.Remove(appId);
		this.appConfig.setValue(appId, "enabled", "no");

		// run uninstall steps
		var appData = this.getAppInfo(appId);
		if ( null != appData ) {
//			OC_App::executeRepairSteps(appId, appData["repair-steps"]["uninstall"]);
		}

//		this.dispatcher.dispatch(ManagerEvent::EVENT_APP_DISABLE, new ManagerEvent(
//			ManagerEvent::EVENT_APP_DISABLE, appId
//		));
		this.clearAppsCache();
	}

	/**
	 * Get the directory for the given app.
	 *
	 * @param string appId
	 * @return string
	 * @throws AppPathNotFoundException if app folder can"t be found
	 */
	public string getAppPath(string appId) {
		var appPath = OC_App.getAppPath(appId);
		if(appPath.IsEmpty()) {
			throw new AppPathNotFoundException("Could not find path for " + appId);
		}
		return appPath;
	}

	/**
	 * Clear the cached list of apps when enabling/disabling an app
	 */
	public void clearAppsCache() {
		var settingsMemCache = this.memCacheFactory.createDistributed("settings");
		settingsMemCache.clear("listApps");
		this.appInfos.Clear();
	}

	/**
	 * Returns a list of apps that need upgrade
	 *
	 * @param string version Nextcloud version as array of version components
	 * @return array list of app info from apps that need an upgrade
	 *
	 * @internal
	 */
	public IList getAppsNeedingUpgrade(string version) {
		var appsToUpgrade = new List<AppInfo>();
		var apps = this.getInstalledApps();
		foreach (var appId in apps) {
			var appInfo = this.getAppInfo(appId);
			var appDbVersion = this.appConfig.getValue(appId, "installed_version");
			if (appDbVersion.IsNotEmpty()
				&& appInfo.Version.IsNotEmpty()
				&& VersionUtility.version_compare(appInfo.Version, appDbVersion, ">")
				&& OC_App.isAppCompatible(version, appInfo)
			) {
				appsToUpgrade.Add(appInfo);
			}
		}

		return appsToUpgrade;
	}

	/**
	 * Returns the app information from "appinfo/info.xml".
	 *
	 * @param string appId app id
	 *
	 * @param bool path
	 * @param null lang
	 * @return array|null app info
	 */
	public AppInfo getAppInfo(string appId, bool path = false, string lang = null)
	{
		string file = "";
		if (path) {
			file = appId;
		} else {
			if (lang == null && this.appInfos.ContainsKey(appId)) {
				return this.appInfos[appId];
			}

			string appPath = "";
			try {
				appPath = this.getAppPath(appId);
			} catch (AppPathNotFoundException e) {
				return null;
			}
			file = appPath + "/appinfo/info.xml";
		}

		var parser = new InfoParser(this.memCacheFactory.createLocal("core.appinfo"));
		var data = parser.parse(file);

		if (lang == null) {
			this.appInfos[appId] = data;
		}

		return data;
	}

	public string getAppVersion(string appId, bool useCache = true) {
		if(!useCache || !this.appVersions.ContainsKey(appId)) {
			var appInfo = getAppInfo(appId);
			this.appVersions[appId] = (appInfo != null && appInfo.Version.IsNotEmpty()) ? appInfo.Version : "0";
		}
		return this.appVersions[appId];
	}

	/**
	 * Returns a list of apps incompatible with the given version
	 *
	 * @param string version Nextcloud version as array of version components
	 *
	 * @return array list of app info from incompatible apps
	 *
	 * @internal
	 */
	public IList<AppInfo> getIncompatibleApps(string version) {
		var apps = this.getInstalledApps();
		var incompatibleApps = new List<AppInfo>();
		foreach (var appId in apps) {
			var info = this.getAppInfo(appId);
			if (info == null)
			{
				info = new AppInfo()
				{
					Id = appId
				};
				incompatibleApps.Add(info);
			} else if (!OC_App.isAppCompatible(version, info)) {
				incompatibleApps.Add(info);
			}
		}
		return incompatibleApps;
	}

	/**
	 * @inheritdoc
	 * In case you change this method, also change \OC\App\CodeChecker\InfoChecker::isShipped()
	 */
	public bool isShipped(string appId) {
		this.loadShippedJson();
		return this.shippedApps.Contains(appId);
	}

	private bool isAlwaysEnabled(string appId) {
		var alwaysEnabledApps = this.getAlwaysEnabledApps();
		return alwaysEnabledApps.Contains(appId);
	}

	/**
	 * In case you change this method, also change \OC\App\CodeChecker\InfoChecker::loadShippedJson()
	 * @throws \Exception
	 */
	private void loadShippedJson() {
		if (this.shippedApps == null) {
			var shippedJson = OC.SERVERROOT + "/core/shipped.json";
			if (!File.Exists(shippedJson)) {
				throw new Exception("File not found: shippedJson");
			}

			var content = JToken.Parse(File.ReadAllText(shippedJson)) as JArray;
			this.shippedApps = content["shippedApps"];
			this.alwaysEnabled = content["alwaysEnabled"];
		}
	}

	/**
	 * @inheritdoc
	 */
	public IList<string> getAlwaysEnabledApps() {
		this.loadShippedJson();
		return this.alwaysEnabled;
	}
}

}