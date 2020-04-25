using System;
using System.Collections.Generic;
using System.Linq;
using ext;
using OC.Authentication.Exceptions;
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
		IDictionary<string, IProvider> providers, IUser user) {

		foreach (var providerPair in providers) {
			if (providerStates.ContainsKey(providerPair.Key))
			{
				// All good
				continue;
			}
			var enabled = providerPair.Value.isTwoFactorAuthEnabledForUser(user);
			if (enabled) {
				this.providerRegistry.enableProviderFor(providerPair.Value, user);
			} else {
				this.providerRegistry.disableProviderFor(providerPair.Value, user);
			}
			providerStates[providerPair.Key] = enabled;
		}

		return providerStates;
	}

	/**
	 * @param array states
	 * @param IProvider providers
	 */
	private bool isProviderMissing(IDictionary<string, bool> states, IDictionary<string, IProvider> providers) {
		IDictionary<string, IProvider> indexed = new Dictionary<string, IProvider>();
		foreach (var providerPair in providers) {
			indexed[providerPair.Key] = providerPair.Value;
		}

		var missing = new List<string>();
		foreach (var statePair in states)
		{
			if (!statePair.Value)
			{
				continue;
			}

			if (!indexed.ContainsKey(statePair.Key))
			{
				missing.Add(statePair.Key);
				this.logger.alert($"two-factor auth provider {statePair.Key} failed to load", new Dictionary<string,string>{{"app", "core"}});
			}
		}

		if (missing.IsNotEmpty())
		{
			// There was at least one provider missing

			this.logger.alert( $" {missing.Count} two-factor auth providers failed to load", new Dictionary<string,string>{{"app", "core"}});

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
		var providerStates = this.providerRegistry.getProviderStates(user);
		var providers = this.providerLoader.getProviders(user);

		var fixedStates = this.fixMissingProviderStates(providerStates, providers, user);
		var isProviderMissing = this.isProviderMissing(fixedStates, providers);
		var enabled = providers.Where(o => fixedStates.ContainsKey(o.Value.getId())).Select(o => o.Value).ToList();
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
	public bool verifyChallenge(string providerId, IUser user, string challenge) {
		var provider = this.getProvider(user, providerId);
		if (provider == null) {
			return false;
		}

		var passed = provider.verifyChallenge(user, challenge);
		if (passed) {
			if ((bool)this.session.get(REMEMBER_LOGIN) == true) {
				// TODO: resolve cyclic dependency and use DI
				OC::server.getUserSession().createRememberMeToken(user);
			}
			this.session.remove(SESSION_UID_KEY);
			this.session.remove(REMEMBER_LOGIN);
			this.session.set(SESSION_UID_DONE, user.getUID());

			// Clear token from db
			var sessionId = this.session.getId();
			var token = this.tokenProvider.getToken(sessionId);
			var tokenId = token.getId();
			this.config.deleteUserValue(user.getUID(), "login_token_2fa", tokenId.ToString());

			var dispatchEvent = new GenericEvent(user, new Dictionary<string, object>{{"provider", provider.getDisplayName()}});
			
			// this.dispatcher.dispatch(IProvider::EVENT_SUCCESS, dispatchEvent);

			this.publishEvent(user, "twofactor_success",
				new Dictionary<string, object> {{"provider", provider.getDisplayName()}});
			
		} else {
			var dispatchEvent = new GenericEvent(user, new Dictionary<string, object>{{"provider", provider.getDisplayName()}});
			// this.dispatcher.dispatch(IProvider::EVENT_FAILED, dispatchEvent);

			this.publishEvent(user, "twofactor_failed", new Dictionary<string, object>{{"provider", provider.getDisplayName()}});
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
	private void publishEvent(IUser user, string @event, IDictionary<string, object> paramList) {
		var activity = this.activityManager.generateEvent();
		activity.setApp("core")
			.setType("security")
			.setAuthor(user.getUID())
			.setAffectedUser(user.getUID())
			.setSubject(event, params);
		try {
			this.activityManager.publish(activity);
		} catch (BadMethodCallException e) {
			this.logger.warning("could not publish activity", new Dictionary<string, object>{{"app", "core"}});
			this.logger.logException(e, new Dictionary<string, object>{{"app", "core"}});
		}
	}

	/**
	 * Check if the currently logged in user needs to pass 2FA
	 *
	 * @param IUser user the currently logged in user
	 * @return boolean
	 */
	public bool needsSecondFactor(IUser user = null) {
		if (user == null) {
			return false;
		}

		// If we are authenticated using an app password skip all this
		if (this.session.exists("app_password")) {
			return false;
		}

		// First check if the session tells us we should do 2FA (99% case)
		if (!this.session.exists(SESSION_UID_KEY)) {

			// Check if the session tells us it is 2FA authenticated already
			if (this.session.exists(SESSION_UID_DONE) &&
				this.session.get(SESSION_UID_DONE) == user.getUID()) {
				return false;
			}

			/*
			 * If the session is expired check if we are not logged in by a token
			 * that still needs 2FA auth
			 */
			try {
				var sessionId = this.session.getId();
				var token = this.tokenProvider.getToken(sessionId);
				var tokenId = token.getId();
				var tokensNeeding2FA = this.config.getUserKeys(user.getUID(), "login_token_2fa");

				
				
				if (!tokensNeeding2FA.Contains(tokenId.ToString())) {
					this.session.set(SESSION_UID_DONE, user.getUID());
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
			this.session.remove(SESSION_UID_KEY);

			var keys = this.config.getUserKeys(user.getUID(), "login_token_2fa");
			foreach (var key in keys) {
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
	public void prepareTwoFactorLogin(IUser user, bool rememberMe) {
		this.session.set(SESSION_UID_KEY, user.getUID());
		this.session.set(REMEMBER_LOGIN, rememberMe);

		var id = this.session.getId();
		var token = this.tokenProvider.getToken(id);
		this.config.setUserValue(user.getUID(), "login_token_2fa", token.getId().ToString(), this.timeFactory.getTime());
	}

	public void clearTwoFactorPending(string userId) {
		var tokensNeeding2FA = this.config.getUserKeys(userId, "login_token_2fa");

		foreach (var tokenId in tokensNeeding2FA) {
			this.tokenProvider.invalidateTokenById(userId, Convert.ToInt32(tokenId));
		}
	}
    }
}