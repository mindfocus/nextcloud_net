using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Mail
{

    /**
     * Interface IEMailTemplate
     *
     * Interface to a class that allows to build HTML emails
     *
     * Example:
     *
     * <?php
     *
     * $emailTemplate = new EMailTemplate($this->defaults, $this->urlGenerator, $this->l10n);
     *
     * $emailTemplate->addHeader();
     * $emailTemplate->addHeading('Welcome aboard');
     * $emailTemplate->addBodyText('Welcome to your Nextcloud account, you can add, protect, and share your data.');
     *
     * $emailTemplate->addBodyButtonGroup(
     *     'Set your password', 'https://example.org/resetPassword/q1234567890qwertz',
     *     'Install Client', 'https://nextcloud.com/install/#install-clients'
     * );
     *
     * $emailTemplate->addFooter('Optional footer text');
     *
     * $htmlContent = $emailTemplate->renderHtml();
     * $plainContent = $emailTemplate->renderText();
     *
     * @since 12.0.0
     */
    public interface IEMailTemplate
    {

        /**
         * Sets the subject of the email
         *
         * @param string $subject
         *
         * @since 13.0.0
         */
        void setSubject(string subject);

        /**
         * Adds a header to the email
         *
         * @since 12.0.0
         */
        void addHeader();

        /**
         * Adds a heading to the email
         *
         * @param string $title
         * @param string|bool $plainTitle Title that is used in the plain text email
         *   if empty the $title is used, if false none will be used
         *
         * @since 12.0.0
         */
        void addHeading(string title, string plainTitle = "");

        /**
         * Adds a paragraph to the body of the email
         *
         * @param string $text; Note: When $plainText falls back to this, HTML is automatically escaped in the HTML email
         * @param string|bool $plainText Text that is used in the plain text email
         *   if empty the $text is used, if false none will be used
         *
         * @since 12.0.0
         */
        void addBodyText(string text, string plainText = "");

        /**
         * Adds a list item to the body of the email
         *
         * @param string $text; Note: When $plainText falls back to this, HTML is automatically escaped in the HTML email
         * @param string $metaInfo; Note: When $plainMetaInfo falls back to this, HTML is automatically escaped in the HTML email
         * @param string $icon Absolute path, must be 16*16 pixels
         * @param string|bool $plainText Text that is used in the plain text email
         *   if empty the $text is used, if false none will be used
         * @param string|bool $plainMetaInfo Meta info that is used in the plain text email
         *   if empty the $metaInfo is used, if false none will be used
         * @since 12.0.0
         */
        void addBodyListItem(string text, string metaInfo = "", string icon = "", string plainText =""   , string plainMetaInfo = "");

        /**
         * Adds a button group of two buttons to the body of the email
         *
         * @param string $textLeft Text of left button; Note: When $plainTextLeft falls back to this, HTML is automatically escaped in the HTML email
         * @param string $urlLeft URL of left button
         * @param string $textRight Text of right button; Note: When $plainTextRight falls back to this, HTML is automatically escaped in the HTML email
         * @param string $urlRight URL of right button
         * @param string $plainTextLeft Text of left button that is used in the plain text version - if empty the $textLeft is used
         * @param string $plainTextRight Text of right button that is used in the plain text version - if empty the $textRight is used
         *
         * @since 12.0.0
         */
        void addBodyButtonGroup(string textLeft, string urlLeft, string textRight, string urlRight, string plainTextLeft = "", string plainTextRight = "");

        /**
         * Adds a button to the body of the email
         *
         * @param string $text Text of button; Note: When $plainText falls back to this, HTML is automatically escaped in the HTML email
         * @param string $url URL of button
         * @param string $plainText Text of button in plain text version
         * 		if empty the $text is used, if false none will be used
         *
         * @since 12.0.0
         */
        void addBodyButton(string text, string url, string plainText = "");

        /**
         * Adds a logo and a text to the footer. <br> in the text will be replaced by new lines in the plain text email
         *
         * @param string $text If the text is empty the default "Name - Slogan<br>This is an automatically sent email" will be used
         *
         * @since 12.0.0
         */
        void addFooter(string text = "");

        /**
         * Returns the rendered email subject as string
         *
         * @return string
         *
         * @since 13.0.0
         */
        string renderSubject();

	/**
	 * Returns the rendered HTML email as string
	 *
	 * @return string
	 *
	 * @since 12.0.0
	 */
	string renderHtml();

	/**
	 * Returns the rendered plain text email as string
	 *
	 * @return string
	 *
	 * @since 12.0.0
	 */
	string renderText();
}

}
