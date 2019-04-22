using System;
using System.Collections.Generic;

namespace OCP.LDAP
{
    /**
     * Interface ILDAPProvider
     *
     * @package OCP\LDAP
     * @since 11.0.0
     */
    public interface ILDAPProvider
    {
        /**
         * Translate a user id to LDAP DN.
         * @param string uid user id
         * @return string
         * @since 11.0.0
         */
        string getUserDN(string uid);

        /**
         * Translate a group id to LDAP DN.
         * @param string gid group id
         * @return string
         * @since 13.0.0
         */
        string  getGroupDN(string gid);

        /**
         * Translate a LDAP DN to an internal user name.
         * @param string dn LDAP DN
         * @return string with the internal user name
         * @throws \Exception if translation was unsuccessful
         * @since 11.0.0
         */
        string getUserName(string dn);

        /**
         * Convert a stored DN so it can be used as base parameter for LDAP queries.
         * @param string dn the DN
         * @return string
         * @since 11.0.0
         */
        string DNasBaseParameter(string dn);

        /**
         * Sanitize a DN received from the LDAP server.
         * @param array dn the DN in question
         * @return array the sanitized DN
         * @since 11.0.0
         */
        IList<string> sanitizeDN(string dn);

        /**
         * Return a new LDAP connection resource for the specified user. 
         * @param string uid user id
         * @return resource of the LDAP connection
         * @since 11.0.0
         */
        int getLDAPConnection(string uid);

        /**
         * Return a new LDAP connection resource for the specified group.
         * @param string gid group id
         * @return resource of the LDAP connection
         * @since 13.0.0
         */
        int getGroupLDAPConnection(string gid);

        /**
         * Get the LDAP base for users.
         * @param string uid user id
         * @return string the base for users
         * @throws \Exception if user id was not found in LDAP
         * @since 11.0.0
         */
        string getLDAPBaseUsers(string uid);

        /**
         * Get the LDAP base for groups.
         * @param string uid user id
         * @return string the base for groups
         * @throws \Exception if user id was not found in LDAP
         * @since 11.0.0
         */
        string getLDAPBaseGroups(string uid);

        /**
         * Check whether a LDAP DN exists
         * @param string dn LDAP DN
         * @return bool whether the DN exists
         * @since 11.0.0
         */
        bool dnExists(string dn);

        /**
         * Clear the cache if a cache is used, otherwise do nothing.
         * @param string uid user id
         * @since 11.0.0
         */
        void clearCache(string uid);

        /**
         * Clear the cache if a cache is used, otherwise do nothing.
         * @param string gid group id
         * @since 13.0.0
         */
        void clearGroupCache(string gid);

        /**
         * Get the LDAP attribute name for the user's display name
         * @param string uid user id
         * @return string the display name field
         * @throws \Exception if user id was not found in LDAP
         * @since 12.0.0
         */
        public function getLDAPDisplayNameField(uid);

        /**
         * Get the LDAP attribute name for the email
         * @param string uid user id
         * @return string the email field
         * @throws \Exception if user id was not found in LDAP
         * @since 12.0.0
         */
        string getLDAPEmailField(string uid);

        /**
         * Get the LDAP attribute name for the type of association betweeen users and groups
         * @param string gid group id
         * @return string the configuration, one of: 'memberUid', 'uniqueMember', 'member', 'gidNumber'
         * @throws \Exception if group id was not found in LDAP
         * @since 13.0.0
         */
        string getLDAPGroupMemberAssoc(string gid);

    }

}
