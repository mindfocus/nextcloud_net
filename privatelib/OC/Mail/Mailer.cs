using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using CommonTypes.Email;
using ext;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using FluentEmail.Mailgun;
using FluentEmail.SendGrid;
using FluentEmail.Smtp;
using OCP;
using OCP.Mail;

namespace OC.Mail
{
/**
 * Class Mailer provides some basic functions to create a mail message that can be used in combination with
 * \OC\Mail\Message.
 *
 * Example usage:
 *
 * 	mailer = \OC::server.getMailer();
 * 	message = mailer.createMessage();
 * 	message.setSubject('Your Subject');
 * 	message.setFrom(array('cloud@domain.org' => 'ownCloud Notifier');
 * 	message.setTo(array('recipient@domain.org' => 'Recipient');
 * 	message.setBody('The message text');
 * 	mailer.send(message);
 *
 * This message can then be passed to send() of \OC\Mail\Mailer
 *
 * @package OC\Mail
 */
class Mailer : IMailer {
	/** @var \Swift_Mailer Cached mailer */
	private ISender instance = null;
	/** @var IConfig */
	private IConfig config;
	/** @var ILogger */
	private ILogger logger;
	/** @var Defaults */
	private Defaults defaults;
	/** @var IURLGenerator */
	private IURLGenerator urlGenerator;
	/** @var IL10N */
	private IL10N l10n;

	/**
	 * @param IConfig config
	 * @param ILogger logger
	 * @param Defaults defaults
	 * @param IURLGenerator urlGenerator
	 * @param IL10N l10n
	 */
	public Mailer(IConfig config,
						 ILogger logger,
						 Defaults defaults,
						 IURLGenerator urlGenerator,
						 IL10N l10n) {
		this.config = config;
		this.logger = logger;
		this.defaults = defaults;
		this.urlGenerator = urlGenerator;
		this.l10n = l10n;
	}

	/**
	 * Creates a new message object that can be passed to send()
	 *
	 * @return IMessage
	 */
	public IMessage createMessage() {
		var plainTextOnly = (bool)this.config.getSystemValue("mail_send_plaintext_only", false);
		return new Message(new EmailData(), plainTextOnly);
	}

	/**
	 * @param string|null data
	 * @param string|null filename
	 * @param string|null contentType
	 * @return IAttachment
	 * @since 13.0.0
	 */
	public IAttachment createAttachment(string data, string filename ,string contentType )
	{
		var attachment = new FluentEmail.Core.Models.Attachment();
		byte[] byteArray = Encoding.ASCII.GetBytes( data );
		MemoryStream stream = new MemoryStream( byteArray );
		attachment.Data =  stream;
		attachment.Filename = filename;
		attachment.ContentType = contentType;
		return new Attachment(attachment);
	}

	/**
	 * @param string path
	 * @param string|null contentType
	 * @return IAttachment
	 * @since 13.0.0
	 */
	public IAttachment createAttachmentFromPath(string path, string contentType = null) {
		var attachment = new FluentEmail.Core.Models.Attachment();
		
		byte[] byteArray = File.ReadAllBytes(path);
		MemoryStream stream = new MemoryStream( byteArray );
		attachment.Data =  stream;
		attachment.ContentType = contentType;
		return new Attachment(attachment);
	}

	/**
	 * Creates a new email template object
	 *
	 * @param string emailId
	 * @param array data
	 * @return IEMailTemplate
	 * @since 12.0.0
	 */
	public IEMailTemplate createEMailTemplate(string emailId, IList<object> data) {
		var clazz = (string)this.config.getSystemValue("mail_template_class", "");

		if (this.GetType().Assembly.GetTypes().Any(x => x.FullName == clazz &&
		                                                (x.IsSubclassOf(typeof(EMailTemplate)) || x.IsEquivalentTo(typeof(EMailTemplate)))))
		{
			var typeClazz = this.GetType().Assembly.GetTypes().Where(x =>
				x.FullName == clazz &&
				(x.IsSubclassOf(typeof(EMailTemplate)) || x.IsEquivalentTo(typeof(EMailTemplate)))).ToList()[0];
			return (EMailTemplate)Activator.CreateInstance(typeClazz, this.defaults, this.urlGenerator, this.l10n, emailId, data);
		}
		return new EMailTemplate(
			this.defaults,
			this.urlGenerator,
			this.l10n,
			emailId,
			data
		);
	}

