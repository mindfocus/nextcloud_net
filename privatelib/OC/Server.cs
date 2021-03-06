﻿using OCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OC.AppFramework.Utility;
using OC.Contacts.ContactsMenu;
using OC.Files;
using OC.Files.Cache;
using OCP.AppFramework;
using OCP.ContactsNs.ContactsMenu;
using OCP.Files;
using OCP.Files.Storage;
using OCP.Remote.Api;
using Pchp.Library;
using Pchp.Library.DateTime;
using Manager = OC.User.Manager;

namespace OC
{
    /**
     * Class Server
     *
     * @package OC
     *
     * TODO: hookup all manager classes
     */
    public class Server : ServerContainer , IServerContainer
    {
    /** @var string */
    private string webRoot;

        /**
         * @param string webRoot
         * @param .OC.Config config
         */
        Server(string webRoot, Config config): base()
    {
            this.webRoot = webRoot;
		// To find out if we are running from CLI or not
//		this.registerParameter("isCLI", .OC::CLI);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var containerBuilder = new ContainerBuilder();
        containerBuilder.Populate(serviceCollection);
        containerBuilder.RegisterType<IServerContainer>();
//        containerBuilder.RegisterType<OC.Calendar.Manager>().Named<OCP.Calendar.IManager>("CalendarManager");
//        containerBuilder.RegisterType<OC.Calendar.Resource.Manager>()
//            .Named<OCP.Calendar.Resource.IManager>("CalendarResourceBackendManager");
//        containerBuilder.RegisterType<OC.Calendar.Room.Manager>().Named<OCP.Calendar.Room.IManager>("CalendarRoomBackendManager");
        containerBuilder.RegisterType<OC.ContactsManager>().Named<OCP.ContactsNs.IManager>("ContactsManager");
        containerBuilder.RegisterType<ActionFactory>().As<IActionFactory>();
        containerBuilder.Register<PreviewManager>(( c,p) =>
        {
	        var server = c.Resolve<Server>();
            return new PreviewManager(server.getConfig(),
	            server.getRootFolder(),
	            server.getAppDataDir("preview"),
	            server.getEventDispatcher(),
	            server.getSession().get("user_id"));
        }).Named<OCP.IPreview>("PreviewManager");

        containerBuilder.Register<Preview.Watcher>((c, p) =>
        {
	        var server = p.TypedAs<Server>();
	        return new Preview.Watcher(server.getAppDataDir("preview"));
        });
        containerBuilder.Register<OCP.Encryption.IManager>((c, p) =>
        {
	        var server = c.Resolve<IServerContainer>();
	        var view = new View();
	        var util = new Encryption.Util(view, server.getUserManager(), server.getGroupManager(), server.getConfig());
	        return new Encryption.Manager(server.getConfig(),server.getLogger(),server.getL10N("core"),
		        new View(), util, new List<string>());
        });
        containerBuilder.RegisterType<OCP.Encryption.IManager>().Named<OCP.Encryption.IManager>("EncryptionManager");

        containerBuilder.Register<OCP.Encryption.IFile>((c, p) =>
        {
	        var server = p.TypedAs<Server>();
	        var util = new Encryption.Util(
		        new View(),
		        server.getUserManager(),
		        server.getGroupManager(),
		        server.getConfig()
	        );
	        return new Encryption.File(
		        util,
		        server.getRootFolder(),
		        server.getShareManager()
	        );
        });
        containerBuilder.Register<OCP.Encryption.Keys.IStorage>((c, p) =>
        {
	        var server = p.TypedAs<Server>();
	        var view = new View();
	        var util = new Encryption.Util(
		        view,
		        server.getUserManager(),
		        server.getGroupManager(),
		        server.getConfig()
		        );
	        return new Encryption.Keys.Storage(view , util);
        }).Named<OCP.Encryption.Keys.IStorage>("EncryptionKeyStorage");
        containerBuilder.Register<OC.Tagging.TagMapper>((c, p) =>
        {
	        var server = p.TypedAs<Server>();
	        return new TagMapper(server.getDatabaseConnection());
        }).Named<OC.Tagging.TagMapper>("TagMapper");
        containerBuilder.Register<OCP.ITagManager>((c, p) =>
        {
	        var server = p.TypedAs<Server>();
	        var tagMapper = c.Resolve<OC.Tagging.TagMapper>;
	        return new TagManager(tagMapper , server.getUserSession());
        });
        containerBuilder.RegisterType<OCP.ITagManager>().Named<OCP.ITagManager>("TagManager");

		this.registerService("SystemTagManagerFactory", function (Server c) {
			config = c.getConfig();
			factoryClass = config.getSystemValue("systemtags.managerFactory", SystemTagManagerFactory::class);
			return new factoryClass(this);
});
		this.registerService(.OCP.SystemTag.ISystemTagManager::class, function(Server c)
{
    return c.query("SystemTagManagerFactory").getManager();
});
		this.registerAlias("SystemTagManager", .OCP.SystemTag.ISystemTagManager::class);

		this.registerService(.OCP.SystemTag.ISystemTagObjectMapper::class, function (Server c) {
			return c.query("SystemTagManagerFactory").getObjectMapper();
		});
		this.registerService("RootFolder", function (Server c) {
			manager = .OC.Files.Filesystem::getMountManager(null);
			view = new View();
			root = new Root(
				manager,
				view,
				null,
				c.getUserMountCache(),
				this.getLogger(),
				this.getUserManager()
			);
			connector = new HookConnector(root, view);
			connector.viewToNode();

			previewConnector = new .OC.Preview.WatcherConnector(root, c.getSystemConfig());
			previewConnector.connectWatcher();

			return root;
		});
		this.registerAlias("SystemTagObjectMapper", .OCP.SystemTag.ISystemTagObjectMapper::class);

		this.registerService(.OCP.Files.IRootFolder::class, function (Server c) {
			return new LazyRoot(function () use (c) {
				return c.query("RootFolder");
			});
		});
		this.registerAlias("LazyRootFolder", .OCP.Files.IRootFolder::class);

		this.registerService(.OC.User.Manager::class, function (Server c) {
			return new .OC.User.Manager(c.getConfig(), c.getEventDispatcher());
		});
		this.registerAlias("UserManager", .OC.User.Manager::class);
		this.registerAlias(.OCP.IUserManager::class, .OC.User.Manager::class);

		this.registerService(.OCP.IGroupManager::class, function (Server c) {
			groupManager = new .OC.Group.Manager(this.getUserManager(), c.getEventDispatcher(), this.getLogger());
			groupManager.listen(".OC.Group", "preCreate", function (gid) {
				.OC_Hook::emit("OC_Group", "pre_createGroup", array("run" => true, "gid" => gid));
			});
			groupManager.listen(".OC.Group", "postCreate", function (.OC.Group.Group gid) {
				.OC_Hook::emit("OC_User", "post_createGroup", array("gid" => gid.getGID()));
			});
			groupManager.listen(".OC.Group", "preDelete", function (.OC.Group.Group group) {
				.OC_Hook::emit("OC_Group", "pre_deleteGroup", array("run" => true, "gid" => group.getGID()));
			});
			groupManager.listen(".OC.Group", "postDelete", function (.OC.Group.Group group) {
				.OC_Hook::emit("OC_User", "post_deleteGroup", array("gid" => group.getGID()));
			});
			groupManager.listen(".OC.Group", "preAddUser", function (.OC.Group.Group group, .OC.User.User user) {
				.OC_Hook::emit("OC_Group", "pre_addToGroup", array("run" => true, "uid" => user.getUID(), "gid" => group.getGID()));
			});
			groupManager.listen(".OC.Group", "postAddUser", function (.OC.Group.Group group, .OC.User.User user) {
				.OC_Hook::emit("OC_Group", "post_addToGroup", array("uid" => user.getUID(), "gid" => group.getGID()));
				//Minimal fix to keep it backward compatible TODO: clean up all the GroupManager hooks
				.OC_Hook::emit("OC_User", "post_addToGroup", array("uid" => user.getUID(), "gid" => group.getGID()));
			});
			return groupManager;
		});
		this.registerAlias("GroupManager", .OCP.IGroupManager::class);

		this.registerService(Store::class, function (Server c) {
			session = c.getSession();
			if (.OC::server.getSystemConfig().getValue("installed", false)) {
				tokenProvider = c.query(IProvider::class);
			} else {
				tokenProvider = null;
			}
			logger = c.getLogger();
			return new Store(session, logger, tokenProvider);
		});
		this.registerAlias(IStore::class, Store::class);
		this.registerService(Authentication.Token.DefaultTokenMapper::class, function (Server c) {
			dbConnection = c.getDatabaseConnection();
			return new Authentication.Token.DefaultTokenMapper(dbConnection);
		});
		this.registerAlias(IProvider::class, Authentication.Token.Manager::class);

		this.registerService(.OC.User.Session::class, function (Server c) {
			manager = c.getUserManager();
			session = new .OC.Session.Memory("");
			timeFactory = new TimeFactory();
			// Token providers might require a working database. This code
			// might however be called when ownCloud is not yet setup.
			if (.OC::server.getSystemConfig().getValue("installed", false)) {
				defaultTokenProvider = c.query(IProvider::class);
			} else {
				defaultTokenProvider = null;
			}

			dispatcher = c.getEventDispatcher();

			userSession = new .OC.User.Session(
				manager,
				session,
				timeFactory,
				defaultTokenProvider,
				c.getConfig(),
				c.getSecureRandom(),
				c.getLockdownManager(),
				c.getLogger()
			);
			userSession.listen(".OC.User", "preCreateUser", function (uid, password) {
				.OC_Hook::emit("OC_User", "pre_createUser", array("run" => true, "uid" => uid, "password" => password));
			});
			userSession.listen(".OC.User", "postCreateUser", function (user, password) {
				/** @var user .OC.User.User */
				.OC_Hook::emit("OC_User", "post_createUser", array("uid" => user.getUID(), "password" => password));
			});
			userSession.listen(".OC.User", "preDelete", function (user) use (dispatcher) {
				/** @var user .OC.User.User */
				.OC_Hook::emit("OC_User", "pre_deleteUser", array("run" => true, "uid" => user.getUID()));
				dispatcher.dispatch("OCP.IUser::preDelete", new GenericEvent(user));
			});
			userSession.listen(".OC.User", "postDelete", function (user) {
				/** @var user .OC.User.User */
				.OC_Hook::emit("OC_User", "post_deleteUser", array("uid" => user.getUID()));
			});
			userSession.listen(".OC.User", "preSetPassword", function (user, password, recoveryPassword) {
				/** @var user .OC.User.User */
				.OC_Hook::emit("OC_User", "pre_setPassword", array("run" => true, "uid" => user.getUID(), "password" => password, "recoveryPassword" => recoveryPassword));
			});
			userSession.listen(".OC.User", "postSetPassword", function (user, password, recoveryPassword) {
				/** @var user .OC.User.User */
				.OC_Hook::emit("OC_User", "post_setPassword", array("run" => true, "uid" => user.getUID(), "password" => password, "recoveryPassword" => recoveryPassword));
			});
			userSession.listen(".OC.User", "preLogin", function (uid, password) {
				.OC_Hook::emit("OC_User", "pre_login", array("run" => true, "uid" => uid, "password" => password));
			});
			userSession.listen(".OC.User", "postLogin", function (user, password, isTokenLogin) {
				/** @var user .OC.User.User */
				.OC_Hook::emit("OC_User", "post_login", array("run" => true, "uid" => user.getUID(), "password" => password, "isTokenLogin" => isTokenLogin));
			});
			userSession.listen(".OC.User", "postRememberedLogin", function (user, password) {
				/** @var user .OC.User.User */
				.OC_Hook::emit("OC_User", "post_login", array("run" => true, "uid" => user.getUID(), "password" => password));
			});
			userSession.listen(".OC.User", "logout", function () {
				.OC_Hook::emit("OC_User", "logout", array());
			});
			userSession.listen(".OC.User", "changeUser", function (user, feature, value, oldValue) use (dispatcher) {
				/** @var user .OC.User.User */
				.OC_Hook::emit("OC_User", "changeUser", array("run" => true, "user" => user, "feature" => feature, "value" => value, "old_value" => oldValue));
				dispatcher.dispatch("OCP.IUser::changeUser", new GenericEvent(user, ["feature" => feature, "oldValue" => oldValue, "value" => value]));
			});
			return userSession;
		});
		this.registerAlias(.OCP.IUserSession::class, .OC.User.Session::class);
		this.registerAlias("UserSession", .OC.User.Session::class);

		this.registerAlias(.OCP.Authentication.TwoFactorAuth.IRegistry::class, .OC.Authentication.TwoFactorAuth.Registry::class);

		this.registerAlias(.OCP.INavigationManager::class, .OC.NavigationManager::class);
		this.registerAlias("NavigationManager", .OCP.INavigationManager::class);

		this.registerService(.OC.AllConfig::class, function (Server c) {
			return new .OC.AllConfig(
				c.getSystemConfig()
			);
		});
		this.registerAlias("AllConfig", .OC.AllConfig::class);
		this.registerAlias(.OCP.IConfig::class, .OC.AllConfig::class);

		this.registerService("SystemConfig", function (c) use (config) {
			return new .OC.SystemConfig(config);
		});

		this.registerService(.OC.AppConfig::class, function (Server c) {
			return new .OC.AppConfig(c.getDatabaseConnection());
		});
		this.registerAlias("AppConfig", .OC.AppConfig::class);
		this.registerAlias(.OCP.IAppConfig::class, .OC.AppConfig::class);

		this.registerService(.OCP.L10N.IFactory::class, function (Server c) {
			return new .OC.L10N.Factory(
				c.getConfig(),
				c.getRequest(),
				c.getUserSession(),
				.OC::SERVERROOT
			);
		});
		this.registerAlias("L10NFactory", .OCP.L10N.IFactory::class);

		this.registerService(.OCP.IURLGenerator::class, function (Server c) {
			config = c.getConfig();
			cacheFactory = c.getMemCacheFactory();
			request = c.getRequest();
			return new .OC.URLGenerator(
				config,
				cacheFactory,
				request
			);
		});
		this.registerAlias("URLGenerator", .OCP.IURLGenerator::class);

		this.registerAlias("AppFetcher", AppFetcher::class);
		this.registerAlias("CategoryFetcher", CategoryFetcher::class);

		this.registerService(.OCP.ICache::class, function (c) {
			return new Cache.File();
		});
		this.registerAlias("UserCache", .OCP.ICache::class);

		this.registerService(Factory::class, function (Server c) {

			arrayCacheFactory = new .OC.Memcache.Factory("", c.getLogger(),
				ArrayCache::class,
				ArrayCache::class,
				ArrayCache::class
			);
			config = c.getConfig();

			if (config.getSystemValue("installed", false) && !(defined("PHPUNIT_RUN") && PHPUNIT_RUN)) {
				v = .OC_App::getAppVersions();
				v["core"] = implode(",", .OC_Util::getVersion());
				version = implode(",", v);
				instanceId = .OC_Util::getInstanceId();
				path = .OC::SERVERROOT;
				prefix = md5(instanceId . "-" . version . "-" . path);
				return new .OC.Memcache.Factory(prefix, c.getLogger(),
					config.getSystemValue("memcache.local", null),
					config.getSystemValue("memcache.distributed", null),
					config.getSystemValue("memcache.locking", null)
				);
			}
			return arrayCacheFactory;

		});
		this.registerAlias("MemCacheFactory", Factory::class);
		this.registerAlias(ICacheFactory::class, Factory::class);

		this.registerService("RedisFactory", function (Server c) {
			systemConfig = c.getSystemConfig();
			return new RedisFactory(systemConfig);
		});

		this.registerService(.OCP.Activity.IManager::class, function (Server c) {
			return new .OC.Activity.Manager(
				c.getRequest(),
				c.getUserSession(),
				c.getConfig(),
				c.query(IValidator::class)
			);
		});
		this.registerAlias("ActivityManager", .OCP.Activity.IManager::class);

		this.registerService(.OCP.Activity.IEventMerger::class, function (Server c) {
			return new .OC.Activity.EventMerger(
				c.getL10N("lib")
			);
		});
		this.registerAlias(IValidator::class, Validator::class);

		this.registerService(AvatarManager::class, function(Server c) {
			return new AvatarManager(
				c.query(.OC.User.Manager::class),
				c.getAppDataDir("avatar"),
				c.getL10N("lib"),
				c.getLogger(),
				c.getConfig()
			);
		});
		this.registerAlias(.OCP.IAvatarManager::class, AvatarManager::class);
		this.registerAlias("AvatarManager", AvatarManager::class);

		this.registerAlias(.OCP.Support.CrashReport.IRegistry::class, .OC.Support.CrashReport.Registry::class);

		this.registerService(.OC.Log::class, function (Server c) {
			logType = c.query("AllConfig").getSystemValue("log_type", "file");
			factory = new LogFactory(c, this.getSystemConfig());
			logger = factory.get(logType);
			registry = c.query(.OCP.Support.CrashReport.IRegistry::class);

			return new Log(logger, this.getSystemConfig(), null, registry);
		});
		this.registerAlias(.OCP.ILogger::class, .OC.Log::class);
		this.registerAlias("Logger", .OC.Log::class);

		this.registerService(ILogFactory::class, function (Server c) {
			return new LogFactory(c, this.getSystemConfig());
		});

		this.registerService(.OCP.BackgroundJob.IJobList::class, function (Server c) {
			config = c.getConfig();
			return new .OC.BackgroundJob.JobList(
				c.getDatabaseConnection(),
				config,
				new TimeFactory()
			);
		});
		this.registerAlias("JobList", .OCP.BackgroundJob.IJobList::class);

		this.registerService(.OCP.Route.IRouter::class, function (Server c) {
			cacheFactory = c.getMemCacheFactory();
			logger = c.getLogger();
			if (cacheFactory.isLocalCacheAvailable()) {
				router = new .OC.Route.CachingRouter(cacheFactory.createLocal("route"), logger);
			} else {
				router = new .OC.Route.Router(logger);
			}
			return router;
		});
		this.registerAlias("Router", .OCP.Route.IRouter::class);

		this.registerService(.OCP.ISearch::class, function (c) {
			return new Search();
		});
		this.registerAlias("Search", .OCP.ISearch::class);

		this.registerService(.OC.Security.RateLimiting.Limiter::class, function (Server c) {
			return new .OC.Security.RateLimiting.Limiter(
				this.getUserSession(),
				this.getRequest(),
				new .OC.AppFramework.Utility.TimeFactory(),
				c.query(.OC.Security.RateLimiting.Backend.IBackend::class)
			);
		});
		this.registerService(.OC.Security.RateLimiting.Backend.IBackend::class, function (c) {
			return new .OC.Security.RateLimiting.Backend.MemoryCache(
				this.getMemCacheFactory(),
				new .OC.AppFramework.Utility.TimeFactory()
			);
		});

		this.registerService(.OCP.Security.ISecureRandom::class, function (c) {
			return new SecureRandom();
		});
		this.registerAlias("SecureRandom", .OCP.Security.ISecureRandom::class);

		this.registerService(.OCP.Security.ICrypto::class, function (Server c) {
			return new Crypto(c.getConfig(), c.getSecureRandom());
		});
		this.registerAlias("Crypto", .OCP.Security.ICrypto::class);

		this.registerService(.OCP.Security.IHasher::class, function (Server c) {
			return new Hasher(c.getConfig());
		});
		this.registerAlias("Hasher", .OCP.Security.IHasher::class);

		this.registerService(.OCP.Security.ICredentialsManager::class, function (Server c) {
			return new CredentialsManager(c.getCrypto(), c.getDatabaseConnection());
		});
		this.registerAlias("CredentialsManager", .OCP.Security.ICredentialsManager::class);

		this.registerService(IDBConnection::class, function (Server c) {
			systemConfig = c.getSystemConfig();
			factory = new .OC.DB.ConnectionFactory(systemConfig);
			type = systemConfig.getValue("dbtype", "sqlite");
			if (!factory.isValidType(type)) {
				throw new .OC.DatabaseException("Invalid database type");
			}
			connectionParams = factory.createConnectionParams();
			connection = factory.getConnection(type, connectionParams);
			connection.getConfiguration().setSQLLogger(c.getQueryLogger());
			return connection;
		});
		this.registerAlias("DatabaseConnection", IDBConnection::class);


		this.registerService(.OCP.WebRequestMethods.Http Client.IClientService::class, function (Server c) {
			user = .OC_User::getUser();
			uid = user ? user : null;
			return new ClientService(
				c.getConfig(),
				new .OC.Security.CertificateManager(
					uid,
					new View(),
					c.getConfig(),
					c.getLogger(),
					c.getSecureRandom()
				)
			);
		});
		this.registerAlias("HttpClientService", .OCP.WebRequestMethods.Http Client.IClientService::class);
		this.registerService(.OCP.Diagnostics.IEventLogger::class, function (Server c) {
			eventLogger = new EventLogger();
			if (c.getSystemConfig().getValue("debug", false)) {
				// In debug mode, module is being activated by default
				eventLogger.activate();
			}
			return eventLogger;
		});
		this.registerAlias("EventLogger", .OCP.Diagnostics.IEventLogger::class);

		this.registerService(.OCP.Diagnostics.IQueryLogger::class, function (Server c) {
			queryLogger = new QueryLogger();
			if (c.getSystemConfig().getValue("debug", false)) {
				// In debug mode, module is being activated by default
				queryLogger.activate();
			}
			return queryLogger;
		});
		this.registerAlias("QueryLogger", .OCP.Diagnostics.IQueryLogger::class);

		this.registerService(TempManager::class, function (Server c) {
			return new TempManager(
				c.getLogger(),
				c.getConfig()
			);
		});
		this.registerAlias("TempManager", TempManager::class);
		this.registerAlias(ITempManager::class, TempManager::class);

		this.registerService(AppManager::class, function (Server c) {
			return new .OC.App.AppManager(
				c.getUserSession(),
				c.query(.OC.AppConfig::class),
				c.getGroupManager(),
				c.getMemCacheFactory(),
				c.getEventDispatcher()
			);
		});
		this.registerAlias("AppManager", AppManager::class);
		this.registerAlias(IAppManager::class, AppManager::class);

		this.registerService(.OCP.IDateTimeZone::class, function (Server c) {
			return new DateTimeZone(
				c.getConfig(),
				c.getSession()
			);
		});
		this.registerAlias("DateTimeZone", .OCP.IDateTimeZone::class);

		this.registerService(.OCP.IDateTimeFormatter::class, function (Server c) {
			language = c.getConfig().getUserValue(c.getSession().get("user_id"), "core", "lang", null);

			return new DateTimeFormatter(
				c.getDateTimeZone().getTimeZone(),
				c.getL10N("lib", language)
			);
		});
		this.registerAlias("DateTimeFormatter", .OCP.IDateTimeFormatter::class);

		this.registerService(.OCP.Files.Config.IUserMountCache::class, function (Server c) {
			mountCache = new UserMountCache(c.getDatabaseConnection(), c.getUserManager(), c.getLogger());
			listener = new UserMountCacheListener(mountCache);
			listener.listen(c.getUserManager());
			return mountCache;
		});
		this.registerAlias("UserMountCache", .OCP.Files.Config.IUserMountCache::class);

		this.registerService(.OCP.Files.Config.IMountProviderCollection::class, function (Server c) {
			loader = .OC.Files.Filesystem::getLoader();
			mountCache = c.query("UserMountCache");
			manager = new .OC.Files.Config.MountProviderCollection(loader, mountCache);

			// builtin providers

			config = c.getConfig();
			manager.registerProvider(new CacheMountProvider(config));
			manager.registerHomeProvider(new LocalHomeMountProvider());
			manager.registerHomeProvider(new ObjectHomeMountProvider(config));

			return manager;
		});
		this.registerAlias("MountConfigManager", .OCP.Files.Config.IMountProviderCollection::class);

		this.registerService("IniWrapper", function (c) {
			return new IniGetWrapper();
		});
		this.registerService("AsyncCommandBus", function (Server c) {
			busClass = c.getConfig().getSystemValue("commandbus");
			if (busClass) {
				list(app, class) = explode("::", busClass, 2);
				if (c.getAppManager().isInstalled(app)) {
					.OC_App::loadApp(app);
					return c.query(class);
				} else {
					throw new ServiceUnavailableException("The app providing the command bus (app) is not enabled");
				}
			} else {
				jobList = c.getJobList();
				return new CronBus(jobList);
			}
		});
		this.registerService("TrustedDomainHelper", function (c) {
			return new TrustedDomainHelper(this.getConfig());
		});
		this.registerService(Throttler::class, function (Server c) {
			return new Throttler(
				c.getDatabaseConnection(),
				new TimeFactory(),
				c.getLogger(),
				c.getConfig()
			);
		});
		this.registerAlias("Throttler", Throttler::class);
		this.registerService("IntegrityCodeChecker", function (Server c) {
			// IConfig and IAppManager requires a working database. This code
			// might however be called when ownCloud is not yet setup.
			if (.OC::server.getSystemConfig().getValue("installed", false)) {
				config = c.getConfig();
				appManager = c.getAppManager();
			} else {
				config = null;
				appManager = null;
			}

			return new Checker(
				new EnvironmentHelper(),
				new FileAccessHelper(),
				new AppLocator(),
				config,
				c.getMemCacheFactory(),
				appManager,
				c.getTempManager()
			);
		});
		this.registerService(.OCP.IRequest::class, function (c) {
			if (isset(this["urlParams"])) {
				urlParams = this["urlParams"];
			} else {
				urlParams = [];
			}

			if (defined("PHPUNIT_RUN") && PHPUNIT_RUN
				&& in_array("fakeinput", stream_get_wrappers())
			) {
				stream = "fakeinput://data";
			} else {
				stream = "php://input";
			}

			return new Request(
				[
					"get" => _GET,
					"post" => _POST,
					"files" => _FILES,
					"server" => _SERVER,
					"env" => _ENV,
					"cookies" => _COOKIE,
					"method" => (isset(_SERVER) && isset(_SERVER["REQUEST_METHOD"]))
						? _SERVER["REQUEST_METHOD"]
						: "",
					"urlParams" => urlParams,
				],
				this.getSecureRandom(),
				this.getConfig(),
				this.getCsrfTokenManager(),
				stream
			);
		});
		this.registerAlias("Request", .OCP.IRequest::class);

		this.registerService(.OCP.Mail.IMailer::class, function (Server c) {
			return new Mailer(
				c.getConfig(),
				c.getLogger(),
				c.query(Defaults::class),
				c.getURLGenerator(),
				c.getL10N("lib")
			);
		});
		this.registerAlias("Mailer", .OCP.Mail.IMailer::class);

		this.registerService("LDAPProvider", function (Server c) {
			config = c.getConfig();
			factoryClass = config.getSystemValue("ldapProviderFactory", null);
			if (is_null(factoryClass)) {
				throw new .Exception("ldapProviderFactory not set");
			}
			/** @var .OCP.LDAP.ILDAPProviderFactory factory */
			factory = new factoryClass(this);
			return factory.getLDAPProvider();
		});
		this.registerService(ILockingProvider::class, function (Server c) {
			ini = c.getIniWrapper();
			config = c.getConfig();
			ttl = config.getSystemValue("filelocking.ttl", max(3600, ini.getNumeric("max_execution_time")));
			if (config.getSystemValue("filelocking.enabled", true) or (defined("PHPUNIT_RUN") && PHPUNIT_RUN)) {
				/** @var .OC.Memcache.Factory memcacheFactory */
				memcacheFactory = c.getMemCacheFactory();
				memcache = memcacheFactory.createLocking("lock");
				if (!(memcache instanceof .OC.Memcache.NullCache)) {
					return new MemcacheLockingProvider(memcache, ttl);
				}
				return new DBLockingProvider(
					c.getDatabaseConnection(),
					c.getLogger(),
					new TimeFactory(),
					ttl,
					!.OC::CLI
				);
			}
			return new NoopLockingProvider();
		});
		this.registerAlias("LockingProvider", ILockingProvider::class);

		this.registerService(.OCP.Files.Mount.IMountManager::class, function () {
			return new .OC.Files.Mount.Manager();
		});
		this.registerAlias("MountManager", .OCP.Files.Mount.IMountManager::class);

		this.registerService(.OCP.Files.IMimeTypeDetector::class, function (Server c) {
			return new .OC.Files.Type.Detection(
				c.getURLGenerator(),
				.OC::configDir,
				.OC::SERVERROOT . "/resources/config/"
			);
		});
		this.registerAlias("MimeTypeDetector", .OCP.Files.IMimeTypeDetector::class);

		this.registerService(.OCP.Files.IMimeTypeLoader::class, function (Server c) {
			return new .OC.Files.Type.Loader(
				c.getDatabaseConnection()
			);
		});
		this.registerAlias("MimeTypeLoader", .OCP.Files.IMimeTypeLoader::class);
		this.registerService(BundleFetcher::class, function () {
			return new BundleFetcher(this.getL10N("lib"));
		});
		this.registerService(.OCP.Notification.IManager::class, function (Server c) {
			return new Manager(
				c.query(IValidator::class)
			);
		});
		this.registerAlias("NotificationManager", .OCP.Notification.IManager::class);

		this.registerService(.OC.CapabilitiesManager::class, function (Server c) {
			manager = new .OC.CapabilitiesManager(c.getLogger());
			manager.registerCapability(function () use (c) {
				return new .OC.OCS.CoreCapabilities(c.getConfig());
			});
			manager.registerCapability(function () use (c) {
				return c.query(.OC.Security.Bruteforce.Capabilities::class);
			});
			return manager;
		});
		this.registerAlias("CapabilitiesManager", .OC.CapabilitiesManager::class);

		this.registerService(.OCP.Comments.ICommentsManager::class, function (Server c) {
			config = c.getConfig();
			factoryClass = config.getSystemValue("comments.managerFactory", CommentsManagerFactory::class);
			/** @var .OCP.Comments.ICommentsManagerFactory factory */
			factory = new factoryClass(this);
			manager = factory.getManager();

			manager.registerDisplayNameResolver("user", function(id) use (c) {
				manager = c.getUserManager();
				user = manager.get(id);
				if(is_null(user)) {
					l = c.getL10N("core");
					displayName = l.t("Unknown user");
				} else {
					displayName = user.getDisplayName();
				}
				return displayName;
			});

			return manager;
		});
		this.registerAlias("CommentsManager", .OCP.Comments.ICommentsManager::class);

		this.registerService("ThemingDefaults", function (Server c) {
			/*
			 * Dark magic for autoloader.
			 * If we do a class_exists it will try to load the class which will
			 * make composer cache the result. Resulting in errors when enabling
			 * the theming app.
			 */
			prefixes = .OC::composerAutoloader.getPrefixesPsr4();
			if (isset(prefixes["OCA..Theming.."])) {
				classExists = true;
			} else {
				classExists = false;
			}

			if (classExists && c.getConfig().getSystemValue("installed", false) && c.getAppManager().isInstalled("theming") && c.getTrustedDomainHelper().isTrustedDomain(c.getRequest().getInsecureServerHost())) {
				return new ThemingDefaults(
					c.getConfig(),
					c.getL10N("theming"),
					c.getURLGenerator(),
					c.getMemCacheFactory(),
					new Util(c.getConfig(), this.getAppManager(), c.getAppDataDir("theming")),
					new ImageManager(c.getConfig(), c.getAppDataDir("theming"), c.getURLGenerator(), this.getMemCacheFactory(), this.getLogger()),
					c.getAppManager(),
					c.getNavigationManager()
				);
			}
			return new .OC_Defaults();
		});
		this.registerService(SCSSCacher::class, function (Server c) {
			return new SCSSCacher(
				c.getLogger(),
				c.query(.OC.Files.AppData.Factory::class),
				c.getURLGenerator(),
				c.getConfig(),
				c.getThemingDefaults(),
				.OC::SERVERROOT,
				this.getMemCacheFactory(),
				c.query(IconsCacher::class),
				new TimeFactory()
			);
		});
		this.registerService(JSCombiner::class, function (Server c) {
			return new JSCombiner(
				c.getAppDataDir("js"),
				c.getURLGenerator(),
				this.getMemCacheFactory(),
				c.getSystemConfig(),
				c.getLogger()
			);
		});
		this.registerService(EventDispatcher::class, function () {
			return new EventDispatcher();
		});
		this.registerAlias("EventDispatcher", EventDispatcher::class);
		this.registerAlias(EventDispatcherInterface::class, EventDispatcher::class);

		this.registerService("CryptoWrapper", function (Server c) {
			// FIXME: Instantiiated here due to cyclic dependency
			request = new Request(
				[
					"get" => _GET,
					"post" => _POST,
					"files" => _FILES,
					"server" => _SERVER,
					"env" => _ENV,
					"cookies" => _COOKIE,
					"method" => (isset(_SERVER) && isset(_SERVER["REQUEST_METHOD"]))
						? _SERVER["REQUEST_METHOD"]
						: null,
				],
				c.getSecureRandom(),
				c.getConfig()
			);

			return new CryptoWrapper(
				c.getConfig(),
				c.getCrypto(),
				c.getSecureRandom(),
				request
			);
		});
		this.registerService("CsrfTokenManager", function (Server c) {
			tokenGenerator = new CsrfTokenGenerator(c.getSecureRandom());

			return new CsrfTokenManager(
				tokenGenerator,
				c.query(SessionStorage::class)
			);
		});
		this.registerService(SessionStorage::class, function (Server c) {
			return new SessionStorage(c.getSession());
		});
		this.registerService(.OCP.Security.IContentSecurityPolicyManager::class, function (Server c) {
			return new ContentSecurityPolicyManager();
		});
		this.registerAlias("ContentSecurityPolicyManager", .OCP.Security.IContentSecurityPolicyManager::class);

		this.registerService("ContentSecurityPolicyNonceManager", function (Server c) {
			return new ContentSecurityPolicyNonceManager(
				c.getCsrfTokenManager(),
				c.getRequest()
			);
		});

		this.registerService(.OCP.Share.IManager::class, function (Server c) {
			config = c.getConfig();
			factoryClass = config.getSystemValue("sharing.managerFactory", ProviderFactory::class);
			/** @var .OCP.Share.IProviderFactory factory */
			factory = new factoryClass(this);

			manager = new .OC.Share20.Manager(
				c.getLogger(),
				c.getConfig(),
				c.getSecureRandom(),
				c.getHasher(),
				c.getMountManager(),
				c.getGroupManager(),
				c.getL10N("lib"),
				c.getL10NFactory(),
				factory,
				c.getUserManager(),
				c.getLazyRootFolder(),
				c.getEventDispatcher(),
				c.getMailer(),
				c.getURLGenerator(),
				c.getThemingDefaults()
			);

			return manager;
		});
		this.registerAlias("ShareManager", .OCP.Share.IManager::class);

		this.registerService(.OCP.Collaboration.Collaborators.ISearch::class, function(Server c) {
			instance = new Collaboration.Collaborators.Search(c);

			// register default plugins
			instance.registerPlugin(["shareType" => "SHARE_TYPE_USER", "class" => UserPlugin::class]);
			instance.registerPlugin(["shareType" => "SHARE_TYPE_GROUP", "class" => GroupPlugin::class]);
			instance.registerPlugin(["shareType" => "SHARE_TYPE_EMAIL", "class" => MailPlugin::class]);
			instance.registerPlugin(["shareType" => "SHARE_TYPE_REMOTE", "class" => RemotePlugin::class]);
			instance.registerPlugin(["shareType" => "SHARE_TYPE_REMOTE_GROUP", "class" => RemoteGroupPlugin::class]);

			return instance;
		});
		this.registerAlias("CollaboratorSearch", .OCP.Collaboration.Collaborators.ISearch::class);
		this.registerAlias(.OCP.Collaboration.Collaborators.ISearchResult::class, .OC.Collaboration.Collaborators.SearchResult::class);

		this.registerAlias(.OCP.Collaboration.AutoComplete.IManager::class, .OC.Collaboration.AutoComplete.Manager::class);

		this.registerAlias(.OCP.Collaboration.Resources.IManager::class, .OC.Collaboration.Resources.Manager::class);

		this.registerService("SettingsManager", function (Server c) {
			manager = new .OC.Settings.Manager(
				c.getLogger(),
				c.getL10N("lib"),
				c.getURLGenerator(),
				c
			);
			return manager;
		});
		this.registerService(.OC.Files.AppData.Factory::class, function (Server c) {
			return new .OC.Files.AppData.Factory(
				c.getRootFolder(),
				c.getSystemConfig()
			);
		});

		this.registerService("LockdownManager", function (Server c) {
			return new LockdownManager(function () use (c) {
				return c.getSession();
			});
		});

		this.registerService(.OCP.OCS.IDiscoveryService::class, function (Server c) {
			return new DiscoveryService(c.getMemCacheFactory(), c.getHTTPClientService());
		});

		this.registerService(ICloudIdManager::class, function (Server c) {
			return new CloudIdManager();
		});

		this.registerService(IConfig::class, function (Server c) {
			return new GlobalScale.Config(c.getConfig());
		});

		this.registerService(ICloudFederationProviderManager::class, function (Server c) {
			return new CloudFederationProviderManager(c.getAppManager(), c.getHTTPClientService(), c.getCloudIdManager(), c.getLogger());
		});

		this.registerService(ICloudFederationFactory::class, function (Server c) {
			return new CloudFederationFactory();
		});

		this.registerAlias(.OCP.AppFramework.Utility.IControllerMethodReflector::class, .OC.AppFramework.Utility.ControllerMethodReflector::class);
		this.registerAlias("ControllerMethodReflector", .OCP.AppFramework.Utility.IControllerMethodReflector::class);

		this.registerAlias(.OCP.AppFramework.Utility.ITimeFactory::class, .OC.AppFramework.Utility.TimeFactory::class);
		this.registerAlias("TimeFactory", .OCP.AppFramework.Utility.ITimeFactory::class);

		this.registerService(Defaults::class, function (Server c) {
			return new Defaults(
				c.getThemingDefaults()
			);
		});
		this.registerAlias("Defaults", .OCP.Defaults::class);

		this.registerService(.OCP.ISession::class, function (SimpleContainer c) {
			return c.query(.OCP.IUserSession::class).getSession();
		});

		this.registerService(IShareHelper::class, function (Server c) {
			return new ShareHelper(
				c.query(.OCP.Share.IManager::class)
			);
		});

		this.registerService(Installer::class, function(Server c) {
			return new Installer(
				c.getAppFetcher(),
				c.getHTTPClientService(),
				c.getTempManager(),
				c.getLogger(),
				c.getConfig()
			);
		});

		this.registerService(IApiFactory::class, function(Server c) {
			return new ApiFactory(c.getHTTPClientService());
		});

		this.registerService(IInstanceFactory::class, function(Server c) {
			memcacheFactory = c.getMemCacheFactory();
			return new InstanceFactory(memcacheFactory.createLocal("remoteinstance."), c.getHTTPClientService());
		});

		this.registerService(IContactsStore::class, function(Server c) {
			return new ContactsStore(
				c.getContactsManager(),
				c.getConfig(),
				c.getUserManager(),
				c.getGroupManager()
			);
		});
		this.registerAlias(IContactsStore::class, ContactsStore::class);
		this.registerAlias(IAccountManager::class, AccountManager::class);

		this.registerService(IStorageFactory::class, function() {
			return new StorageFactory();
		});

		this.registerAlias(IDashboardManager::class, DashboardManager::class);
		this.registerAlias(IFullTextSearchManager::class, FullTextSearchManager::class);

		this.registerService(.OC.Security.IdentityProof.Manager::class, function (Server c) {
			return new .OC.Security.IdentityProof.Manager(
				c.query(.OC.Files.AppData.Factory::class),
				c.getCrypto(),
				c.getConfig()
			);
		});

		this.registerAlias(ISubAdmin::class, SubAdmin::class);

		this.registerAlias(IInitialStateService::class, InitialStateService::class);

		this.connectDispatcher();
	}

