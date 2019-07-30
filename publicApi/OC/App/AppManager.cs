using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ext;
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
	private IDictionary<string, string> appInfos = new Dictionary<string, string>();

	/** @var array */
	private IList<string> appVersions = new List<string>();

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
		var appsForUser = apps.Where(o => this.checkAppForUser(o.Value, user)).ToList();
		appsForUser = array_filter(apps, function (enabled) use (user) {
			return this.checkAppForUser(enabled, user);
		});
		return array_keys(appsForUser);
	}

	/**
	 * Check if an app is enabled for user
	 *
	 * @param string appId
	 * @param \OCP\IUser user (optional) if not defined, the currently logged in user will be used
	 * @return bool
	 */
	public function isEnabledForUser(appId, user = null) {
		if (this.isAlwaysEnabled(appId)) {
			return true;
		}
		if (user === null) {
			user = this.userSession.getUser();
		}
		installedApps = this.getInstalledAppsValues();
		if (isset(installedApps[appId])) {
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
		this.dispatcher.dispatch(ManagerEvent::EVENT_APP_ENABLE, new ManagerEvent(
			ManagerEvent::EVENT_APP_ENABLE, appId
		));
		this.clearAppsCache();
	}

	/**
	 * Whether a list of types contains a protected app type
	 *
	 * @param string[] types
	 * @return bool
	 */
	public function hasProtectedAppType(types) {
		if (empty(types)) {
			return false;
		}

		protectedTypes = array_intersect(this.protectedAppTypes, types);
		return !empty(protectedTypes);
	}

	/**
	 * Enable an app only for specific groups
	 *
	 * @param string appId
	 * @param \OCP\IGroup[] groups
	 * @throws \InvalidArgumentException if app can"t be enabled for groups
	 * @throws AppPathNotFoundException
	 */
	public function enableAppForGroups(appId, groups) {
		// Check if app exists
		this.getAppPath(appId);

		info = this.getAppInfo(appId);
		if (!empty(info["types"]) && this.hasProtectedAppType(info["types"])) {
			throw new \InvalidArgumentException("appId can"t be enabled for groups.");
		}

		groupIds = array_map(function (group) {
			/** @var \OCP\IGroup group */
			return group.getGID();
		}, groups);
		this.installedAppsCache[appId] = json_encode(groupIds);
		this.appConfig.setValue(appId, "enabled", json_encode(groupIds));
		this.dispatcher.dispatch(ManagerEvent::EVENT_APP_ENABLE_FOR_GROUPS, new ManagerEvent(
			ManagerEvent::EVENT_APP_ENABLE_FOR_GROUPS, appId, groups
		));
		this.clearAppsCache();
	}

	/**
	 * Disable an app for every user
	 *
	 * @param string appId
	 * @throws \Exception if app can"t be disabled
	 */
	public function disableApp(appId) {
		if (this.isAlwaysEnabled(appId)) {
			throw new \Exception("appId can"t be disabled.");
		}
		unset(this.installedAppsCache[appId]);
		this.appConfig.setValue(appId, "enabled", "no");

		// run uninstall steps
		appData = this.getAppInfo(appId);
		if (!is_null(appData)) {
			\OC_App::executeRepairSteps(appId, appData["repair-steps"]["uninstall"]);
		}

		this.dispatcher.dispatch(ManagerEvent::EVENT_APP_DISABLE, new ManagerEvent(
			ManagerEvent::EVENT_APP_DISABLE, appId
		));
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
	public IList getAppsNeedingUpgrade(version) {
		var appsToUpgrade = new List<string>();
		var apps = this.getInstalledApps();
		foreach (var appId in apps) {
			var appInfo = this.getAppInfo(appId);
			appDbVersion = this.appConfig.getValue(appId, "installed_version");
			if (appDbVersion
				&& isset(appInfo["version"])
				&& version_compare(appInfo["version"], appDbVersion, ">")
				&& \OC_App::isAppCompatible(version, appInfo)
			) {
				appsToUpgrade[] = appInfo;
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
	public IList getAppInfo(string appId, bool path = false, string lang = null)
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

		if (data is IList) {
			data = OC_App.parseAppInfo(data, lang);
		}

		if (lang === null) {
			this.appInfos[appId] = data;
		}

		return data;
	}

	public function getAppVersion(string appId, bool useCache = true): string {
		if(!useCache || !isset(this.appVersions[appId])) {
			appInfo = \OC::server.getAppManager().getAppInfo(appId);
			this.appVersions[appId] = (appInfo !== null && isset(appInfo["version"])) ? appInfo["version"] : "0";
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
	public function getIncompatibleApps(string version): array {
		apps = this.getInstalledApps();
		incompatibleApps = array();
		foreach (apps as appId) {
			info = this.getAppInfo(appId);
			if (info === null) {
				incompatibleApps[] = ["id" => appId];
			} else if (!\OC_App::isAppCompatible(version, info)) {
				incompatibleApps[] = info;
			}
		}
		return incompatibleApps;
	}

	/**
	 * @inheritdoc
	 * In case you change this method, also change \OC\App\CodeChecker\InfoChecker::isShipped()
	 */
	public function isShipped(appId) {
		this.loadShippedJson();
		return in_array(appId, this.shippedApps, true);
	}

	private function isAlwaysEnabled(appId) {
		alwaysEnabled = this.getAlwaysEnabledApps();
		return in_array(appId, alwaysEnabled, true);
	}

	/**
	 * In case you change this method, also change \OC\App\CodeChecker\InfoChecker::loadShippedJson()
	 * @throws \Exception
	 */
	private function loadShippedJson() {
		if (this.shippedApps === null) {
			shippedJson = \OC::SERVERROOT . "/core/shipped.json";
			if (!file_exists(shippedJson)) {
				throw new \Exception("File not found: shippedJson");
			}
			content = json_decode(file_get_contents(shippedJson), true);
			this.shippedApps = content["shippedApps"];
			this.alwaysEnabled = content["alwaysEnabled"];
		}
	}

	/**
	 * @inheritdoc
	 */
	public function getAlwaysEnabledApps() {
		this.loadShippedJson();
		return this.alwaysEnabled;
	}
}

}