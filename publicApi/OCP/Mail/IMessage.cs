﻿using System;
using System.Collections.Generic;
using System.Text;
using CommonTypes.Email;

namespace OCP.Mail
{

    /**
     * Interface IMessage
     *
     * @package OCP\Mail
     * @since 13.0.0
     */
    public interface IMessage
    {

        /**
         * @param IAttachment attachment
         * @return IMessage
         * @since 13.0.0
         */
        IMessage attach(IAttachment attachment);

        /**
         * Set the from address of this message.
         *
         * If no "From" address is used \OC\Mail\Mailer will use mail_from_address and mail_domain from config.php
         *
         * @param array addresses Example: array('sender@domain.org', 'other@domain.org' => 'A name')
         * @return IMessage
         * @since 13.0.0
         */
        IMessage setFrom(EmailAddress emailAddress);

        /**
         * Set the Reply-To address of this message
         *
         * @param array addresses
         * @return IMessage
         * @since 13.0.0
         */
        IMessage setReplyTo(IList<EmailAddress> addresses);

        /**
         * Set the to addresses of this message.
         *
         * @param array recipients Example: array('recipient@domain.org', 'other@domain.org' => 'A name')
         * @return IMessage
         * @since 13.0.0
         */
        IMessage setTo(IList<EmailAddress> recipients);

        /**
         * Set the CC recipients of this message.
         *
         * @param array recipients Example: array('recipient@domain.org', 'other@domain.org' => 'A name')
         * @return IMessage
         * @since 13.0.0
         */
        IMessage setCc(IList<EmailAddress> recipients);

        /**
         * Set the BCC recipients of this message.
         *
         * @param array recipients Example: array('recipient@domain.org', 'other@domain.org' => 'A name')
         * @return IMessage
         * @since 13.0.0
         */
        IMessage setBcc(IList<EmailAddress> recipients);

        /**
         * @param IEMailTemplate emailTemplate
         * @return IMessage
         * @since 13.0.0
         */
        IMessage useTemplate(IEMailTemplate emailTemplate);
}

}