	/**
	 * @return .OCP.Calendar.IManager
	 */
	public OCP.Calendar.IManager getCalendarManager() {
		return this.query("CalendarManager");
	}

	/**
	 * @return .OCP.Calendar.Resource.IManager
	 */
	public OCP.Calendar.Resource.IManager getCalendarResourceBackendManager() {
		return this.query("CalendarResourceBackendManager");
	}

	/**
	 * @return .OCP.Calendar.Room.IManager
	 */
	public OCP.Calendar.Room.IManager getCalendarRoomBackendManager() {
		return this.query("CalendarRoomBackendManager");
	}

	private void connectDispatcher() {
		dispatcher = this.getEventDispatcher();

		// Delete avatar on user deletion
		dispatcher.addListener("OCP.IUser::preDelete", function(GenericEvent e) {
			logger = this.getLogger();
			manager = this.getAvatarManager();
			/** @var IUser user */
			user = e.getSubject();

			try {
				avatar = manager.getAvatar(user.getUID());
				avatar.remove();
			} catch (NotFoundException e) {
				// no avatar to remove
			} catch (.Exception e) {
				// Ignore exceptions
				logger.info("Could not cleanup avatar of " . user.getUID());
			}
		});

		dispatcher.addListener("OCP.IUser::changeUser", function (GenericEvent e) {
			manager = this.getAvatarManager();
			/** @var IUser user */
			user = e.getSubject();
			feature = e.getArgument("feature");
			oldValue = e.getArgument("oldValue");
			value = e.getArgument("value");

			try {
				avatar = manager.getAvatar(user.getUID());
				avatar.userChanged(feature, oldValue, value);
			} catch (NotFoundException e) {
				// no avatar to remove
			}
		});
	}

