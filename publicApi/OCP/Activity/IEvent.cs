using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi.Activity
{
    /**
 * Interface IEvent
 *
 * @package OCP\Activity
 * @since 8.2.0
 */
    interface IEvent
    {
        /**
	 * Set the app of the activity
	 *
	 * @param string app
	 * @return IEvent
	 * @throws \InvalidArgumentException if the app id is invalid
	 * @since 8.2.0
	 */
        IEvent setApp(string app);

        /**
         * Set the type of the activity
         *
         * @param string type
         * @return IEvent
         * @throws \InvalidArgumentException if the type is invalid
         * @since 8.2.0
         */
        IEvent setType(string type);

        /**
         * Set the affected user of the activity
         *
         * @param string user
         * @return IEvent
         * @throws \InvalidArgumentException if the affected user is invalid
         * @since 8.2.0
         */
        IEvent setAffectedUser(string user);

        /**
         * Set the author of the activity
         *
         * @param string author
         * @return IEvent
         * @throws \InvalidArgumentException if the author is invalid
         * @since 8.2.0
         */
        IEvent setAuthor(string author);

        /**
         * Set the author of the activity
         *
         * @param int timestamp
         * @return IEvent
         * @throws \InvalidArgumentException if the timestamp is invalid
         * @since 8.2.0
         */
        IEvent setTimestamp(int timestamp);

        /**
         * Set the subject of the activity
         *
         * @param string subject
         * @param array parameters
         * @return IEvent
         * @throws \InvalidArgumentException if the subject or parameters are invalid
         * @since 8.2.0
         */
        IEvent setSubject(string subject, Array parameters);

	/**
	 * Set a parsed subject
	 *
	 * HTML is not allowed in the parsed subject and will be escaped
	 * automatically by the clients. You can use the RichObjectString system
	 * provided by the Nextcloud server to highlight important parameters via
	 * the setRichSubject method, but make sure, that a plain text message is
	 * always set via setParsedSubject, to support clients which can not handle
	 * rich strings.
	 *
	 * See https://github.com/nextcloud/server/issues/1706 for more information.
	 *
	 * @param string subject
	 * @return this
	 * @throws \InvalidArgumentException if the subject is invalid
	 * @since 11.0.0
	 */
	IEvent setParsedSubject(string subject);

	/**
	 * @return string
	 * @since 11.0.0
	 */
	string getParsedSubject();

        /**
         * Set a RichObjectString subject
         *
         * HTML is not allowed in the rich subject and will be escaped automatically
         * by the clients, but you can use the RichObjectString system provided by
         * the Nextcloud server to highlight important parameters.
         * Also make sure, that a plain text subject is always set via
         * setParsedSubject, to support clients which can not handle rich strings.
         *
         * See https://github.com/nextcloud/server/issues/1706 for more information.
         *
         * @param string subject
         * @param array parameters
         * @return this
         * @throws \InvalidArgumentException if the subject or parameters are invalid
         * @since 11.0.0
         */
        IEvent setRichSubject(string subject, Array parameters);

	/**
	 * @return string
	 * @since 11.0.0
	 */
	string getRichSubject();

	/**
	 * @return array[]
	 * @since 11.0.0
	 */
	Array getRichSubjectParameters();

        /**
         * Set the message of the activity
         *
         * @param string message
         * @param array parameters
         * @return IEvent
         * @throws \InvalidArgumentException if the message or parameters are invalid
         * @since 8.2.0
         */
        IEvent setMessage(string message, Array parameters);

        /**
         * Set a parsed message
         *
         * HTML is not allowed in the parsed message and will be escaped
         * automatically by the clients. You can use the RichObjectString system
         * provided by the Nextcloud server to highlight important parameters via
         * the setRichMessage method, but make sure, that a plain text message is
         * always set via setParsedMessage, to support clients which can not handle
         * rich strings.
         *
         * See https://github.com/nextcloud/server/issues/1706 for more information.
         *
         * @param string message
         * @return this
         * @throws \InvalidArgumentException if the message is invalid
         * @since 11.0.0
         */
        IEvent setParsedMessage(string message);

	/**
	 * @return string
	 * @since 11.0.0
	 */
	string getParsedMessage();

        /**
         * Set a RichObjectString message
         *
         * HTML is not allowed in the rich message and will be escaped automatically
         * by the clients, but you can use the RichObjectString system provided by
         * the Nextcloud server to highlight important parameters.
         * Also make sure, that a plain text message is always set via
         * setParsedMessage, to support clients which can not handle rich strings.
         *
         * See https://github.com/nextcloud/server/issues/1706 for more information.
         *
         * @param string message
         * @param array parameters
         * @return this
         * @throws \InvalidArgumentException if the message or parameters are invalid
         * @since 11.0.0
         */
        IEvent setRichMessage(string message, Array parameters);

	/**
	 * @return string
	 * @since 11.0.0
	 */
	string getRichMessage();

	/**
	 * @return array[]
	 * @since 11.0.0
	 */
	Array getRichMessageParameters();

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
        IEvent setObject(string objectType, int objectId, string objectName = "");

        /**
         * Set the link of the activity
         *
         * @param string link
         * @return IEvent
         * @throws \InvalidArgumentException if the link is invalid
         * @since 8.2.0
         */
        IEvent setLink(string link);

	/**
	 * @return string
	 * @since 8.2.0
	 */
	string getApp();

	/**
	 * @return string
	 * @since 8.2.0
	 */
	string getType();

	/**
	 * @return string
	 * @since 8.2.0
	 */
	string getAffectedUser();

	/**
	 * @return string
	 * @since 8.2.0
	 */
	string getAuthor();

	/**
	 * @return int
	 * @since 8.2.0
	 */
	int getTimestamp();

	/**
	 * @return string
	 * @since 8.2.0
	 */
	string getSubject();

	/**
	 * @return array
	 * @since 8.2.0
	 */
	Array getSubjectParameters();

	/**
	 * @return string
	 * @since 8.2.0
	 */
	string getMessage();

	/**
	 * @return array
	 * @since 8.2.0
	 */
	Array getMessageParameters();

	/**
	 * @return string
	 * @since 8.2.0
	 */
	string getObjectType();

	/**
	 * @return int
	 * @since 8.2.0
	 */
	int getObjectId();

	/**
	 * @return string
	 * @since 8.2.0
	 */
	string getObjectName();

	/**
	 * @return string
	 * @since 8.2.0
	 */
	string getLink();

	/**
	 * @param string icon
	 * @return this
	 * @throws \InvalidArgumentException if the icon is invalid
	 * @since 11.0.0
	 */
	IEvent setIcon(string icon);

	/**
	 * @return string
	 * @since 11.0.0
	 */
	string getIcon();

        /**
         * @param IEvent child
         * @return this
         * @since 11.0.0 - Since 15.0.0 returns this
         */
        IEvent setChildEvent(IEvent child);

        /**
         * @return IEvent|null
         * @since 11.0.0
         */
        IEvent? getChildEvent();

	/**
	 * @return bool
	 * @since 11.0.0
	 */
	bool isValid();

	/**
	 * @return bool
	 * @since 11.0.0
	 */
	bool isValidParsed();
    }
}
