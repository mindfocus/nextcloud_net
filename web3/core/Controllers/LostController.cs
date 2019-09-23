using System;
using System.Collections.Generic;
using System.Linq;
using OCP;
using OCP.AppFramework;
using OCP.AppFramework.Http;
using OCP.AppFramework.Utility;
using OCP.Encryption;
using OCP.Mail;
using OCP.Security;

namespace OC.core.Controllers
{
    /**
 * Class LostController
 *
 * Successfully changing a password will emit the post_passwordReset hook.
 *
 * @package OC\Core\Controller
 */
    public class LostController : Controller
    {

        /** @var IURLGenerator */
	protected IURLGenerator urlGenerator;
	/** @var IUserManager */
	protected IUserManager userManager;
	/** @var Defaults */
	protected Defaults defaults;
	/** @var IL10N */
	protected IL10N l10n;
	/** @var string */
	protected string from;
	/** @var IManager */
	protected OCP.Encryption.IManager encryptionManager;
	/** @var IConfig */
	protected IConfig config;
	/** @var ISecureRandom */
	protected ISecureRandom secureRandom;
	/** @var IMailer */
	protected IMailer mailer;
	/** @var ITimeFactory */
	protected ITimeFactory timeFactory;
	/** @var ICrypto */
	protected ICrypto crypto;
	/** @var ILogger */
	private ILogger logger;
	/** @var Manager */
	private OC.Authentication.TwoFactorAuth.Manager twoFactorManager;
	/**
	 * @param string appName
	 * @param IRequest request
	 * @param IURLGenerator urlGenerator
	 * @param IUserManager userManager
	 * @param Defaults defaults
	 * @param IL10N l10n
	 * @param IConfig config
	 * @param ISecureRandom secureRandom
	 * @param string defaultMailAddress
	 * @param IManager encryptionManager
	 * @param IMailer mailer
	 * @param ITimeFactory timeFactory
	 * @param ICrypto crypto
	 */
	public LostController(string appName,
								IRequest request,
								IURLGenerator urlGenerator,
								IUserManager userManager,
								Defaults defaults,
								IL10N l10n,
								IConfig config,
								ISecureRandom secureRandom,
								string defaultMailAddress,
								IManager encryptionManager,
								IMailer mailer,
								ITimeFactory timeFactory,
								ICrypto crypto,
								ILogger logger,
								OC.Authentication.TwoFactorAuth.Manager twoFactorManager) : base(appName, request) {
		this.urlGenerator = urlGenerator;
		this.userManager = userManager;
		this.defaults = defaults;
		this.l10n = l10n;
		this.secureRandom = secureRandom;
		this.from = defaultMailAddress;
		this.encryptionManager = encryptionManager;
		this.config = config;
		this.mailer = mailer;
		this.timeFactory = timeFactory;
		this.crypto = crypto;
		this.logger = logger;
		this.twoFactorManager = twoFactorManager;
	}

	/**
	 * Someone wants to reset their password:
	 *
	 * @PublicPage
	 * @NoCSRFRequired
	 *
	 * @param string token
	 * @param string userId
	 * @return TemplateResponse
	 */
	public TemplateResponse resetform(string token, string userId) {
		if (this.config.getSystemValue("lost_password_link", "") != "") {
			return new TemplateResponse("core", "error", 
				new Dictionary<string,object>()
				{
					{"errors", new Dictionary<string, string>(){
					{
						"error", this.l10n.t("Password reset is disabled")
					}
					}}
				},
				"guest"
			);
		}

		try {
			this.checkPasswordResetToken(token, userId);
		} catch (\Exception e) {
			return new TemplateResponse(
				'core', 'error', [
					"errors" => array(array("error" => e.getMessage()))
				],
				'guest'
			);
		}

		return new TemplateResponse(
			'core',
			'lostpassword/resetpassword',
			array(
				'link' => this.urlGenerator.linkToRouteAbsolute('core.lost.setPassword', array('userId' => userId, 'token' => token)),
			),
			'guest'
		);
	}

	/**
	 * @param string token
	 * @param string userId
	 * @throws \Exception
	 */
	protected void checkPasswordResetToken(string token, string userId) {
		var user = this.userManager.get(userId);
		if(user == null || !user.isEnabled()) {
			throw new Exception(this.l10n.t("Couldn't reset password because the token is invalid"));
		}

		var decryptedToken = "";
		try {
			var encryptedToken = this.config.getUserValue<string>(userId, "core", "lostpassword", null);
			var mailAddress = user.getEMailAddress()!= null ? user.getEMailAddress() : "";
			decryptedToken = this.crypto.decrypt(encryptedToken, mailAddress + this.config.getSystemValue("secret"));
		} catch (Exception e) {
			throw new Exception(this.l10n.t("Couldn't reset password because the token is invalid"));
		}

		var splittedToken = decryptedToken.Split(":").ToList();
		if(splittedToken.Count != 2) {
			throw new Exception(this.l10n.t("Couldn't reset password because the token is invalid"));
		}

		if ( Convert.ToInt32(splittedToken[0]) < (this.timeFactory.getTime() - 60*60*24*7) ||
			user.getLastLogin() > Convert.ToInt32(splittedToken[0])){
			throw new Exception(this.l10n.t("Couldn\'t reset password because the token is expired"));
		}

		if (!splittedToken[1].Equals(token)) {
			throw new Exception(this.l10n.t("Couldn\'t reset password because the token is invalid"));
		}
	}

	/**
	 * @param message
	 * @param array additional
	 * @return array
	 */
	private function error(message, array additional=array()) {
		return array_merge(array('status' => 'error', 'msg' => message), additional);
	}

