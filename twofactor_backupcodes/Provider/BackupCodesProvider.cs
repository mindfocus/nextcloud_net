using System.Linq;
using OCP;
using OCP.App;
using OCP.Authentication.TwoFactorAuth;

namespace OCA.TwoFactorBackupCodes.Provider
{
    public class BackupCodesProvider : IProvider, IProvidesPersonalSettings
    {
        /** @var string */
	private string appName;

	/** @var BackupCodeStorage */
	private BackupCodeStorage storage;

	/** @var IL10N */
	private IL10N l10n;

	/** @var AppManager */
	private IAppManager appManager;
	/** @var IInitialStateService */
	private IInitialStateService initialStateService;

	/**
	 * @param string appName
	 * @param BackupCodeStorage storage
	 * @param IL10N l10n
	 * @param AppManager appManager
	 */
	public BackupCodesProvider(string appName,
								BackupCodeStorage storage,
								IL10N l10n,
								IAppManager appManager,
								IInitialStateService initialStateService) {
		this.appName = appName;
		this.l10n = l10n;
		this.storage = storage;
		this.appManager = appManager;
		this.initialStateService = initialStateService;
	}

	/**
	 * Get unique identifier of this 2FA provider
	 *
	 * @return string
	 */
	public string getId() {
		return "backup_codes";
	}

	/**
	 * Get the display name for selecting the 2FA provider
	 *
	 * @return string
	 */
	public string getDisplayName()  {
		return this.l10n.t("Backup code");
	}

	/**
	 * Get the description for selecting the 2FA provider
	 *
	 * @return string
	 */
	public string getDescription() {
		return this.l10n.t("Use backup code");
	}

	/**
	 * Get the template for rending the 2FA provider view
	 *
	 * @param IUser user
	 * @return Template
	 */
	public Template getTemplate(IUser user) {
		return new Template("twofactor_backupcodes", "challenge");
	}

	/**
	 * Verify the given challenge
	 *
	 * @param IUser user
	 * @param string challenge
	 * @return bool
	 */
	public bool verifyChallenge(IUser user, string challenge) {
		return this.storage.validateCode(user, challenge);
	}

	/**
	 * Decides whether 2FA is enabled for the given user
	 *
	 * @param IUser user
	 * @return boolean
	 */
	public bool isTwoFactorAuthEnabledForUser(IUser user) {
		return this.storage.hasBackupCodes(user);
	}

	/**
	 * Determine whether backup codes should be active or not
	 *
	 * Backup codes only make sense if at least one 2FA provider is active,
	 * hence this method checks all enabled apps on whether they provide 2FA
	 * functionality or not. If there's at least one app, backup codes are
	 * enabled on the personal settings page.
	 *
	 * @param IUser user
	 * @return boolean
	 */
	public bool isActive(IUser user) {
		var appIds = this.appManager.getEnabledAppsForUser(user).Where(appId => appId != this.appName).ToList();
		foreach (var appId in appIds)
		{
			var info = this.appManager.getAppInfo(appId);
			if (info.Twofactorproviders != null && info.Twofactorproviders.Providers.Count > 0 )
			{
				return true;
			}
		}

		return false;
	}

	/**
	 * @param IUser user
	 *
	 * @return IPersonalProviderSettings
	 */
	public IPersonalProviderSettings getPersonalSettings(IUser user) {
		var state = this.storage.getBackupCodesState(user);
		this.initialStateService.provideInitialState(this.appName, "state", state);
		return new Personal();
	}
    }
}