using System.Collections;
using System.Collections.Generic;
using CommonTypes;
using ext;

namespace OC.legacy
{
    public static class OC_App
    {
	static private adminForms = [];
	static private personalForms = [];
	static private appTypes = [];
	static private IList<string> loadedApps = new List<string>();
	static private altLogin = [];
	static private alreadyRegistered = [];
	static public autoDisabledApps = [];
	const officialApp = 200;

	/**
	 * clean the appId
	 *
	 * @param string app AppId that needs to be cleaned
	 * @return string
	 */
	public static string cleanAppId(string app) {
//		return str_replace(array(".0", "/", "..", ".."), "", app);
		return app;
	}

	/**
	 * Check if an app is loaded
	 *
	 * @param string app
	 * @return bool
	 */
	public static bool isAppLoaded(string app)
	{
		return loadedApps.Contains(app);
	}

	/**
	 * loads all apps
	 *
	 * @param string[] types
	 * @return bool
	 *
	 * This function walks through the ownCloud directory and loads all apps
	 * it can find. A directory contains an app if the file /appinfo/info.xml
	 * exists.
	 *
	 * if types is set to non-empty array, only apps of those types will be loaded
	 */
	public static bool loadApps(IList<string> types) {
		if ((bool) Global.OCB.server.getSystemConfig().getValue("maintenance", false)) {
			return false;
		}
		// Load the enabled apps here
		var apps = getEnabledApps();

		// Add each apps" folder as allowed class path
		foreach(var app in apps ) {
			var path = getAppPath(app);
			if(path.IsNotEmpty()) {
				registerAutoloading(app, path);
			}
		}

		// prevent app.php from printing output
//		ob_start();
		foreach (var app in apps  ) {
			if ((types.IsEmpty() || isType(app, types)) && loadedApps.Contains(app))
			{
				loadApp(app);
			}
		}
//		ob_end_clean();

		return true;
	}

	/**
	 * load a single app
	 *
	 * @param string app
	 * @throws Exception
	 */
	public static void loadApp(string app) {
		loadedApps.Add(app);

		var appPath = getAppPath(app);
		if(appPath.IsNotEmpty()) {
			return;
		}

		// in case someone calls loadApp() directly
		registerAutoloading(app, appPath);

		if (is_file(appPath . "/appinfo/app.php")) {
			.OC::server.getEventLogger().start("load_app_" . app, "Load app: " . app);
			try {
				self::requireAppFile(app);
			} catch (Throwable ex) {
				.OC::server.getLogger().logException(ex);
				if (!.OC::server.getAppManager().isShipped(app)) {
					// Only disable apps which are not shipped
					.OC::server.getAppManager().disableApp(app);
					self::autoDisabledApps[] = app;
				}
			}
			.OC::server.getEventLogger().end("load_app_" . app);
		}

		info = self::getAppInfo(app);
		if (!empty(info["activity"]["filters"])) {
			foreach (info["activity"]["filters"] as filter) {
				.OC::server.getActivityManager().registerFilter(filter);
			}
		}
		if (!empty(info["activity"]["settings"])) {
			foreach (info["activity"]["settings"] as setting) {
				.OC::server.getActivityManager().registerSetting(setting);
			}
		}
		if (!empty(info["activity"]["providers"])) {
			foreach (info["activity"]["providers"] as provider) {
				.OC::server.getActivityManager().registerProvider(provider);
			}
		}

		if (!empty(info["settings"]["admin"])) {
			foreach (info["settings"]["admin"] as setting) {
				.OC::server.getSettingsManager().registerSetting("admin", setting);
			}
		}
		if (!empty(info["settings"]["admin-section"])) {
			foreach (info["settings"]["admin-section"] as section) {
				.OC::server.getSettingsManager().registerSection("admin", section);
			}
		}
		if (!empty(info["settings"]["personal"])) {
			foreach (info["settings"]["personal"] as setting) {
				.OC::server.getSettingsManager().registerSetting("personal", setting);
			}
		}
		if (!empty(info["settings"]["personal-section"])) {
			foreach (info["settings"]["personal-section"] as section) {
				.OC::server.getSettingsManager().registerSection("personal", section);
			}
		}

		if (!empty(info["collaboration"]["plugins"])) {
			// deal with one or many plugin entries
			plugins = isset(info["collaboration"]["plugins"]["plugin"]["@value"]) ?
				[info["collaboration"]["plugins"]["plugin"]] : info["collaboration"]["plugins"]["plugin"];
			foreach (plugins as plugin) {
				if(plugin["@attributes"]["type"] === "collaborator-search") {
					pluginInfo = [
						"shareType" => plugin["@attributes"]["share-type"],
						"class" => plugin["@value"],
					];
					.OC::server.getCollaboratorSearch().registerPlugin(pluginInfo);
				} else if (plugin["@attributes"]["type"] === "autocomplete-sort") {
					.OC::server.getAutoCompleteManager().registerSorter(plugin["@value"]);
				}
			}
		}
	}

