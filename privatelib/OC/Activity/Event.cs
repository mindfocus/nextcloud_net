using System;
using System.Collections.Generic;
using ext;
using OCP.Activity;
using OCP.RichObjectStrings;

namespace OC.Activity
{
    public class Event : IEvent
    {
        protected IValidator richValidator;
        protected string _app;
        protected string _type;
        protected string _user;
        protected string _author;
        protected int _timestamp;
        protected string _subject;
        protected IList<string> _subjectParameters;
        protected string _parsedSubject;
        protected string _subjectRich;
        protected IList<string> _subjectRichParameters;
        protected string _message;
        protected IList<string> _messageParameters;
        protected string _messageParsed;
        protected string _messageRich;
        protected IList<string> _messageRichParameters;
        protected int _objectId;
        protected string _objectType;
        protected string _objectName;
        protected string _link;
        protected string _icon;
        protected IEvent _child;

        public Event(IValidator richValidator)
        {
            this.richValidator = richValidator;
        }
        public IEvent setApp(string app)
        {
            if (app.IsEmpty() || app.Length > 32)
            {
                throw new ArgumentException("The given app is invalid");
            }
            this._app = app;
            return this;
        }

        public IEvent setType(string type)
        {
            if (type.IsEmpty() || type.Length > 255)
            {
                throw new ArgumentException("The given app is invalid");
            }
            this._type = type;
            return this;
        }

        public IEvent setAffectedUser(string user)
        {
            if (user.IsEmpty() || user.Length > 64)
            {
                throw new ArgumentException("The given app is invalid");
            }
            this._user = user;
            return this;
        }

        public IEvent setAuthor(string author)
        {
            if (author.IsEmpty() || author.Length > 64)
            {
                throw new ArgumentException("The given app is invalid");
            }
            this._author = author;
            return this;
        }

        public IEvent setTimestamp(int timestamp)
        {
            this._timestamp = timestamp;
            return this;
        }

        public IEvent setSubject(string subject, IList<string> parameters)
        {
            if (subject.IsEmpty() || subject.Length > 255)
            {
                throw new ArgumentException("The given app is invalid");
            }
            this._subject = subject;
            this._subjectParameters = parameters;
            return this;
        }

        public IEvent setParsedSubject(string subject)
        {
            if (subject.IsEmpty())
            {
                throw new ArgumentException("The given parsed subject is invalid");
            }
            this._parsedSubject = subject;
            return this;
        }

        public string getParsedSubject()
        {
            return this._parsedSubject;
        }

        public IEvent setRichSubject(string subject, IList<string> parameters)
        {
            if (subject.IsEmpty())
            {
                throw new ArgumentException("The given parsed subject is invalid");
            }

            this._subjectRich = subject;
            this._subjectRichParameters = parameters;
            return this;
        }

        public string getRichSubject()
        {
            return this._subjectRich;
        }

        public IList<string> getRichSubjectParameters()
        {
            return this._subjectRichParameters;
        }

        public IEvent setMessage(string message, IList<string> parameters)
        {
            if (message.Length > 255)
            {
                throw new ArgumentException("The given message is invalid");
            }

            this._message = message;
            this._messageParameters = parameters;
            return this;
        }

        public IEvent setParsedMessage(string message)
        {
            this._messageParsed = message;
            return this;
        }

        public string getParsedMessage()
        {
            return this._messageParsed;
        }

        public IEvent setRichMessage(string message, IList<string> parameters)
        {
            this._messageRich = message;
            this._messageRichParameters = parameters;
            return this;
        }

        public string getRichMessage()
        {
            return this._messageRich;
        }

        public IList<string> getRichMessageParameters()
        {
            return this._messageRichParameters;
        }

        public IEvent setObject(string objectType, int objectId, string objectName = "")
        {
            if (objectType.Length > 255)
            {
                throw new ArgumentException("The given object type is invalid");
            }

            if (objectName.Length > 4000)
            {
                throw new ArgumentException("The given object name is invalid");
            }

            this._objectId = objectId;
            this._objectType = objectType;
            this._objectName = objectName;
            return this;

        }

        public IEvent setLink(string link)
        {
            if (link.Length > 4000)
            {
                throw new ArgumentException("The given link is invalid");
            }

            this._link = link;
            return this;
        }

        public string getApp()
        {
            return this._app;
        }

        public string getType()
        {
            return this._type;
        }

        public string getAffectedUser()
        {
            return this._user;
        }

        public string getAuthor()
        {
            return this._author;
        }

        public int getTimestamp()
        {
            return this._timestamp;
        }

        public string getSubject()
        {
            return this._subject;
        }

        public IList<string> getSubjectParameters()
        {
            return this._subjectParameters;
        }

        public string getMessage()
        {
            return this._message;
        }

        public IList<string> getMessageParameters()
        {
            return this._messageParameters;
        }

        public string getObjectType()
        {
            return this._objectType;
        }

        public int getObjectId()
        {
            return _objectId;
        }

        public string getObjectName()
        {
            return this._objectName;
        }

        public string getLink()
        {
            return _link;
        }

        public IEvent setIcon(string icon)
        {
            if (icon.Length > 4000)
            {
                throw new ArgumentException("The given icon is invalid");
            }

            this._icon = icon;
            return this;
        }

        public string getIcon()
        {
            return this._icon;
        }

        public IEvent setChildEvent(IEvent child)
        {
            this._child = child;
            return this;
        }

        public IEvent getChildEvent()
        {
            return _child;
        }

        public bool isValid()
        {
            return isValidCommon() && this._subject.IsNotEmpty();
        }

        public bool isValidParsed()
        {
            if (this._subjectRich.IsNotEmpty() || this._subjectRichParameters.IsNotEmpty())
            {
                try
                {
                    this.richValidator.validate(this._subjectRich, this._subjectRichParameters);
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            if (this._messageRich.IsNotEmpty() || this._messageRichParameters.IsNotEmpty())
            {
                try
                {
                    this.richValidator.validate(this._messageRich, this._messageRichParameters);
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return isValidCommon() && this._parsedSubject.IsNotEmpty();
        }

        private bool isValidCommon()
        {
            return this._app.IsNotEmpty() &&
                   this._type.IsNotEmpty() &&
                   this._user.IsNotEmpty() &&
                   this._timestamp != 0;
        }
    }
}