	/**
	 * @return .OCP.Contacts.IManager
	 */
	public OCP.ContactsNs.IManager getContactsManager() {
		return this.query("ContactsManager");
	}

	/**
	 * @return .OC.Encryption.Manager
	 */
	public OC.Encryption.Manager getEncryptionManager() {
		return this.query("EncryptionManager");
	}

	/**
	 * @return .OC.Encryption.File
	 */
	public OC.Encryption.File getEncryptionFilesHelper() {
		return this.query("EncryptionFileHelper");
	}

	/**
	 * @return .OCP.Encryption.Keys.IStorage
	 */
	public OCP.Encryption.Keys.IStorage getEncryptionKeyStorage() {
		return this.query("EncryptionKeyStorage");
	}

	/**
	 * The current request object holding all information about the request
	 * currently being processed is returned from this method.
	 * In case the current execution was not initiated by a web request null is returned
	 *
	 * @return .OCP.IRequest
	 */
	public OCP.IRequest getRequest() {
		return this.query("Request");
	}

	/**
	 * Returns the preview manager which can create preview images for a given file
	 *
	 * @return .OCP.IPreview
	 */
	public OCP.IPreview getPreviewManager() {
		return this.query("PreviewManager");
	}

	/**
	 * Returns the tag manager which can get and set tags for different object types
	 *
	 * @see .OCP.ITagManager::load()
	 * @return .OCP.ITagManager
	 */
	public OCP.ITagManager getTagManager() {
		return this.query("TagManager");
	}