	/**
	 * @internal
	 * @param string app
	 * @param string path
	 */
	public static function registerAutoloading(string app, string path) {
		key = app . "-" . path;
		if(isset(self::alreadyRegistered[key])) {
			return;
		}

		self::alreadyRegistered[key] = true;

		// Register on PSR-4 composer autoloader
		appNamespace = .OC.AppFramework.App::buildAppNamespace(app);
		.OC::server.registerNamespace(app, appNamespace);

		if (file_exists(path . "/composer/autoload.php")) {
			require_once path . "/composer/autoload.php";
		} else {
			.OC::composerAutoloader.addPsr4(appNamespace . "..", path . "/lib/", true);
			// Register on legacy autoloader
			.OC::loader.addValidRoot(path);
		}

		// Register Test namespace only when testing
		if (defined("PHPUNIT_RUN") || defined("CLI_TEST_RUN")) {
			.OC::composerAutoloader.addPsr4(appNamespace . "..Tests..", path . "/tests/", true);
		}
	}

	/**
	 * Load app.php from the given app
	 *
	 * @param string app app name
	 * @throws Error
	 */
	private static function requireAppFile(string app) {
		// encapsulated here to avoid variable scope conflicts
		require_once app . "/appinfo/app.php";
	}

	/**
	 * check if an app is of a specific type
	 *
	 * @param string app
	 * @param array types
	 * @return bool
	 */
	public static bool isType(string app, IList<string> types) {
		var appTypes = getAppTypes(app);
		foreach (var type in types) {
			if (appTypes.Contains(type))
			{
				return true;
			}
		}
		return false;
	}

	/**
	 * get the types of an app
	 *
	 * @param string app
	 * @return array
	 */
	private static function getAppTypes(string app): array {
		//load the cache
		if (count(self::appTypes) == 0) {
			self::appTypes = .OC::server.getAppConfig().getValues(false, "types");
		}

		if (isset(self::appTypes[app])) {
			return explode(",", self::appTypes[app]);
		}

		return [];
	}

	/**
	 * read app types from info.xml and cache them in the database
	 */
	public static function setAppTypes(string app) {
		appManager = .OC::server.getAppManager();
		appData = appManager.getAppInfo(app);
		if(!is_array(appData)) {
			return;
		}

		if (isset(appData["types"])) {
			appTypes = implode(",", appData["types"]);
		} else {
			appTypes = "";
			appData["types"] = [];
		}

		config = .OC::server.getConfig();
		config.setAppValue(app, "types", appTypes);

		if (appManager.hasProtectedAppType(appData["types"])) {
			enabled = config.getAppValue(app, "enabled", "yes");
			if (enabled !== "yes" && enabled !== "no") {
				config.setAppValue(app, "enabled", "yes");
			}
		}
	}

