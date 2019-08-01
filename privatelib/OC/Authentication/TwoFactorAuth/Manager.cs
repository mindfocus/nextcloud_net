using System.Collections.Generic;
using System.Linq;
using ext;
using OCP;
using OCP.Activity;
using OCP.AppFramework.Utility;
using OCP.Authentication.TwoFactorAuth;
using OCP.Sym;
using Pchp.Library.Spl;
using IProvider = OCP.Authentication.TwoFactorAuth.IProvider;
using TokenProvider = OC.Authentication.Token.IProvider;
namespace OC.Authentication.TwoFactorAuth
{
    public class Manager
    {
        const string SESSION_UID_KEY = "two_factor_auth_uid";
	const string SESSION_UID_DONE = "two_factor_auth_passed";
	const string REMEMBER_LOGIN = "two_factor_remember_login";
	const string BACKUP_CODES_PROVIDER_ID = "backup_codes";

	/** @var ProviderLoader */
	private ProviderLoader providerLoader;

	/** @var IRegistry */
	private IRegistry providerRegistry;

	/** @var MandatoryTwoFactor */
	private MandatoryTwoFactor mandatoryTwoFactor;

	/** @var ISession */
	private ISession session;

	/** @var IConfig */
	private IConfig config;

	/** @var IManager */
	private IManager activityManager;

	/** @var ILogger */
	private ILogger logger;

	/** @var TokenProvider */
	private TokenProvider tokenProvider;

	/** @var ITimeFactory */
	private ITimeFactory timeFactory;

	/** @var EventDispatcherInterface */
	private EventDispatcherInterface dispatcher;

	public Manager(ProviderLoader providerLoader,
								IRegistry providerRegistry,
								MandatoryTwoFactor mandatoryTwoFactor,
								ISession session, IConfig config,
								IManager activityManager, ILogger logger, TokenProvider tokenProvider,
								ITimeFactory timeFactory, EventDispatcherInterface eventDispatcher) {
		this.providerLoader = providerLoader;
		this.providerRegistry = providerRegistry;
		this.mandatoryTwoFactor = mandatoryTwoFactor;
		this.session = session;
		this.config = config;
		this.activityManager = activityManager;
		this.logger = logger;
		this.tokenProvider = tokenProvider;
		this.timeFactory = timeFactory;
		this.dispatcher = eventDispatcher;
	}

	/**
	 * Determine whether the user must provide a second factor challenge
	 *
	 * @param IUser user
	 * @return boolean
	 */
	public bool isTwoFactorAuthenticated(IUser user)  {
		if (this.mandatoryTwoFactor.isEnforcedFor(user)) {
			return true;
		}

		var providerStates = this.providerRegistry.getProviderStates(user);
		var providers = this.providerLoader.getProviders(user);
		var fixedStates = this.fixMissingProviderStates(providerStates, providers, user);
		var enabled = fixedStates.Where(c => c.Value == false).ToDictionary(o => o.Key, p => p.Value);
		var providerIds = enabled.Keys;
		var providerIdsWithoutBackupCodes = providerIds.Concat(new List<string> {BACKUP_CODES_PROVIDER_ID});

		return providerIdsWithoutBackupCodes.Any();
	}

	/**
	 * Get a 2FA provider by its ID
	 *
	 * @param IUser user
	 * @param string challengeProviderId
	 * @return IProvider|null
	 */
	public IProvider getProvider(IUser user, string challengeProviderId) {
		var providers = this.getProviderSet(user).getProviders();
		return providers[challengeProviderId] ?? null;
	}

	/**
	 * @param IUser user
	 * @return IActivatableAtLogin[]
	 * @throws Exception
	 */
	public IList<IActivatableAtLogin> getLoginSetupProviders(IUser user) {
		var providers = this.providerLoader.getProviders(user);
		return providers.Where(o => o is IActivatableAtLogin).Cast<IActivatableAtLogin>().ToList();
	}

	/**
	 * Check if the persistant mapping of enabled/disabled state of each available
	 * provider is missing an entry and add it to the registry in that case.
	 *
	 * @todo remove in Nextcloud 17 as by then all providers should have been updated
	 *
	 * @param string[] providerStates
	 * @param IProvider[] providers
	 * @param IUser user
	 * @return string[] the updated providerStates variable
	 */
	private IDictionary<string, bool> fixMissingProviderStates(IDictionary<string, bool> providerStates,
		IList<IProvider> providers, IUser user) {

		foreach (var provider in providers) {
			if (providerStates.ContainsKey(provider.getId()))
			{
				// All good
				continue;
			}
			var enabled = provider.isTwoFactorAuthEnabledForUser(user);
			if (enabled) {
				this.providerRegistry.enableProviderFor(provider, user);
			} else {
				this.providerRegistry.disableProviderFor(provider, user);
			}
			providerStates[provider.getId()] = enabled;
		}

		return providerStates;
	}

	/**
	 * @param array states
	 * @param IProvider providers
	 */
	private bool isProviderMissing(array states, array providers) {
		indexed = [];
		foreach (providers as provider) {
			indexed[provider.getId()] = provider;
		}

		missing = [];
		foreach (states as providerId => enabled) {
			if (!enabled) {
				// Don"t care
				continue;
			}

			if (!isset(indexed[providerId])) {
				missing[] = providerId;
				this.logger.alert("two-factor auth provider "providerId" failed to load",
					[
					"app" => "core",
				]);
			}
		}

		if (!empty(missing)) {
			// There was at least one provider missing
			this.logger.alert(count(missing) . " two-factor auth providers failed to load", ["app" => "core"]);

			return true;
		}

		// If we reach this, there was not a single provider missing
		return false;
	}