	/**
	 * Returns the system-tag manager
	 *
	 * @return .OCP.SystemTag.ISystemTagManager
	 *
	 * @since 9.0.0
	 */
	public OCP.SystemTag.ISystemTagManager getSystemTagManager() {
		return this.query("SystemTagManager");
	}

	/**
	 * Returns the system-tag object mapper
	 *
	 * @return .OCP.SystemTag.ISystemTagObjectMapper
	 *
	 * @since 9.0.0
	 */
	public OCP.SystemTag.ISystemTagObjectMapper getSystemTagObjectMapper() {
		return this.query("SystemTagObjectMapper");
	}

	/**
	 * Returns the avatar manager, used for avatar functionality
	 *
	 * @return .OCP.IAvatarManager
	 */
	public OCP.IAvatarManager getAvatarManager() {
		return this.query("AvatarManager");
	}

	/**
	 * Returns the root folder of ownCloud"s data directory
	 *
	 * @return .OCP.Files.IRootFolder
	 */
	public OCP.Files.IRootFolder getRootFolder() {
		return this.query("LazyRootFolder");
	}

	/**
	 * Returns the root folder of ownCloud"s data directory
	 * This is the lazy variant so this gets only initialized once it
	 * is actually used.
	 *
	 * @return .OCP.Files.IRootFolder
	 */
	public OCP.Files.IRootFolder getLazyRootFolder() {
		return this.query("LazyRootFolder");
	}