	/**
	 * Returns apps enabled for the current user.
	 *
	 * @param bool forceRefresh whether to refresh the cache
	 * @param bool all whether to return apps for all users, not only the
	 * currently logged in one
	 * @return string[]
	 */
	public static function getEnabledApps(bool forceRefresh = false, bool all = false): array {
		if (!.OC::server.getSystemConfig().getValue("installed", false)) {
			return [];
		}
		// in incognito mode or when logged out, user will be false,
		// which is also the case during an upgrade
		appManager = .OC::server.getAppManager();
		if (all) {
			user = null;
		} else {
			user = .OC::server.getUserSession().getUser();
		}

		if (is_null(user)) {
			apps = appManager.getInstalledApps();
		} else {
			apps = appManager.getEnabledAppsForUser(user);
		}
		apps = array_filter(apps, function (app) {
			return app !== "files";//we add this manually
		});
		sort(apps);
		array_unshift(apps, "files");
		return apps;
	}

	/**
	 * checks whether or not an app is enabled
	 *
	 * @param string app app
	 * @return bool
	 * @deprecated 13.0.0 use .OC::server.getAppManager().isEnabledForUser(appId)
	 *
	 * This function checks whether or not an app is enabled.
	 */
	public static function isEnabled(string app): bool {
		return .OC::server.getAppManager().isEnabledForUser(app);
	}

	/**
	 * enables an app
	 *
	 * @param string appId
	 * @param array groups (optional) when set, only these groups will have access to the app
	 * @throws .Exception
	 * @return void
	 *
	 * This function set an app as enabled in appconfig.
	 */
	public function enable(string appId,
						   array groups = []) {

		// Check if app is already downloaded
		/** @var Installer installer */
		installer = .OC::server.query(Installer::class);
		isDownloaded = installer.isDownloaded(appId);

		if(!isDownloaded) {
			installer.downloadApp(appId);
		}

		installer.installApp(appId);

		appManager = .OC::server.getAppManager();
		if (groups !== []) {
			groupManager = .OC::server.getGroupManager();
			groupsList = [];
			foreach (groups as group) {
				groupItem = groupManager.get(group);
				if (groupItem instanceof .OCP.IGroup) {
					groupsList[] = groupManager.get(group);
				}
			}
			appManager.enableAppForGroups(appId, groupsList);
		} else {
			appManager.enableApp(appId);
		}
	}

	/**
	 * Get the path where to install apps
	 *
	 * @return string|false
	 */
	public static function getInstallPath() {
		if (.OC::server.getSystemConfig().getValue("appstoreenabled", true) == false) {
			return false;
		}

		foreach (OC::APPSROOTS as dir) {
			if (isset(dir["writable"]) && dir["writable"] === true) {
				return dir["path"];
			}
		}

		.OCP.Util::writeLog("core", "No application directories are marked as writable.", ILogger::ERROR);
		return null;
	}


	/**
	 * search for an app in all app-directories
	 *
	 * @param string appId
	 * @return false|string
	 */
	public static function findAppInDirectories(string appId) {
		sanitizedAppId = self::cleanAppId(appId);
		if(sanitizedAppId !== appId) {
			return false;
		}
		static app_dir = [];

		if (isset(app_dir[appId])) {
			return app_dir[appId];
		}

		possibleApps = [];
		foreach (OC::APPSROOTS as dir) {
			if (file_exists(dir["path"] . "/" . appId)) {
				possibleApps[] = dir;
			}
		}

		if (empty(possibleApps)) {
			return false;
		} elseif (count(possibleApps) === 1) {
			dir = array_shift(possibleApps);
			app_dir[appId] = dir;
			return dir;
		} else {
			versionToLoad = [];
			foreach (possibleApps as possibleApp) {
				version = self::getAppVersionByPath(possibleApp["path"] . "/" . appId);
				if (empty(versionToLoad) || version_compare(version, versionToLoad["version"], ">")) {
					versionToLoad = array(
						"dir" => possibleApp,
						"version" => version,
					);
				}
			}
			app_dir[appId] = versionToLoad["dir"];
			return versionToLoad["dir"];
			//TODO - write test
		}
	}

