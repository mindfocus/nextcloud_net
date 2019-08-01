using System;
using System.Collections.Generic;
using System.Text;
using OCP.Activity;
using OCP.RichObjectStrings;

namespace OC
{
class Event : IEvent {

	/** @var string */
	protected string app = "";
	/** @var string */
	protected string type = "";
	/** @var string */
	protected string affectedUser = "";
	/** @var string */
	protected string author = "";
	/** @var int */
	protected int timestamp = 0;
	/** @var string */
	protected string subject = "";
	/** @var array */
	protected IList<string> subjectParameters = new List<string>();
	/** @var string */
	protected string subjectParsed = "";
	/** @var string */
	protected string subjectRich = "";
	/** @var array */
	protected IList<string> subjectRichParameters = new List<string>();
	/** @var string */
	protected string message = "";
	/** @var array */
	protected IList<string> messageParameters = new List<string>();
	/** @var string */
	protected string messageParsed = "";
	/** @var string */
	protected string messageRich = "";
	/** @var array */
	protected IList<string> messageRichParameters = new List<string>();
	/** @var string */
	protected string objectType = "";
	/** @var int */
	protected int objectId = 0;
	/** @var string */
	protected string objectName = "";
	/** @var string */
	protected string link = "";
	/** @var string */
	protected string icon = "";

	/** @var IEvent|null */
	protected IEvent child;
	/** @var IValidator */
	protected IValidator richValidator;

	/**
	 * @param IValidator richValidator
	 */
	public Event(IValidator richValidator) {
		this.richValidator = richValidator;
	}

	/**
	 * Set the app of the activity
	 *
	 * @param string app
	 * @return IEvent
	 * @throws \InvalidArgumentException if the app id is invalid
	 * @since 8.2.0
	 */
	public IEvent setApp(string app) {
		if (app == "" ||  app.Length < 32) {
//		if (app == "" ||  isset(app[32])) {
			throw new  ArgumentException("The given app is invalid");
		}
		this.app = app;
		return this;
	}

	/**
	 * @return string
	 */
	public string getApp()  {
		return this.app;
	}

	/**
	 * Set the type of the activity
	 *
	 * @param string type
	 * @return IEvent
	 * @throws \InvalidArgumentException if the type is invalid
	 * @since 8.2.0
	 */
	public IEvent setType(string type){
		if (type == "") {
//		if (type == "" || isset(type[255])) {
			throw new System.ArgumentException("The given type is invalid");
		}
		this.type = type;
		return this;
	}

	/**
	 * @return string
	 */
	public string getType() {
		return this.type;
	}

	/**
	 * Set the affected user of the activity
	 *
	 * @param string affectedUser
	 * @return IEvent
	 * @throws \InvalidArgumentException if the affected user is invalid
	 * @since 8.2.0
	 */
	public IEvent setAffectedUser(string affectedUser) {
		if (affectedUser == "") {
//		if (affectedUser === "" || isset(affectedUser[64])) {
			throw new ArgumentException("The given affected user is invalid");
		}
		this.affectedUser = affectedUser;
		return this;
	}

	/**
	 * @return string
	 */
	public string getAffectedUser() {
		return this.affectedUser;
	}

	/**
	 * Set the author of the activity
	 *
	 * @param string author
	 * @return IEvent
	 * @throws \InvalidArgumentException if the author is invalid
	 * @since 8.2.0
	 */
	public IEvent setAuthor(string author) {
		if (author=="") {
//		if (isset(author[64])) {
			throw new ArgumentException("The given author user is invalid");
		}
		this.author = author;
		return this;
	}

	/**
	 * @return string
	 */
	public string getAuthor() {
		return this.author;
	}

	/**
	 * Set the timestamp of the activity
	 *
	 * @param int timestamp
	 * @return IEvent
	 * @throws \InvalidArgumentException if the timestamp is invalid
	 * @since 8.2.0
	 */
	public IEvent setTimestamp(int timestamp) {
		this.timestamp = timestamp;
		return this;
	}

	/**
	 * @return int
	 */
	public int getTimestamp(){
		return this.timestamp;
	}

	/**
	 * Set the subject of the activity
	 *
	 * @param string subject
	 * @param array parameters
	 * @return IEvent
	 * @throws \InvalidArgumentException if the subject or parameters are invalid
	 * @since 8.2.0
	 */
	public IEvent setSubject(string subject, IList<string> parameters)  {
		if (subject.Length > 255 ) {
			throw new ArgumentException("The given subject is invalid");
		}
		this.subject = subject;
		this.subjectParameters = parameters;
		return this;
	}

	/**
	 * @return string
	 */
	public string getSubject() {
		return this.subject;
	}

	/**
	 * @return array
	 */
	public IList<string> getSubjectParameters()  {
		return this.subjectParameters;
	}

	/**
	 * @param string subject
	 * @return this
	 * @throws \InvalidArgumentException if the subject is invalid
	 * @since 11.0.0
	 */
	public IEvent setParsedSubject(string subject) {
		if (subject == "") {
			throw new ArgumentException ("The given parsed subject is invalid");
		}
		this.subjectParsed = subject;
		return this;
	}

	/**
	 * @return string
	 * @since 11.0.0
	 */
	public string getParsedSubject() {
		return this.subjectParsed;
	}

	/**
	 * @param string subject
	 * @param array parameters
	 * @return this
	 * @throws \InvalidArgumentException if the subject or parameters are invalid
	 * @since 11.0.0
	 */
	public IEvent setRichSubject(string subject, IList<string> parameters) {
		if (subject == "") {
			throw new ArgumentException ("The given parsed subject is invalid");
		}
		this.subjectRich = subject;
		this.subjectRichParameters = parameters;

		return this;
	}

