using System;
using System.Collections.Generic;
using System.Linq;
using OC.App;
using OC.Contacts.ContactsMenu.Providers;
using OCP;
using OCP.AppFramework;
using OCP.ContactsNs.ContactsMenu;

namespace OC.Contacts.ContactsMenu
{
    class ActionProviderStore {

        /** @var IServerContainer */
        private IServerContainer serverContainer;

        /** @var AppManager */
        private AppManager appManager;

        /** @var ILogger */
        private ILogger logger;

        /**
         * @param IServerContainer serverContainer
         * @param AppManager appManager
         * @param ILogger logger
         */
        public ActionProviderStore(IServerContainer serverContainer, AppManager appManager, ILogger logger) {
            this.serverContainer = serverContainer;
                this.appManager = appManager;
                this.logger = logger;
        }

        /**
         * @param IUser user
         * @return IProvider[]
         * @throws Exception
         */
        public IList<IProvider> getProviders(IUser user) {
            var appClasses = this.getAppProviderClasses(user);
            var providerClasses = this.getServerProviderClasses();
            var allClasses = providerClasses.Concat(appClasses); // array_merge(providerClasses, appClasses);
            var providers = new List<IProvider>();

            foreach (var @class in allClasses) {
                try {
                    providers.Add(this.serverContainer.query(@class) as IProvider);
                } catch (QueryException ex)
                {
                    this.logger.logException(ex,
                        new Dictionary<string, object>
                            {{"message", "Could not load contacts menu action provider class"}, {"app", "core"}});
                    throw new Exception("Could not load contacts menu action provider");
                }
            }

            return providers;
        }

        /**
         * @return string[]
         */
        private IList<string> getServerProviderClasses()
        {
            
            return new List<string> {{ typeof(EMailProvider).FullName}};
        }

        /**
         * @param IUser user
         * @return string[]
         */
        private IList<string> getAppProviderClasses(IUser user)
        {
            return this.appManager.getEnabledAppsForUser(user);
            // return array_reduce(this.appManager.getEnabledAppsForUser(user), function(all, appId) {
            //     info = this.appManager.getAppInfo(appId);
            //
            //     if (!isset(info["contactsmenu"]) || !isset(info["contactsmenu"])) {
            //         // Nothing to add
            //         return all;
            //     }
            //
            //     providers = array_reduce(info["contactsmenu"], function(all, provider) {
            //         return array_merge(all, [provider]);
            //     }, []);
            //
            //     return array_merge(all, providers);
            // }, []);
        }

    }

}