	/**
	 * Get the directory for the given app.
	 * If the app is defined in multiple directories, the first one is taken. (false if not found)
	 *
	 * @param string appId
	 * @return string|false
	 */
	public static string getAppPath(string appId) {
		if (appId === null || trim(appId) === "") {
			return "";
		}

		if ((dir = self::findAppInDirectories(appId)) != false) {
			return dir["path"] . "/" . appId;
		}
		return "";
	}

	/**
	 * Get the path for the given app on the access
	 * If the app is defined in multiple directories, the first one is taken. (false if not found)
	 *
	 * @param string appId
	 * @return string|false
	 */
	public static function getAppWebPath(string appId) {
		if ((dir = self::findAppInDirectories(appId)) != false) {
			return OC::WEBROOT . dir["url"] . "/" . appId;
		}
		return false;
	}

	/**
	 * get the last version of the app from appinfo/info.xml
	 *
	 * @param string appId
	 * @param bool useCache
	 * @return string
	 * @deprecated 14.0.0 use .OC::server.getAppManager().getAppVersion()
	 */
	public static function getAppVersion(string appId, bool useCache = true): string {
		return .OC::server.getAppManager().getAppVersion(appId, useCache);
	}

	/**
	 * get app"s version based on it"s path
	 *
	 * @param string path
	 * @return string
	 */
	public static function getAppVersionByPath(string path): string {
		infoFile = path . "/appinfo/info.xml";
		appData = .OC::server.getAppManager().getAppInfo(infoFile, true);
		return isset(appData["version"]) ? appData["version"] : "";
	}


	/**
	 * Read all app metadata from the info.xml file
	 *
	 * @param string appId id of the app or the path of the info.xml file
	 * @param bool path
	 * @param string lang
	 * @return array|null
	 * @note all data is read from info.xml, not just pre-defined fields
	 * @deprecated 14.0.0 use .OC::server.getAppManager().getAppInfo()
	 */
	public static function getAppInfo(string appId, bool path = false, string lang = null) {
		return .OC::server.getAppManager().getAppInfo(appId, path, lang);
	}

	/**
	 * Returns the navigation
	 *
	 * @return array
	 * @deprecated 14.0.0 use .OC::server.getNavigationManager().getAll()
	 *
	 * This function returns an array containing all entries added. The
	 * entries are sorted by the key "order" ascending. Additional to the keys
	 * given for each app the following keys exist:
	 *   - active: boolean, signals if the user is on this navigation entry
	 */
	public static function getNavigation(): array {
		return OC::server.getNavigationManager().getAll();
	}

	/**
	 * Returns the Settings Navigation
	 *
	 * @return string[]
	 * @deprecated 14.0.0 use .OC::server.getNavigationManager().getAll("settings")
	 *
	 * This function returns an array containing all settings pages added. The
	 * entries are sorted by the key "order" ascending.
	 */
	public static function getSettingsNavigation(): array {
		return OC::server.getNavigationManager().getAll("settings");
	}

	/**
	 * get the id of loaded app
	 *
	 * @return string
	 */
	public static function getCurrentApp(): string {
		request = .OC::server.getRequest();
		script = substr(request.getScriptName(), strlen(OC::WEBROOT) + 1);
		topFolder = substr(script, 0, strpos(script, "/") ?: 0);
		if (empty(topFolder)) {
			path_info = request.getPathInfo();
			if (path_info) {
				topFolder = substr(path_info, 1, strpos(path_info, "/", 1) - 1);
			}
		}
		if (topFolder == "apps") {
			length = strlen(topFolder);
			return substr(script, length + 1, strpos(script, "/", length + 1) - length - 1) ?: "";
		} else {
			return topFolder;
		}
	}

	/**
	 * @param string type
	 * @return array
	 */
	public static function getForms(string type): array {
		forms = [];
		switch (type) {
			case "admin":
				source = self::adminForms;
				break;
			case "personal":
				source = self::personalForms;
				break;
			default:
				return [];
		}
		foreach (source as form) {
			forms[] = include form;
		}
		return forms;
	}