	/**
	 * Returns a view to ownCloud"s files folder
	 *
	 * @param string userId user ID
	 * @return .OCP.Files.Folder|null
	 */
	public OCP.Files.Folder? getUserFolder(string userId = null) {
		if (userId == null) {
			var user = this.getUserSession().getUser();
			if (!user) {
				return null;
			}
			userId = user.getUID();
		}
		var root = this.getRootFolder();
		return root.getUserFolder(userId);
	}

	/**
	 * Returns an app-specific view in ownClouds data directory
	 *
	 * @return .OCP.Files.Folder
	 * @deprecated since 9.2.0 use IAppData
	 */
	public OCP.Files.Folder getAppFolder() {
		dir = "/" . .OC_App::getCurrentApp();
		root = this.getRootFolder();
		if (!root.nodeExists(dir)) {
			folder = root.newFolder(dir);
		} else {
			folder = root.get(dir);
		}
		return folder;
	}

	/**
	 * @return .OC.User.Manager
	 */
	public OC.User.Manager getUserManager() {
		return this.query("UserManager");
	}

	/**
	 * @return .OC.Group.Manager
	 */
	public OC.Group.Manager getGroupManager() {
		return this.query("GroupManager");
	}

	/**
	 * @return .OC.User.Session
	 */
	public OC.User.Session getUserSession() {
		return this.query("UserSession");
	}

