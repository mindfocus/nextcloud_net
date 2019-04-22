﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Interface IL10N
     *
     * @package OCP
     * @since 6.0.0
     */
    public interface IL10N
    {
        /**
         * Translating
         * @param string text The text we need a translation for
         * @param array|string parameters default:array() Parameters for sprintf
         * @return string Translation or the same text
         *
         * Returns the translation. If no translation is found, text will be
         * returned.
         * @since 6.0.0
         */
        string t(string text, IList<string> parameters);

        /**
         * Translating
         * @param string text_singular the string to translate for exactly one object
         * @param string text_plural the string to translate for n objects
         * @param integer count Number of objects
         * @param array parameters default:array() Parameters for sprintf
         * @return string Translation or the same text
         *
         * Returns the translation. If no translation is found, text will be
         * returned. %n will be replaced with the number of objects.
         *
         * The correct plural is determined by the plural_forms-function
         * provided by the po file.
         * @since 6.0.0
         *
         */
        string n(string text_singular, string text_plural, int count, IList<string> parameters);

	/**
	 * Localization
	 * @param string type Type of localization
	 * @param \DateTime|int|string data parameters for this localization
	 * @param array options currently supports following options:
	 * 			- 'width': handed into \Punic\Calendar::formatDate as second parameter
	 * @return string|int|false
	 *
	 * Returns the localized data.
	 *
	 * Implemented types:
	 *  - date
	 *    - Creates a date
	 *    - l10n-field: date
	 *    - params: timestamp (int/string)
	 *  - datetime
	 *    - Creates date and time
	 *    - l10n-field: datetime
	 *    - params: timestamp (int/string)
	 *  - time
	 *    - Creates a time
	 *    - l10n-field: time
	 *    - params: timestamp (int/string)
	 * @since 6.0.0 - parameter options was added in 8.0.0
	 */
	object l(string type,object data, IList<string> options );


        /**
         * The code (en, de, ...) of the language that is used for this IL10N object
         *
         * @return string language
         * @since 7.0.0
         */
        string getLanguageCode();

        /**
         * * The code (en_US, fr_CA, ...) of the locale that is used for this IL10N object
         *
         * @return string locale
         * @since 14.0.0
         */
        string getLocaleCode();
}

}
