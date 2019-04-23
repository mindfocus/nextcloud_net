using System;
using System.Collections.Generic;

namespace OCP.Notification
{
    /**
     * Interface INotification
     *
     * @package OCP\Notification
     * @since 9.0.0
     */
    public interface INotification
    {
        /**
         * @param string app
         * @return this
         * @throws \InvalidArgumentException if the app id is invalid
         * @since 9.0.0
         */
        INotification setApp(string app);

        /**
         * @return string
         * @since 9.0.0
         */
        string getApp();

        /**
         * @param string user
         * @return this
         * @throws \InvalidArgumentException if the user id is invalid
         * @since 9.0.0
         */
        INotification setUser(string user);

        /**
         * @return string
         * @since 9.0.0
         */
        string getUser();

        /**
         * @param \DateTime dateTime
         * @return this
         * @throws \InvalidArgumentException if the dateTime is invalid
         * @since 9.0.0
         */
        INotification setDateTime(DateTime dateTime);

        /**
         * @return \DateTime
         * @since 9.0.0
         */
        DateTime getDateTime();

        /**
         * @param string type
         * @param string id
         * @return this
         * @throws \InvalidArgumentException if the object type or id is invalid
         * @since 9.0.0
         */
        INotification setObject(string type, string id);

        /**
         * @return string
         * @since 9.0.0
         */
        string getObjectType();

        /**
         * @return string
         * @since 9.0.0
         */
        string getObjectId();

        /**
         * @param string subject
         * @param array parameters
         * @return this
         * @throws \InvalidArgumentException if the subject or parameters are invalid
         * @since 9.0.0
         */
        INotification setSubject(string subject, IList<string> parameters);

        /**
         * @return string
         * @since 9.0.0
         */
        string getSubject();

        /**
         * @return string[]
         * @since 9.0.0
         */
        IList<string> getSubjectParameters();

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
         * @since 9.0.0
         */
        INotification setParsedSubject(string subject);

        /**
         * @return string
         * @since 9.0.0
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
        INotification setRichSubject(string subject, IList<string> parameters);

        /**
         * @return string
         * @since 11.0.0
         */
        string getRichSubject();

        /**
         * @return array[]
         * @since 11.0.0
         */
        IList<string> getRichSubjectParameters();

        /**
         * @param string message
         * @param array parameters
         * @return this
         * @throws \InvalidArgumentException if the message or parameters are invalid
         * @since 9.0.0
         */
        INotification setMessage(string message, IList<string>  parameters);

        /**
         * @return string
         * @since 9.0.0
         */
        string getMessage();

        /**
         * @return string[]
         * @since 9.0.0
         */
        IList<string> getMessageParameters();

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
         * @since 9.0.0
         */
        INotification setParsedMessage(string message);

        /**
         * @return string
         * @since 9.0.0
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
        INotification setRichMessage(string message, IList<string> parameters);

        /**
         * @return string
         * @since 11.0.0
         */
        string getRichMessage();

        /**
         * @return array[]
         * @since 11.0.0
         */
        IList<string> getRichMessageParameters();

        /**
         * @param string link
         * @return this
         * @throws \InvalidArgumentException if the link is invalid
         * @since 9.0.0
         */
        INotification setLink(string link);

        /**
         * @return string
         * @since 9.0.0
         */
        string getLink();

        /**
         * @param string icon
         * @return this
         * @throws \InvalidArgumentException if the icon is invalid
         * @since 11.0.0
         */
        INotification setIcon(string icon);

        /**
         * @return string
         * @since 11.0.0
         */
        string getIcon();

        /**
         * @return IAction
         * @since 9.0.0
         */
        IAction createAction();

        /**
         * @param IAction action
         * @return this
         * @throws \InvalidArgumentException if the action is invalid
         * @since 9.0.0
         */
        INotification addAction(IAction action);

        /**
         * @return IAction[]
         * @since 9.0.0
         */
        IList<IAction> getActions();

        /**
         * @param IAction action
         * @return this
         * @throws \InvalidArgumentException if the action is invalid
         * @since 9.0.0
         */
        INotification addParsedAction(IAction action);

        /**
         * @return IAction[]
         * @since 9.0.0
         */
        IList<IAction> getParsedActions();

        /**
         * @return bool
         * @since 9.0.0
         */
        bool isValid();

        /**
         * @return bool
         * @since 9.0.0
         */
        bool isValidParsed();
    }

}
