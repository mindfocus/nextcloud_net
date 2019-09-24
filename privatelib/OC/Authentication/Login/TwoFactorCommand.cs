using System.Collections.Generic;
using ext;
using OC.Authentication.TwoFactorAuth;
using OCP;
using TwoFactorManager = OC.Authentication.TwoFactorAuth.Manager;
namespace OC.Authentication.Login
{
    public class TwoFactorCommand : ALoginCommand
    {
        private TwoFactorManager twoFactorManager;
        private MandatoryTwoFactor mandatoryTwoFactor;
        private IURLGenerator urlGenerator;

        public TwoFactorCommand(TwoFactorManager twoFactorManager, MandatoryTwoFactor mandatoryTwoFactor, IURLGenerator urlGenerator)
        {
            this.twoFactorManager = twoFactorManager;
            this.mandatoryTwoFactor = mandatoryTwoFactor;
            this.urlGenerator = urlGenerator;
        }

        public override LoginResult process(LoginData loginData)
        {
            if (!this.twoFactorManager.isTwoFactorAuthenticated(loginData.getUser()))
            {
                return this.processNextOrFinishSuccessfully(loginData);
            }

            this.twoFactorManager.prepareTwoFactorLogin(loginData.getUser(), loginData.isRememberLogin());
            var providerSet = this.twoFactorManager.getProviderSet(loginData.getUser());
            var loginProviders = this.twoFactorManager.getLoginSetupProviders(loginData.getUser());
            var providers = providerSet.getPrimaryProviders();
            var url = "";
            var urlParams = new Dictionary<string, string>();
            if (providers.IsEmpty() && !providerSet.isProviderMissing() && loginProviders.IsNotEmpty() && this.mandatoryTwoFactor.isEnforcedFor(loginData.getUser()))
            {
                url = "core.TwoFactorChallenge.setupProviders";
                urlParams = new Dictionary<string, string>();
            }
            else if (!providerSet.isProviderMissing() && providers.Count == 1)
            {
                var provider = providers[0];
                url = "core.TwoFactorChallenge.showChallenge";
                urlParams = new Dictionary<string, string>()
                {
                    {"challengeProviderId", provider.getId()}
                };
            }
            else
            {
                url = "core.TwoFactorChallenge.selectChallenge";
                urlParams = new Dictionary<string, string>();
            }

            if (loginData.getRedirectUrl() != null)
            {                
                urlParams = new Dictionary<string, string>()
                {
                    {"redirect_url", loginData.getRedirectUrl()}
                };
            }
            return LoginResult.success(loginData, this.urlGenerator.linkToRoute(url, urlParams));
        }
    }
}