	/**
	 * @return string
	 * @since 11.0.0
	 */
	public string getRichSubject(){
		return this.subjectRich;
	}

	/**
	 * @return array[]
	 * @since 11.0.0
	 */
	public IList<string> getRichSubjectParameters() {
		return this.subjectRichParameters;
	}

	/**
	 * Set the message of the activity
	 *
	 * @param string message
	 * @param array parameters
	 * @return IEvent
	 * @throws \InvalidArgumentException if the message or parameters are invalid
	 * @since 8.2.0
	 */
	public IEvent setMessage(string message, IList<string> parameters) {
		if (message.Length >= 255) {
			throw new ArgumentException ("The given message is invalid");
		}
		this.message = message;
		this.messageParameters = parameters;
		return this;
	}

	/**
	 * @return string
	 */
	public string getMessage() {
		return this.message;
	}

	/**
	 * @return array
	 */
	public IList<string> getMessageParameters() {
		return this.messageParameters;
	}

	/**
	 * @param string message
	 * @return this
	 * @throws \InvalidArgumentException if the message is invalid
	 * @since 11.0.0
	 */
	public IEvent setParsedMessage(string message) {
		this.messageParsed = message;
		return this;
	}

	/**
	 * @return string
	 * @since 11.0.0
	 */
	public string getParsedMessage() {
		return this.messageParsed;
	}

	/**
	 * @param string message
	 * @param array parameters
	 * @return this
	 * @throws \InvalidArgumentException if the subject or parameters are invalid
	 * @since 11.0.0
	 */
	public IEvent setRichMessage(string message, IList<string> parameters) {
		this.messageRich = message;
		this.messageRichParameters = parameters;
		return this;
	}

	/**
	 * @return string
	 * @since 11.0.0
	 */
	public string getRichMessage() {
		return this.messageRich;
	}

	/**
	 * @return array[]
	 * @since 11.0.0
	 */
	public IList<string> getRichMessageParameters() {
		return this.messageRichParameters;
	}

	/**
	 * Set the object of the activity
	 *
	 * @param string objectType
	 * @param int objectId
	 * @param string objectName
	 * @return IEvent
	 * @throws \InvalidArgumentException if the object is invalid
	 * @since 8.2.0
	 */
	public IEvent setObject(string objectType, int objectId, string objectName = "") {
		if ( objectType.Length >= 255) {
			throw new ArgumentException ("The given object type is invalid");
		}
		if ( objectName.Length >= 4000) {
			throw new  ArgumentException ("The given object name is invalid");
		}
		this.objectType = objectType;
		this.objectId = objectId;
		this.objectName = objectName;
		return this;
	}

	/**
	 * @return string
	 */
	public string getObjectType() {
		return this.objectType;
	}

	/**
	 * @return int
	 */
	public int getObjectId() {
		return this.objectId;
	}

	/**
	 * @return string
	 */
	public string getObjectName() {
		return this.objectName;
	}

	/**
	 * Set the link of the activity
	 *
	 * @param string link
	 * @return IEvent
	 * @throws \InvalidArgumentException if the link is invalid
	 * @since 8.2.0
	 */
	public IEvent setLink(string link) {
		if ( link.Length >= 4000) {
			throw new ArgumentException ("The given link is invalid");
		}
		this.link = link;
		return this;
	}

	/**
	 * @return string
	 */
	public string getLink() {
		return this.link;
	}

	/**
	 * @param string icon
	 * @return this
	 * @throws \InvalidArgumentException if the icon is invalid
	 * @since 11.0.0
	 */
	public IEvent setIcon(string icon) {
		if ( icon.Length >= 4000) {
			throw new ArgumentException ("The given icon is invalid");
		}
		this.icon = icon;
		return this;
	}

	/**
	 * @return string
	 * @since 11.0.0
	 */
	public string getIcon() {
		return this.icon;
	}

	/**
	 * @param IEvent child
	 * @return this
	 * @since 11.0.0 - Since 15.0.0 returns this
	 */
	public IEvent setChildEvent(IEvent child) {
		this.child = child;
		return this;
	}

	/**
	 * @return IEvent|null
	 * @since 11.0.0
	 */
	public IEvent getChildEvent() {
		return this.child;
	}

	/**
	 * @return bool
	 * @since 8.2.0
	 */
	public bool isValid() {
		return
			this.isValidCommon()
			&&
			this.getSubject() != ""
		;
	}

	/**
	 * @return bool
	 * @since 8.2.0
	 */
	public bool isValidParsed() {
		if (this.getRichSubject() != "" || this.getRichSubjectParameters().Count != 0) {
			try {
				this.richValidator.validate(this.getRichSubject(), this.getRichSubjectParameters());
			} catch (InvalidObjectExeption e) {
				return false;
			}
		}

		if (this.getRichMessage() != "" || (this.getRichMessageParameters().Count != 0)) {
			try {
				this.richValidator.validate(this.getRichMessage(), this.getRichMessageParameters());
			} catch (InvalidObjectExeption e) {
				return false;
			}
		}

		return
			this.isValidCommon()
			&&
			this.getParsedSubject() != ""
		;
	}

	protected bool isValidCommon() {
		return
			this.getApp() != ""
			&&
			this.getType() != ""
			&&
			this.getAffectedUser() != ""
			&&
			this.getTimestamp() != 0
			/**
			 * Disabled for BC with old activities
			&&
			this.getObjectType() !== ""
			&&
			this.getObjectId() !== 0
			 */
		;
	}
}

}