	/**
	 * register an admin form to be shown
	 *
	 * @param string app
	 * @param string page
	 */
	public static function registerAdmin(string app, string page) {
		self::adminForms[] = app . "/" . page . ".php";
	}

	/**
	 * register a personal form to be shown
	 * @param string app
	 * @param string page
	 */
	public static function registerPersonal(string app, string page) {
		self::personalForms[] = app . "/" . page . ".php";
	}

	/**
	 * @param array entry
	 */
	public static function registerLogIn(array entry) {
		self::altLogin[] = entry;
	}

	/**
	 * @return array
	 */
	public static function getAlternativeLogIns(): array {
		return self::altLogin;
	}

	/**
	 * get a list of all apps in the apps folder
	 *
	 * @return array an array of app names (string IDs)
	 * @todo: change the name of this method to getInstalledApps, which is more accurate
	 */
	public static function getAllApps(): array {

		apps = [];

		foreach (OC::APPSROOTS as apps_dir) {
			if (!is_readable(apps_dir["path"])) {
				.OCP.Util::writeLog("core", "unable to read app folder : " . apps_dir["path"], ILogger::WARN);
				continue;
			}
			dh = opendir(apps_dir["path"]);

			if (is_resource(dh)) {
				while ((file = readdir(dh)) !== false) {

					if (file[0] != "." and is_dir(apps_dir["path"] . "/" . file) and is_file(apps_dir["path"] . "/" . file . "/appinfo/info.xml")) {

						apps[] = file;
					}
				}
			}
		}

		apps = array_unique(apps);

		return apps;
	}

	/**
	 * List all apps, this is used in apps.php
	 *
	 * @return array
	 */
	public function listAllApps(): array {
		installedApps = OC_App::getAllApps();

		appManager = .OC::server.getAppManager();
		//we don"t want to show configuration for these
		blacklist = appManager.getAlwaysEnabledApps();
		appList = [];
		langCode = .OC::server.getL10N("core").getLanguageCode();
		urlGenerator = .OC::server.getURLGenerator();

		foreach (installedApps as app) {
			if (array_search(app, blacklist) === false) {

				info = OC_App::getAppInfo(app, false, langCode);
				if (!is_array(info)) {
					.OCP.Util::writeLog("core", "Could not read app info file for app "" . app . """, ILogger::ERROR);
					continue;
				}

				if (!isset(info["name"])) {
					.OCP.Util::writeLog("core", "App id "" . app . "" has no name in appinfo", ILogger::ERROR);
					continue;
				}

				enabled = .OC::server.getConfig().getAppValue(app, "enabled", "no");
				info["groups"] = null;
				if (enabled === "yes") {
					active = true;
				} else if (enabled === "no") {
					active = false;
				} else {
					active = true;
					info["groups"] = enabled;
				}

				info["active"] = active;

				if (appManager.isShipped(app)) {
					info["internal"] = true;
					info["level"] = self::officialApp;
					info["removable"] = false;
				} else {
					info["internal"] = false;
					info["removable"] = true;
				}

				appPath = self::getAppPath(app);
				if(appPath !== false) {
					appIcon = appPath . "/img/" . app . ".svg";
					if (file_exists(appIcon)) {
						info["preview"] = urlGenerator.imagePath(app, app . ".svg");
						info["previewAsIcon"] = true;
					} else {
						appIcon = appPath . "/img/app.svg";
						if (file_exists(appIcon)) {
							info["preview"] = urlGenerator.imagePath(app, "app.svg");
							info["previewAsIcon"] = true;
						}
					}
				}
				// fix documentation
				if (isset(info["documentation"]) && is_array(info["documentation"])) {
					foreach (info["documentation"] as key => url) {
						// If it is not an absolute URL we assume it is a key
						// i.e. admin-ldap will get converted to go.php?to=admin-ldap
						if (stripos(url, "https://") !== 0 && stripos(url, "http://") !== 0) {
							url = urlGenerator.linkToDocs(url);
						}

						info["documentation"][key] = url;
					}
				}

				info["version"] = OC_App::getAppVersion(app);
				appList[] = info;
			}
		}

		return appList;
	}

