using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Settings
{
    /**
     * @since 9.1
     */
    interface IManager
    {
        /**
         * @since 9.1.0
         */
        const string KEY_ADMIN_SETTINGS = "admin";

        /**
         * @since 9.1.0
         */
        const string KEY_ADMIN_SECTION  = "admin-section";

        /**
         * @since 13.0.0
         */
        const string KEY_PERSONAL_SETTINGS = "personal";

        /**
         * @since 13.0.0
         */
        const string KEY_PERSONAL_SECTION  = "personal-section";

        /**
         * @param string type 'admin' or 'personal'
         * @param string section Class must implement OCP\Settings\ISection
         * @since 14.0.0
         */
        void registerSection(string type, string section);

        /**
         * @param string type 'admin' or 'personal'
         * @param string setting Class must implement OCP\Settings\ISetting
         * @since 14.0.0
         */
        void registerSetting(string type, string setting);

        /**
         * returns a list of the admin sections
         *
         * @return array array of ISection[] where key is the priority
         * @since 9.1.0
         */
        IDictionary<int, ISection> getAdminSections();

	/**
	 * returns a list of the personal sections
	 *
	 * @return array array of ISection[] where key is the priority
	 * @since 13.0.0
	 */
	IDictionary<int,ISection> getPersonalSections();

        /**
         * returns a list of the admin settings
         *
         * @param string section the section id for which to load the settings
         * @return array array of IAdmin[] where key is the priority
         * @since 9.1.0
         */
        IDictionary<int, IAdmin> getAdminSettings(string section);

        /**
         * returns a list of the personal  settings
         *
         * @param string section the section id for which to load the settings
         * @return array array of IPersonal[] where key is the priority
         * @since 13.0.0
         */
        IDictionary<int, IPersonal> getPersonalSettings(string section);
}

}