	/**
	 * Send the specified message. Also sets the from address to the value defined in config.php
	 * if no-one has been passed.
	 *
	 * @param IMessage|Message message Message to send
	 * @return string[] Array with failed recipients. Be aware that this depends on the used mail backend and
	 * therefore should be considered
	 * @throws \Exception In case it was not possible to send the message. (for example if an invalid mail address
	 * has been supplied.)
	 */
	public IList<string> send(IMessage message){
		var debugMode = (bool)this.config.getSystemValue("mail_smtpdebug", false);

		if ( ((Message)(message)).getFrom().Address.IsEmpty())
		{
			message.setFrom(new EmailAddress
			{
				Name = this.defaults.getName(),
				Address = OCP.Util.getDefaultEmailAddress(this.defaults.getName())
			});
		}

		var failedRecipients = new List<string>();

		var mailer = this.getInstance();

		// Enable logger if debug mode is enabled
//		if(debugMode) {
//			var mailLogger = new \Swift_Plugins_Loggers_ArrayLogger();
//			mailer.registerPlugin(new \Swift_Plugins_LoggerPlugin(mailLogger));
//		}

		var email = Email.From(((Message) (message)).getFrom().Address, ((Message) (message)).getFrom().Name)
			.To(((Message) message).getTo().ToList().ConvertAll(o => new Address(o.Address, o.Name)))
			.Subject(((Message) message).getSubject())
			.Body(((Message) message).getPlainBody())
			.Send();

//		mailer.send(((Message)message).getSwiftMessage(), failedRecipients);

		// Debugging logging
		var logMessage = string.Format("Sent mail to \"{0}\" with subject \"{1}\"", ((Message) message).getTo(),
			((Message) message).getSubject());
		this.logger.debug(logMessage, new Dictionary<string,string> {{"app", "core"}});
//		if(debugMode && isset(mailLogger)) {
//			this.logger.debug(mailLogger.dump(), ['app' => 'core']);
//		}

		return email.ErrorMessages;
	}

	/**
	 * Checks if an e-mail address is valid
	 *
	 * @param string email Email address to be validated
	 * @return bool True if the mail address is valid, false otherwise
	 */
	public bool validateMailAddress(string email) {
//		var validator = new EmailValidator();
//		var validation = new RFCValidation();
//
//		return validator.isValid(this.convertEmail(email), validation);
		return true;
	}

	/**
	 * SwiftMailer does currently not work with IDN domains, this function therefore converts the domains
	 *
	 * FIXME: Remove this once SwiftMailer supports IDN
	 *
	 * @param string email
	 * @return string Converted mail address if `idn_to_ascii` exists
	 */
//	protected string convertEmail(string email) {
//		if (!function_exists('idn_to_ascii') || !defined('INTL_IDNA_VARIANT_UTS46') || strpos(email, '@') === false) {
//			return email;
//		}
//
//		list(name, domain) = explode('@', email, 2);
//		domain = idn_to_ascii(domain, 0,INTL_IDNA_VARIANT_UTS46);
//		return name.'@'.domain;
//	}

	protected ISender getInstance() {
		if (this.instance != null) {
			return this.instance;
		}

		object sender = null;
		switch (this.config.getSystemValue("mail_smtpmode", "smtp")) {
			case "sendgrid":
				sender = new SendGridSender("", true);
				break;
			case "mailgun":
				sender = new MailgunSender("","");
				break;
			case "smtp":
			default:
				sender = this.getSmtpInstance();
				break;
		}

		return (ISender) sender;
	}

	/**
	 * Returns the SMTP transport
	 *
	 * @return \Swift_SmtpTransport
	 */
	protected ISender getSmtpInstance() {
		var host = (string)(this.config.getSystemValue("mail_smtphost", "127.0.0.1"));
		var port = (int)(this.config.getSystemValue("mail_smtpport", 25));
		var smtpClient = new SmtpClient(host,port);
		var timeout = (int)(this.config.getSystemValue("mail_smtptimeout", 10));
		smtpClient.Timeout = timeout;
		var credentials = new NetworkCredential();
		
		if ((bool)this.config.getSystemValue("mail_smtpauth", false)) {
			var username = (string)(this.config.getSystemValue("mail_smtpname", ""));
			var password = (string)(this.config.getSystemValue("mail_smtppassword", ""));
			var authMode = (string)(this.config.getSystemValue("mail_smtpauthtype", "LOGIN"));
			credentials.Password = password;
			credentials.UserName = username;
		}
		var smtpSecurity = (string)this.config.getSystemValue("mail_smtpsecure", "");
		if (smtpSecurity.IsNotEmpty()) {
			var encryption = (smtpSecurity);
		}
		var streamingOptions = this.config.getSystemValue("mail_smtpstreamoptions", new Dictionary<string,string>());
//		if (is_array(streamingOptions) && !empty(streamingOptions)) {
//			var streamOptions = (streamingOptions);
//		}
		smtpClient.Credentials = credentials;
		var sender = new SmtpSender( smtpClient );

		return sender;
	}

	/**
	 * Returns the sendmail transport
	 *
	 * @return \Swift_SendmailTransport
	 */
//	protected function getSendMailInstance(): \Swift_SendmailTransport {
//		switch (this.config.getSystemValue('mail_smtpmode', 'smtp')) {
//			case 'qmail':
//				binaryPath = '/var/qmail/bin/sendmail';
//				break;
//			default:
//				sendmail = \OC_Helper::findBinaryPath('sendmail');
//				if (sendmail === null) {
//					sendmail = '/usr/sbin/sendmail';
//				}
//				binaryPath = sendmail;
//				break;
//		}
//
//		switch (this.config.getSystemValue('mail_sendmailmode', 'smtp')) {
//			case 'pipe':
//				binaryParam = ' -t';
//				break;
//			default:
//				binaryParam = ' -bs';
//				break;
//		}
//
//		return new \Swift_SendmailTransport(binaryPath . binaryParam);
//	}
}

}