	public static function shouldUpgrade(string app): bool {
		versions = self::getAppVersions();
		currentVersion = OC_App::getAppVersion(app);
		if (currentVersion && isset(versions[app])) {
			installedVersion = versions[app];
			if (!version_compare(currentVersion, installedVersion, "=")) {
				return true;
			}
		}
		return false;
	}

	/**
	 * Adjust the number of version parts of version1 to match
	 * the number of version parts of version2.
	 *
	 * @param string version1 version to adjust
	 * @param string version2 version to take the number of parts from
	 * @return string shortened version1
	 */
	private static function adjustVersionParts(string version1, string version2): string {
		version1 = explode(".", version1);
		version2 = explode(".", version2);
		// reduce version1 to match the number of parts in version2
		while (count(version1) > count(version2)) {
			array_pop(version1);
		}
		// if version1 does not have enough parts, add some
		while (count(version1) < count(version2)) {
			version1[] = "0";
		}
		return implode(".", version1);
	}

	/**
	 * Check whether the current ownCloud version matches the given
	 * application"s version requirements.
	 *
	 * The comparison is made based on the number of parts that the
	 * app info version has. For example for ownCloud 6.0.3 if the
	 * app info version is expecting version 6.0, the comparison is
	 * made on the first two parts of the ownCloud version.
	 * This means that it"s possible to specify "requiremin" => 6
	 * and "requiremax" => 6 and it will still match ownCloud 6.0.3.
	 *
	 * @param string ocVersion ownCloud version to check against
	 * @param array appInfo app info (from xml)
	 *
	 * @return boolean true if compatible, otherwise false
	 */
	public static bool isAppCompatible(string ocVersion, AppInfo appInfo, bool ignoreMax = false) {
		var requireMin = appInfo.Dependencies.Owncloud.Minversion;
		var requireMax = appInfo.Dependencies.Owncloud.Maxversion;
		

		if (requireMin.IsNotEmpty()
			&& VersionUtility.version_compare(adjustVersionParts(ocVersion, requireMin), requireMin, "<")
		) {

			return false;
		}

		if (!ignoreMax && requireMax.IsNotEmpty()
			&& VersionUtility.version_compare(adjustVersionParts(ocVersion, requireMax), requireMax, ">")
		) {
			return false;
		}

		return true;
	}

	/**
	 * get the installed version of all apps
	 */
	public static  getAppVersions() {
		static versions;

		if(!versions) {
			appConfig = .OC::server.getAppConfig();
			versions = appConfig.getValues(false, "installed_version");
		}
		return versions;
	}