	/**
	 * Get the list of 2FA providers for the given user
	 *
	 * @param IUser user
	 * @throws Exception
	 */
	public ProviderSet getProviderSet(IUser user) {
		providerStates = this.providerRegistry.getProviderStates(user);
		providers = this.providerLoader.getProviders(user);

		fixedStates = this.fixMissingProviderStates(providerStates, providers, user);
		isProviderMissing = this.isProviderMissing(fixedStates, providers);

		enabled = array_filter(providers, function (IProvider provider) use (fixedStates) {
			return fixedStates[provider.getId()];
		});
		return new ProviderSet(enabled, isProviderMissing);
	}

	/**
	 * Verify the given challenge
	 *
	 * @param string providerId
	 * @param IUser user
	 * @param string challenge
	 * @return boolean
	 */
	public function verifyChallenge(string providerId, IUser user, string challenge): bool {
		provider = this.getProvider(user, providerId);
		if (provider === null) {
			return false;
		}

		passed = provider.verifyChallenge(user, challenge);
		if (passed) {
			if (this.session.get(self::REMEMBER_LOGIN) === true) {
				// TODO: resolve cyclic dependency and use DI
				\OC::server.getUserSession().createRememberMeToken(user);
			}
			this.session.remove(self::SESSION_UID_KEY);
			this.session.remove(self::REMEMBER_LOGIN);
			this.session.set(self::SESSION_UID_DONE, user.getUID());

			// Clear token from db
			sessionId = this.session.getId();
			token = this.tokenProvider.getToken(sessionId);
			tokenId = token.getId();
			this.config.deleteUserValue(user.getUID(), "login_token_2fa", tokenId);

			dispatchEvent = new GenericEvent(user, ["provider" => provider.getDisplayName()]);
			this.dispatcher.dispatch(IProvider::EVENT_SUCCESS, dispatchEvent);

			this.publishEvent(user, "twofactor_success", [
				"provider" => provider.getDisplayName(),
			]);
		} else {
			dispatchEvent = new GenericEvent(user, ["provider" => provider.getDisplayName()]);
			this.dispatcher.dispatch(IProvider::EVENT_FAILED, dispatchEvent);

			this.publishEvent(user, "twofactor_failed", [
				"provider" => provider.getDisplayName(),
			]);
		}
		return passed;
	}

	/**
	 * Push a 2fa event the user"s activity stream
	 *
	 * @param IUser user
	 * @param string event
	 * @param array params
	 */
	private function publishEvent(IUser user, string event, array params) {
		activity = this.activityManager.generateEvent();
		activity.setApp("core")
			.setType("security")
			.setAuthor(user.getUID())
			.setAffectedUser(user.getUID())
			.setSubject(event, params);
		try {
			this.activityManager.publish(activity);
		} catch (BadMethodCallException e) {
			this.logger.warning("could not publish activity", ["app" => "core"]);
			this.logger.logException(e, ["app" => "core"]);
		}
	}

	/**
	 * Check if the currently logged in user needs to pass 2FA
	 *
	 * @param IUser user the currently logged in user
	 * @return boolean
	 */
	public function needsSecondFactor(IUser user = null): bool {
		if (user === null) {
			return false;
		}

		// If we are authenticated using an app password skip all this
		if (this.session.exists("app_password")) {
			return false;
		}

		// First check if the session tells us we should do 2FA (99% case)
		if (!this.session.exists(self::SESSION_UID_KEY)) {

			// Check if the session tells us it is 2FA authenticated already
			if (this.session.exists(self::SESSION_UID_DONE) &&
				this.session.get(self::SESSION_UID_DONE) === user.getUID()) {
				return false;
			}

			/*
			 * If the session is expired check if we are not logged in by a token
			 * that still needs 2FA auth
			 */
			try {
				sessionId = this.session.getId();
				token = this.tokenProvider.getToken(sessionId);
				tokenId = token.getId();
				tokensNeeding2FA = this.config.getUserKeys(user.getUID(), "login_token_2fa");

				if (!\in_array(tokenId, tokensNeeding2FA, true)) {
					this.session.set(self::SESSION_UID_DONE, user.getUID());
					return false;
				}
			} catch (InvalidTokenException e) {
			}
		}

		if (!this.isTwoFactorAuthenticated(user)) {
			// There is no second factor any more . let the user pass
			//   This prevents infinite redirect loops when a user is about
			//   to solve the 2FA challenge, and the provider app is
			//   disabled the same time
			this.session.remove(self::SESSION_UID_KEY);

			keys = this.config.getUserKeys(user.getUID(), "login_token_2fa");
			foreach (keys as key) {
				this.config.deleteUserValue(user.getUID(), "login_token_2fa", key);
			}
			return false;
		}

		return true;
	}

	/**
	 * Prepare the 2FA login
	 *
	 * @param IUser user
	 * @param boolean rememberMe
	 */
	public function prepareTwoFactorLogin(IUser user, bool rememberMe) {
		this.session.set(self::SESSION_UID_KEY, user.getUID());
		this.session.set(self::REMEMBER_LOGIN, rememberMe);

		id = this.session.getId();
		token = this.tokenProvider.getToken(id);
		this.config.setUserValue(user.getUID(), "login_token_2fa", token.getId(), this.timeFactory.getTime());
	}

	public function clearTwoFactorPending(string userId) {
		tokensNeeding2FA = this.config.getUserKeys(userId, "login_token_2fa");

		foreach (tokensNeeding2FA as tokenId) {
			this.tokenProvider.invalidateTokenById(userId, tokenId);
		}
	}
    }
}