using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.L10N
{
    /**
 * @since 8.2.0
 */
public interface IFactory {
	/**
	 * Get a language instance
	 *
	 * @param string app
	 * @param string|null lang
	 * @param string|null locale
	 * @return \OCP\IL10N
	 * @since 8.2.0
	 */
	IL10N get(string app, string lang = null, string locale = null);

	/**
	 * Find the best language
	 *
	 * @param string|null app App id or null for core
	 * @return string language If nothing works it returns 'en'
	 * @since 9.0.0
	 */
	string findLanguage(string app = null);

	/**
	 * @param string|null lang user language as default locale
	 * @return string locale If nothing works it returns 'en_US'
	 * @since 14.0.0
	 */
	string findLocale(string lang = null);

	/**
	 * find the matching lang from the locale
	 *
	 * @param string app
	 * @param string locale
	 * @return null|string
	 * @since 14.0.1
	 */
	string? findLanguageFromLocale(string app = "core", string locale = null);

	/**
	 * Find all available languages for an app
	 *
	 * @param string|null app App id or null for core
	 * @return string[] an array of available languages
	 * @since 9.0.0
	 */
	IList<string> findAvailableLanguages(string app = null);

	/**
	 * @return array an array of available
	 * @since 14.0.0
	 */
	IList<string> findAvailableLocales();

	/**
	 * @param string|null app App id or null for core
	 * @param string lang
	 * @return bool
	 * @since 9.0.0
	 */
	bool languageExists(string app, string lang);

	/**
	 * @param string locale
	 * @return bool
	 * @since 14.0.0
	 */
	bool localeExists(string locale);

	/**
	 * Creates a function from the plural string
	 *
	 * @param string string
	 * @return string Unique function name
	 * @since 14.0.0
	 */
	string createPluralFunction(string pluralString);

	/**
	 * iterate through language settings (if provided) in this order:
	 * 1. returns the forced language or:
	 * 2. if applicable, the trunk of 1 (e.g. "fu" instead of "fu_BAR"
	 * 3. returns the user language or:
	 * 4. if applicable, the trunk of 3
	 * 5. returns the system default language or:
	 * 6. if applicable, the trunk of 5
	 * 7+∞. returns 'en'
	 *
	 * Hint: in most cases findLanguage() suits you fine
	 *
	 * @since 14.0.0
	 */
	ILanguageIterator getLanguageIterator(IUser user = null);
}

}