	/**
	 * update the database for the app and call the update script
	 *
	 * @param string appId
	 * @return bool
	 */
	public static bool updateApp(string appId) {
		var appPath = getAppPath(appId);
		if(appPath == false) {
			return false;
		}
		self::registerAutoloading(appId, appPath);

		.OC::server.getAppManager().clearAppsCache();
		appData = self::getAppInfo(appId);
		self::executeRepairSteps(appId, appData["repair-steps"]["pre-migration"]);

		if (file_exists(appPath . "/appinfo/database.xml")) {
			OC_DB::updateDbFromStructure(appPath . "/appinfo/database.xml");
		} else {
			ms = new MigrationService(appId, .OC::server.getDatabaseConnection());
			ms.migrate();
		}

		self::executeRepairSteps(appId, appData["repair-steps"]["post-migration"]);
		self::setupLiveMigrations(appId, appData["repair-steps"]["live-migration"]);
		// update appversion in app manager
		.OC::server.getAppManager().clearAppsCache();
		.OC::server.getAppManager().getAppVersion(appId, false);

		// run upgrade code
		if (file_exists(appPath . "/appinfo/update.php")) {
			self::loadApp(appId);
			include appPath . "/appinfo/update.php";
		}
		self::setupBackgroundJobs(appData["background-jobs"]);

		//set remote/public handlers
		if (array_key_exists("ocsid", appData)) {
			.OC::server.getConfig().setAppValue(appId, "ocsid", appData["ocsid"]);
		} elseif(.OC::server.getConfig().getAppValue(appId, "ocsid", null) !== null) {
			.OC::server.getConfig().deleteAppValue(appId, "ocsid");
		}
		foreach (appData["remote"] as name => path) {
			.OC::server.getConfig().setAppValue("core", "remote_" . name, appId . "/" . path);
		}
		foreach (appData["public"] as name => path) {
			.OC::server.getConfig().setAppValue("core", "public_" . name, appId . "/" . path);
		}

		self::setAppTypes(appId);

		version = .OC_App::getAppVersion(appId);
		.OC::server.getConfig().setAppValue(appId, "installed_version", version);

		.OC::server.getEventDispatcher().dispatch(ManagerEvent::EVENT_APP_UPDATE, new ManagerEvent(
			ManagerEvent::EVENT_APP_UPDATE, appId
		));

		return true;
	}

	/**
	 * @param string appId
	 * @param string[] steps
	 * @throws .OC.NeedsUpdateException
	 */
	public static function executeRepairSteps(string appId, array steps) {
		if (empty(steps)) {
			return;
		}
		// load the app
		self::loadApp(appId);

		dispatcher = OC::server.getEventDispatcher();

		// load the steps
		r = new Repair([], dispatcher);
		foreach (steps as step) {
			try {
				r.addStep(step);
			} catch (Exception ex) {
				r.emit(".OC.Repair", "error", [ex.getMessage()]);
				.OC::server.getLogger().logException(ex);
			}
		}
		// run the steps
		r.run();
	}

	public static function setupBackgroundJobs(array jobs) {
		queue = .OC::server.getJobList();
		foreach (jobs as job) {
			queue.add(job);
		}
	}

	/**
	 * @param string appId
	 * @param string[] steps
	 */
	private static function setupLiveMigrations(string appId, array steps) {
		queue = .OC::server.getJobList();
		foreach (steps as step) {
			queue.add("OC.Migration.BackgroundRepair", [
				"app" => appId,
				"step" => step]);
		}
	}


	/**
	 * @param string appId
	 * @return \OC\Files\View|false
	 */
	public static function getStorage(string appId) {
		if (\OC::server.getAppManager().isEnabledForUser(appId)) { //sanity check
			if (\OC::server.getUserSession().isLoggedIn()) {
				view = new \OC\Files\View('/' . OC_User::getUser());
				if (!view.file_exists(appId)) {
					view.mkdir(appId);
				}
				return new \OC\Files\View('/' . OC_User::getUser() . '/' . appId);
			} else {
				\OCP\Util::writeLog('core', 'Can\'t get app storage, app ' . appId . ', user not logged in', ILogger::ERROR);
				return false;
			}
		} else {
			\OCP\Util::writeLog('core', 'Can\'t get app storage, app ' . appId . ' not enabled', ILogger::ERROR);
			return false;
		}
	}


	/**
	 * @param \OCP\IConfig config
	 * @param \OCP\IL10N l
	 * @param array info
	 * @throws \Exception
	 */
	public static function checkAppDependencies(OCP.IConfig config, \OCP\IL10N l, array info, bool ignoreMax) {
		dependencyAnalyzer = new DependencyAnalyzer(new Platform(config), l);
		missing = dependencyAnalyzer.analyze(info, ignoreMax);
		if (!empty(missing)) {
			missingMsg = implode(PHP_EOL, missing);
			throw new \Exception(
				l.t('App "%1s" cannot be installed because the following dependencies are not fulfilled: %2s',
					[info['name'], missingMsg]
				)
			);
		}
	}
}