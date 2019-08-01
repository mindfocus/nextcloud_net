using System.Collections.Generic;
using System.Linq;
using OCA.TwoFactorBackupCodes.Provider;
using OCP.Authentication.TwoFactorAuth;

namespace OC.Authentication.TwoFactorAuth
{
    /**
     * Contains all two-factor provider information for the two-factor login challenge
     */
    public class ProviderSet
    {
        /** @var IProvider */
        private IDictionary<string,IProvider> providers;

        /** @var bool */
        private bool providerMissing;

        /**
         * @param IProvider[] providers
         * @param bool providerMissing
         */
        public ProviderSet(IList<IProvider> providers, bool providerMissing) {
            this.providers = new Dictionary<string, IProvider>();
            foreach (var provider in providers) {
                this.providers[provider.getId()] = provider;
            }
            this.providerMissing = providerMissing;
        }

        /**
         * @param string providerId
         * @return IProvider|null
         */
        public IProvider getProvider(string providerId) {
            return this.providers[providerId] ?? null;
        }

        /**
         * @return IProvider[]
         */
        public IList<IProvider> getProviders()
        {
            return this.providers.Values.ToList();
        }

        /**
         * @return IProvider[]
         */
        public IList<IProvider> getPrimaryProviders()
        {
            return this.providers.Values.Where(o => !(o is BackupCodesProvider)).ToList();
        }

        public bool isProviderMissing() {
            return this.providerMissing;
        }
    }
}