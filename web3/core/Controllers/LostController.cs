using System;
using System.Collections.Generic;
using System.Linq;
using CommonTypes.Email;
using ext;
using Microsoft.AspNetCore.Mvc;
using OC.core.Exception;
using OCP;
using OCP.AppFramework.Http;
using OCP.AppFramework.Utility;
using OCP.Encryption;
using OCP.Mail;
using OCP.Security;
using Controller = OCP.AppFramework.Controller;

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
		} catch (System.Exception e) {
			return new TemplateResponse(
				"core", "error",
				new Dictionary<string, object>
				{
					{"errors", new List<object>(){ new Dictionary<string ,string>()
					{
						{"error", e.Message}
					}}}
				},
				"guest"
			);
		}

		return new TemplateResponse(
			"core",
			"lostpassword/resetpassword", new Dictionary<string, object>()
			{
				{"link", this.urlGenerator.linkToRouteAbsolute("core.lost.setPassword", new Dictionary<string, object>()
				{
					{"userId", userId},
					{"token", token}
				})}
			},
			"guest"
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
			throw new System.Exception(this.l10n.t("Couldn't reset password because the token is invalid"));
		}

		var decryptedToken = "";
		try {
			var encryptedToken = this.config.getUserValue<string>(userId, "core", "lostpassword", null);
			var mailAddress = user.getEMailAddress()!= null ? user.getEMailAddress() : "";
			decryptedToken = this.crypto.decrypt(encryptedToken, mailAddress + this.config.getSystemValue("secret"));
		} catch (System.Exception e) {
			throw new System.Exception(this.l10n.t("Couldn't reset password because the token is invalid"));
		}

		var splittedToken = decryptedToken.Split(":").ToList();
		if(splittedToken.Count != 2) {
			throw new System.Exception(this.l10n.t("Couldn't reset password because the token is invalid"));
		}

		if ( Convert.ToInt32(splittedToken[0]) < (this.timeFactory.getTime() - 60*60*24*7) ||
			user.getLastLogin() > Convert.ToInt32(splittedToken[0])){
			throw new System.Exception(this.l10n.t("Couldn\'t reset password because the token is expired"));
		}

		if (!splittedToken[1].Equals(token)) {
			throw new System.Exception(this.l10n.t("Couldn\'t reset password because the token is invalid"));
		}
	}

	/**
	 * @param message
	 * @param array additional
	 * @return array
	 */
	private IDictionary<string, object> error(string message, IDictionary<string, object> additional = null) {
		IDictionary<string, object> data = new Dictionary<string, object>();
		data.AddOrMerge("status", "error");
		data.AddOrMerge("msg", message);
		if (additional != null)
		{
			data.AddOrMergeRange(additional);
		}
		return data;
	}

	/**
	 * @param array data
	 * @return array
	 */
	private IDictionary<string, object> success(IDictionary<string, object> data = null)
	{
		if (data == null)
		{
			data = new Dictionary<string, object>();
		}
		return data.AddOrMerge("status", "success");
	}

	/**
	 * @PublicPage
	 * @BruteForceProtection(action=passwordResetEmail)
	 * @AnonRateThrottle(limit=10, period=300)
	 *
	 * @param string user
	 * @return JSONResponse
	 */
	public IActionResult email(string user){
		if (this.config.getSystemValue("lost_password_link", "") != "") {
			return new JsonResult(error(l10n.t("Password reset is disabled")));
		}

		// \OCP\Util::emitHook(
		// 	'\OCA\Files_Sharing\API\Server2Server',
		// 	'preLoginNameUsedAsUserName',
		// 	['uid' => &user]
		// );

		// FIXME: use HTTP error codes
		try {
			this.sendEmail(user);
		} catch (ResetPasswordException e) {
			// Ignore the error since we do not want to leak this info
			this.logger.warning("Could not send password reset email: " + e.Message);
		} catch (System.Exception e) {
			this.logger.logException(e);
		}

		// response = new JSONResponse(this.success());
		// response.throttle();
		return new JsonResult(success());
	}

	/**
	 * @PublicPage
	 * @param string token
	 * @param string userId
	 * @param string password
	 * @param boolean proceed
	 * @return array
	 */
	public IActionResult setPassword(string token, string userId, string password, bool proceed) {
		if (this.config.getSystemValue("lost_password_link", "") != "") {
			return new JsonResult(this.error(this.l10n.t("Password reset is disabled")));
		}
		if (this.encryptionManager.isEnabled() && !proceed) {
			var encryptionModules = this.encryptionManager.getEncryptionModules();
			foreach (var module in encryptionModules) {
				/** @var IEncryptionModule instance */
				// instance = call_user_func(module['callback']);
				// this way we can find out whether per-user keys are used or a system wide encryption key
				// if (instance.needDetailedAccessList()) {
				// 	return this.error('', array('encryption' => true));
				// }
			}
		}

		try {
			this.checkPasswordResetToken(token, userId);
			var user = this.userManager.get(userId);

			// \OC_Hook::emit('\OC\Core\LostPassword\Controller\LostController', 'pre_passwordReset', array('uid' => userId, 'password' => password));

			if (!user.setPassword(password)) {
				throw new System.Exception();
			}

			// \OC_Hook::emit('\OC\Core\LostPassword\Controller\LostController', 'post_passwordReset', array('uid' => userId, 'password' => password));

			this.twoFactorManager.clearTwoFactorPending(userId);

			this.config.deleteUserValue(userId, "core", "lostpassword");
			// @\OC::server.getUserSession().unsetMagicInCookie();
		} catch (HintException e){
			return new JsonResult(this.error(e.getHint()));
		} catch (System.Exception e){
			return  new JsonResult(this.error(e.Message));
		}

		return new JsonResult(this.success(new Dictionary<string, object> {{"user", userId}}));
	}

	/**
	 * @param string input
	 * @throws ResetPasswordException
	 * @throws \OCP\PreConditionNotMetException
	 */
	protected void sendEmail(string input) {
		var user = this.findUserByIdOrMail(input);
		var email = user.getEMailAddress();

		if (email.IsEmpty()) {
			throw new ResetPasswordException($"Could not send reset e-mail since there is no email for username {input}");
		}

		// Generate the token. It is stored encrypted in the database with the
		// secret being the users' email address appended with the system secret.
		// This makes the token automatically invalidate once the user changes
		// their email address.
		var token = this.secureRandom.generate(
			21,
			SecureRandomType.CHAR_DIGITS + SecureRandomType.CHAR_LOWER + SecureRandomType.CHAR_UPPER
		);
		var tokenValue = this.timeFactory.getTime() + ":" + token;
		var encryptedValue = this.crypto.encrypt(tokenValue, email + this.config.getSystemValue("secret"));
		this.config.setUserValue(user.getUID(), "core", "lostpassword", encryptedValue);

		var link = this.urlGenerator.linkToRouteAbsolute("core.lost.resetform",  new Dictionary<string, object> {{"userId", user.getUID()}, {"token", token}});
		

		var emailTemplate = this.mailer.createEMailTemplate("core.ResetPassword", new Dictionary<string, object> {{"link", link}});

		emailTemplate.setSubject(this.l10n.t("%s password reset", this.defaults.getName()));
		emailTemplate.addHeader();
		emailTemplate.addHeading(this.l10n.t("Password reset"));

		emailTemplate.addBodyText(
			(this.l10n.t("Click the following button to reset your password. If you have not requested the password reset, then ignore this email.")),
			this.l10n.t("Click the following link to reset your password. If you have not requested the password reset, then ignore this email.")
		);

		emailTemplate.addBodyButton(
			(this.l10n.t("Reset your password")),
			link,
			""
		);
		emailTemplate.addFooter();

		try {
			var message = this.mailer.createMessage();
			message.setTo(new List<EmailAddress> { new EmailAddress(email, user.getUID())});
			message.setFrom(new EmailAddress(this.@from, this.defaults.getName()));
			message.useTemplate(emailTemplate);
			this.mailer.send(message);
		} catch (System.Exception e) {
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