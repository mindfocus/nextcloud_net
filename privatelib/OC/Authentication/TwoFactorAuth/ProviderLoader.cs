using System;
using System.Collections.Generic;
using OC.legacy;
using OCP;
using OCP.App;
using OCP.AppFramework;
using OCP.Authentication.TwoFactorAuth;

namespace OC.Authentication.TwoFactorAuth
{
    public class ProviderLoader
    {
        const string BACKUP_CODES_APP_ID = "twofactor_backupcodes";

        /** @var IAppManager */
        private IAppManager appManager;

        public ProviderLoader(IAppManager appManager) {
            this.appManager = appManager;
        }

        /**
         * Get the list of 2FA providers for the given user
         *
         * @return IProvider[]
         * @throws Exception
         */
        public IDictionary<string, IProvider> getProviders(IUser user) {
            var allApps = this.appManager.getEnabledAppsForUser(user);
            var providers = new Dictionary<string, IProvider>();

            foreach (var appId in allApps) {
                var info = this.appManager.getAppInfo(appId);
                if (info.Twofactorproviders != null) {
                    /** @var string[] providerClasses */
                    var providerClasses = info.Twofactorproviders.Providers;
                    foreach (var clazz in providerClasses ) {
                        try {
                            this.loadTwoFactorApp(appId);
                            var provider = (IProvider)OC.server.query(clazz);
                            providers[provider.getId()] = provider;
                        } catch (QueryException exc) {
                            // Provider class can not be resolved
                            throw new Exception("Could not load two-factor auth provider class");
                        }
                    }
                }
            }

            return providers;
        }

        /**
         * Load an app by ID if it has not been loaded yet
         *
         * @param string appId
         */
        protected void loadTwoFactorApp(string appId) {
            if (!OC_App.isAppLoaded(appId)) {
                OC_App.loadApp(appId);
            }
        }
    }
}