	/**
	 * @param array data
	 * @return array
	 */
	private function success(data = []) {
		return array_merge(data, ['status'=>'success']);
	}

	/**
	 * @PublicPage
	 * @BruteForceProtection(action=passwordResetEmail)
	 * @AnonRateThrottle(limit=10, period=300)
	 *
	 * @param string user
	 * @return JSONResponse
	 */
	public function email(user){
		if (this.config.getSystemValue('lost_password_link', '') !== '') {
			return new JSONResponse(this.error(this.l10n.t('Password reset is disabled')));
		}

		\OCP\Util::emitHook(
			'\OCA\Files_Sharing\API\Server2Server',
			'preLoginNameUsedAsUserName',
			['uid' => &user]
		);

		// FIXME: use HTTP error codes
		try {
			this.sendEmail(user);
		} catch (ResetPasswordException e) {
			// Ignore the error since we do not want to leak this info
			this.logger.warning('Could not send password reset email: ' . e.getMessage());
		} catch (\Exception e) {
			this.logger.logException(e);
		}

		response = new JSONResponse(this.success());
		response.throttle();
		return response;
	}

	/**
	 * @PublicPage
	 * @param string token
	 * @param string userId
	 * @param string password
	 * @param boolean proceed
	 * @return array
	 */
	public function setPassword(token, userId, password, proceed) {
		if (this.config.getSystemValue('lost_password_link', '') !== '') {
			return this.error(this.l10n.t('Password reset is disabled'));
		}

		if (this.encryptionManager.isEnabled() && !proceed) {
			encryptionModules = this.encryptionManager.getEncryptionModules();
			foreach (encryptionModules as module) {
				/** @var IEncryptionModule instance */
				instance = call_user_func(module['callback']);
				// this way we can find out whether per-user keys are used or a system wide encryption key
				if (instance.needDetailedAccessList()) {
					return this.error('', array('encryption' => true));
				}
			}
		}

		try {
			this.checkPasswordResetToken(token, userId);
			user = this.userManager.get(userId);

			\OC_Hook::emit('\OC\Core\LostPassword\Controller\LostController', 'pre_passwordReset', array('uid' => userId, 'password' => password));

			if (!user.setPassword(password)) {
				throw new \Exception();
			}

			\OC_Hook::emit('\OC\Core\LostPassword\Controller\LostController', 'post_passwordReset', array('uid' => userId, 'password' => password));

			this.twoFactorManager.clearTwoFactorPending(userId);

			this.config.deleteUserValue(userId, 'core', 'lostpassword');
			@\OC::server.getUserSession().unsetMagicInCookie();
		} catch (HintException e){
			return this.error(e.getHint());
		} catch (\Exception e){
			return this.error(e.getMessage());
		}

		return this.success(['user' => userId]);
	}

	/**
	 * @param string input
	 * @throws ResetPasswordException
	 * @throws \OCP\PreConditionNotMetException
	 */
	protected function sendEmail(input) {
		user = this.findUserByIdOrMail(input);
		email = user.getEMailAddress();

		if (empty(email)) {
			throw new ResetPasswordException('Could not send reset e-mail since there is no email for username ' . input);
		}

		// Generate the token. It is stored encrypted in the database with the
		// secret being the users' email address appended with the system secret.
		// This makes the token automatically invalidate once the user changes
		// their email address.
		token = this.secureRandom.generate(
			21,
			ISecureRandom::CHAR_DIGITS.
			ISecureRandom::CHAR_LOWER.
			ISecureRandom::CHAR_UPPER
		);
		tokenValue = this.timeFactory.getTime() .':'. token;
		encryptedValue = this.crypto.encrypt(tokenValue, email . this.config.getSystemValue('secret'));
		this.config.setUserValue(user.getUID(), 'core', 'lostpassword', encryptedValue);

		link = this.urlGenerator.linkToRouteAbsolute('core.lost.resetform', array('userId' => user.getUID(), 'token' => token));

		emailTemplate = this.mailer.createEMailTemplate('core.ResetPassword', [
			'link' => link,
		]);

		emailTemplate.setSubject(this.l10n.t('%s password reset', [this.defaults.getName()]));
		emailTemplate.addHeader();
		emailTemplate.addHeading(this.l10n.t('Password reset'));

		emailTemplate.addBodyText(
			htmlspecialchars(this.l10n.t('Click the following button to reset your password. If you have not requested the password reset, then ignore this email.')),
			this.l10n.t('Click the following link to reset your password. If you have not requested the password reset, then ignore this email.')
		);

		emailTemplate.addBodyButton(
			htmlspecialchars(this.l10n.t('Reset your password')),
			link,
			false
		);
		emailTemplate.addFooter();

		try {
			message = this.mailer.createMessage();
			message.setTo([email => user.getUID()]);
			message.setFrom([this.from => this.defaults.getName()]);
			message.useTemplate(emailTemplate);
			this.mailer.send(message);
		} catch (\Exception e) {
			// Log the exception and continue
			this.logger.logException(e);
		}
	}

	/**
	 * @param string input
	 * @return IUser
	 * @throws ResetPasswordException
	 */
	protected IUser findUserByIdOrMail(string input) {
		var user = this.userManager.get(input);
		if (user is IUser) {
			if (!user.isEnabled()) {
				throw new ResetPasswordException("User is disabled");
			}

			return user;
		}

		var users = this.userManager.getByEmail(input).Where(o => o.isEnabled()).ToList();
		if (users.Count == 1)
		{
			return users[0];
		}
//		if (count(users) === 1) {
//			return reset(users);
//		}

		throw new ResetPasswordException("Could not find user");
	}
    }
}