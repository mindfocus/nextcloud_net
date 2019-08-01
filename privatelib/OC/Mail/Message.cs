using System.Collections.Generic;
using System.Linq;
using CommonTypes.Email;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using OCP.Mail;

namespace OC.Mail
{
/**
 * Class Message provides a wrapper around SwiftMail
 *
 * @package OC\Mail
 */
class Message : IMessage {
	/** @var Swift_Message */
	private EmailData swiftMessage;
	/** @var bool */
	private bool plainTextOnly;

	public Message(EmailData swiftMessage, bool plainTextOnly) {
		this.swiftMessage = swiftMessage;
		this.plainTextOnly = plainTextOnly;
	}

	/**
	 * @param IAttachment attachment
	 * @return this
	 * @since 13.0.0
	 */
	public IMessage attach(IAttachment attachment) {
		/** @var Attachment attachment */
		this.swiftMessage.Attachments.Add((((Attachment) attachment).getSwiftAttachment()));
		return this;
	}
	
	/**
	 * Set the from address of this message.
	 *
	 * If no "From" address is used \OC\Mail\Mailer will use mail_from_address and mail_domain from config.php
	 *
	 * @param array addresses Example: array('sender@domain.org', 'other@domain.org' => 'A name')
	 * @return this
	 */
	public IMessage setFrom(EmailAddress emailAddress) {
		this.swiftMessage.FromAddress.EmailAddress = emailAddress.Address;
		this.swiftMessage.FromAddress.Name = emailAddress.Name;
		return this;
	}

	/**
	 * Get the from address of this message.
	 *
	 * @return array
	 */
	public EmailAddress getFrom()
	{
		return new EmailAddress(swiftMessage.FromAddress.EmailAddress, swiftMessage.FromAddress.Name);
	}

	/**
	 * Set the Reply-To address of this message
	 *
	 * @param array addresses
	 * @return this
	 */
	public IMessage setReplyTo(IList<EmailAddress> addresses) {
		foreach (var emailAddress in addresses)
		{
			this.swiftMessage.ReplyToAddresses.Add(new Address()
			{
				EmailAddress = emailAddress.Address,
				Name = emailAddress.Name
			});
		}
		return this;
	}

	/**
	 * Returns the Reply-To address of this message
	 *
	 * @return string
	 */
	public string getReplyTo()
	{

		return ""; //this.swiftMessage.ReplyToAddresses
	}

	/**
	 * Set the to addresses of this message.
	 *
	 * @param array recipients Example: array('recipient@domain.org', 'other@domain.org' => 'A name')
	 * @return this
	 */
	public IMessage setTo(IList<EmailAddress> recipients) {
		foreach (var address in recipients)
		{
			this.swiftMessage.ToAddresses.Add(new Address(address.Address,address.Name));
		}
		return this;
	}

	/**
	 * Get the to address of this message.
	 *
	 * @return array
	 */
	public IList<EmailAddress>  getTo() {
		var result = new List<EmailAddress>();
		foreach (var toAddress in this.swiftMessage.ToAddresses)
		{
			result.Add(new EmailAddress(toAddress.EmailAddress, toAddress.Name));
		}
		return result;
	}

	/**
	 * Set the CC recipients of this message.
	 *
	 * @param array recipients Example: array('recipient@domain.org', 'other@domain.org' => 'A name')
	 * @return this
	 */
	public IMessage setCc(IList<EmailAddress> recipients) {
		foreach (var emailAddress in recipients)
		{
			this.swiftMessage.CcAddresses.Add(new Address(emailAddress.Address,emailAddress.Name));
		}
		return this;
	}

	/**
	 * Get the cc address of this message.
	 *
	 * @return array
	 */
	public IList<EmailAddress> getCc() {
		return this.swiftMessage.CcAddresses.ConvertAll( o=> new EmailAddress(o.EmailAddress,o.Name));
	}

	/**
	 * Set the BCC recipients of this message.
	 *
	 * @param array recipients Example: array('recipient@domain.org', 'other@domain.org' => 'A name')
	 * @return this
	 */
	public IMessage setBcc(IList<EmailAddress> recipients) {
		foreach (var emailAddress in recipients)
		{
			this.swiftMessage.BccAddresses.Add(new Address(emailAddress.Address,emailAddress.Name));
		}
		return this;
	}

	/**
	 * Get the Bcc address of this message.
	 *
	 * @return array
	 */
	public IList<EmailAddress> getBcc() {
		return this.swiftMessage.BccAddresses.ConvertAll(o => new EmailAddress(o.EmailAddress, o.Name));
	}

	/**
	 * Set the subject of this message.
	 *
	 * @param string subject
	 * @return IMessage
	 */
	public IMessage setSubject(string subject) {
		this.swiftMessage.Subject = subject;
		return this;
	}

	/**
	 * Get the from subject of this message.
	 *
	 * @return string
	 */
	public string getSubject()
	{
		return this.swiftMessage.Subject;
	}

	/**
	 * Set the plain-text body of this message.
	 *
	 * @param string body
	 * @return this
	 */
	public IMessage setPlainBody(string body) {
		this.swiftMessage.Body = body;
		return this;
	}

	/**
	 * Get the plain body of this message.
	 *
	 * @return string
	 */
	public string getPlainBody()
	{
		return this.swiftMessage.Body;
	}

	/**
	 * Set the HTML body of this message. Consider also sending a plain-text body instead of only an HTML one.
	 *
	 * @param string body
	 * @return this
	 */
	public IMessage setHtmlBody(string body) {
		if (!this.plainTextOnly) {
			this.swiftMessage.Body = body;
				this.swiftMessage.IsHtml = true;
		}
		return this;
	}

	/**
	 * Get's the underlying SwiftMessage
	 * @return Swift_Message
	 */
	public EmailData getSwiftMessage() {
		return this.swiftMessage;
	}

	/**
	 * @param string body
	 * @param string contentType
	 * @return this
	 */
	public IMessage setBody(string body, string contentType) {
		if (!this.plainTextOnly || contentType != "text/html") {
			this.swiftMessage.Body= body;
//				.setBody(body, contentType);
		}
		return this;
	}

	/**
	 * @param IEMailTemplate emailTemplate
	 * @return this
	 */
	public IMessage useTemplate(IEMailTemplate emailTemplate) {
		this.setSubject(emailTemplate.renderSubject());
		this.setPlainBody(emailTemplate.renderText());
		if (!this.plainTextOnly) {
			this.setHtmlBody(emailTemplate.renderHtml());
		}
		return this;
	}
}

}