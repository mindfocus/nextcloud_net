using System.Collections.Generic;
using OC.App;
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
            appClasses = this.getAppProviderClasses(user);
            providerClasses = this.getServerProviderClasses();
                allClasses = array_merge(providerClasses, appClasses);
            providers = [];

            foreach (allClasses as class) {
                try {
                    providers[] = this.serverContainer.query(class);
                } catch (QueryException ex) {
                    this.logger.logException(ex, [
                    "message" => "Could not load contacts menu action provider class",
                    "app" => "core",
                        ]);
                    throw new Exception("Could not load contacts menu action provider");
                }
            }

            return providers;
        }

        /**
         * @return string[]
         */
        private function getServerProviderClasses() {
            return [
            EMailProvider::class,
                ];
        }

        /**
         * @param IUser user
         * @return string[]
         */
        private IList<string> getAppProviderClasses(IUser user) {
            return array_reduce(this.appManager.getEnabledAppsForUser(user), function(all, appId) {
                info = this.appManager.getAppInfo(appId);

                if (!isset(info["contactsmenu"]) || !isset(info["contactsmenu"])) {
                    // Nothing to add
                    return all;
                }

                providers = array_reduce(info["contactsmenu"], function(all, provider) {
                    return array_merge(all, [provider]);
                }, []);

                return array_merge(all, providers);
            }, []);
        }

    }

}