	/**
	 * @return .OCP.ISession
	 */
	public OCP.ISession getSession() {
		return this.query("UserSession").getSession();
	}

	/**
	 * @param .OCP.ISession session
	 */
	public OCP.ISession setSession(OCP.ISession session) {
		this.query(SessionStorage::class).setSession(session);
		this.query("UserSession").setSession(session);
		this.query(Store::class).setSession(session);
	}

	/**
	 * @return .OC.Authentication.TwoFactorAuth.Manager
	 */
	public OC.Authentication.TwoFactorAuth.Manager getTwoFactorAuthManager() {
		return this.query(".OC.Authentication.TwoFactorAuth.Manager");
	}

	/**
	 * @return .OC.NavigationManager
	 */
	public OC.NavigationManager getNavigationManager() {
		return this.query("NavigationManager");
	}

	/**
	 * @return .OCP.IConfig
	 */
	public OCP.IConfig getConfig() {
		return this.query("AllConfig");
	}

	/**
	 * @return .OC.SystemConfig
	 */
	public OC.SystemConfig getSystemConfig() {
		return this.query("SystemConfig");
	}

	/**
	 * Returns the app config manager
	 *
	 * @return .OCP.IAppConfig
	 */
	public OCP.IAppConfig getAppConfig() {
		return this.query("AppConfig");
	}

