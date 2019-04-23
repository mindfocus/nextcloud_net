using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Settings
{
    /**
     * @deprecated 12 Use IIconSection instead
     * @since 9.1
     */
    public interface ISection
    {
        /**
         * returns the ID of the section. It is supposed to be a lower case string,
         * e.g. 'ldap'
         *
         * @returns string
         * @since 9.1
         */
        string getID();

        /**
         * returns the translated name as it should be displayed, e.g. 'LDAP / AD
         * integration'. Use the L10N service to translate it.
         *
         * @return string
         * @since 9.1
         */
        string getName();

        /**
         * @return int whether the form should be rather on the top or bottom of
         * the settings navigation. The sections are arranged in ascending order of
         * the priority values. It is required to return a value between 0 and 99.
         *
         * E.g.: 70
         * @since 9.1
         */
        int getPriority();
    }

}