	/**
	 * @return .OCP.L10N.IFactory
	 */
	public OCP.L10N.IFactory getL10NFactory() {
		return this.query("L10NFactory");
	}

	/**
	 * get an L10N instance
	 *
	 * @param string app appid
	 * @param string lang
	 * @return IL10N
	 */
	public IL10N getL10N(string app, string lang = null) {
		return this.getL10NFactory().get(app, lang);
	}

	/**
	 * @return .OCP.IURLGenerator
	 */
	public OCP.IURLGenerator getURLGenerator() {
		return this.query("URLGenerator");
	}

	/**
	 * @return AppFetcher
	 */
	public AppFetcher getAppFetcher() {
		return this.query(AppFetcher::class);
	}

	/**
	 * Returns an ICache instance. Since 8.1.0 it returns a fake cache. Use
	 * getMemCacheFactory() instead.
	 *
	 * @return .OCP.ICache
	 * @deprecated 8.1.0 use getMemCacheFactory to obtain a proper cache
	 */
	public OCP.ICache getCache() {
		return this.query("UserCache");
	}

	/**
	 * Returns an .OCP.CacheFactory instance
	 *
	 * @return .OCP.ICacheFactory
	 */
	public OCP.ICacheFactory getMemCacheFactory() {
		return this.query("MemCacheFactory");
	}

	/**
	 * Returns an .OC.RedisFactory instance
	 *
	 * @return .OC.RedisFactory
	 */
	public OC.RedisFactory getGetRedisFactory() {
		return this.query("RedisFactory");
	}


	/**
	 * Returns the current session
	 *
	 * @return .OCP.IDBConnection
	 */
	public OCP.IDBConnection getDatabaseConnection() {
		return this.query("DatabaseConnection");
	}

	/**
	 * Returns the activity manager
	 *
	 * @return .OCP.Activity.IManager
	 */
	public OCP.Activity.IManager getActivityManager() {
		return this.query("ActivityManager");
	}

	/**
	 * Returns an job list for controlling background jobs
	 *
	 * @return .OCP.BackgroundJob.IJobList
	 */
	public OCP.BackgroundJob.IJobList getJobList() {
		return this.query("JobList");
	}

	/**
	 * Returns a logger instance
	 *
	 * @return .OCP.ILogger
	 */
	public OCP.ILogger getLogger() {
		return this.query("Logger");
	}

	/**
	 * @return ILogFactory
	 * @throws .OCP.AppFramework.QueryException
	 */
	public ILoggerFactory getLogFactory() {
		return this.query(ILogFactory::class);
	}

	/**
	 * Returns a router for generating and matching urls
	 *
	 * @return .OCP.Route.IRouter
	 */
	public OCP.Route.IRouter getRouter() {
		return this.query("Router");
	}

	/**
	 * Returns a search instance
	 *
	 * @return .OCP.ISearch
	 */
	public OCP.ISearch getSearch() {
		return this.query("Search");
	}

	/**
	 * Returns a SecureRandom instance
	 *
	 * @return .OCP.Security.ISecureRandom
	 */
	public function getSecureRandom() {
		return this.query("SecureRandom");
	}

	/**
	 * Returns a Crypto instance
	 *
	 * @return .OCP.Security.ICrypto
	 */
	public function getCrypto() {
		return this.query("Crypto");
	}

	/**
	 * Returns a Hasher instance
	 *
	 * @return .OCP.Security.IHasher
	 */
	public function getHasher() {
		return this.query("Hasher");
	}

	/**
	 * Returns a CredentialsManager instance
	 *
	 * @return .OCP.Security.ICredentialsManager
	 */
	public function getCredentialsManager() {
		return this.query("CredentialsManager");
	}

	/**
	 * Get the certificate manager for the user
	 *
	 * @param string userId (optional) if not specified the current loggedin user is used, use null to get the system certificate manager
	 * @return .OCP.ICertificateManager | null if uid is null and no user is logged in
	 */
	public OCP.ICertificateManager getCertificateManager(userId = "") {
		if (userId === "") {
			userSession = this.getUserSession();
			user = userSession.getUser();
			if (is_null(user)) {
				return null;
			}
			userId = user.getUID();
		}
		return new CertificateManager(
			userId,
			new View(),
			this.getConfig(),
			this.getLogger(),
			this.getSecureRandom()
		);
	}

	/**
	 * Returns an instance of the HTTP client service
	 *
	 * @return .OCP.Http.Client.IClientService
	 */
	public OCP.Http.Client.IClientService getHTTPClientService() {
		return this.query("HttpClientService");
	}

	/**
	 * Create a new event source
	 *
	 * @return .OCP.IEventSource
	 */
	public OCP.IEventSource createEventSource() {
		return new OC_EventSource();
	}

	/**
	 * Get the active event logger
	 *
	 * The returned logger only logs data when debug mode is enabled
	 *
	 * @return .OCP.Diagnostics.IEventLogger
	 */
	public OCP.Diagnostics.IEventLogger getEventLogger() {
		return this.query("EventLogger");
	}

	/**
	 * Get the active query logger
	 *
	 * The returned logger only logs data when debug mode is enabled
	 *
	 * @return .OCP.Diagnostics.IQueryLogger
	 */
	public OCP.Diagnostics.IQueryLogger getQueryLogger() {
		return this.query("QueryLogger");
	}

	/**
	 * Get the manager for temporary files and folders
	 *
	 * @return .OCP.ITempManager
	 */
	public OCP.ITempManager getTempManager() {
		return this.query("TempManager");
	}

	/**
	 * Get the app manager
	 *
	 * @return .OCP.App.IAppManager
	 */
	public OCP.App.IAppManager getAppManager() {
		return this.query("AppManager");
	}

	/**
	 * Creates a new mailer
	 *
	 * @return .OCP.Mail.IMailer
	 */
	public OCP.Mail.IMailer getMailer() {
		return this.query("Mailer");
	}

	/**
	 * Get the webroot
	 *
	 * @return string
	 */
	public string getWebRoot() {
		return this.webRoot;
	}

	/**
	 * @return .OC.OCSClient
	 */
	public OC.OCSClient getOcsClient() {
		return this.query("OcsClient");
	}

	/**
	 * @return .OCP.IDateTimeZone
	 */
	public OCP.IDateTimeZone getDateTimeZone() {
		return this.query("DateTimeZone");
	}

	/**
	 * @return .OCP.IDateTimeFormatter
	 */
	public OCP.IDateTimeFormatter getDateTimeFormatter() {
		return this.query("DateTimeFormatter");
	}

	/**
	 * @return .OCP.Files.Config.IMountProviderCollection
	 */
	public OCP.Files.Config.IMountProviderCollection getMountProviderCollection() {
		return this.query("MountConfigManager");
	}

	/**
	 * Get the IniWrapper
	 *
	 * @return IniGetWrapper
	 */
	public IniParser getIniWrapper() {
		return this.query("IniWrapper");
	}

	/**
	 * @return .OCP.Command.IBus
	 */
	public OCP.Command.IBus getCommandBus() {
		return this.query("AsyncCommandBus");
	}

	/**
	 * Get the trusted domain helper
	 *
	 * @return TrustedDomainHelper
	 */
	public TrustedDomainHelper getTrustedDomainHelper() {
		return this.query("TrustedDomainHelper");
	}

	/**
	 * Get the locking provider
	 *
	 * @return .OCP.Lock.ILockingProvider
	 * @since 8.1.0
	 */
	public OCP.Lock.ILockingProvider getLockingProvider() {
		return this.query("LockingProvider");
	}

	/**
	 * @return .OCP.Files.Mount.IMountManager
	 **/
	OCP.Files.Mount.IMountManager getMountManager() {
		return this.query("MountManager");
	}

	/** @return .OCP.Files.Config.IUserMountCache */
	OCP.Files.Config.IUserMountCache getUserMountCache() {
		return this.query("UserMountCache");
	}

	/**
	 * Get the MimeTypeDetector
	 *
	 * @return .OCP.Files.IMimeTypeDetector
	 */
	public OCP.Files.IMimeTypeDetector getMimeTypeDetector() {
		return this.query("MimeTypeDetector");
	}

	/**
	 * Get the MimeTypeLoader
	 *
	 * @return .OCP.Files.IMimeTypeLoader
	 */
	public OCP.Files.IMimeTypeLoader getMimeTypeLoader() {
		return this.query("MimeTypeLoader");
	}

	/**
	 * Get the manager of all the capabilities
	 *
	 * @return .OC.CapabilitiesManager
	 */
	public OC.CapabilitiesManager getCapabilitiesManager() {
		return this.query("CapabilitiesManager");
	}

	/**
	 * Get the EventDispatcher
	 *
	 * @return EventDispatcherInterface
	 * @since 8.2.0
	 */
	public function getEventDispatcher() {
		return this.query("EventDispatcher");
	}

	/**
	 * Get the Notification Manager
	 *
	 * @return .OCP.Notification.IManager
	 * @since 8.2.0
	 */
	public function getNotificationManager() {
		return this.query("NotificationManager");
	}

	/**
	 * @return .OCP.Comments.ICommentsManager
	 */
	public function getCommentsManager() {
		return this.query("CommentsManager");
	}

	/**
	 * @return .OCA.Theming.ThemingDefaults
	 */
	public function getThemingDefaults() {
		return this.query("ThemingDefaults");
	}

	/**
	 * @return .OC.IntegrityCheck.Checker
	 */
	public function getIntegrityCodeChecker() {
		return this.query("IntegrityCodeChecker");
	}

	/**
	 * @return .OC.Session.CryptoWrapper
	 */
	public function getSessionCryptoWrapper() {
		return this.query("CryptoWrapper");
	}

	/**
	 * @return CsrfTokenManager
	 */
	public function getCsrfTokenManager() {
		return this.query("CsrfTokenManager");
	}

	/**
	 * @return Throttler
	 */
	public function getBruteForceThrottler() {
		return this.query("Throttler");
	}

	/**
	 * @return IContentSecurityPolicyManager
	 */
	public function getContentSecurityPolicyManager() {
		return this.query("ContentSecurityPolicyManager");
	}

	/**
	 * @return ContentSecurityPolicyNonceManager
	 */
	public function getContentSecurityPolicyNonceManager() {
		return this.query("ContentSecurityPolicyNonceManager");
	}

	/**
	 * Not a public API as of 8.2, wait for 9.0
	 *
	 * @return .OCA.Files_External.Service.BackendService
	 */
	public function getStoragesBackendService() {
		return this.query("OCA..Files_External..Service..BackendService");
	}

	/**
	 * Not a public API as of 8.2, wait for 9.0
	 *
	 * @return .OCA.Files_External.Service.GlobalStoragesService
	 */
	public function getGlobalStoragesService() {
		return this.query("OCA..Files_External..Service..GlobalStoragesService");
	}

	/**
	 * Not a public API as of 8.2, wait for 9.0
	 *
	 * @return .OCA.Files_External.Service.UserGlobalStoragesService
	 */
	public function getUserGlobalStoragesService() {
		return this.query("OCA..Files_External..Service..UserGlobalStoragesService");
	}

	/**
	 * Not a public API as of 8.2, wait for 9.0
	 *
	 * @return .OCA.Files_External.Service.UserStoragesService
	 */
	public function getUserStoragesService() {
		return this.query("OCA..Files_External..Service..UserStoragesService");
	}

	/**
	 * @return .OCP.Share.IManager
	 */
	public OCP.ShareNS.IManager getShareManager() {
		return this.query("ShareManager");
	}

	/**
	 * @return .OCP.Collaboration.Collaborators.ISearch
	 */
	public function getCollaboratorSearch() {
		return this.query("CollaboratorSearch");
	}

	/**
	 * @return .OCP.Collaboration.AutoComplete.IManager
	 */
	public OCP.Collaboration.AutoComplete.IManager getAutoCompleteManager(){
		return this.query(IManager::class);
	}

	/**
	 * Returns the LDAP Provider
	 *
	 * @return .OCP.LDAP.ILDAPProvider
	 */
	public OCP.LDAP.ILDAPProvider getLDAPProvider() {
		return this.query("LDAPProvider");
	}

	/**
	 * @return .OCP.Settings.IManager
	 */
	public OCP.Settings.IManager getSettingsManager() {
		return this.query("SettingsManager");
	}

	/**
	 * @return .OCP.Files.IAppData
	 */
	public IAppData getAppDataDir(string app) {
		/** @var .OC.Files.AppData.Factory factory */
		factory = this.query(.OC.Files.AppData.Factory::class);
		return factory.get(app);
	}

	/**
	 * @return .OCP.Lockdown.ILockdownManager
	 */
	public OCP.Lockdown.ILockdownManager getLockdownManager() {
		return this.query("LockdownManager");
	}

	/**
	 * @return .OCP.Federation.ICloudIdManager
	 */
	public function getCloudIdManager() {
		return this.query(ICloudIdManager::class);
	}

	/**
	 * @return .OCP.GlobalScale.IConfig
	 */
	public function getGlobalScaleConfig() {
		return this.query(IConfig::class);
	}

	/**
	 * @return .OCP.Federation.ICloudFederationProviderManager
	 */
	public function getCloudFederationProviderManager() {
		return this.query(ICloudFederationProviderManager::class);
	}

	/**
	 * @return .OCP.Remote.Api.IApiFactory
	 */
	public OCP.Remote.Api.IApiFactory getRemoteApiFactory() {
		return this.query(IApiFactory);
	}

	/**
	 * @return .OCP.Federation.ICloudFederationFactory
	 */
	public function getCloudFederationFactory() {
		return this.query(ICloudFederationFactory::class);
	}

	/**
	 * @return .OCP.Remote.IInstanceFactory
	 */
	public OCP.Remote.IInstanceFactory getRemoteInstanceFactory() {
		return this.query(IInstanceFactory::class);
	}

	/**
	 * @return IStorageFactory
	 */
	public OCP.Files.Storage.IStorageFactory getStorageFactory() {
		return this.query(OCP.Files.Storage.IStorageFactory);
	}
}

}
